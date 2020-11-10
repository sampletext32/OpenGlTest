using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace ComponentsLib
{
    public class TextInputComponent : RenderableComponentGroup
    {
        private float BlinkTimeSec = 1f;
        private float Elapsed = 0f;

        private bool IsCaretActive = false;

        private ColorRectComponent BackgroundColorComponent { get; set; }
        private VerticalLineComponent BlinkVerticalLineComponent { get; set; }

        private TextComponent DisplayedTextComponent { get; set; }

        public TextInputComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
            BackgroundColorComponent = new ColorRectComponent(locX, locY, sizeX, sizeY);
            BackgroundColorComponent.Color = Color.White;
            Components.Add(BackgroundColorComponent);

            BlinkVerticalLineComponent = new VerticalLineComponent(locX, locY, sizeY, Color.Black);
            Components.Add(BlinkVerticalLineComponent);

            DisplayedTextComponent = new TextComponent(0, 0, sizeX, sizeY);
            Components.Add(DisplayedTextComponent);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            Elapsed += dt;
            if (Elapsed > BlinkTimeSec)
            {
                IsCaretActive = !IsCaretActive;
                Elapsed = 0f;
            }
        }

        public override void Render(RenderTarget target)
        {
            BackgroundColorComponent.Render(target);
            if (IsCaretActive)
            {
                BlinkVerticalLineComponent.Render(target);
            }
            DisplayedTextComponent.Render(target);
        }

        public override void ProcessKeyPress(string key, bool ctrl, bool alt, bool shift)
        {
            base.ProcessKeyPress(key, ctrl, alt, shift);
            if (key.Length == 1)
            {
                DisplayedTextComponent.Text += shift ? key.ToUpper() : key.ToLower();
            }
            else if (key == "Backspace")
            {
                string text = DisplayedTextComponent.Text;
                if (text.Length > 0)
                {
                    text = text.Substring(0, text.Length - 1);
                }

                DisplayedTextComponent.Text = text;
            }
            var width = DisplayedTextComponent.MeasureSelfText();
            BlinkVerticalLineComponent.LocationX = width;
            UpdateRequired = true;
        }
    }
}