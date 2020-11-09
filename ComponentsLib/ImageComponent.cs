using System.Diagnostics;
using GraphicsLib;
using SFML.Graphics;
using SFML.System;

namespace ComponentsLib
{
    public class ImageComponent : DisplayableComponent
    {
        public ImageComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        protected Texture SfmlTexture { get; set; }

        protected ITexture Texture { get; set; }
        protected Sprite Sprite { get; set; }

        public override bool UpdateRequired => HardUpdateRequired || base.UpdateRequired;

        protected bool HardUpdateRequired { get; set; }

        public override void Update()
        {
            base.Update();
            if (UpdateRequired)
            {
                if (HardUpdateRequired)
                {
                    SfmlTexture.Swap(new Texture(SizeX, SizeY));
                    Sprite.TextureRect = new IntRect(0, 0, (int)SfmlTexture.Size.X, (int)SfmlTexture.Size.Y);
                    Sprite.Position = new Vector2f(LocationX, LocationY);
                    HardUpdateRequired = false;
                    Debug.WriteLine($"{GetType()} Hard Updated");
                }

                SfmlTexture.Update(Texture.GetBytes());
                UpdateRequired = false;
                // Debug.WriteLine($"{GetType()} Updated");
            }
        }

        public override void Init()
        {
            base.Init();
            Texture = new CpuTexture(SizeX, SizeY);
            Texture.Clear(0u);
            SfmlTexture = new Texture(SizeX, SizeY);
            Sprite = new Sprite(SfmlTexture);
            Sprite.Position = new Vector2f(LocationX, LocationY);
        }

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
            HardUpdateRequired = true;
        }
    }
}