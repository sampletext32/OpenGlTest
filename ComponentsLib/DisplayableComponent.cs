using System.Diagnostics;
using SFML.Graphics;

namespace ComponentsLib
{
    public class DisplayableComponent : IComponent
    {
        public DisplayableComponent(uint locX, uint locY, uint sizeX, uint sizeY)
        {
            LocationX = locX;
            LocationY = locY;
            SizeX = sizeX;
            SizeY = sizeY;
        }

        public uint LocationX { get; protected set; }
        public uint LocationY { get; protected set; }
        public uint SizeX { get; protected set; }
        public uint SizeY { get; protected set; }

        public virtual bool UpdateRequired { get; set; }
        public bool IsInited { get; set; }

        public virtual void Update(float dt)
        {
        }

        public virtual void Init()
        {
            IsInited = true;
        }

        public virtual void Resize(float scaleX, float scaleY)
        {
            LocationX = (uint)(LocationX * scaleX);
            LocationY = (uint)(LocationY * scaleY);
            SizeX = (uint)(SizeX * scaleX);
            SizeY = (uint)(SizeY * scaleY);
            Debug.WriteLine($"{GetType()} Resized to {{{SizeX}, {SizeY}}}");
        }

        public virtual void Render(RenderTarget target)
        {
        }
    }
}