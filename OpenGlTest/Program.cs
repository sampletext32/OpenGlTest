using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
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

        private static DisplayableComponentGroup _displayableComponentGroup;
        private static ComponentGroup<IComponent> _nonDisplayableComponentGroup;

        private const uint AppWidth = 800;
        private const uint AppHeight = 600;

        private static void Test()
        {
            RenderWindow window = new RenderWindow(new VideoMode(AppWidth, AppHeight), "SFML running in .NET Core");
            window.Closed += (_, __) => window.Close();
            window.SetVerticalSyncEnabled(true);
            window.SetActive(false);
            TextInputComponent textInput = new TextInputComponent(0,0, 200, 24);

            Clock clock = new Clock();
            textInput.Init();
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Black);

                float dt = clock.Restart().AsSeconds();

                textInput.Update(dt);
                textInput.Render(window);
                window.Display();
            }
        }

        static void Main(string[] args)
        {
            // Test();
            // return;

            AudioPlayerWindow audioPlayerWindow = new AudioPlayerWindow();
            audioPlayerWindow.Run();
            return;

            window = new RenderWindow(new VideoMode(AppWidth, AppHeight), "SFML running in .NET Core");
            window.Closed += (_, __) => window.Close();
            window.Resized += (_, e) =>
            {
                var scaleX = e.Width / window.GetView().Size.X;
                var scaleY = e.Height / window.GetView().Size.Y;
                window.Size = new Vector2u(e.Width, e.Height);
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
                _displayableComponentGroup.Resize(scaleX, scaleY);
            };
            window.MouseButtonPressed += Window_MouseButtonPressed;
            window.KeyPressed += Window_KeyPressed;
            window.SetVerticalSyncEnabled(true);
            window.SetActive(false);

            _displayableComponentGroup = new DisplayableComponentGroup(0, 0, AppWidth, AppHeight);
            _nonDisplayableComponentGroup = new ComponentGroup<IComponent>();

            WaveformComponent waveformComponent = new CpuWaveformComponent(0, 0, AppWidth, AppHeight);
            _displayableComponentGroup.AddComponent(waveformComponent);

            var musicComponent = new MusicComponent("file.mp3");
            _nonDisplayableComponentGroup.AddComponent(musicComponent);

            waveformComponent.MusicComponent = musicComponent;

            PlaybackTimeTextComponent textComponent = new PlaybackTimeTextComponent(0, 0, 150, 24);
            textComponent.MusicComponent = musicComponent;
            _displayableComponentGroup.AddComponent(textComponent);

            VerticalLineComponent verticalLineComponent = new VerticalLineComponent(0, 0, AppHeight, Color.Yellow);
            _displayableComponentGroup.AddComponent(verticalLineComponent);

            window.SetActive(true);

            _nonDisplayableComponentGroup.Init();
            _displayableComponentGroup.Init();

            Clock clock = new Clock();

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Black);

                verticalLineComponent.LocationX =
                    (uint)(musicComponent.TimeSeconds / musicComponent.Duration * window.Size.X);

                float dt = clock.Restart().AsSeconds();

                _nonDisplayableComponentGroup.Update(dt);
                _displayableComponentGroup.Update(dt);

                _displayableComponentGroup.Render(window);
                window.Display();
            }
        }

        private static void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                var musicComponent = _nonDisplayableComponentGroup.FirstOfType<MusicComponent>();
                musicComponent.TimeSeconds = (float)e.X / window.Size.X * musicComponent.Duration;
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
            else if (e.Code == Keyboard.Key.Space)
            {
                var musicComponent = _nonDisplayableComponentGroup.FirstOfType<MusicComponent>();
                musicComponent.PlayPause();
            }
        }
    }
}