using System;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ComponentsLib
{
    public class Window
    {
        public Window(uint width = 800, uint height = 600, string title = "Sfml Application")
        {
            Width = width;
            Height = height;
            Title = title;
            SfmlWindow = new RenderWindow(new VideoMode(Width, Height), Title);
            SfmlWindow.SetVerticalSyncEnabled(true);

            SfmlWindow.Resized += OnWindowResized;
            SfmlWindow.Closed += OnWindowClosed;
            SfmlWindow.KeyPressed += OnWindowKeyPressed;
            SfmlWindow.MouseButtonPressed += OnWindowMouseButtonPressed;

            DisplayableComponents = new DisplayableComponentGroup(0, 0, Width, Height);
            NonDisplayableComponents = new ComponentGroup<IComponent>();
        }

        protected DisplayableComponentGroup DisplayableComponents { get; }
        protected ComponentGroup<IComponent> NonDisplayableComponents { get; }

        protected string Title { get; }

        protected uint Width { get; }
        protected uint Height { get; }

        protected RenderWindow SfmlWindow { get; }

        protected virtual void OnWindowMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
        }

        protected virtual void OnWindowKeyPressed(object sender, KeyEventArgs e)
        {
        }

        public void AddComponent(IComponent component)
        {
            if (component is DisplayableComponent displayableComponent)
                DisplayableComponents.AddComponent(displayableComponent);
            else
                NonDisplayableComponents.AddComponent(component);
        }

        protected virtual void OnWindowClosed(object sender, EventArgs e)
        {
            SfmlWindow.Close();
        }

        protected virtual void OnWindowResized(object sender, SizeEventArgs e)
        {
            var scaleX = e.Width / SfmlWindow.GetView().Size.X;
            var scaleY = e.Height / SfmlWindow.GetView().Size.Y;
            SfmlWindow.Size = new Vector2u(e.Width, e.Height);
            SfmlWindow.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
            DisplayableComponents.Resize(scaleX, scaleY);
        }

        protected virtual void InnerUpdate(){}

        public virtual void Run()
        {
            NonDisplayableComponents.Init();
            DisplayableComponents.Init();

            var clock = new Clock();

            SfmlWindow.SetActive(true);
            while (SfmlWindow.IsOpen)
            {
                SfmlWindow.DispatchEvents();
                SfmlWindow.Clear(Color.Black);

                var dt = clock.Restart().AsSeconds();
                
                InnerUpdate();

                DisplayableComponents.Update(dt);
                NonDisplayableComponents.Update(dt);

                DisplayableComponents.Render(SfmlWindow);
                SfmlWindow.Display();
            }
        }
    }
}