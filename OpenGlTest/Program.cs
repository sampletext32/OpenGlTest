﻿using System.Collections.Generic;
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

        private static ComponentGroup _componentGroup;

        private const uint AppWidth = 800;
        private const uint AppHeight = 600;

        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(AppWidth, AppHeight), "SFML running in .NET Core");
            window.Closed += (_, __) => window.Close();
            window.Resized += (_, e) =>
            {
                var scaleX = e.Width / window.GetView().Size.X;
                var scaleY = e.Height / window.GetView().Size.Y;
                window.Size = new Vector2u(e.Width, e.Height);
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
                _componentGroup.Resize(scaleX, scaleY);
            };
            window.MouseButtonPressed += Window_MouseButtonPressed;
            window.KeyPressed += Window_KeyPressed;
            window.SetVerticalSyncEnabled(true);
            window.SetActive(false);

            _componentGroup = new ComponentGroup(0, 0, AppWidth, AppHeight);
            
            WaveformComponent waveformComponent = new CpuWaveformComponent(0, 0, AppWidth, AppHeight);
            _componentGroup.AddComponent(waveformComponent);

            TextComponent textComponent = new TextComponent(0, 0, 150, 24);
            _componentGroup.AddComponent(textComponent);

            RectangleShape rect = new RectangleShape(new Vector2f(1, AppHeight));
            rect.FillColor = Color.White;

            _componentGroup.Init();

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

                _componentGroup.Update();
                _componentGroup.Render(window);
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