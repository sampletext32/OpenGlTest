using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AudioLib
{
    public class FileLoader
    {
        public static event Action OnBeginMp3Decompression;
        public static event Action OnBeginWavWriting;

        public static async Task<Stream> LoadAndDecompressMp3(Stream fileStream)
        {
            //открываем файл
            var reader = new Mp3FileReader(fileStream);

            OnBeginMp3Decompression?.Invoke();

            //создаём PCM поток
            var waveStream = WaveFormatConversionStream.CreatePcmStream(reader);

            MemoryStream ms = new MemoryStream();

            OnBeginWavWriting?.Invoke();

            //переписываем MP3 в Wav файл в потоке
            await Task.Run(() => WaveFileWriter.WriteWavFileToStream(ms, waveStream));

            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public static async Task<Stream> LoadAny(string filepath)
        {
            using (var fileStream = File.OpenRead(filepath))
            {
                if (filepath.EndsWith(".mp3"))
                {
                    return await LoadAndDecompressMp3(fileStream);
                }
                else if (filepath.EndsWith(".wav"))
                {
                    return fileStream;
                }
                else
                {
                    throw new NotSupportedException("FileFormat Unknown");
                }
            }
        }
    }
}