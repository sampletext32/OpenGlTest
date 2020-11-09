#version 420 core
in vec3 position;
void main()
{
    vec2 realPosition = position.xy - 1;
    gl_Position = vec4(realPosition, 0.0, 1.0); // Напрямую передаем vec3 в vec4
}