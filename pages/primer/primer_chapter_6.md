---
title: Convolution Filters
tags: [Primer, Introduction]
keywords: getting_started
last_updated: June 12, 2018
summary: "A closer look at using convolution filters in ISF."
sidebar: home_sidebar
permalink: primer_chapter_6.html
folder: primer
---

# Convolution Filter

In image filtering one of the commonly used techniques from mathematics is are convolution matrices, also sometimes called a kernel.  Convolution is a process by which an element, in this case a pixel, is adjusted by performing some sort of function along with its neighboring pixels.  This is generally used for blurring, sharpening, embossing, edge detection, and other situations where multiple pixels are being blended together in some fashion.  This lesson on convolution will be relevant whether you are writing shaders to meet the ISF specification and for using GLSL other environments.

For this chapter we'll look at:
- What a convolution matrix is and how they are used to process images.
- How to set up a generalized convolution ISF filter.
- How to build specific use cases of convolution filters.
- Discuss other similar use cases.

These concepts will also come into play in later chapters as we dive further into advanced topics of GLSL and ISF.

## What is a convolution matrix?

In mathematics, a convolution matrix, or kernel, is a set of weights that describe how a number of elements are to be added together.  For our purposes each kernel is a 3x3 grid of numbers, but in some cases you may encounter 5x5 or even larger kernels.

An example kernel may look like:
```
//	A blurring kernel
[0.0625, 0.125, 0.0625,
0.125, 0.25, 0.125,
0.0625, 0.125, 0.0625]
```

Each of the numbers in this grid is called a `weight`.

Here you can imagine the middle pixel as the one being evaluated.  In our GLSL code we so far have retrieved this by using the `IMG_THIS_PIXEL()` function on an image.  To obtain the output result, we obtain the values of all of the neighboring pixels and combine them using the weights as multipliers.  When this operation is performed for every pixel in your image, you can get wildly different results by changing these weight values.

	Note: For further reading on kernels visit the [Wikipedia page on convolution for image processing](https://en.wikipedia.org/wiki/Kernel_(image_processing)).

### Example Kernels

Many different effects can be created by using different weight values as inputs.  To get a sense for how some of the most important image filters works let's look at some sample kernels.

#### The Identity Kernel 

```
//	The Identity Kernel
[0.0, 0.0, 0.0,
0.0, 1.0, 0.0,
0.0, 0.0, 0.0]
```

In this case each neighboring pixel is multiplied by 0.0 and the middle pixel is multiplied by 1.0, so the output is simply equal to the middle pixel.  When using this special kernel, known as the `Identity`, the output will always look the same as the input.

#### The Box Blur Kernel 

Another commonly found kernel is the one for a `Box Blur`.  Many blur filters are based on this idea of averaging the middle pixel with the values around it.  The stronger the weights of the neighboring pixels in comparison to the middle, the stronger the blur.  In such cases it is important for the sum of all of the weights to be 1.0.

```
//	The Box Blur Kernel
[0.11111, 0.11111, 0.11111,
0.11111, 0.11111, 0.11111,
0.11111, 0.11111, 0.11111]
```

Though box blurs and other similar blurs are easily created with convolutions, a single pass of a 3x3 kernel does not produce a particularly deep blurring effect.  In the next chapter covering multi-pass shaders we'll look at how performing these kernels more than once can greatly increase the depth of the blur.

#### Sharpen Kernel

Often considered the opposite of the blur is the sharpen, which subtracts values on the diagonals instead of averaging.  This can often make edges appear more pronounced.

```
//	The Sharpen Kernel
[-1.0, 0.0, -1.0,
0.0, 5.0, 0.0,
-1.0, 0.0, -1.0]
```

Like with blurs, increasing the weight of the middle pixel compared to the amount subtracted will result in a stronger sharpening effect.  Similarly the sum of the weights must always be 1.0 for sharpen filters.

#### Edge Detection Kernels

In a similar fashion to sharpen kernels for edge detection the trick is to subtract.  However in this case, instead of the the sum of all of the weights being 1.0, they'll come out to 0.0;  this way it requires that certain pixels be much brighter for others to stay in the image, while most of them are set to black.

```
//	Edge Detection 1
[1.0, 0.0, 1.0,
0.0, -4.0, 0.0,
1.0, 0.0, 1.0]
```

```
//	Edge Detection 2
[-1.0, -1.0, -1.0,
-1.0, 8.0, -1.0,
-1.0, -1.0, 1.0]
```

Just as there are many ways to write sharpen and blur filters with varying amount of strength based on the relative weights, the same is true of edge detection kernels.

## Writing a Generalized Convolution Filter in ISF

When creating GLSL shaders that evaluate convolution kernels it can be useful to make use of the vertex shader stage when possible.  Like with rotations this allows us to prepare coordinate values that are used for lookup during the fragment shader stage, saving valuable GPU time during rendering.

### Create the Vertex Shader

First we'll make the vertex shader for the convolution kernel.  Whether you are creating a generalized use case like in this example, or creating a shader based on a specific kernel, your vertex shader will likely look something like this:

```
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

Here we've declared eight different varying vec2 variables, one for each of the neighboring pixels.  Combined with the middle coordinate itself located at `isf_FragNormCoord` this completes the calculations needed to do the image pixel lookups in the fragment shader stage.

### Create the Fragment Shader

Now we can create the fragment shader that performs the actual convolution.  For this generalized shader we declare 9 float values, one for each weight and give each a range of -8.0 to 8.0.  Only the middle pixel is set to 1.0 by default, so when first loaded the filter will function as a pass-thru.

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
			"NAME": "w00",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w10",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w20",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w01",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w11",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "w21",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w02",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w12",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w22",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		}
	]
}*/


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
	vec4 colorLA = IMG_NORM_PIXEL(inputImage, lefta_coord);
	vec4 colorA = IMG_NORM_PIXEL(inputImage, above_coord);
	vec4 colorRA = IMG_NORM_PIXEL(inputImage, righta_coord);

	vec4 colorL = IMG_NORM_PIXEL(inputImage, left_coord);
	vec4 color = IMG_THIS_NORM_PIXEL(inputImage);
	vec4 colorR = IMG_NORM_PIXEL(inputImage, right_coord);
	
	vec4 colorLB = IMG_NORM_PIXEL(inputImage, leftb_coord);
	vec4 colorB = IMG_NORM_PIXEL(inputImage, below_coord);
	vec4 colorRB = IMG_NORM_PIXEL(inputImage, rightb_coord);

	//	make the average for the RGB values
	vec3 convolution = (w11 * color + w01 * colorL + w21 * colorR + w10 * colorA + w12 * colorB + w00 * colorLA + w20 * colorRA + w02 * colorLB + w22 * colorRB).rgb;
	
	//	keep the alpha the same as the original pixel
	gl_FragColor = vec4(convolution,color.a);
}
```

In the code section below the JSON, we can see that the `varying` vec2 variables from the vertex shader have also been declared here.  This makes it possible for those values to be read here in the fragment shader.

Within the main() {} function the initial chunk of code gathers each neighboring pixel.  Next the convolution result vec3 RGB color is created by multiplying each pixel by its weight and adding them all together.  One small detail is that we use the alpha channel from the original pixel.

## Creating Specific Kernels As Filters

As you might imagine, getting a specific look by manipulating 9 different sliders can be a bit much.  This is why often it can be useful to pick a particular kernel and create a specific filter that includes a one or two high level parameters which modify the weights to adjust the strength.

Note that for these examples we will use a vertex shader identical to the one used above for the general convolution case.  Be sure to duplicate that code in its own file with a name that matches your fragment shaders below and uses the `.vs` extension.

### Creating a Varying Box Blur Filter in ISF

A very simple blur filter that has a single strength value can be written as such.

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
			"NAME": "blurLevel",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.0
		}
	]
}*/

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

	float mWeight = 1.0 - blurLevel;
	float nWeight = blurLevel / 8.0;
	
	vec4 color = IMG_THIS_NORM_PIXEL(inputImage);
	
	//	note that we can skip the pixel lookups here if nWeight is 0.0
	vec4 colorL = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, left_coord) : vec4(0.0);
	vec4 colorR = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, right_coord) : vec4(0.0);
	vec4 colorA = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, above_coord) : vec4(0.0);
	vec4 colorB = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, below_coord) : vec4(0.0);

	vec4 colorLA = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, lefta_coord) : vec4(0.0);
	vec4 colorRA = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, righta_coord) : vec4(0.0);
	vec4 colorLB = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, leftb_coord) : vec4(0.0);
	vec4 colorRB = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, rightb_coord) : vec4(0.0);

	vec3 blur = ((mWeight * color) + nWeight * (colorL + colorR + colorA + colorB + colorLA + colorRA + colorLB + colorRB)).rgb;
	
	gl_FragColor = vec4(blur,color.a);
}
```

Unlike our previous box blur kernel which had fixed values, this determines its weights based on the `blurLevel` uniform variable declared in the JSON section.  As that value moves from 0.0 to 1.0, the weight of the neighbors increases while the weight of the middle pixel decreases.  Recall that for a blur or sharpen we need the sum of our weights to be 1.0.

As a challenge, try adapting one of the other kernels we looked at, such as the sharpen kernel, and adapt this code to apply it instead of the box blur.

### Creating Convolution Filters With For Loops

In the examples so far we've used a vertex shader to pre-compute our coordinate points used in our fragment shaders.  While this is recommended when possible, there are times when your algorithm may use an indeterminate number of lookup points, or the lookup points may vary depending on other factors within your fragment shader code.

```
/*
{
  "CATEGORIES" : [
    "Blur"
  ],
  "DESCRIPTION" : "",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "blurLevel",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0
    }
  ],
  "CREDIT" : "by VIDVOX"
}
*/

void main()	{
	float mWeight = 1.0 - blurLevel;
	float nWeight = blurLevel / 8.0;
	vec4 result = vec4(0.0);
	if (blurLevel > 0.0)	{
		for (int i = -1;i <= 1;++i)	{
			for (int j = -1;j <= 1;++j)	{
				vec2 loc = gl_FragCoord.xy + vec2(i,j);
				vec4 color = IMG_PIXEL(inputImage,loc);
				if ((i == 0)&&(j == 0))	{
					result.rgb += mWeight * color.rgb;
					result.a = color.a;
				}
				else	{
					result.rgb += nWeight * color.rgb;
				}
			}
		}
	}
	else	{
		result = IMG_THIS_NORM_PIXEL(inputImage);	
	}
	
	gl_FragColor = result;
}
```

Here instead of using the vertex shader to pre-compute the lookup points and pass them over with varying variables, the lookup points are computed each time inside of the for loop.  With this method it would be easy to adapt this blur to doing a 5x5 neighbor pixel averaging without having to declare any new variables.

As a note, it is recommended to avoid doing too many pixel lookups and comparisons within your code.  While there is no technical limit on this these operations are comparatively costly.  In some cases you can write in optimizations, such as for situations where you know a weight value for a pixel is 0.0, you can skip its corresponding lookup.

### More Examples Of Convolution Shaders

A number of the sample shaders on the [ISF Sharing Website](http://interactiveshaderformat.com) are great examples of convolution filters.  As hinted at, many of the convolution shaders that are used in actual practice use another advanced technique that we will soon learn about for creating ISF compositions with multiple render passes.

## Other Similar Use Cases

Though not technically considered convolution filters because they don't just simply apply a kernel to a set of pixels, there are many visual effects that can be created by comparing a pixel to its neighbors.  Once you have the data in vec4 variables you can apply whatever functions or operations you can think of to combine the values into a single result.

Here are a few filters in particular that use a similar technique:
- An `Erode` effect searches surrounding pixels looking for minimum values.
- A `Dilate` effect searches surrounding pixels looking for maximum values.
- A `Frosted Glass` style effect can blend together several local pixels in a non-standard pattern.
- An `Emboss` effect uses a combination of a convolution kernel and other post processing to create its output.

Examples of these shaders and other related examples can be found on [ISF Sharing Website](http://interactiveshaderformat.com).