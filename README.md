# OpenGL ray marcher with mandelbulb fractal.

# Build on Mac OS (And possibly linux)
This is how you could build this project:


1. Go to root folder.
2. Pull glfw, glad and glm dependencies.
```
git submudule init
git submodule update
```
3. Create an output directory and move to it.
```
mkdir build
cd build
```
4. Create a makefile with cmake. 
```
cmake -S ../ -B ./ 
```
5. Build executable.
```
make
```
6. Run it.
```
./raymarch
```

If you make a chanche to a shader file, it's enough to just re-run the binary since shaders are loaded at run time.
Camera and shader classes are from https://github.com/JoeyDeVries/LearnOpenGL/
