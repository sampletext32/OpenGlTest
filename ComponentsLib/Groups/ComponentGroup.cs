using System.Collections.Generic;
using System.Linq;

namespace ComponentsLib
{
    public class ComponentGroup<T> where T : IComponent
    {
        public ComponentGroup()
        {
            Components = new List<T>();
        }

        protected List<T> Components { get; set; }

        public bool UpdateRequired { get; set; }
        public bool IsInited { get; set; }

        public virtual void Update(float dt)
        {
            Components.ForEach(c => c.Update(dt));
        }

        public virtual void Init()
        {
            Components.ForEach(c => c.Init());
            IsInited = true;
        }

        public virtual void Resize(float scaleX, float scaleY)
        {
        }

        public void AddComponent(T component)
        {
            Components.Add(component);
        }

        public virtual List<TK> OfType<TK>() where TK : IComponent
        {
            return Components.Where(t => t.GetType() == typeof(TK)).Cast<TK>().ToList();
        }

        public virtual TK FirstOfType<TK>() where TK : class, IComponent
        {
            return Components.FirstOrDefault(t => t.GetType() == typeof(TK)) as TK;
        }
    }
}