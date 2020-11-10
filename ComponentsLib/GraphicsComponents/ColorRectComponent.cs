using SFML.Graphics;
using SFML.System;

namespace ComponentsLib
{
    public class ColorRectComponent : DisplayableComponent
    {
        private RectangleShape SfmlRect { get; set; }

        private Color _color;

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                UpdateRequired = true;
            }
        }

        public ColorRectComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        public override void Init()
        {
            base.Init();
            SfmlRect = new RectangleShape(new Vector2f(SizeX, SizeY));
            SfmlRect.Position = new Vector2f(LocationX, LocationY);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            if (UpdateRequired)
            {
                SfmlRect.FillColor = _color;
                UpdateRequired = false;
            }
        }

        public override void Render(RenderTarget target)
        {
            base.Render(target);
            target.Draw(SfmlRect);
        }
    }
}
