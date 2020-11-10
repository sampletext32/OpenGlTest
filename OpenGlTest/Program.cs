using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using AudioLib;
using ComponentsLib;
using GraphicsLib;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Window = ComponentsLib.Window;

namespace OpenGlTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test();
            // return;

            Window window = new TestWindow();
            window.Run();
        }
    }
}