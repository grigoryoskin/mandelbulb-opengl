cmake_minimum_required(VERSION 3.2 FATAL_ERROR)
project(raymarch)
set (CMAKE_CXX_STANDARD 11)

# Add source files
file(GLOB_RECURSE SOURCE_FILES 
	${CMAKE_SOURCE_DIR}/src/*.c
	${CMAKE_SOURCE_DIR}/src/*.cpp)
	
# Add header files
file(GLOB_RECURSE HEADER_FILES 
	${CMAKE_SOURCE_DIR}/src/*.h
	${CMAKE_SOURCE_DIR}/src/*.hpp)

# Configure assets header file
configure_file(${CMAKE_SOURCE_DIR}/src/utils/RootDir.h.in ${CMAKE_SOURCE_DIR}/src/utils/RootDir.h)

# this will make executable wi
add_executable(${PROJECT_NAME} ${HEADER_FILES} ${SOURCE_FILES})

# OpenGL
find_package(OpenGL REQUIRED)
add_library(GLAD "external/glad/src/glad.c")

# Go to glfw directory and build it using it's own cmake file.
add_subdirectory(external/glfw)

#include_directories will tell the linker to look for header files there.
include_directories( PUBLIC external/glfw/include
				     PUBLIC external/glad/include
					 PUBLIC external/glm)

target_link_directories(${PROJECT_NAME} PRIVATE external/glfw/src)

set(LIBS OpenGL::GL glfw GLAD)

target_link_libraries(${PROJECT_NAME} ${LIBS})
