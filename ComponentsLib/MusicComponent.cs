using System.IO;
using AudioLib;
using SFML.Audio;
using SFML.System;

namespace ComponentsLib
{
    public class MusicComponent : IComponent
    {
        public MusicComponent(string filePath) : this(FileLoader.LoadAny(filePath).Result)
        {
        }

        public MusicComponent(Stream wavStream)
        {
            InnerInit(wavStream);
        }

        public MusicComponent(byte[] wavBytes)
        {
            InnerInit(wavBytes);
        }

        private Music SfmlMusic { get; set; }

        public float TimeSeconds
        {
            get => IsInited ? SfmlMusic.PlayingOffset.AsSeconds() : 0f;
            set
            {
                if (IsInited) SfmlMusic.PlayingOffset = Time.FromSeconds(value);
            }
        }

        public float Duration => IsInited ? SfmlMusic.Duration.AsSeconds() : 0f;

        public byte[] WavBytes { get; private set; }
        public WavFile WavFile { get; private set; }

        public bool UpdateRequired { get; set; }

        public bool IsInited { get; set; }

        public void Update(float dt)
        {
        }

        public void Init()
        {
            SfmlMusic = new Music(WavBytes);
            SfmlMusic.RelativeToListener = true;
            IsInited = true;
        }

        private void InnerInit(Stream wavStream)
        {
            WavBytes = new BinaryReader(wavStream).ReadBytes((int)wavStream.Length);
            wavStream.Seek(0, SeekOrigin.Begin);
            WavFile = new WavFile(wavStream);
        }

        private void InnerInit(byte[] wavBytes)
        {
            WavBytes = wavBytes;
            WavFile = new WavFile(wavBytes);
            SfmlMusic = new Music(wavBytes);
        }

        public void Play()
        {
            if (IsInited) SfmlMusic.Play();
        }

        public void Pause()
        {
            if (IsInited) SfmlMusic.Pause();
        }

        public void Stop()
        {
            if (IsInited) SfmlMusic.Stop();
        }

        public void PlayPause()
        {
            if (IsInited)
            {
                if (SfmlMusic.Status == SoundStatus.Playing)
                {
                    Pause();
                }
                else
                {
                    Play();
                }
            }
        }
    }
}