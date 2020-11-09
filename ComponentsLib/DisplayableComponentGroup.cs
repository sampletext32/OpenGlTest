using SFML.Graphics;

namespace ComponentsLib
{
    public class DisplayableComponentGroup : ComponentGroup<DisplayableComponent>
    {
        public DisplayableComponentGroup(uint locX, uint locY, uint sizeX, uint sizeY)
        {
        }

        public void Render(RenderTarget target)
        {
            Components.ForEach(c => c.Render(target));
        }

        public virtual void Resize(float scaleX, float scaleY)
        {
            Components.ForEach(c => c.Resize(scaleX, scaleY));
        }
    }
}