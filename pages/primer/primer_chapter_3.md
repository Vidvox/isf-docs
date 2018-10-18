---
title: Using and Creating ISF Compositions
tags: [Primer, Introduction]
keywords: getting_started
last_updated: June 12, 2018
summary: "A closer look at how to use and create ISF compositions."
sidebar: home_sidebar
permalink: primer_chapter_3.html
folder: primer
---

# Using ISF Compositions

One of the key details of creating shaders in the ISF specification is that they can be used in host applications as generators, effects and transitions.  While an individual composition can be a lot of fun on its own, the real power of ISF comes from being able to include them as smaller re-usable elements in larger projects.

In this chapter we will look at:
- The tools for creating and editing ISF compositions.
- How the compositions that we create for this book, or ones that you download from the web, can be used in different host software and environments.

## Tools for creating, editing and publishing ISF compositions

For the most part while reading this book you can use the provided code editing section, you may find it useful to work with one of the other available methods, as these will be the tools you will later use to write ISF compositions to load into other software or publish online.

### Using a text editor

You can write and edit shader files using most standard text editors.  When using a text editor 

- When saving documents make sure to use the ".fs" extension for fragment shaders.  Unless otherwise specified all of the examples in this book use only a single fragment shader file.
- In a later chapter we will discuss optionally including a vertex shader, which use the ".vs" file extension, as part of an ISF composition.
- If you use TextEdit on the Mac, make sure to use the "Make Plain Text" option from the Format menu.
- You may also want to try out one of many great third party text editors such BBEdit, TextWrangler, TextMate, Sublime Text, Xcode and Atom which include syntax highlighting for GLSL.
- Like most programming languages, if you enjoy command line text editors such as Vim, Emacs or Nano, these also work just fine for writing GLSL code.

### The Online ISF Editor

ISF files can be created, viewed and shared online at the [isf.video](http://interactiveshaderformat.com) website.  These are some of its basic features:
- Browse, preview and download shaders from the online community.
- Use a web-cam, static images, or uploaded GIFs as sources for image filters.
- UI items are automatically created for inputs, allowing you to interact with your ISF file.
- Shaders uploaded to this site can be shared publicly or made private.

### The ISF Editor for Mac

A free ISF Editor for Mac is available here: 
[ISF Editor.app.zip](https://www.vidvox.net/download/ISF_Editor_2.9.7.3.dmg).

These are some of its basic features:
- Browses, renders and displays ISF files. Has a built-in video source, and can also use any QC comps, movie files, image files, Syphon video servers, or AVCapture-compatible video inputs as a video source for testing ISF-based image filters.
- Automatically publishes the rendered output as a Syphon source.
- UI items are automatically created for inputs, allowing you to interact with your ISF file.
- Built-in shader editor with syntax coloring and integrated error display along with plenty of logging to facilitate creating and debugging shaders.
- Output window can be paused, and can also be used to view the output of the individual render passes in your ISF file, which facilitates debugging by providing shader devs with a quick and easy way to visualize values being used in their shaders.
- "Import from Shadertoy/GLSL Sandbox" feature can be used to automatically convert the vast majority of shaders found on Shadertoy and GLSL Sandbox to ISF sources and filters. Some shaders may need further modification, but it's shocking how many will "just work".

### Glitch.com

An example implementation of using ISF on Glitch.com can be found in the [ISF Generator Example on Glitch](https://glitch.com/edit/#!/isf-example?path=README.md).  This can be used a starting point for a sandbox to develop your own shaders that can be previewed in a web browser.

## Using ISF Compositions in host software

ISF compositions can be used in a variety of different host applications.  In this section we will briefly look at how ISF works in a few different pieces of software, but for detailed information you may want to consult with the corresponding documentation provided by the developer.

On the Mac, compositions that are located in the "/Library/Graphics/ISF/" and "~/Library/Graphics/ISF/" directories are considered to be global and are commonly automatically accessed by any software that supports the specification.

### VDMX

The ISF specification was originally designed for use in [VDMX](http://vidvox.net) and is a fitting first example.

ISF compositions installed in the global directories are automatically detected and included as available assets in the appropriate place:
- ISF Generators can be found in the Layer Source menu, or added to media bin pages where they can be triggered to layers like any other source.  When using ISF Generators, the rendering resolution can be specified from the Layer Source inspector.
- ISF FX can be found in the Load Asset menu in the Layer FX.  Each composition is placed in its specified categories.  The list of FX can also be browsed from the Assets section of the Workspace Inspector.

Additionally, ISF Generators that are located in other directories on your computer can be directly loaded into media bin pages by dragging from the Finder or using the Media Browser window.

Whether being used as a generator or FX any elements specified in the `INPUTS` for the ISF composition will be available as user interface controls.  For example, inputs of type "float" appear as sliders, "bools" will appear as toggle buttons, and "colors" will appear as color pickers.  The exception to this is when using "inputImage" to declare an FX, as this element is used to pass the incoming layer image into the shader.

For an example of using audio waveforms and FFTs see [the VDMX tutorial on visualizing audio fft and waveforms](https://vdmx.vidvox.net/tutorials/visualizing-audio-analysis-fft-and-waveforms).

Additional tutorials on using ISF in VDMX can be found on the [VDMX Tutorials Site](https://vdmx.vidvox.net/search?q=ISF).

### Other supported software

Here are links to websites of a few different other softwares that support the ISF specification:

- [CoGe](https://imimot.com/cogevj/)
- [Fugio](http://www.bigfug.com/software/fugio/)
- [Mad Mappper](http://madmapper.com/)
- [Millumin](http://www.millumin.com/)

### Using ISF in WebGL

The [interactive-shader-format-js](https://github.com/msfeldstein/interactive-shader-format-js) project includes code for loading and rendering ISF compositions as part of a WebGL pipeline.  An example implementation of using the javascript implementation of ISF can be found in the [ISF Generator Example on Glitch.com](https://glitch.com/edit/#!/isf-example?path=README.md).

### Related open source projects

Developers interested in working with ISF files on a code level can take advantage of several existing libraries, frameworks and example projects.

The VIDVOX ISF Editor and VVISFKit framework codebases are open source and can be found in the [VVOpenSource](https://github.com/mrRay/vvopensource) repository.

Additionally here are a few other useful open source projects that use ISF to check out:
- [ISF-JS-Renderer](https://www.github.com/msfeldstein/ISF-JS-Renderer) can be used to parse and render ISF files through javascript.
- [libisf](https://github.com/jcelerier/libisf) is a library for parsing ISF files and includes an example editor made with Qt.
- [ISF TouchDesigner](https://github.com/marcinbiegun/isf-touchdesigner) has partial support for using ISF files in TouchDesigner projects.
- [ofxISF](https://github.com/satoruhiga/ofxISF) has partial support for using ISF files in OpenFrameworks projects.
- [ISFHeavyM](https://github.com/sophiadigitalart/ISFHeavyM) and [ISFgif](https://github.com/sophiadigitalart/ISFGif) can be used to create a ISF shared library with GIF preview for HeavyM.