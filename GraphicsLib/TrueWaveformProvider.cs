using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GraphicsLib
{
    public class TrueWaveformProvider
    {
        private static void WriteSample(ITexture texture, uint leftColor, uint rightColor, int leftSample,
            int rightSample,
            uint x, uint baseLineLeft, uint baseLineRight)
        {
            if (leftSample < 0)
            {
                for (uint y = baseLineLeft; y < baseLineLeft - leftSample; y++)
                {
                    texture[x, y] = leftColor;
                }
            }
            else
            {
                for (uint y = (uint)(baseLineLeft - leftSample); y < baseLineLeft; y++)
                {
                    texture[x, y] = leftColor;
                }
            }

            if (rightSample < 0)
            {
                for (uint y = baseLineRight; y < baseLineRight - rightSample; y++)
                {
                    texture[x, y] = rightColor;
                }
            }
            else
            {
                for (uint y = (uint)(baseLineRight - rightSample); y < baseLineRight; y++)
                {
                    texture[x, y] = rightColor;
                }
            }
        }

        private static void MapWaveform(uint leftColor, uint rightColor,
            float[] leftChannel, float[] rightChannel, int samplesCount,
            int startSample, int endSample, float verticalScale, int takeRate,
            ITexture texture)
        {
            //TODO: Exclude directBitmap.Height, include height property
            uint verticalHalf = texture.SizeY / 2;
            uint verticalQuarter = verticalHalf / 2;
            uint verticalThreeQuarters = verticalHalf * 3 / 2;

            float maxVerticalSize = verticalQuarter * verticalScale;

            for (int currentSample = startSample; currentSample < endSample; currentSample += takeRate)
            {
                uint xPosition =
                    (uint) (currentSample / (float) samplesCount * texture.SizeX);

                int valueL =
                    (int) (leftChannel[currentSample] * maxVerticalSize);
                int valueR =
                    (int) (rightChannel[currentSample] * maxVerticalSize);

                WriteSample(texture, leftColor, rightColor, valueL, valueR, xPosition, verticalQuarter,
                    verticalThreeQuarters);
            }
        }

        private static void MasterWaveformMapping(uint leftColor, uint rightColor,
            float[] leftChannel, float[] rightChannel, int inputSamplesCount, int startSample, int endSample, float verticalScale,
            int portions, int iterations,
            bool splitWorkFirst,
            ITexture texture)
        {
            int sampleToMapCount = endSample - startSample;

            int samplesPerPortion = sampleToMapCount / portions;

            if (splitWorkFirst)
            {
                for (int portion = 0; portion < portions; portion++)
                {
                    int portionStartSample = portion * samplesPerPortion;
                    int portionEndSample = (portion + 1) * samplesPerPortion - 1;

                    Debug.WriteLine("Portion {0}: {1} - {2}", portion, portionStartSample, portionEndSample);

                    for (int i = 0; i < iterations; i++)
                    {
                        int iterationOffset = i;
                        int takeRate = iterations;

                        Debug.WriteLine("Iteration {0}", i);

                        MapWaveform(leftColor, rightColor, leftChannel, rightChannel, inputSamplesCount,
                            portionStartSample + iterationOffset,
                            portionEndSample, verticalScale, takeRate, texture);
                    }
                }
            }
            else
            {
                for (int i = 0; i < iterations; i++)
                {
                    int iterationOffset = i;
                    int takeRate = iterations;

                    Debug.WriteLine("Iteration {0}", i);

                    for (int portion = 0; portion < portions; portion++)
                    {
                        int portionStartSample = portion * samplesPerPortion;
                        int portionEndSample = (portion + 1) * samplesPerPortion - 1;
                        Debug.WriteLine("Portion {0}: {1} - {2}", portion, portionStartSample, portionEndSample);

                        MapWaveform(leftColor, rightColor, leftChannel, rightChannel, inputSamplesCount,
                            portionStartSample + iterationOffset,
                            portionEndSample, verticalScale, takeRate, texture);
                    }
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
            {
                throw new ArgumentException("One Of Required Parameters Missing");
            }
            
            uint leftColor = (uint) parameters["leftColor"];
            uint rightColor = (uint) parameters["rightColor"];
            float[] leftChannel = (float[]) parameters["leftChannel"];
            float[] rightChannel = (float[]) parameters["rightChannel"];
            int samplesCount = (int) parameters["samplesCount"];
            float verticalScale = (float) parameters["verticalScale"];
            ITexture texture = (ITexture) parameters["texture"];
            bool splitWorkFirst = (bool) parameters["splitWorkFirst"];
            int portions = (int) parameters["portions"];
            int iterations = (int) parameters["iterations"];

            MasterWaveformMapping(leftColor, rightColor, leftChannel, rightChannel, samplesCount,
                0, samplesCount, verticalScale, portions, iterations, splitWorkFirst, texture);
        }

        public void RecreateAsync(Dictionary<string, object> parameters)
        {
            Task.Run(() => Recreate(parameters));
        }
    }
}