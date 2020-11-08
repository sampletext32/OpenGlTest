using System;
using GraphicsLib;
using SFML.Graphics;
using SFML.System;

namespace ComponentsLib
{
    public class ImageComponent : Component
    {
        protected Texture SfmlTexture { get; set; }

        public ImageComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
            Texture = new CpuTexture(SizeX, SizeY);
            Texture.Clear(0u);
            SfmlTexture = new Texture(SizeX, SizeY);
            Sprite = new Sprite(SfmlTexture);
            Sprite.Position = new Vector2f(LocationX, LocationY);
        }

        protected ITexture Texture { get; set; }
        protected Sprite Sprite { get; set; }

        public override void Render(RenderTarget target)
        {
            base.Render(target);
            target.Draw(Sprite);
        }

        public override void Resize(float scaleX, float scaleY)
        {
            base.Resize(scaleX, scaleY);
            Texture = new CpuTexture(SizeX, SizeY);
            Texture.Clear(0u);
            SfmlTexture = new Texture(SizeX, SizeY);
            Sprite.Texture = SfmlTexture;
            Sprite.TextureRect = new IntRect(0,0, (int)SfmlTexture.Size.X, (int)SfmlTexture.Size.Y);
            Sprite.Position = new Vector2f(LocationX, LocationY);
        }

        public override void UpdateSfmlComponent()
        {
            base.UpdateSfmlComponent();
            SfmlTexture.Update(Texture.GetBytes());
        }
    }
}