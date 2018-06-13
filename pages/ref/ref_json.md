---
title: ISF JSON Reference
tags: [JSON, Reference]
keywords: PASSINDEX, PERSISTENT, INPUTS, VSN, ISFVSN, CREDIT, DESCRIPTION, CATEGORIES, MIN, MAX, DEFAULT, TARGET, IMPORTED, LABEL
last_updated: June 12, 2018
summary: "An overview of JSON formatting and tags for ISF."
sidebar: home_sidebar
permalink: ref_json.html
folder: ref
---

# ISF JSON Reference

The ISF specification requires that each shader include a JSON blob that includes attributes describing the rendering setup, type of shader (generator, FX or transition), input parameters and other meta-data that host applications may want to make use of.

In addition to this reference you may find it useful to download the "Test____.fs" sample filters located here:
[ISF Test/Tutorial filters](http://vidvox.net/rays_oddsnends/ISF%20tests+tutorials.zip)
These demonstrate the basic set of attributes available and provides examples of each input parameter type.  You will probably learn more, faster, from the examples than you'll get by reading this document: each example describes a single aspect of the ISF file format, and they're extremely handy for testing, reference, or as a tutorial.

## Including JSON in an ISF

The first thing in your ISF file needs to be a comment (delineated using "/\*" and "\*/") containing a JSON dict. If the comment doesn't exist- or the JSON dict is malformed or can't be parsed- your ISF file can't be loaded (ISF files can be tested with the ISF Editor linked to elsewhere on this page). This JSON dict is referred to as your "top-level dict" throughout the rest of this document.

A basic ISF may have a JSON blob that looks something like this:
```
/*{
	"DESCRIPTION": "demonstrates the use of float-type inputs",
	"CREDIT": "by zoidberg",
	"ISFVSN": "2.0",
	"VSN": "2.0",
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

void main()
{
	vec4		srcPixel = IMG_THIS_PIXEL(inputImage);
	float		luma = (srcPixel.r+srcPixel.g+srcPixel.b)/3.0;
	vec4		dstPixel = (luma>level) ? srcPixel : vec4(0,0,0,1);
	gl_FragColor = dstPixel;
}
```

### ISF Attributes

- If there's a string in the top-level dict stored at the `ISFVSN` key, this string will describe the version of the ISF specification this shader was written for.  This key should be considered mandatory- if it's missing, the assumption is that the shader was written for version 1.0 of the ISF spec (which didn't specify this key).  The string is expected to contain one or more integers separated by dots (eg: '2', or '2.1', or '2.1.1').
- If there's a string in the top-level dict stored at the `VSN` key, this string will describe the version of this ISF file.    This key is completely optional, and its use is up to the host or editor- the goal is to provide a simple path for tracking changes in ISF files.  Like the `ISFVSN` key, this string is expected to contain one or more integers separated by dots.
- If there's a string in the top-level dict stored at the `DESCRIPTION` key, this string will be displayed as a description associated with this filter in the host app. the use of this key is optional.
- The `CATEGORIES` key in your top-level dict should store an array of strings. The strings are the category names you want the filter to appear in (assuming the host app displays categories).
- The `INPUTS` key of your top-level dict should store an array of dictionaries (each dictionary describes a different input- the inputs should appear in the host app in the order they're listed in this array). For each input dictionary:
  - The value stored with the key `NAME` must be a string, and it must not contain any whitespaces. This is the name of the input, and will also be the variable name of the input in your shader.
  - The value stored with the key `TYPE` must be a string. This string describes the type of the input, and must be one of the following values: "event", "bool", "long", "float", "point2D", "color", "image", "audio", or "audioFFT".
    - The input types "audio" and "audioFFT" specify that the input will be sent audio data of some sort from an audio source- "audio" expects to receive a raw audio wave, and "audioFFT" expects the results of an FFT performed on the raw audio wave.  This audio data is passed to the shader as an image, so "audio"- and "audioFFT"-type inputs should be treated as if they were images within the actual shader.  By default, hosts should try to provide this data at a reasonably high precision (32- or 16-bit float GL textures, for example), but if this isn't possible then lower precision is fine.
      - The images sent to "audio"-type inputs contains one row of image data for each channel of audio data (multiple channels of audio data can be passed in a single image), while each column of the image represents a single sample of the wave, the value of which is centered around 0.5.
      - The images sent to "audioFFT"-type inputs contains one row of image data for each channel of audio data (multiple channels of audio data can be passed in a single image), while each column of the image represents a single value in the FFT results.
      - Both "audio"- and "audioFFT"-type inputs allow you to specify the number of samples (the "width" of the images in which the audio data is sent) via the `MAX` key (more on this later in the discussion of `MAX`).
  - Where appropriate, `DEFAULT`, `MIN`, `MAX`, and `IDENTITY` may be used to further describe value attributes of the input. Note that "image"-type inputs don't have any of these, and that "color"-type inputs use an array of floats to describe min/max/default colors. Everywhere else values are stored as native JSON values where possible (float as float, bool as bool, etc).
    - "audio"- and "audioFFT"-type inputs support the use of the `MAX` key- but in this context, `MAX` specifies the number of samples that the shader wants to receive.  This key is optional- if `MAX` is not defined then the shader will receive audio data with the number of samples that were provided natively.  For example, if the `MAX` of an "audio"-type input is defined as 1, the resulting 1-pixel-wide image is going to accurately convey the "total volume" of the audio wave; if you want a 4-column FFT graph, specify a `MAX` of 4 on an "audioFFT"-type input, etc.
  - The value stored with the key `LABEL` must be a string. This key is optional- the `NAME` of an input is the variable name, and as such it can't contain any spaces/etc. The `LABEL` key provides host sofware with the opportunity to display a more human-readable name. This string is purely for display purposes and isn't used for processing at all.
  - Other notes:
    - "event" type inputs describe events that do not have an associated value- a momentary click button.
    - The "long" type input is used to implement pop-up buttons/pop-up menus in the host UI. As such, "long"-type input dictionaries have a few extra keys:
      - The `VALUES` key stores an array of integer values. This array may have repeats, and the values correspond to the labels. When you choose an item from the pop-up menu, the corresponding value from this array is sent to your shader.
      - The `LABELS` key stores an array of strings. This array may have repeats, and the strings/labels correspond to the array of values.
- The `PASSES` key should store an array of dictionaries. Each dictionary describes a different rendering pass. This key is optional: you don't need to include it, and if it's not present your effect will be assumed to be single-pass.
  - The `TARGET` string in the pass dict describes the name of the buffer this pass renders to.  The ISF host will automatically create a temporary buffer using this name, and you can read the pixels from this temporary buffer back in your shader in a subsequent rendering pass using this name.  By default, these temporary buffers are deleted (or returned to a pool) after the ISF file has finished rendering a frame of output- they do not persist from one frame to another.  No particular requirements are made for the default texture format- it's assumed that the host will use a common texture format for images of reasonable visual quality.
  - If the pass dict has a positive value stored at the `PERSISTENT` key, it indicates that the target buffer will be persistent- that it will be saved across frames, and stay with your effect until its deletion.  If you ask the filter to render a frame at a different resolution, persistent buffers are resized to accommodate.  Persistent buffers are useful for passing data from one frame to the next- for an image accumulator, or motion blur, for example.  This key is optional- if it isn't present (or contains a 0 or false value), the target buffer isn't persistent.
  - If the pass dict has a positive value stored at the `FLOAT` key, it indicates that the target buffer created by the host will have 32bit float per channel precision.  Float buffers are proportionally slower to work with, but if you need precision- for image accumulators or visual persistence projects, for example- then you should use this key.  Float-precision buffers can also be used to store variables or values between passes or between frames- each pixel can store four 32-bit floats, so you can render a low-res pass to a float buffer to store values, and then read them back in subsequent rendering passes.   This key is optional- if it isn't present (or contains a 0 or false value), the target buffer will be of normal precision.
  - If the pass dictionary has a value for the keys `WIDTH` or `HEIGHT` (these keys are optional), that value is expected to be a string with an equation describing the width/height of the buffer. This equation may reference variables: the width and height of the image requested from this filter are passed to the equation as `$WIDTH` and `$HEIGHT`, and the value of any other inputs declared in `INPUTS` can also be passed to this equation (for example, the value from the float input "blurAmount" would be represented in an equation as "$blurAmount"). This equation is evaluated once per frame, when you initially pass the filter a frame (it's not evaluated multiple times if the ISF file describes multiple rendering passes to produce a sigle frame). For more information (constants, built-in functions, etc) on math expression evaluations, please see the documentation for the excellent DDMathParser by Dave DeLong, which is what we're presently using.
- The `IMPORTED` key describes buffers that will be created for image files that you want ISF to automatically import. This key is optional: you don't need to include it, and if it's not present your ISF file just won't import any external images. The item stored at this key should be a dictionary.
  - Each key-value pair in the `IMPORTED` dictionary describes a single image file to import. The key for each item in the `IMPORTED` dictionary is the name of the buffer as it will be used in your ISF file, and the value for each item in the `IMPORTED` dictionary is another dictionary describing the file to be imported.
    - The dictionary describing the image to import must have a `PATH` key, and the object stored at that key must be a string. This string should describe the path to the image file, relative to the ISF file being evaluated. For example, a file named "asdf.jpg" in the same folder as the ISF file would have the `PATH` "asdf.jpg", or "./asdf.jpg" (both describe the same location). If the jpg were located in your ISF file's parent directory, its `PATH` would be "../asdf.jpg", etc.

### ISF Conventions

Within ISF there are three main usages for compositions: generators, filters and transitions.  Though not explicitly an attribute of the JSON blob itself, the usage can be specified by including for specific elements in the `INPUTS` array.  When the ISF is loaded by the host application, instead of the usual matching interface controls, these elements may be connected to special parts of the software rendering pipeline.

- ISF shaders that are to be used as image filters are expected to pass the image to be filtered using the "inputImage" variable name. This input needs to be declared like any other image input, and host developers can assume that any ISF shader specifying an "image"-type input named "inputImage" can be operated as an image filter.
- ISF shaders that are to be used as transitions require three inputs: two image inputs ("startImage" and "endImage"), and a normalized float input ("progress") used to indicate the progress of the transition. Like image filters, all of these inputs need to be declared as you would declare any other input, and any ISF that implements "startImage", "endImage", and "progress" can be assumed to operate as a transition.

ISF files that are neither filters nor transitions should be considered to be generators.