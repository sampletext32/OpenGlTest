using GraphicsLib;
using SFML.Graphics;
using SFML.System;

namespace ComponentsLib
{
    public class Component
    {
        protected ITexture Texture { get; set; }
        protected Sprite Sprite { get; set; }

        public Component(uint locX, uint locY, uint sizeX, uint sizeY)
        {
            Texture = new CpuTexture(sizeX, sizeY);
            Texture.Clear(0u);
            Sprite = new Sprite(Texture.SfmlTexture.Value);
            Sprite.Position = new Vector2f(locX, locY);
        }

        public virtual void Render(RenderTarget target)
        {
            target.Draw(Sprite);
        }

        public virtual void Update()
        {
        }

        public virtual void Init()
        {
        }

        public virtual void Resize(float scaleX, float scaleY)
        {
            Texture = new CpuTexture((uint)(Texture.SizeX * scaleX), (uint)(Texture.SizeY * scaleY));
            Sprite sprite = new Sprite(Texture.SfmlTexture.Value);
            sprite.Position = new Vector2f(Sprite.Position.X * scaleX, Sprite.Position.Y * scaleY);
            Sprite = sprite;
        }
    }
}