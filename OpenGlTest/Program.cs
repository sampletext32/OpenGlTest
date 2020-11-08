using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AudioLib;
using ComponentsLib;
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
        private static WavFile wavFile;

        private static List<Component> components;

        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(800, 600), "SFML running in .NET Core");
            window.Closed += (_, __) => window.Close();
            window.Resized += (_, e) =>
            {
                var scaleX = e.Width / window.GetView().Size.X;
                var scaleY = e.Height / window.GetView().Size.Y;
                window.Size = new Vector2u(e.Width, e.Height);
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
                components.ForEach(c => c.Resize(scaleX, scaleY));
            };
            window.MouseButtonPressed += Window_MouseButtonPressed;
            window.KeyPressed += Window_KeyPressed;
            window.SetVerticalSyncEnabled(true);
            window.SetActive(false);

            components = new List<Component>();

            WaveformComponent waveformComponent = new WaveformComponent(0, 0, 800, 600);
            components.Add(waveformComponent);

            TextComponent textComponent = new TextComponent(0, 0, 0, 24);
            components.Add(textComponent);

            RectangleShape rect = new RectangleShape(new Vector2f(1, 600));
            rect.FillColor = Color.White;

            components.ForEach(c => c.Init());

            music = new Music(waveformComponent.WavBytes);
            music.Play();

            window.SetActive(true);
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Black);

                rect.Position =
                    new Vector2f(music.PlayingOffset.AsSeconds() / music.Duration.AsSeconds() * window.Size.X, 0);

                var milliseconds = music.PlayingOffset.AsMilliseconds();
                var seconds = milliseconds / 1000;
                var minutes = seconds / 60 % 60;
                var hours = seconds / 3600;
                milliseconds %= 1000;
                seconds %= 60;

                textComponent.Text = $"{hours:00}:{minutes:00}:{seconds:00}:{milliseconds:0000}";

                components.ForEach(c => c.Update());
                components.ForEach(c => c.UpdateSfmlComponent());
                components.ForEach(c => c.Render(window));
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