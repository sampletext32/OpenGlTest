using ComponentsLib.Basics;

namespace ComponentsLib
{
    public interface IRenderableComponent : IComponent, IRenderable
    {
        public void ProcessMouseClick(uint x, uint y);
    }
}