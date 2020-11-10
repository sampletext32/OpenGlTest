using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace ComponentsLib
{
    public class AudioPlayerWindow : Window
    {
        public AudioPlayerWindow()
        {
            SfmlWindow.SetActive(true);
            WaveformComponent waveformComponent = new CpuWaveformComponent(0, 0, Width, Height);
            AddComponent(waveformComponent);

            var musicComponent = new MusicComponent("file.mp3");
            AddComponent(musicComponent);

            waveformComponent.MusicComponent = musicComponent;

            PlaybackTimeTextComponent textComponent = new PlaybackTimeTextComponent(0, 0, 150, 24);
            textComponent.MusicComponent = musicComponent;
            AddComponent(textComponent);

            VerticalLineComponent verticalLineComponent = new VerticalLineComponent(0, 0, Width, Color.Yellow);
            AddComponent(verticalLineComponent);
            SfmlWindow.SetActive(false);
        }

        protected override void OnWindowMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                var musicComponent = NonDisplayableComponents.FirstOfType<MusicComponent>();
                musicComponent.TimeSeconds = (float)e.X / SfmlWindow.Size.X * musicComponent.Duration;
            }
        }

        protected override void OnWindowKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                SfmlWindow.Close();
            }
            else if (e.Code == Keyboard.Key.Space)
            {
                var musicComponent = NonDisplayableComponents.FirstOfType<MusicComponent>();
                musicComponent.PlayPause();
            }
        }

        protected override void InnerUpdate()
        {
            // TODO: Encapsulate music component inside vertical line
            var musicComponent = NonDisplayableComponents.FirstOfType<MusicComponent>();
            var verticalLineComponent = DisplayableComponents.FirstOfType<VerticalLineComponent>();

            verticalLineComponent.LocationX =
                (uint)(musicComponent.TimeSeconds / musicComponent.Duration * SfmlWindow.Size.X);
        }
    }
}
