namespace ComponentsLib
{
    public interface IComponent
    {
        public bool UpdateRequired { get; set; }

        public bool IsInited { get; set; }

        public void ProcessKeyPress(string key, bool ctrl, bool alt, bool shift);

        public void Update(float dt);

        public void Init();

        public void Resize(float scaleX, float scaleY);
    }
}