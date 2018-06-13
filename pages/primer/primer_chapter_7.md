---
title: Multi-Pass and Persistent Buffers in ISF
tags: [Primer, Introduction]
keywords: getting_started
last_updated: June 12, 2018
summary: "A closer look at creating multi-pass shaders and using persistent buffers in ISF."
sidebar: home_sidebar
permalink: primer_chapter_7.html
folder: primer
---

# Multi-Pass Shaders and Persistent Buffers in ISF

Two extremely powerful concepts that ISF adds on to GLSL are the ability to retain image information between render passes (persistent buffers) and creating compound shaders that have multiple rendering stages (multi-pass shaders) at potentially varying sizes.

In this chapter we'll look at:
- How to set up a persistent buffer.
- Discuss creating feedback style effects with persistent buffers.
- How to set up multiple render passes.
- Discuss creating a deep blur using multiple render passes.
- How to make a Conway's Game of Life generator using a persistent buffer.

## Persistent Buffers

ISF files can define persistent buffers.  These buffers are images (GL textures) that stay with the ISF file for as long as it exists. This is useful if you want to "build up" an image over time- you can repeatedly query and update the contents of persistent buffers by rendering into them- or if you want to perform calculations across the entire image, storing the results somewhere for later evaluation. Further details on exactly how to do this are in the full [ISF Specification Page](https://github.com/mrRay/ISF_Spec/).

```
/*{
	"DESCRIPTION": "demonstrates the use of a persistent buffer to create a motion-blur type effect. also demonstrates the simplest use of steps: a one-step rendering pass",
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
			"NAME": "blurAmount",
			"TYPE": "float"
		}
	],
	"PASSES": [
		{
			"TARGET": "bufferVariableNameA",
			"PERSISTENT": true,
			"FLOAT": true
		}
	]
	
}*/

void main()
{
	vec4		freshPixel = IMG_THIS_PIXEL(inputImage);
	vec4		stalePixel = IMG_THIS_PIXEL(bufferVariableNameA);
	gl_FragColor = mix(freshPixel,stalePixel,blurAmount);
}
```

In this simple example we have added a new section to our JSON blob called `PASSES` which is used to describe each render pass of the shader.  Here you can set the options for each pass.
- The `TARGET` attribute is used to set the name by which this render pass will be referred to in the code section.  This will be available as an `IMAGE` type.
- For each buffer that you wish to retain between passes, the `PERSISTENT` can be set to `true`.
- If you wish to have the value stored as a 32-bit floating point value the additional `FLOAT` attribute can be included and set to `true`.  Using 32-bit textures will use up more memory, but in some cases can be extremely useful.

For this ISF we have a single render pass, that is persistent and stores floating point values.

In the code section we refer to the image `bufferVariableNameA`, which holds the output from the previous frame.

When the `PASSES` section is left out, as in our previous examples, it is presumed that the shader includes a single render pass and that the output is not stored in memory to be used later on.

### Video Feedback

One of the most common usages of persistent buffers is creating video feedback loops.  This is a process that goes back to the days of analog video and the same idea can be done digitally.  The above shader is an example of of this technique: By blending the previous pixel with the current frame, the visual effect of a feedback style motion blur is created.  Adding in additional functionality to this shader such as zooming, rotating, inverting, applying convolution kernels to blur / sharpen can create all kinds of interesting results.

Here is an example of how to modify the example to include an invert stage:

```
/*{
	"DESCRIPTION": "creates a simple inverting feedback effect",
	"CREDIT": "by VIDVOX",
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
			"NAME": "blurAmount",
			"TYPE": "float"
		}
	],
	"PASSES": [
		{
			"TARGET": "bufferVariableNameA",
			"PERSISTENT": true,
			"FLOAT": true
		}
	]
	
}*/

void main()
{
	vec4		freshPixel = IMG_THIS_PIXEL(inputImage);
	vec4		stalePixel = IMG_THIS_PIXEL(bufferVariableNameA);
	stalePixel.rgb = 1.0 - stalePixel.rgb;
	gl_FragColor = mix(freshPixel,stalePixel,blurAmount);
}
```

Here we have simply added the line that on each pass inverts the rgb of the `stalePixel`.  As a challenge, try adding the rotate or blurring sample code from previous chapters and apply them to the `stalePixel` before the `mix` function is called.

## Multi-Pass Shaders

The ISF file format defines the ability to execute a shader multiple times in the process of rendering a frame for output- each time the shader's executed (each pass), the uniform int variable `PASSINDEX` is incremented. Details on how to accomplish this are described below in the spec, but the basic process involves adding an array of dicts to the `PASSES` key in your top-level JSON dict. Each dict in the `PASSES` array describes a different rendering pass- the ISF host will automatically create buffers to render into, and those buffers (and therefore the results of those rendering passes) can be accessed like any other buffer/input image/imported image (you can render to a texture in one pass, and then read that texture back in and render something else in another pass).  The dicts in `PASSES` recognize a number of different keys to specify different properties of the rendering passes- more details are in the spec below.

```
/*{
	"DESCRIPTION": "demonstrates the use of two-pass rendering- the first pass renders to a persistent buffer which is substantially smaller than the res of the image being drawn.  the second pass renders at the default requested size and scales up the image from the first pass",
	"CREDIT": "by zoidberg",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"TEST-GLSL FX"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}
	],
	"PASSES": [
		{
			"TARGET":"bufferVariableNameA",
			"PERSISTENT": true,
			"WIDTH": "$WIDTH/16.0",
			"HEIGHT": "$HEIGHT/16.0"
		},
		{
		
		}
	]
	
}*/

void main()
{
	//	first pass: read the "inputImage"- remember, we're drawing to the persistent buffer "bufferVariableNameA" on the first pass
	if (PASSINDEX == 0)	{
		gl_FragColor = IMG_THIS_NORM_PIXEL(inputImage);
	}
	//	second pass: read from "bufferVariableNameA".  output looks chunky and low-res.
	else if (PASSINDEX == 1)	{
		gl_FragColor = IMG_THIS_NORM_PIXEL(bufferVariableNameA);
	}
}
```

Like in our previous example, we had added the new `PASSES` section to the JSON blob.  This time there are two entries – the first is persistent and has a target name, the second contains no attributes.

The first pass also has two new attributes: `WIDTH` and `HEIGHT` which can be used to resize the image before it is provided to the shader.  These attributes can be set to specific values, or you can enter in simple mathematically equations that allow them to vary depending on the actual width and height of the incoming image.  Declared uniform variables can also be used in these equations.  In this particular case the buffer will be resized to 1/16th its original width and height.

In the code section itself, the new variable `PASSINDEX` is also used.  The `PASSINDEX` is a special automatically created uniform variable that is used to tell the main() {} function which rendering pass is currently being executed.  This allows you to write compound shaders that perform different operations on each pass.

- The `PASSINDEX` starts at 0 and increments on each pass.  So the first pass is 0, the second pass is 1, the third is 2, and so on.
- Because it is a uniform, the `PASSINDEX` variable is available to both the fragment and vertex shaders.

### Creating a Multi-Pass Blur Effect

As you may have noted in the previous chapter on convolution, a basic 3x3 kernel does not blur an image very much, unless you are very zoomed in.  One way to address this was to use larger kernel sizes that average together even more neighboring pixels on each pass.  The downside of using large kernels is that they are computationally very costly.

Another way to create stronger blurring effects is to re-apply the blur multiple times in a single effect.

Like with most things in GLSL, there are several ways you can go about writing a multi-pass blur and you can find several advanced examples on the ISF Sharing Site in the Blurs category.  In this section we will look at one of the more basic examples called Soft Blur.fs which is a 3-pass blur shader.

First we will write the vertex shader.  This is exactly the same as the .vs we used for the basic convolution shaders in the previous chapter.

```
//	Soft Blur.vs
varying vec2 left_coord;
varying vec2 right_coord;
varying vec2 above_coord;
varying vec2 below_coord;

varying vec2 lefta_coord;
varying vec2 righta_coord;
varying vec2 leftb_coord;
varying vec2 rightb_coord;


void main()
{
	isf_vertShaderInit();
	vec2 texc = vec2(isf_FragNormCoord[0],isf_FragNormCoord[1]);
	vec2 d = 1.0/RENDERSIZE;

	left_coord = clamp(vec2(texc.xy + vec2(-d.x , 0)),0.0,1.0);
	right_coord = clamp(vec2(texc.xy + vec2(d.x , 0)),0.0,1.0);
	above_coord = clamp(vec2(texc.xy + vec2(0,d.y)),0.0,1.0);
	below_coord = clamp(vec2(texc.xy + vec2(0,-d.y)),0.0,1.0);

	lefta_coord = clamp(vec2(texc.xy + vec2(-d.x , d.x)),0.0,1.0);
	righta_coord = clamp(vec2(texc.xy + vec2(d.x , d.x)),0.0,1.0);
	leftb_coord = clamp(vec2(texc.xy + vec2(-d.x , -d.x)),0.0,1.0);
	rightb_coord = clamp(vec2(texc.xy + vec2(d.x , -d.x)),0.0,1.0);
}
```

Next we have the fragment shader:

```
/*{
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Blur"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "softness",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.9
		},
		{
			"NAME": "depth",
			"TYPE": "float",
			"MIN": 1.0,
			"MAX": 10.0,
			"DEFAULT": 10.0
		}
	],
	"PASSES": [
		{
			"TARGET": "smaller",
			"WIDTH": "max(floor($WIDTH*0.02),1.0)",
			"HEIGHT": "max(floor($HEIGHT*0.02),1.0)"
		},
		{
			"TARGET": "small",
			"WIDTH": "max(floor($WIDTH*0.25),1.0)",
			"HEIGHT": "max(floor($HEIGHT*0.25),1.0)"
		},
		{
		
		}
	]
}*/


//	A simple three pass blur – first reduce the size, then do a weighted blur, then do the same thing 


varying vec2 left_coord;
varying vec2 right_coord;
varying vec2 above_coord;
varying vec2 below_coord;

varying vec2 lefta_coord;
varying vec2 righta_coord;
varying vec2 leftb_coord;
varying vec2 rightb_coord;

void main()
{
	
	vec4 color = IMG_THIS_NORM_PIXEL(inputImage);
	vec4 colorL = IMG_NORM_PIXEL(inputImage, left_coord);
	vec4 colorR = IMG_NORM_PIXEL(inputImage, right_coord);
	vec4 colorA = IMG_NORM_PIXEL(inputImage, above_coord);
	vec4 colorB = IMG_NORM_PIXEL(inputImage, below_coord);

	vec4 colorLA = IMG_NORM_PIXEL(inputImage, lefta_coord);
	vec4 colorRA = IMG_NORM_PIXEL(inputImage, righta_coord);
	vec4 colorLB = IMG_NORM_PIXEL(inputImage, leftb_coord);
	vec4 colorRB = IMG_NORM_PIXEL(inputImage, rightb_coord);

	vec4 avg = (color + colorL + colorR + colorA + colorB + colorLA + colorRA + colorLB + colorRB) / 9.0;
	
	if (PASSINDEX == 1)	{
		vec4 blur = IMG_THIS_NORM_PIXEL(smaller);
		avg = mix(color, (avg + depth*blur)/(1.0+depth), softness);
	}
	else if (PASSINDEX == 2)	{
		vec4 blur = IMG_THIS_NORM_PIXEL(small);
		avg = mix(color, (avg + depth*blur)/(1.0+depth), softness);
	}
	gl_FragColor = avg;
}
```

Looking at the JSON blob in the `PASSES` section, we can see that there are three render passes.  The first two render passes use the `WIDTH` and `HEIGHT` attributes to resize the image being passed in.  This is a useful trick when creating multi-pass blur effects that averages together pixels during the size reduction and makes those averages the new neighboring pixels where they can be processed by the kernel.

On each rendering pass the basic Box Blur kernel is applied and sent to the output.  When the `PASSINDEX` is 1 or 2 (on the 2nd and third render passes), the result of the Box Blur is combined with the result from the previous pass.  Instead of directly changing the kernel, the declared `INPUT` variables are used to adjust the amount of this blending.

## Other uses of persistent buffers and multi-pass shaders

### Conway's Game of Life

In computing simulation, one of the most famous algorithms is known as Conway's Game of Life.  Game of Life is a cellular automaton devised by the British mathematician John Horton Conway in 1970.  Though it is called a game, there aren't any players - the board starts with a random or preconfigured state and then evolves from there.

The [Wikipedia page on Game of Life](https://en.wikipedia.org/wiki/Conway's_Game_of_Life) describes the rules as such:

	The universe of the Game of Life is an infinite two-dimensional orthogonal grid of square cells, each of which is in one of two possible states, alive or dead, or "populated" or "unpopulated". Every cell interacts with its eight neighbours, which are the cells that are horizontally, vertically, or diagonally adjacent. At each step in time, the following transitions occur:

	- Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
	- Any live cell with two or three live neighbours lives on to the next generation.
	- Any live cell with more than three live neighbours dies, as if by overpopulation.
	- Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
	
The idea of comparing a cell to its surround eight neighbors sounds an awful lot like what we have been doing with our convolution filters.  The only difference here is instead of using a kernel to process an incoming image, we'll need to start with some initial state and then iterate on that.

For this shader we can once again re-use the basic convolution vertex shader and include the matching `varying` variables in the fragment shader below.

```
/*{
	"DESCRIPTION": "Based on Conway's Game of Life",
	"CREDIT": "VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "restartNow",
			"TYPE": "event"
		},
		{
			"NAME": "startThresh",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		}
	],
	"PASSES": [
		{
			"TARGET":"lastData",
			"PERSISTENT": true
		}
	]
	
}*/

/*

Any live cell with fewer than two live neighbours dies, as if caused by under-population.
Any live cell with two or three live neighbours lives on to the next generation.
Any live cell with more than three live neighbours dies, as if by over-population.
Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

*/

varying vec2 left_coord;
varying vec2 right_coord;
varying vec2 above_coord;
varying vec2 below_coord;

varying vec2 lefta_coord;
varying vec2 righta_coord;
varying vec2 leftb_coord;
varying vec2 rightb_coord;

//	used to get the grayscale version of a pixel
float gray(vec4 n)
{
	return (n.r + n.g + n.b)/3.0;
}

//	used to randomize the start state
float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main()	{
	vec4		inputPixelColor = vec4(0.0);
	vec2		loc = gl_FragCoord.xy;
	
	//	if we are starting or the restartNow event is active, randomize
	if ((FRAMEINDEX < 1)||(restartNow))	{
		//	randomize the start conditions
		float	alive = rand(vec2(TIME+1.0,2.1*TIME+0.1)*loc);
		if (alive > 1.0 - startThresh)	{
			inputPixelColor = vec4(1.0);
		}
	}
	else	{
		vec4	color = IMG_PIXEL(lastData, loc);
		vec4	colorL = IMG_PIXEL(lastData, left_coord);
		vec4	colorR = IMG_PIXEL(lastData, right_coord);
		vec4	colorA = IMG_PIXEL(lastData, above_coord);
		vec4	colorB = IMG_PIXEL(lastData, below_coord);

		vec4	colorLA = IMG_PIXEL(lastData, lefta_coord);
		vec4	colorRA = IMG_PIXEL(lastData, righta_coord);
		vec4	colorLB = IMG_PIXEL(lastData, leftb_coord);
		vec4	colorRB = IMG_PIXEL(lastData, rightb_coord);
		
		float	neighborSum = gray(colorL + colorR + colorA + colorB + colorLA + colorRA + colorLB + colorRB);
		float	state = gray(color);
		
		//	live cell
		if (state > 0.0)	{
			if (neighborSum < 2.0)	{
				//	under population
				inputPixelColor = vec4(0.0);
			}
			else if (neighborSum < 4.0)	{
				//	status quo
				inputPixelColor = vec4(1.0);
			}
			else	{
				//	over population
				inputPixelColor = vec4(0.0);
			}
		}
		//	dead cell
		else	{
			if ((neighborSum > 2.0)&&(neighborSum < 4.0))	{
				//	reproduction
				inputPixelColor = vec4(1.0);
			}
			else if (neighborSum < 2.0)	{
				//	stay dead
			}
		}
	}
	
	gl_FragColor = inputPixelColor;
}
```

Walking through this code, the initial `if` statement is used to determine if the shader needs to randomize the state.  This happens under one of two conditions, either the `FRAMEINDEX` is 0 (the first frame) or the `restartNow` uniform (declared as an `event` type in the JSON blob) has been set to true by the host application.  The initial state is created by calling our custom rand() function which creates pseudo-random numbers between 0.0 and 1.0.  When these numbers are above the startThresh, the pixel starts as alive.

When the state is not being randomized, we follow the ruleset.  There are two pieces of information we need to collect and based on those there can result in four possible outcomes.  In particular we need to know the current alive / dead state of the current pixel (1 or 0) and we need to know the summation of the states of the neighboring pixels.  Here the `state` variable holds the state of the current pixel and `neighborSum` is used to hold the sum of the surrounding 8 pixels.

Once this information is collected we can create a set of `if` statements based on their values, starting with whether or not the state is 1 or 0.  When the cell is alive, it can die as a result of over or under population, otherwise it stays the same.  When the cell is dead, it can become alive due to reproduction if there are enough neighboring living cells.

This simple set of rules creates a wide variety of different outcomes and the basic concept of cellular automaton can be used as a starting point for creating evolving behaviors within other shaders.  Recall that with ISF, you can make Game of Life just the first of multiple render passes that use the state information as part of more complex generators or effects.

The Game of Life shader itself can be remixed in various interesting ways.  Here are a few challenges:
- Create a version that uses the RGB channels to run three different simulations in a single output.
- Add an option to switch between pseudo-random numbers and sin waves for the starting state.
- Add options for random birth and random death of cells.
- Change the rules to increase or decrease the birth rate.

