using System.Collections.Generic;
using AudioLib;
using GraphicsLib;
using SFML.Graphics;

namespace ComponentsLib
{
    public class WaveformComponent : Component
    {
        public byte[] WavBytes { get; private set; }
        public WavFile WavFile { get; private set; }

        public WaveformComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        public override void Update()
        {
            base.Update();
            Texture.UpdateSfmlTexture();
        }

        public override void Render(RenderTarget target)
        {
            base.Render(target);
        }

        private void Recreate()
        {
            var waveformParameters = new Dictionary<string, object>();

            waveformParameters["texture"] = Texture;
            waveformParameters["leftColor"] = 0x7cfc00FFu; //LawnGreen
            waveformParameters["rightColor"] = 0xff4500FFu; //OrangeRed
            waveformParameters["leftChannel"] = WavFile.ChannelsSamples[0];
            waveformParameters["rightChannel"] = WavFile.ChannelsSamples[1];
            waveformParameters["samplesCount"] = WavFile.samplesCount;
            waveformParameters["verticalScale"] = 0.9f;
            waveformParameters["takeRate"] = 3;
            waveformParameters["iterations"] = 2;
            waveformParameters["splitWorkFirst"] = true;
            waveformParameters["portions"] = 10;
            new TrueWaveformProvider().RecreateAsync(waveformParameters);
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