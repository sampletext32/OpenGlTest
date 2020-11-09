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

        private void MapPortion(uint leftColor, uint rightColor,
            float[] leftChannel, float[] rightChannel, uint samplesCount,
            uint startSample, uint endSample, float verticalScale, uint takeRate,
            ITexture texture)
        {
            //TODO: Exclude directBitmap.Height, include height property
            var verticalHalf = texture.SizeY / 2;
            var verticalQuarter = verticalHalf / 2;
            var verticalThreeQuarters = verticalHalf * 3 / 2;

            var maxVerticalSize = verticalQuarter * verticalScale;
            
            uint samplesPerColumn = samplesCount / texture.SizeX;

            for (; startSample < endSample; startSample += samplesPerColumn)
            {
                float lowestL = 0f;
                float lowestR = 0f;
                float highestL = 0f;
                float highestR = 0f;
                
                var xPosition =
                    (uint)(startSample / (float)samplesCount * texture.SizeX);

                for (uint currentSample = startSample, end = Math.Min(endSample ,startSample + samplesPerColumn);
                    currentSample < end;
                    currentSample++)
                {
                    lowestL = MathF.Min(lowestL, leftChannel[currentSample]);
                    lowestR = MathF.Min(lowestR, rightChannel[currentSample]);
                    highestL = MathF.Max(highestL, leftChannel[currentSample]);
                    highestR = MathF.Max(highestR, rightChannel[currentSample]);
                }

                var valueL =
                    (uint)((highestL - lowestL) * maxVerticalSize);
                var valueR =
                    (uint)((highestR - lowestR) * maxVerticalSize);

                WriteSample(texture, leftColor, rightColor, valueL, valueR, xPosition, verticalQuarter,
                    verticalThreeQuarters);
            }

            Notify?.Invoke();
        }

        private void MasterWaveformMapping(uint leftColor, uint rightColor,
            float[] leftChannel, float[] rightChannel, uint inputSamplesCount, uint startSample, uint endSample,
            float verticalScale,
            uint portions, uint iterations,
            bool splitWorkFirst,
            ITexture texture)
        {
            var sampleToMapCount = endSample - startSample;

            var samplesPerPortion = sampleToMapCount / portions;

            if (splitWorkFirst)
                for (uint portion = 0; portion < portions; portion++)
                {
                    if (_isStopped) break;
                    var portionStartSample = portion * samplesPerPortion;
                    var portionEndSample = (portion + 1) * samplesPerPortion - 1;

                    // Debug.WriteLine("Portion {0}: {1} - {2}", portion, portionStartSample, portionEndSample);

                    for (uint i = 0; i < iterations; i++)
                    {
                        if (_isStopped) break;
                        var iterationOffset = i;
                        var takeRate = iterations;

                        // Debug.WriteLine("Iteration {0}", i);

                        MapPortion(leftColor, rightColor, leftChannel, rightChannel, inputSamplesCount,
                            (uint)(portionStartSample + iterationOffset),
                            (uint)portionEndSample, verticalScale, takeRate, texture);
                    }
                }
            else
                for (uint i = 0; i < iterations; i++)
                {
                    if(_isStopped) break;
                    var iterationOffset = i;
                    var takeRate = iterations;

                    // Debug.WriteLine("Iteration {0}", i);

                    for (uint portion = 0; portion < portions; portion++)
                    {
                        if (_isStopped) break;
                        var portionStartSample = portion * samplesPerPortion;
                        var portionEndSample = (portion + 1) * samplesPerPortion - 1;
                        // Debug.WriteLine("Portion {0}: {1} - {2}", portion, portionStartSample, portionEndSample);

                        MapPortion(leftColor, rightColor, leftChannel, rightChannel, inputSamplesCount,
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
            var samplesCount = (uint)parameters["samplesCount"];
            var verticalScale = (float)parameters["verticalScale"];
            var texture = (ITexture)parameters["texture"];
            var splitWorkFirst = (bool)parameters["splitWorkFirst"];
            var portions = (uint)parameters["portions"];
            var iterations = (uint)parameters["iterations"];

            MasterWaveformMapping(leftColor, rightColor, leftChannel, rightChannel, samplesCount,
                0, samplesCount, verticalScale, portions, iterations, splitWorkFirst, texture);
        }

        public void RecreateAsync(Dictionary<string, object> parameters)
        {
            Task.Run(() => Recreate(parameters));
        }

        public event Action Notify;
    }
}