using System.Diagnostics;
using SFML.Graphics;

namespace ComponentsLib
{
    public class Component
    {
        public Component(uint locX, uint locY, uint sizeX, uint sizeY)
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
        public bool UpdateRequired { get; protected set; }

        public virtual void Render(RenderTarget target)
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Init()
        {
        }

        public virtual void Resize(float scaleX, float scaleY)
        {
            LocationX = (uint)(LocationX * scaleX);
            LocationY = (uint)(LocationY * scaleY);
            SizeX = (uint)(SizeX * scaleX);
            SizeY = (uint)(SizeY * scaleY);
            Debug.WriteLine($"{GetType()} Resized to {{{SizeX}, {SizeY}}}");
        }
    }
}