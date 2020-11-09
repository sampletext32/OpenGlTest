#version 420
// out vec4 color;
uniform float sizeX;
uniform float sizeY;
uniform float leftChannel[___1___];
uniform float rightChannel[___2___];
uniform int samples;

void main()
{
    // float horizontalProcess = gl_FragCoord.x / sizeX;
    // float verticalProcess = 1. - gl_FragCoord.y / sizeY;
    // int samplesPerPixel = int(float(samples) / sizeX);
    // float largestValueL = 0.0;
    // float largestValueR = 0.0;
    // float lowestValueL = 0.0;
    // float lowestValueR = 0.0;
    // int startSample = int(horizontalProcess * float(samples));
    // for (int i = 0; i < samplesPerPixel; i++)
    // {
    //     int index = startSample + i;
    //     float valL = leftChannel[index];
    //     float valR = rightChannel[index];
    //     lowestValueL = min(lowestValueL, valL);
    //     lowestValueR = min(lowestValueR, valR);
    //     largestValueL = max(largestValueL, valL);
    //     largestValueR = max(largestValueR, valR);
    // }
    // 
    // if (verticalProcess < 0.5)
    // {
    //     // Map left
    //     float leftPixelProcess = verticalProcess / 0.5;
    //     if (leftPixelProcess > lowestValueL && leftPixelProcess < largestValueL)
    //     {
    //         gl_FragColor = vec4(0.0, 1.0, 0.0, 1.0);
    //         return;
    //     }
    // }
    // else 
    // {
    //     // Map right
    //     float rightPixelProcess = (verticalProcess - 0.5) / 0.5;
    //     if (rightPixelProcess > lowestValueR && rightPixelProcess < largestValueR)
    //     {
    //         gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);
    //         return;
    //     }
    // }
    gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);
} 