#version 330 core
// out vec4 color;
uniform float sizeX;
uniform float sizeY;

void main()
{
    gl_FragColor = vec4(1 - gl_FragCoord.y / sizeY, 0.0, 0.0, gl_FragCoord.x / sizeX);
} 