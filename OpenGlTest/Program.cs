using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using AudioLib;
using ComponentsLib;
using GraphicsLib;
using LoggerLib;
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

            Logger.WriteLine(args[0]);

            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                Logger.WriteLine(eventArgs.ExceptionObject);
            };

            var fileStream = File.Open("C:\\Users\\Admin\\Downloads\\Similar Outskirts - Shockwave.mp3", FileMode.Open);

            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            var sound = new Sound(new BinaryReader(fileStream));
            sound.decodeFullyInto(writer);
            fileStream.Close();

            var wavBytes = ms.ToArray();

            short[] samples = new short[wavBytes.Length / 2];
            Buffer.BlockCopy(wavBytes, 0, samples, 0, wavBytes.Length);

            var sound1 = new SFML.Audio.Sound(new SoundBuffer(samples, 2, 44100));
            sound1.Play();


            //Music sfmlMusic = new Music(wavBytes);
            //sfmlMusic.Play();

            Console.Read();

            Window window = new AudioPlayerWindow(args[0]);
            window.Run();
        }
    }
}