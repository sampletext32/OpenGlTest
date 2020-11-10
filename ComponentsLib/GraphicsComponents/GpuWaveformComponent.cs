using System;
using System.Diagnostics;
using System.IO;
using SFML.Graphics;
using SFML.System;

namespace ComponentsLib
{
    public class GpuWaveformComponent : WaveformComponent
    {
        public GpuWaveformComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        protected override void Recreate()
        {
            RenderTexture renderTexture = new RenderTexture(SizeX, SizeY);
            Debug.WriteLine($"Shader.IsAvailable {Shader.IsAvailable}");
            Debug.WriteLine($"File.Exists(fragment_shader.glsl) {File.Exists("fragment_shader.glsl")}");

            var vertexShader = File.ReadAllText("vertex_shader.glsl");
            var fragmentShader = File.ReadAllText("fragment_shader.glsl");
            fragmentShader = fragmentShader.Replace("___1___", MusicComponent.WavFile.ChannelsSamples[0].Length.ToString());
            fragmentShader = fragmentShader.Replace("___2___", MusicComponent.WavFile.ChannelsSamples[1].Length.ToString());

            // Debug.WriteLine($"vertexShader: \n{vertexShader}");
            // Debug.WriteLine($"fragmentShader: \n{fragmentShader}");

            Shader shader = Shader.FromString(vertexShader, null, fragmentShader);

            shader.SetUniformArray("leftChannel", MusicComponent.WavFile.ChannelsSamples[0]);
            shader.SetUniformArray("rightChannel", MusicComponent.WavFile.ChannelsSamples[1]);
            shader.SetUniform("sizeX", SizeX);
            shader.SetUniform("sizeY", SizeY);
            shader.SetUniform("samples", MusicComponent.WavFile.ChannelsSamples[0].Length);
            shader.SetUniform("texture", Shader.CurrentTexture);

            var renderStates = new RenderStates
            {
                Shader = shader,
                BlendMode = BlendMode.Alpha
            };

            RectangleShape rect = new RectangleShape(new Vector2f(SizeX, SizeY));
            rect.Position = new Vector2f(0, 0);

            renderTexture.Draw(rect, renderStates);
            renderTexture.Display();

            // copy to local texture
            using var image = renderTexture.Texture.CopyToImage();
            Array.Copy(image.Pixels, Texture.GetBytes(), image.Pixels.Length);

            // base component will take care of updated texture
            UpdateRequired = true;

            // throws an error
            // SfmlTexture.Update(texture.Texture, SizeX, SizeY);
        }
    }
}