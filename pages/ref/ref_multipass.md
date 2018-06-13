---
title: ISF Multi-Pass and Persistent Buffer Reference
tags: [Multi-pass, Persistent Buffers]
keywords: PASSINDEX, PERSISTENT
last_updated: June 12, 2018
summary: "An overview of using multi-pass and persistent buffers in ISF."
sidebar: home_sidebar
permalink: ref_multipass.html
folder: ref
---

# ISF Multi-Pass and Persistent Buffer Reference

Two extremely powerful concepts that ISF adds on to GLSL are the ability to retain image information between render passes (persistent buffers) and creating compound shaders that have multiple rendering stages (multi-pass shaders) at potentially varying sizes.

## Persistent Buffers

ISF files can define persistent buffers.  These buffers are images (GL textures) that stay with the ISF file for as long as it exists. This is useful if you want to "build up" an image over time- you can repeatedly query and update the contents of persistent buffers by rendering into them- or if you want to perform calculations across the entire image, storing the results somewhere for later evaluation. Further details on exactly how to do this are in the full [ISF Specification Page](https://github.com/mrRay/ISF_Spec/).

For each buffer that you wish to retain between passes, the `PERSISTENT` can be set to `true`.  If you wish to have the value stored as a 32-bit floating point value the additional `FLOAT` attribute can be included and set to `true`.  Using 32-bit textures will use up more memory, but in some cases can be extremely useful.

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