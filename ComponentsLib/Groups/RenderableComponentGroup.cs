using ComponentsLib.Basics;
using SFML.Graphics;

namespace ComponentsLib
{
    public class RenderableComponentGroup : ComponentGroup<IRenderableComponent>, IRenderableComponent
    {
        public RenderableComponentGroup(uint locX, uint locY, uint sizeX, uint sizeY)
        {
        }

        public virtual void Render(RenderTarget target)
        {
            Components.ForEach(c => c.Render(target));
        }

        public override void Resize(float scaleX, float scaleY)
        {
            Components.ForEach(c => c.Resize(scaleX, scaleY));
        }


        public virtual void ProcessMouseClick(uint x, uint y)
        {
            Components.ForEach(c => c.ProcessMouseClick(x, y));
        }

        public override void ProcessKeyPress(string key, bool ctrl, bool alt, bool shift)
        {
            Components.ForEach(c => c.ProcessKeyPress(key, ctrl, alt, shift));
        }
    }
}