---
title: ISF JSON Reference
tags: [Reference, Functions]
keywords: IMG_PIXEL(), IMG_NORM_PIXEL(), IMG_THIS_PIXEL(), IMG_NORM_THIS_PIXEL(), IMG_SIZE(), abs(), mod(), pow(), fract(), sign(), max(), min(), ceil(), floor(), radians(), degrees(), sin(), cos(), tan(), asin(), acos(), atan(), sqrt(), exp(), log(), log2(), inversesqrt(), clamp(), mix(), step(), smoothstep(), length(), dot(), cross(), faceforward(), reflect(), refract(), lessThan(), lessThanEqual(), greaterThan(), greaterThanEquala(), equal(), notEqual(), any(), all()
last_updated: June 12, 2018
summary: "An overview of ISF and GLSL functions."
sidebar: home_sidebar
permalink: ref_functions.html
folder: ref
---

# Built-In GLSL / ISF Functions

The base language of GLSL includes many useful functions.  If you are writing shaders against the ISF specification there are a few additional functions that can be used for working with image data.

## ISF Exclusive Functions

ISF extends GLSL with the following functions.

```
vec4 pixelColor = IMG_PIXEL(image imageName, vec2 pixelCoord); 
vec4 pixelColor = IMG_NORM_PIXEL(image imageName, vec2 normalizedPixelCoord);
vec4 pixelColor = IMG_THIS_PIXEL(image imageName); 
vec4 pixelColor = IMG_NORM_THIS_PIXEL(image imageName); 
vec2 imageSize = IMG_SIZE(image imageName);
```

- `IMG_PIXEL()` and `IMG_NORM_PIXEL()` fetch the color of a pixel in an image using either pixel-based coords or normalized coords, respectively, and should be used *instead of* `texture2D()` or `texture2DRect()`. In both functions, "imageName" refers to the variable name of the image you want to work with.
- `IMG_THIS_PIXEL()` is essentially the same as `IMG_PIXEL()` but automatically fills in the pixel coordinate for the pixel being rendered.
- `IMG_NORM_THIS_PIXEL()` is essentially the same as `IMG_THIS_PIXEL()` but automatically fills in the pixel coordinate for the pixel being rendered using a normalized coordinate range.
- `IMG_SIZE()` returns a two-element vector describing the size of the image in pixels.

## Standard GLSL Functions

This is a reference for many of the commonly used built-in functions from the OpenGL Shading Language, aka GLSL.

### Basic Number Operations

For each of these functions, you can use float input, or as vec2, vec3, or vec4 to perform the operations on multiple values at once. In most cases these functions take a single input parameter (x) and where specified take two parameters (x and y) For example:
```
float abs(float x)  
vec2 abs(vec2 x)  
vec3 abs(vec3 x)  
vec4 abs(vec4 x)
```
or
```
float mod(float x, float y)  
vec2 mod(vec2 x, vec2 y)  
vec3 mod(vec3 x, vec3 y)  
vec4 mod(vec4 x, vec4 y)
```

- `pow`: The abs function returns the absolute value of x, i.e. x when x is positive or zero and -x for negative x.
- `sign`: The sign function returns 1.0 when x is positive, 0.0 when x is zero and -1.0 when x is negative.
- `floor`: The floor function returns the largest integer number that is smaller or equal to x. The return value is of type floating scalar or float vector although the result of the operation is an integer.
- `ceil`: The ceiling function returns the smallest number that is larger or equal to x. The return value is of type floating scalar or float vector although the result of the operation is an integer.
- `fract`: The fract function returns the fractional part of x, i.e. x minus floor(x).
- `min`: The min function returns the smaller of the two arguments, x and y.
- `max`: The max function returns the larger of the two arguments, x and y.
- `mod`: The mod, short for Modulo, function returns x minus the product of y and floor(x/y).

There are also a variations of the `min`, `max` and `mod` functions where the second parameter is always a floating scalar. For example,
```
float mod(float x, float y)  
vec2 mod(vec2 x, float y)  
vec3 mod(vec3 x, float y)  
vec4 mod(vec4 x, float y)
```

### Angle and Trigonometry

For each of these functions, you can use float input, or as vec2, vec3, or vec4 to perform the operations on multiple values at once. For example:
```
float radians(float degrees)  
vec2 radians(vec2 degrees)  
vec3 radians(vec3 degrees)  
vec4 radians(vec4 degrees)
```

- `radians`: The radians function converts degrees to radians.
- `degrees`: The degrees function converts radians to degrees.
- `sin`: The sin function returns the sine of an angle in radians. The input parameter can be a floating scalar or a float vector. In case of a float vector the sine is calculated separately for every component.
- `cos`: The cos function returns the cosine of an angle in radians.
- `tan`: The tan function returns the tangent of an angle in radians
- `asin`: The asin function returns the arcsine of an angle in radians. It is the inverse function of sine.
- `acos`: The acos function returns the arccosine of an angle in radians. It is the inverse function of cosine.
- `atan`: The atan function returns the arctangent of an angle in radians. It is the inverse function of tangent.

There is also a two-argument variation of the atan function. For a point with Cartesian coordinates (x, y) the function returns the angle θ of the same point with polar coordinates (r, θ).

```
float atan(float y, float x)  
vec2 atan(vec2 y, vec2 x)  
vec3 atan(vec3 y, vec3 x)  
vec4 atan(vec4 y, vec4 x)
```

### Exponential

For each of these functions, you can use float input, or as vec2, vec3, or vec4 to perform the operations on multiple values at once. For example:
```
float sqrt(float x)  
vec2 sqrt(vec2 x)  
vec3 sqrt(vec3 x)  
vec4 sqrt(vec4 x)
```
or
```
float pow(float x, float y)  
vec2 pow(vec2 x, vec2 y)  
vec3 pow(vec3 x, vec3 y)  
vec4 pow(vec4 x, vec4 y)
```

- `pow`: The power function returns x raised to the power of y.
- `exp`: The exp function returns the constant e raised to the power of x.
- `log`: The log function returns the power to which the constant e has to be raised to produce x, also known as the natural logarithm function.
- `exp2`: The exp2 function returns 2 raised to the power of x.
Exponential function (base 2)
- `log2`: The log2 function returns the power to which 2 has to be raised to produce x.
- `sqrt`: The sqrt function returns the square root of x.
- `inversesqrt`: The inversesqrt function returns the inverse square root of x, i.e. the reciprocal of the square root.

### Clamping and Interpolation

GLSL provides several useful functions for clamping and interpolating between values.  Many of these functions can be used with a variety of different input parameter arrangements.

`clamp`: The clamp function returns x if it is larger than minVal and smaller than maxVal. In case x is smaller than minVal, minVal is returned. If x is larger than maxVal, maxVal is returned.
```
float clamp(float x, float minVal, float maxVal)  
vec2 clamp(vec2 x, vec2 minVal, vec2 maxVal)  
vec3 clamp(vec3 x, vec3 minVal, vec3 maxVal)  
vec4 clamp(vec4 x, vec4 minVal, vec4 maxVal)
```

There is also a variation of the `clamp` function where the second and third parameters are always a floating scalars:
```
float clamp(float x, float minVal, float maxVal)  
vec2 clamp(vec2 x, float minVal, float maxVal)  
vec3 clamp(vec3 x, float minVal, float maxVal)  
vec4 clamp(vec4 x, float minVal, float maxVal)
```

`mix`: The mix function returns the linear blend of x and y, i.e. the product of x and (1 - a) plus the product of y and a.
```
float mix(float x, float y, float a)  
vec2 mix(vec2 x, vec2 y, vec2 a)  
vec3 mix(vec3 x, vec3 y, vec3 a)  
vec4 mix(vec4 x, vec4 y, vec4 a)
```

There is also a variation of the `mix` function where the third parameter is always a floating scalar.
```
float mix(float x, float y, float a)  
vec2 mix(vec2 x, vec2 y, float a)  
vec3 mix(vec3 x, vec3 y, float a)  
vec4 mix(vec4 x, vec4 y, float a)
```

`step`: The step function returns 0.0 if x is smaller then edge and otherwise 1.0.
```
float step(float edge, float x)  
vec2 step(vec2 edge, vec2 x)  
vec3 step(vec3 edge, vec3 x)  
vec4 step(vec4 edge, vec4 x)
```

There is also a variation of the step function where the edge parameter is always a floating scalar:
```
float step(float edge, float x)  
vec2 step(float edge, vec2 x)  
vec3 step(float edge, vec3 x)  
vec4 step(float edge, vec4 x)
```

`smoothstep`: The smoothstep function returns 0.0 if x is smaller then edge0 and 1.0 if x is larger than edge1. Otherwise the return value is interpolated between 0.0 and 1.0 using Hermite polynomials.
```
float smoothstep(float edge0, float edge1, float x)  
vec2 smoothstep(vec2 edge0, vec2 edge1, vec2 x)  
vec3 smoothstep(vec3 edge0, vec3 edge1, vec3 x)  
vec4 smoothstep(vec4 edge0, vec4 edge1, vec4 x)
```

There is also a variation of the smoothstep function where the edge0 and edge1 parameters are always floating scalars:
```
float smoothstep(float edge0, float edge1, float x)  
vec2 smoothstep(float edge0, float edge1, vec2 x)  
vec3 smoothstep(float edge0, float edge1, vec3 x)  
vec4 smoothstep(float edge0, float edge1, vec4 x)
```

### Geometry

Unless otherwise specified these functions take the following form where applicable:
```
float length(float x)  
float length(vec2 x)  
float length(vec3 x)  
float length(vec4 x)
```
or
```
float dot(float x, float y)  
float dot(vec2 x, vec2 y)  
float dot(vec3 x, vec3 y)  
float dot(vec4 x, vec4 y)
```

- `length`: The length function returns the length of a vector defined by the Euclidean norm, i.e. the square root of the sum of the squared components.
- `distance`: The distance function returns the distance between two points. The distance of two points is the length of the vector d = p0 - p1, that starts at p1 and points to p0.
- `normalize`: The normalize function returns a vector with length 1.0 that is parallel to x, i.e. x divided by its length.
- `dot`: The dot function returns the dot product of the two input parameters, i.e. the sum of the component-wise products. If x and y are the same the square root of the dot product is equivalent to the length of the vector.
- `cross`: The cross function returns the cross product of the two input parameters, i.e. a vector that is perpendicular to the plane containing x and y and has a magnitude that is equal to the area of the parallelogram that x and y span.  The cross product is equivalent to the product of the length of the vectors times the sinus of the(smaller) angle between x and y.  The cross function will only take a pair of vec3 variables as input parameters and will always return a vec3 as a result.
	```
	vec3 cross(vec3 x, vec3 y)
	```
- `faceforward`: The faceforward function returns a vector that points in the same direction as a reference vector. The function has three input parameters of the type floating scalar or float vector: N, the vector to orient, I, the incident vector, and Nref, the reference vector. If the dot product of I and Nref is smaller than zero the return value is N. Otherwise -N is returned.
	```
	float faceforward(float N, float I, float Nref)  
	vec2 faceforward(vec2 N, vec2 I, vec2 Nref)  
	vec3 faceforward(vec3 N, vec3 I, vec3 Nref)  
	vec4 faceforward(vec4 N, vec4 I, vec4 Nref)
	```
- `reflect`: The reflect function returns a vector that points in the direction of reflection. The function has two input parameters of the type floating scalar or float vector: I, the incident vector, and N, the normal vector of the reflecting surface. Side note: To obtain the desired result the vector N has to be normalized.
	```
	float reflect(float I, float N)  
	vec2 reflect(vec2 I, vec2 N)  
	vec3 reflect(vec3 I, vec3 N)  
	vec4 reflect(vec4 I, vec4 N)
	```
- `refract`: The refract function returns a vector that points in the direction of refraction.  The function has two input parameters of the type floating scalar or float vector and one input parameter of the type floating scalar: I, the incident vector, N, the normal vector of the refracting surface, and eta, the ratio of indices of refraction.  To obtain the desired result the vectors I and N have to be normalized.
	```
	float refract(float I, float N, float eta)  
	vec2 refract(vec2 I, vec2 N, float eta)  
	vec3 refract(vec3 I, vec3 N, float eta)  
	vec4 refract(vec4 I, vec4 N, float eta)
	```

### Vector Logic Comparisons

Unless otherwise specified, these functions all work on both floating and integer vector inputs these two forms:
```
bvec2 lessThan(vec2 x, vec2 y)  
bvec3 lessThan(vec3 x, vec3 y)    
bvec4 lessThan(vec4 x, vec4 y)  
```
and
```
bvec2 lessThan(ivec2 x, ivec2 y)  
bvec3 lessThan(ivec3 x, ivec3 y)  
bvec4 lessThan(ivec4 x, ivec4 y)
```

- `lessThan`: The lessThan function returns a boolean vector as result of a component-wise comparison in the form of x[i] < y[i].
- `lessThanEqual`: The lessThan function returns a boolean vector as result of a component-wise comparison in the form of x[i] <= y[i].
- `greaterThan`: The lessThan function returns a boolean vector as result of a component-wise comparison in the form of x[i] > y[i].
- `greaterThanEqual`: The lessThan function returns a boolean vector as result of a component-wise comparison in the form of x[i] >= y[i].
- `equal`: The lessThan function returns a boolean vector as result of a component-wise comparison in the form of x[i] == y[i].
- `notEqual`: The lessThan function returns a boolean vector as result of a component-wise comparison in the form of x[i] != y[i].
- `any`: The any function returns a boolean value as result of the evaluation whether any component of the input vector is TRUE.
	```
	bool any(bvec2 x)  
	bool any(bvec3 x)  
	bool any(bvec4 x)
	```
- `all`: The any function returns a boolean value as result of the evaluation whether all component of the input vector is TRUE.
	```
	bool all(bvec2 x)  
	bool all(bvec3 x)  
	bool all(bvec4 x)
	```