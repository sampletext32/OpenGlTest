using System;
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

        public WavFile WavFile => LazyWavFile?.Value;
        public Lazy<WavFile> LazyWavFile { get; private set; }

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

        public void Resize(float scaleX, float scaleY)
        {
        }

        private void InnerInit(Stream wavStream)
        {
            WavBytes = new BinaryReader(wavStream).ReadBytes((int)wavStream.Length);
            LazyWavFile = new Lazy<WavFile>(() => new WavFile(WavBytes));
        }

        private void InnerInit(byte[] wavBytes)
        {
            WavBytes = wavBytes;
            LazyWavFile = new Lazy<WavFile>(() => new WavFile(WavBytes));
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