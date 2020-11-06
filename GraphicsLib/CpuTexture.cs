using System;
using SFML.Graphics;

namespace GraphicsLib
{
    // Represents RAM texture with CPU manipulation with RGBA Format
    public class CpuTexture : ITexture
    {
        protected byte[] Pixels { get; }

        public uint SizeX { get; }
        public uint SizeY { get; }

        private object UpdateLocker = new object();
        private bool UpdateRequired { get; set; } = true;

        public Lazy<Texture> SfmlTexture { get; private set; }

        public void UpdateSfmlTexture()
        {
            lock (UpdateLocker)
            {
                if (UpdateRequired)
                {
                    SfmlTexture.Value.Update(Pixels);
                    UpdateRequired = false;
                }
            }
        }

        // SFML texture is RGBA, but DirectBitmap is ARGB, so it requires bytes mixing

        public CpuTexture(uint sizeX, uint sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Pixels = new byte[sizeX * sizeY * 4];
            SfmlTexture = new Lazy<Texture>(() => new Texture(SizeX, SizeY));
        }

        // Get or sets one pixel on RGBA format
        public virtual uint this[uint x, uint y]
        {
            get
            {
                uint pixelStart = (y * SizeX + x) * 4;
                uint pixel = (uint)(Pixels[pixelStart + 0] << 24 |
                                    Pixels[pixelStart + 1] << 16 |
                                    Pixels[pixelStart + 2] << 8 |
                                    Pixels[pixelStart + 3] << 0);
                return pixel;
            }
            set
            {
                uint pixelStart = (y * SizeX + x) * 4;
                Pixels[pixelStart + 0] = (byte)(value >> 24 & 0xff);
                Pixels[pixelStart + 1] = (byte)(value >> 16 & 0xff);
                Pixels[pixelStart + 2] = (byte)(value >> 8 & 0xff);
                Pixels[pixelStart + 3] = (byte)(value >> 0 & 0xff);

                UpdateRequired = true;
            }
        }

        public void Clear(uint color)
        {
            for (uint i = 0; i < SizeX; i++)
            {
                for (uint j = 0; j < SizeY; j++)
                {
                    this[i, j] = color;
                }
            }
        }
    }

    public class CpuTextureSafe : CpuTexture
    {
        public CpuTextureSafe(uint sizeX, uint sizeY) : base(sizeX, sizeY)
        {
        }

        private void CheckCoordinates(uint x, uint y)
        {
            if (x >= SizeX)
            {
                throw new IndexOutOfRangeException($"X{{{x}}} is out of range {{0, {SizeX}}}");
            }

            if (y >= SizeY)
            {
                throw new IndexOutOfRangeException($"X{{{y}}} is out of range {{0, {SizeY}}}");
            }
        }

        public override uint this[uint x, uint y]
        {
            get
            {
                CheckCoordinates(x, y);
                return base[x, y];
            }
            set
            {
                CheckCoordinates(x, y);
                base[x, y] = value;
            }
        }
    }
}