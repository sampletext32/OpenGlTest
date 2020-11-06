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

        public string Text { get; set; }
        private Text SfmlText { get; }

        public override void Render(RenderTarget target)
        {
            base.Render(target);
            target.Draw(SfmlText);
        }

        public override void Resize(float scaleX, float scaleY)
        {
            base.Resize(scaleX, scaleY);
            SfmlText.CharacterSize = (uint)(SfmlText.CharacterSize * scaleY);
        }

        public override void Init()
        {
            base.Init();
            SfmlText.CharacterSize = SizeY;
            SfmlText.Position = new Vector2f(LocationX, LocationY);
            SfmlText.FillColor = Color.White;
        }

        public override void UpdateSfmlComponent()
        {
            base.UpdateSfmlComponent();
            SfmlText.DisplayedString = Text;
        }
    }
}