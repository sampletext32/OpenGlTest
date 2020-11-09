using AudioLib;
using SFML.Graphics;

namespace ComponentsLib
{
    public class WaveformComponent : ImageComponent
    {
        public WaveformComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        public byte[] WavBytes { get; private set; }
        public WavFile WavFile { get; private set; }

        protected virtual void Recreate()
        {
        }

        public override void Init()
        {
            base.Init();

            WavBytes = FileLoader.LoadAny("file.mp3").Result;

            WavFile = new WavFile(WavBytes);

            Recreate();
        }

        public override void Resize(float scaleX, float scaleY)
        {
            base.Resize(scaleX, scaleY);
            Recreate();
        }
    }
}