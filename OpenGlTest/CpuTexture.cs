using System;

namespace OpenGlTest
{
    // Represents RAM texture with CPU manipulation with RGBA Format
    public class CpuTexture
    {
        protected byte[] Pixels { get; }
        public int SizeX { get; }
        public int SizeY { get; }

        // SFML texture is RGBA, but DirectBitmap is ARGB, so it requires bytes mixing

        public CpuTexture(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Pixels = new byte[sizeX * sizeY * 4];
        }

        // Get or sets one pixel on RGBA format
        public virtual uint this[int x, int y]
        {
            get
            {
                int pixelStart = (y * SizeX + x) * 4;
                uint pixel = (uint)(Pixels[pixelStart + 0] << 24 |
                                    Pixels[pixelStart + 1] << 16 |
                                    Pixels[pixelStart + 2] << 8 |
                                    Pixels[pixelStart + 3] << 0);
                return pixel;
            }
            set
            {
                int pixelStart = (y * SizeX + x) * 4;
                Pixels[pixelStart + 0] = (byte)(value >> 24 & 0xff);
                Pixels[pixelStart + 1] = (byte)(value >> 16 & 0xff);
                Pixels[pixelStart + 2] = (byte)(value >> 8 & 0xff);
                Pixels[pixelStart + 3] = (byte)(value >> 0 & 0xff);
            }
        }
    }

    public class CpuTextureSafe : CpuTexture
    {
        public CpuTextureSafe(int sizeX, int sizeY) : base(sizeX, sizeY)
        {
        }

        private void CheckCoordinates(int x, int y)
        {
            if (x < 0 || x >= SizeX)
            {
                throw new IndexOutOfRangeException($"X{{{x}}} is out of range {{0, {SizeX}}}");
            }

            if (y < 0 || y >= SizeY)
            {
                throw new IndexOutOfRangeException($"X{{{y}}} is out of range {{0, {SizeY}}}");
            }
        }

        public override uint this[int x, int y]
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