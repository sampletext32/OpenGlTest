using System.Collections.Generic;
using AudioLib;
using GraphicsLib;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace OpenGlTest
{
    class Program
    {
        private static RenderWindow window;
        private static Music music;

        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(800, 600), "SFML running in .NET Core");
            window.Closed += (_, __) => window.Close();
            window.Resized += (_, e) => { window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height))); };
            window.MouseButtonPressed += Window_MouseButtonPressed;
            window.KeyPressed += Window_KeyPressed;
            window.SetVerticalSyncEnabled(true);
            window.SetActive(false);

            Font font = new Font("arial.ttf");
            Text text = new Text("", font);
            text.FillColor = Color.White;

            var bytes = FileLoader.LoadAny("file.mp3").Result;

            WavFile wavFile = new WavFile(bytes);

            var waveformTexture = new CpuTexture(800, 600);

            waveformTexture.Clear(0u);

            var waveformParameters = new Dictionary<string, object>();

            waveformParameters["texture"] = waveformTexture;
            waveformParameters["leftColor"] = 0x7cfc00FFu; //LawnGreen
            waveformParameters["rightColor"] = 0xff4500FFu; //OrangeRed
            waveformParameters["leftChannel"] = wavFile.ChannelsSamples[0];
            waveformParameters["rightChannel"] = wavFile.ChannelsSamples[1];
            waveformParameters["samplesCount"] = wavFile.samplesCount;
            waveformParameters["verticalScale"] = 0.9f;
            waveformParameters["takeRate"] = 3;
            waveformParameters["iterations"] = 2;
            waveformParameters["splitWorkFirst"] = true;
            waveformParameters["portions"] = 10;
            new TrueWaveformProvider().RecreateAsync(waveformParameters);

            RectangleShape rect = new RectangleShape(new Vector2f(1, 600));
            rect.FillColor = Color.White;

            Sprite sprite = new Sprite(waveformTexture.SfmlTexture.Value);

            music = new Music(bytes);
            music.Play();

            window.SetActive(true);
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Black);
                waveformTexture.UpdateSfmlTexture();

                rect.Position = new Vector2f(music.PlayingOffset.AsSeconds() / music.Duration.AsSeconds() * 800, 0);

                var milliseconds = music.PlayingOffset.AsMilliseconds();
                var seconds = milliseconds / 1000;
                var minutes = seconds / 60 % 60;
                var hours = seconds / 3600;
                milliseconds %= 1000;
                seconds %= 60;

                text.DisplayedString = $"{hours:00}:{minutes:00}:{seconds:00}:{milliseconds:0000}";

                window.Draw(sprite);
                window.Draw(text);
                window.Draw(rect);
                window.Display();
            }
        }

        private static void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                music.PlayingOffset = Time.FromSeconds((float)e.X / window.Size.X * music.Duration.AsSeconds());
            }
        }

        // private static byte[] BitmapToBytes(DirectBitmap bitmap)
        // {
        //     byte[] bytes = new byte[bitmap.Width * bitmap.Height * 4];
        //     for (var i = 0; i < bitmap.Bits.Length; i++)
        //     {
        //         bytes[i * 4 + 0] = (byte)(bitmap.Bits[i] >> 16 & 0xFF);
        //         bytes[i * 4 + 1] = (byte)(bitmap.Bits[i] >> 8 & 0xFF);
        //         bytes[i * 4 + 2] = (byte)(bitmap.Bits[i] >> 0 & 0xFF);
        //         bytes[i * 4 + 3] = (byte)(0xFF);
        //     }
        // 
        //     return bytes;
        // }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
    }
}