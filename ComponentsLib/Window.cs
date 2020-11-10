using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ComponentsLib
{
    public class Window
    {
        private const uint AppWidth = 800;
        private const uint AppHeight = 600;

        public Window(uint width = AppWidth, uint height = AppHeight, string title = "Sfml Application")
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

            RenderableComponents = new RenderableComponentGroup(0, 0, Width, Height);
            NonRenderableComponents = new ComponentGroup<IComponent>();
        }

        protected RenderableComponentGroup RenderableComponents { get; }
        protected ComponentGroup<IComponent> NonRenderableComponents { get; }

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
            if (component is IRenderableComponent displayableComponent)
                RenderableComponents.AddComponent(displayableComponent);
            else
                NonRenderableComponents.AddComponent(component);
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
            RenderableComponents.Resize(scaleX, scaleY);
        }

        protected virtual void InnerUpdate()
        {
        }

        public virtual void Run()
        {
            NonRenderableComponents.Init();
            RenderableComponents.Init();

            var clock = new Clock();

            SfmlWindow.SetActive(true);
            while (SfmlWindow.IsOpen)
            {
                SfmlWindow.DispatchEvents();
                SfmlWindow.Clear(Color.Black);

                var dt = clock.Restart().AsSeconds();

                InnerUpdate();

                RenderableComponents.Update(dt);
                NonRenderableComponents.Update(dt);

                RenderableComponents.Render(SfmlWindow);
                SfmlWindow.Display();
            }
        }
    }
}