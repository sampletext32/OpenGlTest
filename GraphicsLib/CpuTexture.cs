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

        // SFML texture is RGBA, but DirectBitmap is ARGB, so it requires bytes mixing

        public CpuTexture(uint sizeX, uint sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Pixels = new byte[sizeX * sizeY * 4];
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
            }
        }

        public void Clear(uint color)
        {
            for (uint j = 0; j < SizeY; j++)
            {
                WriteHorizontal(j, 0, SizeX - 1, color);
            }
        }

        public void WriteHorizontal(uint y, uint left, uint right, uint color)
        {
            byte b1 = (byte)(color >> 24 & 0xff);
            byte b2 = (byte)(color >> 16 & 0xff);
            byte b3 = (byte)(color >> 8 & 0xff);
            byte b4 = (byte)(color >> 0 & 0xff);

            uint pixelStart = (y * SizeX + left) * 4;
            uint pixelEnd = (y * SizeX + right + 1) * 4;

            byte[] pixels = Pixels;

            while (pixelStart < pixelEnd)
            {
                pixels[pixelStart + 0] = b1;
                pixels[pixelStart + 1] = b2;
                pixels[pixelStart + 2] = b3;
                pixels[pixelStart + 3] = b4;
                pixelStart += 4;
            }
        }

        public void WriteVertical(uint x, uint top, uint bottom, uint color)
        {
            byte b1 = (byte)(color >> 24 & 0xff);
            byte b2 = (byte)(color >> 16 & 0xff);
            byte b3 = (byte)(color >> 8 & 0xff);
            byte b4 = (byte)(color >> 0 & 0xff);

            uint pixelStart = (top * SizeX + x) * 4;
            uint pixelEnd = ((bottom + 1) * SizeX + x) * 4;
            
            byte[] pixels = Pixels;

            while (pixelStart < pixelEnd)
            {
                pixels[pixelStart + 0] = b1;
                pixels[pixelStart + 1] = b2;
                pixels[pixelStart + 2] = b3;
                pixels[pixelStart + 3] = b4;
                pixelStart += SizeX * 4;
            }
        }

        public byte[] GetBytes()
        {
            return Pixels;
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