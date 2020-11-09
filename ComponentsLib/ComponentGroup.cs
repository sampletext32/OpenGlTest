using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace ComponentsLib
{
    public class ComponentGroup : Component
    {
        private List<Component> _components;

        public ComponentGroup(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
            _components = new List<Component>();
        }

        public void AddComponent(Component component)
        {
            _components.Add(component);
        }

        public List<T> OfType<T>() where T : Component
        {
            return _components.Where(t => t.GetType() == typeof(T)).Cast<T>().ToList();
        }

        public T FirstOfType<T>() where T : Component
        {
            return (T)_components.FirstOrDefault(t => t.GetType() == typeof(T));
        }

        public override void Update()
        {
            base.Update();
            _components.ForEach(c => c.Update());
        }

        public override void Init()
        {
            base.Init();
            _components.ForEach(c => c.Init());
        }

        public override void Render(RenderTarget target)
        {
            base.Init();
            _components.ForEach(c => c.Render(target));
        }

        public override void Resize(float scaleX, float scaleY)
        {
            base.Resize(scaleX, scaleY);
            _components.ForEach(c => c.Resize(scaleX, scaleY));
        }
    }
}