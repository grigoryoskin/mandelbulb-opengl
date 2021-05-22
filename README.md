# OpenGL ray marcher with mandelbulb fractal.

![Screen Recording 2021-05-22 at 17 33 44-2](https://user-images.githubusercontent.com/44236259/119221202-21cbf500-bb29-11eb-9956-13f886788a06.gif)

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
