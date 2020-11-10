using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace ComponentsLib
{
    public class TestWindow : Window
    {
        public TestWindow()
        {
            TextInputComponent textInput = new TextInputComponent(0, 0, 200, 24);
            AddComponent(textInput);
        }

        protected override void InnerRender(RenderTarget target)
        {
            NonDisplayableComponents.FirstOfType<TextInputComponent>().Render(target);
        }
    }
}
