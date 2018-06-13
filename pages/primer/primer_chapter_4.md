---
title: Data Types, Standard Variables and Functions
tags: [Primer, Introduction]
keywords: getting_started
last_updated: June 12, 2018
summary: "A closer look at the data types and functions available in GLSL and ISF."
sidebar: home_sidebar
permalink: primer_chapter_4.html
folder: primer
---

# Data Types, Standard Variables and Functions

As a language, GLSL is made up of several different building blocks that make up its foundation.  The four that we'll be investigating in this chapter are:
- Data Types
- Variables
- Functions
- Statements

Each of these concepts work together to create the foundation for most of the GLSL code that you will encounter and write yourself.  Most the topics in this chapter focus on the core language itself, but where specified the material is specific to writing shaders in ISF.

## Data Types

So far in this guide we have used a few different basic types of variables to hold pieces of data within our shaders, such as 'float', 'vec2', 'vec3', and 'vec4'.  These are a few of the available data types that are available within GLSL.

There are two main categories of data types in GLSL that we are going to look at in this chapter: Scalar and Vector.

### Scalar Data

Scalar types represent a single number within a particular set of numbers:
- `bool` is short for boolean.  These values represent on / off states, 1 / 0, and have the state 'true' or 'false'
- `int`, is short for integer, and holds a number such as -3, 0, 1, or 7.
- `long`, is another form of integer, and likewise holds a number such as -3, 0, 1, or 7.
- `float`, is short for floating point number, which are used to hold decimal numbers such as -2.718, 0.0, 3.14 or 12.34.

Here is an example of GLSL code that makes use of each type of scalar variable:
```
void main() {
	int		myInt = 3;
	float	myFloat = 0.1;
	bool	myBool = true;
	float	alpha = 0.0;
	if (myBool == true)	{
		int i;
		for (i = 0;i < myInt;++i)	{
			alpha += myFloat;
		}
	}
	gl_FragColor = vec4(1.0,0.5,0.0,alpha);
}
```

### Vector Data

Vector types are compound types that build on the basic scalar types.  Though these types each have many common usages, you can use them anytime you need to group together several scalars in a single variable name.  Most of the time you'll be using the compound types for holding multiple floating point values together and they take the following form:
- `vec2` holds 2 floating point values.  Commonly used for holding 2D coordinates.
- `vec3` holds 3 floating point values.  Commonly used for holding 3D point coordinates or RGB color information.
- `vec4` holds 4 floating point values.  Commonly used for holding 4D coordinates, rectangular positions, or RGBA values.

To access the individual scalars that each vector is made up from, GLSL uses a '.' syntax in the following fashion: 'variablename'.'scalar variable name'
- `vec2` has '.x' and '.y' sub-values.
- `vec3` has '.x', '.y', and '.z' sub-values.  You can also interchange each of these respectively with '.r', '.g.', and '.b'.
- `vec4` has '.x', '.y', '.z' and '.w' sub-values.  You can also interchange each of these respectively with '.r', '.g.', '.b', and 'a'.

Here is an example of using some of these vector types together:
```
void main() {
	float	myX = 0.25;
	float	myY = 0.5;
	vec2	myPoint = vec2(myX, myY);
	myPoint += 0.1;
	myX = myPoint.x;
	myY = myPoint.y;
	vec3	myColor = vec3(myX,myY,0.5);
	gl_FragColor = vec4(myColor,1.0);
}
```

First in this code we create two scalar float variables called myX and myY and set to them to 0.25 and 0.5 respectively.  We then create a vector variable called myPoint and use the vec2() function to turn these two scalar values into a single vec2 value.

With the line "myPoint += 0.1;" the shader will add 0.1 to both the x and y properties for the myPoint variable.  If instead you enter "myPoint.x += 0.1;" or "myPoint.y += 0.1;" only the specified part of the vector will change.

Next we create a new variable called myColor and use the vec3() function to turn three scalar values into a single vec3 value.  Note that we can pass both variables and fixed numbers into these functions at the same time.

In the final line of code, a vec4() function is used to create a color to set gl_FragColor for this pixel.  Here we make use of a handy feature in GLSL which allows us to create a vec4 by passing in a vec3 and extending it with additional values.

### Into the void

There is one other data type that is worth mentioning called 'void'.  Technically void is a scalar, but unlike the other scalar types, it does not hold a value.  It is used in places where functions are called but do not return any value.  You may have already noticed that we are including 'void' in every shader we've written so far as part of the 'main' function.   We'll discuss how this special type gets used when discussing functions later in this section.

## Uniform, Varying and Local Variables

Regardless of the data type, depending on where in your ISF file a variable is declared effects what we call the "scope" of the value.  This is a concept common to most programming languages and GLSL is similar to C in this respect.

Typically in GLSL there are three main types of variable scopes that you'll encounter:
- 'Uniform' variables are used to communicate with your shader from "outside" making this a fancy way of saying that these values are the "inputs" to your plugin.  In your shader use the uniform qualifier to declare the variable.  These values are read-only and can be accessed anywhere in your shader code.  
- 'Varying' variables provide an interface between Vertex and Fragment Shader.  We'll look at these in more detail with vertex shaders in a later chapter.
- 'Local' variables are created inside of functions and can only be used within that function.  

Declaring variables in your ISF will typically happen in three places:
- In the JSON blob, where parameters that are global to composition and available to be accessed are declared in the 'INPUTS' array.  For each element of the array a 'uniform' variable of the desired type is automatically created for your shader from the given description.
- You can declare additional variables that as uniforms or constant values in the area above your main function.  Though these values won't be published to the host application, they can be accessed from your shader code.
- Within individual functions local variables are declared as needed.

Let's take the Twirl.fs FX as an example:

```
/*{
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Distortion Effect"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "radius",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 5.0
		},
		{
			"NAME": "amount",
			"TYPE": "float",
			"MIN": -10.0,
			"MAX": 10.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "center",
			"TYPE": "point2D",
			"DEFAULT": [
				0.5,
				0.5
			]
		}
	]
}*/


const float pi = 3.14159265359;


void main (void)
{
	vec2 uv = isf_FragNormCoord;
	vec2 texSize = RENDERSIZE;
	vec2 tc = uv * texSize;
	float radius_sized = radius * max(RENDERSIZE.x,RENDERSIZE.y);
	tc -= center;
	float dist = length(tc);
	if (dist < radius_sized) 	{
		float percent = (radius_sized - dist) / radius_sized;
		float theta = percent * percent * amount * 2.0 * pi;
		float s = sin(theta);
		float c = cos(theta);
		tc = vec2(dot(tc, vec2(c, -s)), dot(tc, vec2(s, c)));
	}
	tc += center;
	vec2 loc = tc / texSize;
	vec4 color = IMG_NORM_PIXEL(inputImage, loc);

	if ((loc.x < 0.0)||(loc.y < 0.0)||(loc.x > 1.0)||(loc.y > 1.0))	{
		gl_FragColor = vec4(0.0);
	}
	else	{
		gl_FragColor = color;
	}
}
```

Here we can see variables being declared in three different places.
- In the JSON blob the 'uniform' variables titled "radius", "amount", and "center" are declared.  These will translate into float, float and vec2 variables respectively with the same names.
- A 'const float' called "pi" is declared outside of any functions and is therefore globally accessible by any function.
- Within the main() function several variables are declared and used locally.  When using if() statements, the scope of any variables declared is limited to within its {}s.

### Uniform Variables in ISF

As we mentioned 'Uniform' variables are a special kind of variable used to communicate with your shader from the "outside".  In the case of ISF this would mean any host application that you used to load the generator or FX, such as VDMX or Mad Mapper which would create control interfaces for each element, or as part of a webpage that adjusts published parameters through javascript functions.

If you come from a background of writing GLSL shaders for specific environments, you may already be familiar with the concept of 'Uniform' variables and how they are declared in your code.  When writing for ISF, the only minor difference is that you specify these variables as part of the 'INPUTS' section of your JSON blob and the host application will automatically fill in the appropriate code for you when the shader is loaded.

Within the description for each element in the 'INPUTS' array is an attribute called 'TYPE' that tells the host application what data type should be created for the shader.  The available options are:
- `float`: Creates a 'float' variable.
- `bool`: Creates a 'bool' variable.
- `event`: Creates a 'bool' variable that is set to true for a single render pass when the event is triggered.
- `long`: Creates a 'long' variable. Typically used to create pop-up menu interfaces.
- `color`: Creates a 'vec4' that contains an RGBA value
- `point2D`: Creates a 'vec2' that is typically populated by a coordinate value

As noted, there are three other special input types that do not correspond to standard GLSl scalar / vector data types.  These are “image”, “audio”, and “audioFFT” which pass in data as OpenGL textures whose values accessed by using special functions.

You can find additional detailed information about each of the ISF types on the specification page.

## Adding Custom Functions

While in the course of writing fragment shaders, you may find that certain chunks of code, for whatever reason, might be nicely abstracted into its own re-usable snippit that could easily be referenced instead of rewriting the whole thing each time.  Many programming languages include a concept of functions for just this purpose.  By encapsulating parts of your code in different functions, you can make your shaders both more concise and more human readable.

The process of adding your own custom functions is easy and you've already gotten some experience with the basic idea from using the main() function.

Each function you declare in your shader will look something like this:

```
'data type that gets returned' 'name of the function' ('variables passed into the function')
{
	//	some code
	return 'data type that gets returned'
}
```

And of course it may help to see a few actual examples:

```
//	determines the grayscale value for the pixel by averaging the RGB channels
float grayscaleAmount(vec4 color)
{
	//	ignores alpha
	return (color.r + color.g + color.b) / 3.0;
}

//	converts a color from RGB to HSV color space
vec3 hsv2rgb(vec3 c)	{
	//	note how we can use .rgba and .xyzw interchangably
	//	and how though vec3 commonly holds RGB or XYZ data,
	//	it can represent any 3 float values...
	vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
```

When discussing data types, there was a special type known as 'void' which is reserved for functions that do not 'return' anything at the end.  Here is an example function that changes the state of the variable that is passed in:

```
//	flip the x and y values
void flipValues(vec2 point)
{
	vec2	tmp = point;
	point.x = tmp.y;
	point.y = tmp.x;
}
```

Functions can be placed either before or after your "void main(){...}" section of the fragment shader, however if you wish to put the code after, you must also include function declarations before main.

For example, each of these are essentially equivalent:

1.
```
/*{
	"DESCRIPTION": "Grayscale each pixel",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Color Effect"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}
	]
}*/

//	declaring the function and including the code before main...
float grayscaleAmount(vec4 color)
{
	//	ignores alpha
	return (color.r + color.g + color.b) / 3.0;
}

void main() {
	vec4	srcPixel = IMG_THIS_PIXEL(inputImage);
	float	grayLevel = grayscaleAmount(srcPixel);
	gl_FragColor = vec4(grayLevel,grayLevel,grayLevel,srcPixel.a);
}
```

2.
```
/*{
	"DESCRIPTION": "Grayscale each pixel",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Color Effect"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}
	]
}*/

//	declaring the function, but not actually writing the code here...
float grayscaleAmount(vec4 color);

void main() {
	vec4	srcPixel = IMG_THIS_PIXEL(inputImage);
	float	grayLevel = grayscaleAmount(srcPixel);
	gl_FragColor = vec4(grayLevel,grayLevel,grayLevel,srcPixel.a);
}

//	actual code for the function after main()...
float grayscaleAmount(vec4 color)
{
	//	ignores alpha
	return (color.r + color.g + color.b) / 3.0;
}
```

It is up to you to decide which style works the best for your own needs and you may find examples of either being used in sample code from other people.

## Built-In Variables and Constants

Along with the variables that you explicitly declare within the JSON and code portions of your composition, GLSL and ISF each provide several default variables and constants that you can access.

You can also find this list the on [ISF and GLSL Variables and Uniforms Reference Page](ref_variables) for quick lookup.

- vec4 gl_FragCoord;  is automatically declared, hold the values of the fragment coordinate vector are given in the window coordinate system.  In 2D space the .xy from this can be used to get the non-normalized pixel location.
- vec2 isf_FragNormCoord is automatically declared. This is a convenience variable, and repesents the normalized coordinates of the current fragment ([0,0] is the bottom-left, [1,1] is the top-right).
- int PASSINDEX;  is automatically declared, and set to 0 on the first rendering pass. Subsequent passes increment this int.
- vec2 RENDERSIZE;  is automatically declared, and is set to the rendering size (in pixels) of the current rendering pass.
- float TIME;  is automatically declared, and is set to the current rendering time (in seconds) of the shader.  This variable is updated once per rendered frame- if a frame requires multiple rendering passes, the variable is only updated once for all the passes.
- float TIMEDELTA;  is automatically declared, and is set to the time (in seconds) that have elapsed since the last frame was rendered. This value will be 0.0 when rendering the first frame.
- vec4 DATE;  is automatically declared, and is used to pass the date and time to the shader. The first element of the vector is the year, the second element is the month, the - int FRAMEINDEX;  is automatically declared, and is used to pass the index of the frame being rendered to the shader- this value is 0 when the first frame is rendered, and is incremented after each frame has finished rendering.

Each of these can be referenced as read-only from within your GLSL code.

## Built-In Functions

Along with your 'main' function and any other custom functions declared in your code, GLSL and ISF each provide a set of useful built-in functions for you to take advantage of. In this section we'll look at a handful of these functions and you can find a more extensive list on the [ISF and GLSL Functions Reference Page](ref_functions).

### ISF Exclusive Functions

If you are writing shaders against the ISF specification there are a few additional functions that can be used for working with image data.

```
vec4 pixelColor = IMG_PIXEL(image imageName, vec2 pixelCoord); 
vec4 pixelColor = IMG_NORM_PIXEL(image imageName, vec2 normalizedPixelCoord);
vec4 pixelColor = IMG_THIS_PIXEL(image imageName); 
vec2 imageSize = IMG_SIZE(image imageName);
```

- `IMG_PIXEL()` and `IMG_NORM_PIXEL()` fetch the color of a pixel in an image using either pixel-based coords or normalized coords, respectively, and should be used *instead of* `texture2D()` or `texture2DRect()`. In both functions, "imageName" refers to the variable name of the image you want to work with.
- `IMG_THIS_PIXEL()` and `IMG_THIS_NORM_PIXEL()` are essentially the same as `IMG_PIXEL()` but automatically fill in the pixel coordinate for the pixel being rendered.
- `IMG_SIZE()` returns a two-element vector describing the size of the image in pixels.

```
/*{
	"DESCRIPTION": "Image get pixel functions",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"TEST-GLSL FX"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}
	]
	
}*/

void main()	{
	vec4		inputPixelColor;
	//	both of these are the same
	inputPixelColor = IMG_THIS_PIXEL(inputImage);
	inputPixelColor = IMG_PIXEL(inputImage, gl_FragCoord.xy);
	
	//	both of these are also the same
	inputPixelColor = IMG_NORM_PIXEL(inputImage, isf_FragNormCoord.xy);
	inputPixelColor = IMG_THIS_NORM_PIXEL(inputImage);
	
	gl_FragColor = inputPixelColor;
}
```

### Standard GLSL Functions

The base language of GLSL includes many useful mathematical functions for working with exponents, logarithms, trigonometry, geometry and interpolation.  Here we will look at a two very basic examples that make use of some of these functions and how they can be used in your shaders.

#### The Maximum Function

One of the commonly used math functions is `max` which is short for maximum.  You can use this function to compare two scalar values and return the one that is greater.  As you might imagine, there is also a `min` function that works in a similar fashion, only returning the smaller of the two values passed in.

While this is not the case with all functions, when looking up `max` in a reference you'll note that it is declared four different times:

```
float max(float x, float y)  
vec2 max(vec2 x, vec2 y)  
vec3 max(vec3 x, vec3 y)  
vec4 max(vec4 x, vec4 y)
```

This is because you can use `max` with four different types of inputs: float, vec2, vec3, and vec4.  When using the function on vectors, each individual component in variable x is compared to its corresponding component variable y.  In each case the return is the same as the types that were passed in.

When looking at other functions you may see all sorts of combinations of inputs and return value, likewise you can create your own functions to work with a variety of different forms of inputs and returns.

Now let's look at an actual shader that makes use of the `max` function.

```
/*
{
  "CATEGORIES" : [
    "TEST-GLSL FX"
  ],
  "DESCRIPTION" : "Performs a component-wise maximum comparison",
  "ISFVSN" : "2",
  "INPUTS" : [
	{
      "NAME" : "color1",
      "TYPE" : "color",
      "DEFAULT" : [
        0.5,
        0.5,
        0.75,
        1.0
      ]
    },
    {
      "NAME" : "color2",
      "TYPE" : "color",
      "DEFAULT" : [
        0.25,
        0.25,
        0.95,
        1.0
      ]
    }
  ],
  "CREDIT" : "by VIDVOX"
}
*/

void main()
{
	//	return a component-wise maximum
	gl_FragColor = max(color1,color2);
}
```

This shader itself doesn't do very much besides render a solid color for each pixel.  However, as you adjust values for the individual colors, note that the output color is created by using the largest red, the largest green, the largest blue and the largest alpha values when comparing the two inputs `color1` and `color2`.  If you try changing the code from using `max` to `min` you'll get the opposite behavior, with the darker of each component used for the output.

As a challenge, try adapting this shader into a filter that compares an image input to a particular color.  Here are two hints – you'll need to change something in the "INPUTS" section of the JSON blob and you'll need to add an ISF exclusive function to the GLSL code.

#### The Mix Function

You may recall from chapter 2 this example where we used the mix function to create a linear gradient.

```
/*{
	"DESCRIPTION": "Creates a linear gradient from one color to another",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"TEST-GLSL FX"
	],
	"INPUTS": [
		{
			"NAME": "theColor1",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.5,
				0.0,
				1.0
			]
		},
		{
			"NAME": "theColor2",
			"TYPE": "color",
			"DEFAULT": [
				0.0,
				0.0,
				1.0,
				1.0
			]
		}
	]
}*/

void main()
{
	gl_FragColor = mix(theColor1,theColor2,isf_FragNormCoord.x);
}
```

Now we can take a second to look a little more closely at this helpful function itself.  The mix function returns the linear blend of x and y, i.e. the product of x and (1 - a) plus the product of y and a.  When the 'a' value is 0.0, the result is x, when the 'a' value is 1.0, the result is y, and in between you'll get a linear transition between the two.

Like with the `max` function, we can use `mix` in a piecewise component fashion.

```
float mix(float x, float y, float a)  
vec2 mix(vec2 x, vec2 y, vec2 a)  
vec3 mix(vec3 x, vec3 y, vec3 a)  
vec4 mix(vec4 x, vec4 y, vec4 a)
```

There is also a variation of the `mix` function where the third parameter is always a floating scalar, which is how it is used in the linear gradient example.

```
float mix(float x, float y, float a)  
vec2 mix(vec2 x, vec2 y, float a)  
vec3 mix(vec3 x, vec3 y, float a)  
vec4 mix(vec4 x, vec4 y, float a)
```

Similar functions for interpolation include `step` and `smoothstep`, both of which you can read more about on the [ISF and GLSL Functions Reference Page](ref_functions).

#### Other functions

In future chapers we'll look at some other functions but it is also recommended that you take a quick look through the [ISF and GLSL Functions Reference Page](ref_functions) to get a sense of what else is available.

## GLSL Statements

Like most other programming languages, GLSL provides a standard set of ways to implement logic within your code.  These fall into two categories, conditionals and loops.  Conditionals make it possible to vary the route of the code execution based on the states of different variables.  Loops make it possible to write a section of code that is repeated until a certain condition is met.

### Conditionals

Within GLSL there are two main ways to add logic to your code based on the results of conditional statements.  Regardless of which method you are using, there is a key element of a comparison

#### Comparing two values

Each conditional statement eventually boils down to one of two states: `true` or `false` (1 or 0).  This is fine for when you are working with booleans, but when you are working with other scalar data types GLSL, like other languages, provides several standard ways to compare two values and return a boolean result.  These special comparisons are similar to functions, but they don't require using the function syntax we've seen before.

- `>`: returns `true` if the value on the left is greater than the value on the right.
- `>=`: returns `true` if the value on the left is greater than or equal to the value on the right.
- `<`: returns `true` if the value on the right is greater than the value on the left.
- `<=`: returns `true` if the value on the right is greater than or equal to the value on the left.
- `==`: returns `true` if the value on the left is equal to the value on the right.
- `!=`: returns `true` if the value on the left is not equal to the value on the right.

Conditionals can also be combined to create compound logic by using the following symbols:
- `||`: returns `true` if the value on the left OR the value on the right is true
- `&&`: returns `true` if the value on the left AND the value on the right are both true

Next we'll see how these comparisons are used to write our conditional statements.

#### If / Else

Though not as fast as the other option for working with logic in shaders, the if / else method is the more flexible of the two options as it can create multiple logic branches that can each contain any amount of additional code.

You can write if statements in a variety of ways, but the most general form looks something like this:
```
if (<some condition is true>)
{
	<here is some code>
}
else if (<some other condition is true>)
{
	<here is some other code>
}
...
else if (<some other condition is true>)
{
	<here is some other code>
}
else	{
	<here is the code for when none of the conditions were met>
}
```

Note that here while we can have as many `else if` sub-conditions to branch out the logic, they are not required.

Let's go back to the Twirl.fs filter discussed previously on this page.  In this shader if statements are used twice.

The first usage of if is fairly straight forward.  Read outloud this would say “if the distance is less than the radius size, do the code in these curly braces”

```
	if (dist < radius_sized) 	{
		//	do some stuff
	}
```

The if statement used at the end to determine which color is used for gl_FragColor is a little more complex:

```
	if ((loc.x < 0.0)||(loc.y < 0.0)||(loc.x > 1.0)||(loc.y > 1.0))	{
		gl_FragColor = vec4(0.0);
	}
	else	{
		gl_FragColor = color;
	}
```

The idea behind this code is that if the translated pixel falls outside of the normalized coordinate rate of (0.0, 0.0) and (1.0, 1.0), then it should return a transparent black pixel instead of the color read from the image.  Here we can see that several comparison statements have been connected together using a special pair of characters `||` which is known as the `or` operator.

You can read this outloud as “if thing 1, or thing 2, or thing 3, or thing 4 is true, do this”.  In this case only one of our conditions needs to met in order for this code to execute.

If you only learn one technique, this would be it, however if you really wish to master GLSL and write high performance shaders, it is also good to learn about the other form of conditional statements...

#### Ternary Operator

When possible, it is recommended that you use the `ternary` operator, as it is both concise and more quickly excuted by the GPU.

Ternary operator, as the name implies, has three parts, and takes the following general form:
```
val = (<some comparison statement>) ? (<option1>) : (<option2>);
```

This is essentially equivalent to writing the code:
```
if (<some comparison statement>)
	val = <option1>
else
	val = <option2>
```

Now let's look at our previous max function example, but change it up a bit so that we return the brighter color based on the overall brightness of each provided color.

```
/*
{
  "CATEGORIES" : [
    "TEST-GLSL FX"
  ],
  "DESCRIPTION" : "Returns the brighter of two colors",
  "ISFVSN" : "2",
  "INPUTS" : [
	{
      "NAME" : "color1",
      "TYPE" : "color",
      "DEFAULT" : [
        0.5,
        0.5,
        0.75,
        1.0
      ]
    },
    {
      "NAME" : "color2",
      "TYPE" : "color",
      "DEFAULT" : [
        0.25,
        0.25,
        0.95,
        1.0
      ]
    }
  ],
  "CREDIT" : "by VIDVOX"
}
*/

//	returns the average color level, using alpha
float level (vec4 c)
{
	return c.a * (c.r + c.g + c.b) / 3.0;
}

void main()
{
	//	get the level for each input color
	float	gray1 = level(color1);
	float	gray2 = level(color2);
	//	if gray1 is greater than or equal to gray2 return color1, else return color2
	gl_FragColor = (gray1 >= gray2) ? color1 : color2;
}
```

Note here that we've created a custom function called `level` which takes in a vec4 and returns a single float value.  Two scalar variables `gray1` and `gray2` are created to temporarily store the values returned by calling our custom function on each color.

Then in the final line of code our new ternary operator is to determine which color is used.  In the first part of the operator is the comparison "(gray1 >= gray2)" which is followed by a `?` symbol.  Immediately after that is the result when this statement is true, in this case `color1`.  This is followed by  a `:` and the value to be used when the statement is false, in this case `color2`.

As mentioned, humans and computers alike can read ternary operators faster than if-else statements, so you should use them whenever possible.

### Loops

Another form of useful logic to be able to include when writing code is the ability to create a `loop`, which is a section of code that will repeat until certain conditions are met.  These conditions are essentially the same kinds we dealt with for conditional statements, sometimes including a special counter variable to make a section repeat a certain number of times.

In GLSL are three main types of loops: `for`, `while`, and `do-while` which have different usages.

As an important note, remember that loops in GLSL fragment shaders are executing for every single pixel in your image.  If they include complex mathetmatical operations or are run many number of times, they can be quite costly in terms of performance.  Loops in GLSL should be used sparingly and with great care.

#### for(;;){} Loops in GLSL

When using the keyword `for` when creating a loop, it is implied that the loop is controlled by a counter variable.

```
	for (<counter variable>;<counter conditional>;<counter varying>)
	{
		//	code to be executed
	}
```

The parentheses enclose three expressions that initialize, check and update the variable used as counter.  The body defined by curly braces encloses the statements that are executed at each pass of the loop.

And example of a filled in for loop might look something like this:
```
	float	val = 0.0;
	for (int i = 0;i < 4;i++)
	{
		val += 0.1;
	}
```

Here we have a variable called `val`, and on each pass of the for loop we add 0.1 to it and the loop runs 4 times.  In first portion of the for loop you can either declare a variable (like in our example) or use this section to simply set a variable to a value.

In certain versions of GLSL the second portion of the for loop, the comparison must be against a constant value, not a variable.  If you need a for loop that executes a variable number of times, for full compatability the `break` statement can be used to exit for loops based on sub-conditions, such as comparing to a variable.  As an example:

```
	float	val = 0.0;
	int		actualMax = 3;
	for (int i = 0;i < 100;i++)
	{
		if (i > actualMax)
			break;
		val += 0.1;
		if (val >= 1.0)
			break;
	}
```

With this for loop, the statement will exit early under two different conditions, when (i > actualMax) or when (val >= 1.0).  Otherwise it will run 100 times.  Given the code example, how many times will this loop actually run?

#### while(){} Loops in GLSL

The while loop is similar to the for loop, but instead of using a counting variable it uses a custom conditional.  Typically this conditional relies on some variable inside of the loop itself that is changing.  They take the general form of:

```
	while (<while conditional>)
	{
		//	code to be executed
	}
```

And example of a filled in while loop might look something like this:
```
	float	val = 0.0;
	while (val < 1.0)
	{
		val += 0.1;
	}
```

An important note with while loops is that if the condition is not initially met, the code inside the curly braces will not execute.  Like with for loops, you can use the `break` statement to exit a while loop based on sub-conditions.

#### do {} while () Loops

The do-while loop is essentially the same as the while loop, with the minor difference that the code will execute at least once, even if the conditions within the () are not  initially met. They take the general form of:

```
	do 
	{
		//	code to be executed
	}
	while (<while conditional>);
```

And example of a filled in do-while loop might look something like this:
```
	float	val = 0.0;
	do
	{
		val += 0.1;
	}
	while (val < 1.0);
```

## End of the chapter

In this chapter we have covererd a lot of new material, both related to ISF and the language of GLSL itself.  This has included an introduction to the different data types that are available in GLSL and how to declare them in ISF's JSON blob, creating our own custom functions and finallly the concepts of conditional statements and loops which are used to add logic branches into our code.  We also examined the automatically created uniforms provided by ISF and some of the provided built-in functions which are further discussed in the [ISF Reference Pages](ref_index).

Now that we've gotten a lot of the basics out of the way we can begin to move into some of the more advanced topics and example shaders.
