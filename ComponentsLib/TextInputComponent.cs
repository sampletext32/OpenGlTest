using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace ComponentsLib
{
    public class TextInputComponent : DisplayableComponentGroup
    {
        private float BlinkTimeSec = 1f;
        private float Elapsed = 0f;

        private bool IsCaretActive = false;

        private ImageComponent BackgroundImageComponent { get; set; }
        private VerticalLineComponent BlinkVerticalLineComponent { get; set; }

        public TextInputComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
            BackgroundImageComponent = new ImageComponent(locX, locY, sizeX, sizeY);
            Components.Add(BackgroundImageComponent);

            BlinkVerticalLineComponent = new VerticalLineComponent(locX, locY, sizeY, Color.Black);
            Components.Add(BlinkVerticalLineComponent);
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
            BackgroundImageComponent.Render(target);
            if (IsCaretActive)
            {
                BlinkVerticalLineComponent.Render(target);
            }
        }

        public override void Init()
        {
            base.Init();
            BackgroundImageComponent.MutateTexture(t => t.Clear(0xFFFFFFFFu));
        }
    }
}