using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GraphicsLib
{
    public class TrueWaveformProvider
    {
        private bool _isStopped;

        public void Stop()
        {
            _isStopped = true;
        }

        private void WriteSample(ITexture texture, uint leftColor, uint rightColor, uint leftSample,
            uint rightSample,
            uint x, uint baseLineLeft, uint baseLineRight)
        {
            texture.WriteVertical(x, baseLineLeft - leftSample / 2, baseLineLeft + leftSample / 2, leftColor);

            texture.WriteVertical(x, baseLineRight - rightSample / 2, baseLineRight + rightSample / 2, rightColor);
        }

        private void MasterWaveformMapping(uint leftColor, uint rightColor,
            float[] leftChannel, float[] rightChannel, uint startSample, uint endSample,
            float verticalScale,
            ITexture texture)
        {
            // TODO: Check bounds
            var samplesCount = endSample - startSample;

            //TODO: Exclude directBitmap.Height, include height property
            var verticalHalf = texture.SizeY / 2;
            var verticalQuarter = verticalHalf / 2;
            var verticalThreeQuarters = verticalHalf * 3 / 2;

            var maxVerticalSize = verticalQuarter * verticalScale;

            var samplesPerColumn = samplesCount / texture.SizeX;

            var xPosition =
                (uint)(startSample / (float)samplesCount * texture.SizeX);

            for (uint columnStart = startSample,
                columnEnd = columnStart + samplesPerColumn;
                columnStart < endSample;
                columnStart += samplesPerColumn,
                columnEnd += samplesPerColumn,
                xPosition++)
            {
                var lowestL = 0f;
                var lowestR = 0f;
                var highestL = 0f;
                var highestR = 0f;

                for (uint sample = columnStart,
                    end = Math.Min(endSample, columnEnd);
                    sample < end;
                    sample++)
                {
                    lowestL = MathF.Min(lowestL, leftChannel[sample]);
                    lowestR = MathF.Min(lowestR, rightChannel[sample]);
                    highestL = MathF.Max(highestL, leftChannel[sample]);
                    highestR = MathF.Max(highestR, rightChannel[sample]);
                }

                var valueL =
                    (uint)((highestL - lowestL) * maxVerticalSize);
                var valueR =
                    (uint)((highestR - lowestR) * maxVerticalSize);

                WriteSample(
                    texture,
                    leftColor,
                    rightColor,
                    valueL,
                    valueR,
                    xPosition,
                    verticalQuarter,
                    verticalThreeQuarters);
            }

            Notify?.Invoke();
        }


        public void Recreate(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("leftColor") ||
                !parameters.ContainsKey("rightColor") ||
                !parameters.ContainsKey("leftChannel") ||
                !parameters.ContainsKey("rightChannel") ||
                !parameters.ContainsKey("samplesCount") ||
                !parameters.ContainsKey("verticalScale") ||
                !parameters.ContainsKey("texture"))
                throw new ArgumentException("One Of Required Parameters Missing");

            var leftColor = (uint)parameters["leftColor"];
            var rightColor = (uint)parameters["rightColor"];
            var leftChannel = (float[])parameters["leftChannel"];
            var rightChannel = (float[])parameters["rightChannel"];
            var samplesCount = (uint)parameters["samplesCount"];
            var verticalScale = (float)parameters["verticalScale"];
            var texture = (ITexture)parameters["texture"];

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            MasterWaveformMapping(
                leftColor,
                rightColor,
                leftChannel,
                rightChannel,
                0,
                samplesCount,
                verticalScale,
                texture);
            stopwatch.Stop();
            Debug.WriteLine(stopwatch.ElapsedMilliseconds / 1000f);
        }

        public void RecreateAsync(Dictionary<string, object> parameters)
        {
            Task.Run(() => Recreate(parameters));
        }

        public event Action Notify;
    }
}