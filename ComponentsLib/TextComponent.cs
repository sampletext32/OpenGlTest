using SFML.Graphics;
using SFML.System;

namespace ComponentsLib
{
    public class TextComponent : Component
    {
        public TextComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
            var f = new Font("arial.ttf");
            SfmlText = new Text(Text, f);
        }

        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                UpdateRequired = true;
            }
        }

        private Text SfmlText { get; }

        public override void Render(RenderTarget target)
        {
            base.Render(target);
            target.Draw(SfmlText);
        }

        public override void Update()
        {
            base.Update();
            if (UpdateRequired)
            {
                SfmlText.CharacterSize = SizeY;
                UpdateRequired = false;
            }
        }

        public override void Init()
        {
            base.Init();
            SfmlText.CharacterSize = SizeY;
            SfmlText.Position = new Vector2f(LocationX, LocationY);
            SfmlText.FillColor = Color.White;
        }
    }
}