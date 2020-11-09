using System.Collections.Generic;
using GraphicsLib;

namespace ComponentsLib
{
    public class CpuWaveformComponent : WaveformComponent
    {
        private TrueWaveformProvider _waveformProvider;

        public CpuWaveformComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        protected override void Recreate()
        {
            var waveformParameters = new Dictionary<string, object>();

            waveformParameters["texture"] = Texture;
            waveformParameters["leftColor"] = 0x7cfc00FFu; //LawnGreen
            waveformParameters["rightColor"] = 0xff4500FFu; //OrangeRed
            waveformParameters["leftChannel"] = MusicComponent.WavFile.ChannelsSamples[0];
            waveformParameters["rightChannel"] = MusicComponent.WavFile.ChannelsSamples[1];
            waveformParameters["samplesCount"] = MusicComponent.WavFile.samplesCount;
            waveformParameters["verticalScale"] = 0.9f;
            waveformParameters["takeRate"] = 3;
            waveformParameters["iterations"] = 2;
            waveformParameters["splitWorkFirst"] = true;
            waveformParameters["portions"] = 10;

            _waveformProvider?.Stop();
            _waveformProvider = new TrueWaveformProvider();
            _waveformProvider.Notify += () => { UpdateRequired = true; };
            _waveformProvider.RecreateAsync(waveformParameters);
        }
    }
}