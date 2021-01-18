using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace ComponentsLib
{
    public class AudioPlayerWindow : Window
    {
        public AudioPlayerWindow(string filename)
        {
            SfmlWindow.SetActive(true);
            WaveformComponent waveformComponent = new CpuWaveformComponent(0, 0, Width, Height);
            AddComponent(waveformComponent);

            var musicComponent = new MusicComponent(filename);
            AddComponent(musicComponent);

            waveformComponent.MusicComponent = musicComponent;

            PlaybackTimeTextComponent textComponent = new PlaybackTimeTextComponent(0, 0, 150, 24);
            textComponent.MusicComponent = musicComponent;
            AddComponent(textComponent);

            VerticalLineComponent verticalLineComponent = new VerticalLineComponent(0, 0, Width, Color.Yellow);
            AddComponent(verticalLineComponent);
            SfmlWindow.SetActive(false);
        }

        protected override void InnerUpdate()
        {
            // TODO: Encapsulate music component inside vertical line
            var musicComponent = NonRenderableComponents.FirstOfType<MusicComponent>();
            var verticalLineComponent = RenderableComponents.FirstOfType<VerticalLineComponent>();

            verticalLineComponent.LocationX =
                (uint)(musicComponent.TimeSeconds / musicComponent.Duration * SfmlWindow.Size.X);
        }
    }
}