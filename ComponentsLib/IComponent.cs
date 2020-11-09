namespace ComponentsLib
{
    public interface IComponent
    {
        public bool UpdateRequired { get; set; }

        public bool IsInited { get; set; }

        public void Update();

        public void Init();
    }
}