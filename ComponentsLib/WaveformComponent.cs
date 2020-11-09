namespace ComponentsLib
{
    public abstract class WaveformComponent : ImageComponent
    {
        private MusicComponent _musicComponent;

        public WaveformComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        public MusicComponent MusicComponent
        {
            get => _musicComponent;
            set
            {
                _musicComponent = value;
            }
        }

        public override void Init()
        {
            base.Init();
            Recreate();
        }

        protected abstract void Recreate();

        public override void Resize(float scaleX, float scaleY)
        {
            base.Resize(scaleX, scaleY);
            Recreate();
        }
    }
}