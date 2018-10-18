---
title: The Anatomy of an ISF
tags: [Primer, Introduction]
keywords: getting_started
last_updated: June 12, 2018
summary: "A closer look at how to make an ISF composition."
sidebar: home_sidebar
permalink: primer_chapter_2.html
folder: primer
---

# The anatomy of an ISF

As we discussed in chapter 1, ISF is built on top of GLSL, and as such uses the same file types.  For the first few chapters we'll be dealing with Fragment Shaders that have a '.fs' as a  file extension.  In most cases for generating and processing video you'll only need a fragment shader, however in future chapters we'll also look at another type known as Vertex Shaders (.vs) and how they can be used.

Here we will look at:
- The JSON and GLSL portions of an ISF composition.
- How to create a shader that generates the same fixed color for every pixel.
- How to create a shader that generates the same variable color for every pixel.
- How to create a shader that generates a linear gradient between two colors.
- The basics of creating an image filter with ISF.
- Creating an image filter shader that inverts the colors of an image.
- Creating an image filter that moves pixels around, animated over time.
- How to include comments in your code to make it easier to understand.

## JSON and GLSL

Each .fs file that meets the ISF specification is broken up into two sections.  At the top is a JSON blob that contains information used by host applications.  Below that is the GLSL code that is compiled and executed when the plugin is loaded.

All of the code examples in this book that include both a JSON blob and a main() {} function can directly be pasted into a text / code editor, saved as a file with a '.fs' file extension and loaded into software as a generator or filter.

### JSON

JSON, or JavaScript Object Notation is an open-standard file format that uses human-readable text to transmit data objects consisting of attribute–value pairs and array data types.  That's a fancy way of saying that is it a way to write information that is easy for both humans and computers to work with.

An example of a the JSON blob from the 'Test-Float.fs' file:

```
/*{
	"DESCRIPTION": "demonstrates the use of float-type inputs",
	"CREDIT": "by zoidberg",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"TEST-GLSL FX"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "level",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		}
	]
}*/
```

As you can see, this text is written in such a way that a human can follow along but also contains enough rigid structure for a computer to parse it.  Within the JSON there are several attributes that each pair with a value that can be used by a host application. 

Some of these attributes, such as DESCRIPTION, CREDIT and CATEGORIES, are meta-data tags that can be useful for organizing, searching or describing individual shaders.  Other attributes, such as INPUTS and ISFVSN contain information that help describe the code section below.

### GLSL

Recalling our discussion from chapter 1, GLSL is also commonly known as the "OpenGL Shading Language" and it has what programmers like to call a C-like syntax because it resembles the C programming language.

With fragment shaders each pixel for the output is preloaded with the same same `main() {}` function and its behavior can vary depending on its coordinate position and other input variables that are specific to the individual pixel.

{% include image.html file="/primer/2/primer_2_pixels.jpg" alt="Rendering a 8x8 pixel grid" caption="Fragment Shaders render the same main() {} function for each pixel" %}


This is often a different way of thinking about graphical programming or visual design than many people are accustomed to, but makes sense when you consider the parallel processing power of the GPU.  Rather than write a complex program that needs to step through each coordinate, these smaller micro-programs can be executed on each individual pixel at the same time.

Within a fragment shader meeting the ISF specification, GLSL code is placed in the bottom section of the document.  For the most part, standard shader code can be used, however there are a few small differences with a few functions to help with cross-platform compatibility.

An example of a the GLSL from the 'Test-Float.fs' file:

```
void main()
{
	vec4		srcPixel = IMG_THIS_PIXEL(inputImage);
	float		luma = (srcPixel.r+srcPixel.g+srcPixel.b)/3.0;
	vec4		dstPixel = (luma>level) ? srcPixel : vec4(0,0,0,1);
	gl_FragColor = dstPixel;
}
```

Here we can see the basics of an image filter where pixels below a certain brightness level are set to black.  Let's look a little closer at the lines of code involved here:

- void main() - This declares the main function; each shader must have this, as it is called from the host application when the shader is rendered.
- vec4 srcPixel = IMG_THIS_PIXEL(inputImage); - This creates a variable of type vec4 (a vector containing four floating point numbers) and uses the IMG_THIS_PIXEL function to get the color for the pixel that this code will execute on.
- float luma = (srcPixel.r+srcPixel.g+srcPixel.b)/3.0; - Calculates the brightness of the RGB channels of the pixels, ignoring the alpha channel.
- vec4		dstPixel = (luma>level) ? srcPixel : vec4(0,0,0,1); - Compares the brightness of the pixel to the level specified by the input variable in the JSON blob.
- gl_FragColor = dstPixel; - Sets the final color of the pixel to the result from the previous line of code. Each shader must set gl_FragColor to some color value as part of its main function in order to output.

As you may have noted, two parts of our JSON blob showed up here in the GLSL section, the 'INPUTS' section describes two elements called 'inputImage' and 'level' that are now available as variables in the code.  Going through the attributes we can see that 'level' has a DEFAULT value of 0.5, a MIN value of 0.0 and a MAX value of 1.0 for a variable of type float.

## Generating Images

Now that we've gotten a basic look at how an ISF is put together, we can start on writing our first basic shader.

### Pick A Color

The most basic ISF example would be something that returns a single color for each pixel.  It might look something like this.

```
/*{
	"DESCRIPTION": "Every pixel is orange",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"TEST-GLSL FX"
	]
}*/

void main() {
	gl_FragColor = vec4(1.0,0.5,0.0,1.0);
}
```

### Add an input

Now let's expand this shaders so that instead of rendering a single fixed value we can set the render color based on one provided from a host application.

The ISF specification supports several different types of INPUTS, including image, float, bool, long, color, event, audio, audioFFT and point.  Here we'll be adding in a "color" variable which contains an RGBA value.

```
/*{
	"DESCRIPTION": "Demonstrates the use of color-type image inputs",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"TEST-GLSL FX"
	],
	"INPUTS": [
		{
			"NAME": "theColor",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.5,
				0.0,
				1.0
			]
		}
	]
}*/

void main()
{
	gl_FragColor = theColor;
}
```

Just like with our other variables declared in the JSON blob in previous examples, "theColor" can be used in our GLSL code anywhere we'd use a vec4 we declared locally.

### Varying based on position

So far we've examined how to set each pixel in a generator shader to the exact same color.  What if we wanted them to be different based on some criteria?  This is where the "isf_FragNormCoord" and "gl_FragCoord.xy" variables come into play.  Each is a vec2 (a vector that contains two floating point numbers) that contains the xy coordinate of the pixel being rendered.

- gl_FragCoord.xy is a standard variable in GLSL shaders and represents the specific integer pixel coordinates of current the pixel.
- isf_FragNormCoord is an extension for ISF that contains a normalized (ranged 0 to 1) version of the current pixel coordinate.
- RENDERSIZE is another extension for ISF that contains the overall pixel dimensions for the output being rendered.  This tells you the range for the gl_FragCoord.xy variable.

Let's look how we can create a shader that fades between two colors, varying over the x position (left to right) of the image.

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

Here there isn't much new in our JSON blob other than we've added a second color variable.  In the GLSL code there are two important details to check out:
- We've introduced 'mix' as our first function other than the 'main' function.  The mix function can take two colors and a floating point number from 0.0 to 1.0 as inputs and returns a color that somewhere in between the provided colors based on the provided mix percentage.  GLSL provides many useful [built-in functions](ref_functions) and you can create your own re-usable functions which can be useful for writing concise code.
- As the position of the mix percentage we've entered 'isf_FragNormCoord.x' which tells the shader to use the normalized x position for the current position.  As mentioned above, the normalized value goes from 0.0 on the left to 1.0 on the right side of the texture frame.  To have this displayed as a vertical gradient, you can change this variable to 'isf_FragNormCoord.y'.


## Processing images

So far we've seen how to use ISF to generate totally new images by returning the desired color for each individual output pixel.  The other big use case for ISF compositions is to take existing pixel color information, process it in some way and then return the result.

### Changing colors

For our first example we'll examine one of the most basic standard FX, Color Invert.fs, which takes an input pixel and inverts the rgb channels while leaving the alpha channel intact.

```
/*{
	"DESCRIPTION": "Inverts each pixel",
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

void main() {
	vec4	srcPixel = IMG_THIS_PIXEL(inputImage);
	gl_FragColor = vec4(1.0-srcPixel.r, 1.0-srcPixel.g, 1.0-srcPixel.b, srcPixel.a);
}
```

Make a special note of how here we have one object in our 'INPUTS' array called "inputImage" that is of type "image" – this is an important detail in the ISF specification that tells host applications that this file is meant to be used as an 'FX' instead of a generator.

In the code section below the JSON blob, the "inputImage" variable is used with a special function called 'IMG_THIS_PIXEL' which returns the color for the corresponding pixel that is being processed.  This value is stored in a vec4 called 'srcPixel'.

If you are already familiar with GLSL, the function 'IMG_THIS_PIXEL' is a one of several functions available in ISF to be used instead of texture2D() and texture2DRect().  For those who are curious, this is further explained in the ISF Specification document.

For the final line of code, gl_FragColor is set to a new color value that is created by subtracting each RGB color channel from 1.0, and leaving the alpha channel intact.  We could also write this final line of code a different way and get the same result:

```
gl_FragColor = vec4(1.0-srcPixel.rgb,srcPixel.a);
```

### Moving pixels

Another common usage for image processing is changing the positions of pixels.  Like with our previous example of creating a color gradient, for in this shader we will make use of the automatically provided 'isf_FragNormCoord' variable to get the location of the pixel being processed.

```
/*{
	"DESCRIPTION": "Shift pixels to the left",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
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

void main() {
	vec2	loc = isf_FragNormCoord;
	float	shift = TIME;
	loc.x += shift;
	loc.x = mod(loc.x,1.0);
	gl_FragColor = IMG_NORM_PIXEL(inputImage,loc);
}
```

As before we have an "image" titled "inputImage" as part of the 'INPUTS' array.

To create the shift effect, the isf_FragNormCoord is first stored in a local vec2 variable called "loc" which is then adjusted by adding the amount of shift and applying the modulus function to create a wrap at the edge.  Try commenting out the line with the 'mod' call to see the difference.

You may have also noted that we've introduced 'TIME' as a variable in the GLSL code without declaring it in our JSON blob!  Normally this would not work, however 'TIME' is one of a few special variables available in ISF that are automatically available if needed.  In this case the variable always contains the current time, in seconds, since the ISF has begun rendering.  We will look at the other automatic variables in ISF in the next chapter.

Finally we use the 'IMG_NORM_PIXEL' function to get the color of the pixel at the desired location and return it instead of the original pixel.  Since no further processing will be applied there is no need to store this result in its own vec4, we can use it to directly set the gl_FragColor.

If you've been working with C-syntax for some time, you may have noted that we could have written this same code more concisely like this:

```
void main() {
	gl_FragColor = IMG_NORM_PIXEL(inputImage,vec2(mod(isf_FragNormCoord.x+TIME,1.0),isf_FragNormCoord.y));
}
```

However it is not as easy to read for a tutorial lesson like this.  As you write more code, it will be up to you as to whether or not you prefer to optimize your code for legibility by humans or conciseness.

## Commenting Code

When writing code, whether for your own personal use or for sharing with others who may want to learn from it, GLSL allows for including lines of text called 'comments' that are ignored when it comes time to run the shader.

In GLSL comments come in two syntaxes that are common to many other languages.
- On any line, any text after a `//` will be ignored.
- Any text that is between an opening `/*` and a closing `*/` pair will be ignored.

Note that you can put comments before, after, or directly within your code.

Though comments are not required, it is a generally encouraged practice that you may want to include as part of your workflow.