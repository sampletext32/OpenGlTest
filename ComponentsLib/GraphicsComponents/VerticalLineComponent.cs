using SFML.Graphics;
using SFML.System;

namespace ComponentsLib
{
    public class VerticalLineComponent : DisplayableComponent
    {
        private Color _color;

        public VerticalLineComponent(uint locX, uint locY, uint sizeY, Color color) : base(locX, locY, 1, sizeY)
        {
            Color = color;
        }

        private RectangleShape SfmlRect { get; set; }

        public override uint LocationX
        {
            get => base.LocationX;
            set
            {
                UpdateRequired = true;
                base.LocationX = value;
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                UpdateRequired = true;
                _color = value;
            }
        }

        public override void Init()
        {
            base.Init();
            SfmlRect = new RectangleShape(new Vector2f(1, SizeY));
            SfmlRect.FillColor = Color;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            if (UpdateRequired)
            {
                SfmlRect.Position = new Vector2f(LocationX, LocationY);
                SfmlRect.FillColor = Color;
                SfmlRect.Size = new Vector2f(1, SizeY);
                UpdateRequired = false;
            }
        }

        public override void Render(RenderTarget target)
        {
            base.Render(target);
            target.Draw(SfmlRect);
        }

        public override void Resize(float scaleX, float scaleY)
        {
            base.Resize(scaleX, scaleY);
            UpdateRequired = true;
        }
    }
}