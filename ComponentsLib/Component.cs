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

        protected uint LocationX { get; set; }
        protected uint LocationY { get; set; }
        protected uint SizeX { get; set; }
        protected uint SizeY { get; set; }

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
        }

        public virtual void UpdateSfmlComponent()
        {
        }
    }
}