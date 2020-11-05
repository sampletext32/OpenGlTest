using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using WavVisualize;

namespace OpenGlTest
{
    class Program
    {
        private static RenderWindow window;

        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(800, 600), "SFML running in .NET Core");
            window.Closed += (_, __) => window.Close();
            window.KeyPressed += Window_KeyPressed;
            window.SetVerticalSyncEnabled(true);
            window.SetActive(false);

            Font font = new Font("arial.ttf");
            Text text = new Text("", font);
            text.FillColor = Color.White;

            var bytes = FileLoader.LoadAny("file.mp3").Result;

            WavFileData wavFileData = new WavFileData(bytes);

            DirectBitmap waveformBitmap = new DirectBitmap(800, 600);
            waveformBitmap.Clear();

            var waveformParameters = new Dictionary<string, object>();

            waveformParameters["directBitmap"] = waveformBitmap;
            waveformParameters["leftColor"] = (int)(0x7cfc00 | (0xFF << 24)); //LawnGreen
            waveformParameters["rightColor"] = (int)(0xff4500 | (0xFF << 24)); //OrangeRed
            waveformParameters["leftChannel"] = wavFileData.ChannelsSamples[0];
            waveformParameters["rightChannel"] = wavFileData.ChannelsSamples[1];
            waveformParameters["samplesCount"] = wavFileData.samplesCount;
            waveformParameters["verticalScale"] = 0.9f;
            waveformParameters["takeRate"] = 3;
            waveformParameters["iterations"] = 2;
            waveformParameters["splitWorkFirst"] = false;
            waveformParameters["portions"] = 2;
            new TrueWaveformProvider().RecreateAsync(waveformParameters);

            Texture texture = new Texture(800, 600);
            Sprite sprite = new Sprite(texture);

            var music = new Music(bytes);
            music.Play();

            window.SetActive(true);
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Black);

                texture.Update(BitmapToBytes(waveformBitmap));

                var milliseconds = music.PlayingOffset.AsMilliseconds();
                var seconds = milliseconds / 1000;
                var minutes = seconds / 60 % 60;
                var hours = seconds / 3600;
                milliseconds %= 1000;
                seconds %= 60;

                text.DisplayedString = $"{hours:00}:{minutes:00}:{seconds:00}:{milliseconds:0000}";

                window.Draw(sprite);
                window.Draw(text);
                window.Display();
            }
        }

        private static byte[] BitmapToBytes(DirectBitmap bitmap)
        {
            byte[] bytes = new byte[bitmap.Width * bitmap.Height * 4];
            for (var i = 0; i < bitmap.Bits.Length; i++)
            {
                bytes[i + 0] = (byte)(bitmap.Bits[i] >> 16 & 0xFF);
                bytes[i + 1] = (byte)(bitmap.Bits[i] >> 8 & 0xFF);
                bytes[i + 2] = (byte)(bitmap.Bits[i] >> 0 & 0xFF);
                bytes[i + 3] = (byte)(0xFF);
            }

            return bytes;
        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
    }
}