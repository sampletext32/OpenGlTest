using System.Diagnostics;
using System.Linq;
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
                if (_text == value)
                {
                    // if we accidentally set the same text as displayed, don't update anything
                    return;
                }
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
                SfmlText.DisplayedString = Text;
                float offset = 0f;
                for (int i = 0; i < Text.Length; i++)
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
            SfmlText.CharacterSize = SizeY;
            SfmlText.Position = new Vector2f(LocationX, LocationY);
            SfmlText.FillColor = Color.White;
        }
    }
}