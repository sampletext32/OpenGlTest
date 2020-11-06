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

        private void WriteSample(ITexture texture, uint leftColor, uint rightColor, int leftSample,
            int rightSample,
            uint x, uint baseLineLeft, uint baseLineRight)
        {
            if (leftSample < 0)
                for (var y = baseLineLeft; y < baseLineLeft - leftSample; y++)
                    texture[x, y] = leftColor;
            else
                for (var y = (uint)(baseLineLeft - leftSample); y < baseLineLeft; y++)
                    texture[x, y] = leftColor;

            if (rightSample < 0)
                for (var y = baseLineRight; y < baseLineRight - rightSample; y++)
                    texture[x, y] = rightColor;
            else
                for (var y = (uint)(baseLineRight - rightSample); y < baseLineRight; y++)
                    texture[x, y] = rightColor;
        }

        private void MapWaveform(uint leftColor, uint rightColor,
            float[] leftChannel, float[] rightChannel, int samplesCount,
            int startSample, int endSample, float verticalScale, int takeRate,
            ITexture texture)
        {
            //TODO: Exclude directBitmap.Height, include height property
            var verticalHalf = texture.SizeY / 2;
            var verticalQuarter = verticalHalf / 2;
            var verticalThreeQuarters = verticalHalf * 3 / 2;

            var maxVerticalSize = verticalQuarter * verticalScale;

            for (var currentSample = startSample; currentSample < endSample; currentSample += takeRate)
            {
                if (_isStopped) break;

                var xPosition =
                    (uint)(currentSample / (float)samplesCount * texture.SizeX);

                var valueL =
                    (int)(leftChannel[currentSample] * maxVerticalSize);
                var valueR =
                    (int)(rightChannel[currentSample] * maxVerticalSize);

                WriteSample(texture, leftColor, rightColor, valueL, valueR, xPosition, verticalQuarter,
                    verticalThreeQuarters);
            }
        }

        private void MasterWaveformMapping(uint leftColor, uint rightColor,
            float[] leftChannel, float[] rightChannel, int inputSamplesCount, int startSample, int endSample,
            float verticalScale,
            int portions, int iterations,
            bool splitWorkFirst,
            ITexture texture)
        {
            var sampleToMapCount = endSample - startSample;

            var samplesPerPortion = sampleToMapCount / portions;

            if (splitWorkFirst)
                for (var portion = 0; portion < portions; portion++)
                {
                    if (_isStopped) break;
                    var portionStartSample = portion * samplesPerPortion;
                    var portionEndSample = (portion + 1) * samplesPerPortion - 1;

                    Debug.WriteLine("Portion {0}: {1} - {2}", portion, portionStartSample, portionEndSample);

                    for (var i = 0; i < iterations; i++)
                    {
                        if (_isStopped) break;
                        var iterationOffset = i;
                        var takeRate = iterations;

                        Debug.WriteLine("Iteration {0}", i);

                        MapWaveform(leftColor, rightColor, leftChannel, rightChannel, inputSamplesCount,
                            portionStartSample + iterationOffset,
                            portionEndSample, verticalScale, takeRate, texture);
                    }
                }
            else
                for (var i = 0; i < iterations; i++)
                {
                    if(_isStopped) break;
                    var iterationOffset = i;
                    var takeRate = iterations;

                    Debug.WriteLine("Iteration {0}", i);

                    for (var portion = 0; portion < portions; portion++)
                    {
                        if (_isStopped) break;
                        var portionStartSample = portion * samplesPerPortion;
                        var portionEndSample = (portion + 1) * samplesPerPortion - 1;
                        Debug.WriteLine("Portion {0}: {1} - {2}", portion, portionStartSample, portionEndSample);

                        MapWaveform(leftColor, rightColor, leftChannel, rightChannel, inputSamplesCount,
                            portionStartSample + iterationOffset,
                            portionEndSample, verticalScale, takeRate, texture);
                    }
                }
        }


        public void Recreate(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("leftColor") ||
                !parameters.ContainsKey("rightColor") ||
                !parameters.ContainsKey("leftChannel") ||
                !parameters.ContainsKey("rightChannel") ||
                !parameters.ContainsKey("samplesCount") ||
                !parameters.ContainsKey("verticalScale") ||
                !parameters.ContainsKey("texture") ||
                !parameters.ContainsKey("takeRate") ||
                !parameters.ContainsKey("splitWorkFirst") ||
                !parameters.ContainsKey("portions") ||
                !parameters.ContainsKey("iterations"))
                throw new ArgumentException("One Of Required Parameters Missing");

            var leftColor = (uint)parameters["leftColor"];
            var rightColor = (uint)parameters["rightColor"];
            var leftChannel = (float[])parameters["leftChannel"];
            var rightChannel = (float[])parameters["rightChannel"];
            var samplesCount = (int)parameters["samplesCount"];
            var verticalScale = (float)parameters["verticalScale"];
            var texture = (ITexture)parameters["texture"];
            var splitWorkFirst = (bool)parameters["splitWorkFirst"];
            var portions = (int)parameters["portions"];
            var iterations = (int)parameters["iterations"];

            MasterWaveformMapping(leftColor, rightColor, leftChannel, rightChannel, samplesCount,
                0, samplesCount, verticalScale, portions, iterations, splitWorkFirst, texture);
        }

        public void RecreateAsync(Dictionary<string, object> parameters)
        {
            Task.Run(() => Recreate(parameters));
        }
    }
}