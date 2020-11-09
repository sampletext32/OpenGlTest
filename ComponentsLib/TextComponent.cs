using SFML.Graphics;
using SFML.System;

namespace ComponentsLib
{
    public class TextComponent : DisplayableComponent
    {
        private string _text;

        public TextComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        public string Text
        {
            get => _text;
            set
            {
                // if we accidentally set the same text as displayed, don't update anything
                if (_text == value)
                    return;
                _text = value;
                UpdateRequired = true;
            }
        }

        private Text SfmlText { get; set; }

        public override void Render(RenderTarget target)
        {
            base.Render(target);
            target.Draw(SfmlText);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            if (UpdateRequired)
            {
                SfmlText.CharacterSize = SizeY;
                SfmlText.DisplayedString = Text;
                var offset = 0f;
                for (var i = 0; i < Text.Length; i++)
                {
                    offset += SfmlText.Font.GetGlyph(Text[i], SfmlText.CharacterSize, false, 0f).Advance;
                    if (offset > SizeX)
                    {
                        SfmlText.DisplayedString = Text.Substring(0, i);
                        break;
                    }
                }

                UpdateRequired = false;
                // Debug.WriteLine($"{GetType()} Updated");
            }
        }

        public override void Init()
        {
            base.Init();
            var f = new Font("arial.ttf");
            SfmlText = new Text(Text, f);
            SfmlText.CharacterSize = SizeY;
            SfmlText.Position = new Vector2f(LocationX, LocationY);
            SfmlText.FillColor = Color.White;
        }
    }
}