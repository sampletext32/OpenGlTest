using System;
using System.Diagnostics;
using System.IO;
using AudioLib;
using GraphicsLib;
using SFML.Graphics;
using SFML.System;

namespace ComponentsLib
{
    public class WaveformComponent : ImageComponent
    {
        private TrueWaveformProvider _waveformProvider;

        public WaveformComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        public byte[] WavBytes { get; private set; }
        public WavFile WavFile { get; private set; }

        public override void Update()
        {
            base.Update();
        }

        public override void UpdateSfmlComponent()
        {
            base.UpdateSfmlComponent();
        }

        public override void Render(RenderTarget target)
        {
            base.Render(target);
        }

        private void Recreate()
        {
            // var waveformParameters = new Dictionary<string, object>();
            // 
            // waveformParameters["texture"] = Texture;
            // waveformParameters["leftColor"] = 0x7cfc00FFu; //LawnGreen
            // waveformParameters["rightColor"] = 0xff4500FFu; //OrangeRed
            // waveformParameters["leftChannel"] = WavFile.ChannelsSamples[0];
            // waveformParameters["rightChannel"] = WavFile.ChannelsSamples[1];
            // waveformParameters["samplesCount"] = WavFile.samplesCount;
            // waveformParameters["verticalScale"] = 0.9f;
            // waveformParameters["takeRate"] = 3;
            // waveformParameters["iterations"] = 2;
            // waveformParameters["splitWorkFirst"] = true;
            // waveformParameters["portions"] = 10;
            // 
            // _waveformProvider?.Stop();
            // _waveformProvider = new TrueWaveformProvider();
            // _waveformProvider.RecreateAsync(waveformParameters);

            RenderTexture renderTexture = new RenderTexture(SizeX, SizeY);
            Debug.WriteLine($"Shader.IsAvailable {Shader.IsAvailable}");
            Debug.WriteLine($"File.Exists(fragment_shader.glsl) {File.Exists("fragment_shader.glsl")}");

            var vertexShader = File.ReadAllText("vertex_shader.glsl");
            var fragmentShader = File.ReadAllText("fragment_shader.glsl");
            Debug.WriteLine($"vertexShader: \n{vertexShader}");
            Debug.WriteLine($"fragmentShader: \n{fragmentShader}");

            Shader shader = Shader.FromString(vertexShader, null, fragmentShader);

            // shader.SetUniformArray("leftChannel", WavFile.ChannelsSamples[0]);
            // shader.SetUniformArray("rightChannel", WavFile.ChannelsSamples[1]);
            shader.SetUniform("sizeX", (float)SizeX);
            shader.SetUniform("sizeY", SizeY);
            shader.SetUniform("texture", Shader.CurrentTexture);

            var renderStates = new RenderStates()
            {
                Shader = shader,
                BlendMode = BlendMode.Alpha
            };

            RectangleShape rect = new RectangleShape(new Vector2f(SizeX, SizeY));
            rect.Position = new Vector2f(0, 0);

            renderTexture.Draw(rect, renderStates);
            renderTexture.Display();

            SfmlTexture.Swap(renderTexture.Texture);

            var image = SfmlTexture.CopyToImage();
            Array.Copy(image.Pixels, Texture.GetBytes(), image.Pixels.Length); 

            // throws an error
            //SfmlTexture.Update(texture.Texture, SizeX, SizeY);
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