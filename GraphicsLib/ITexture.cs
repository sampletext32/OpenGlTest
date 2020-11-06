using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace GraphicsLib
{
    public interface ITexture
    {
        public uint SizeX { get; }
        public uint SizeY { get; }

        public uint this[uint x, uint y] { get; set; }
        public void Clear(uint color);
        public byte[] GetBytes();
    }
}