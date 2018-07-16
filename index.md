---
title: "Getting started with Interactive Shader Format"
keywords: home
tags: [getting_started]
sidebar: home_sidebar
permalink: index.html
summary: An overview of ISF, the topics covered in these documentation pages and related resources.
---

# What is ISF?

ISF stands for "Interactive Shader Format", and is a file format that describes a GLSL fragment shader, as well as how to execute and interact with it. The goal of this file format is to provide a simple and minimal interface for image filters and generative video sources that allows them to be interacted with and reused in a generic and modular fashion. ISF is nothing more than a [slightly modified] GLSL fragment shader with a JSON blob at the beginning that describes how to interact with the shader (how many inputs/uniform variables it has, what their names are, what kind of inputs/variables they are, that sort of thing). ISF isn't some crazy new groundbreaking technology- it's just a simple and useful combination of two things that have been around for a while to make a minimal- but highly effective- filter format.

## Using ISF Compositions

Shaders written in the ISF specification can be used in supported environments on desktop, mobile and the web.  To use ISF files in a specific piece of software consult the appropriate documentation.

- In many cases, ISF generator files can be directly loaded into the media bin / media player section of host software.
- ISF files that you would like to be globally available to all software on your Mac can be placed in the "/Library/Graphics/ISF" or "~/Library/Graphics/ISF" directories.  Generators, filters and transitions in these directories should generally be automatically available within supported software where applicable.


### Supported software

The ISF specification was original created for [VDMX](http://vidvox.net) and is now supported by several different applications as a format including:
- [CoGe](https://imimot.com/cogevj/)
- [Fugio](http://www.bigfug.com/software/fugio/)
- [Mad Mappper](http://madmapper.com/)
- [Millumin](http://www.millumin.com/)
- [VDMX](http://vidvox.net)
- ISF shaders can be made as full page standalone webpages, with or without controls.  An example implementation can be found in the [ISF Generator Example on Glitch](https://glitch.com/edit/#!/isf-example?path=README.md).
- ISF files can be created, viewed and shared online at the [interactiveshaderformat.com](http://interactiveshaderformat.com) website.

## Creating and Remixing ISF Compositions

Whether you are a GLSL expert or just getting started, there are several resources and specialized tools that can be useful when writing or remixing ISF compositions.

### Online resources

- The [ISF Quick Start](quickstart) is a guide to quickly get started with writing GLSL shaders in the ISF specification.  A good starting point for people who already know how to code and are just looking to understand the core concepts of ISF.
- The [ISF Reference Pages](ref_index) contain an overview of the available built-in variables, functions and other conventions used by ISF as a quick reference for shader developers.
- The [ISF Primer](primer_index) is a set of in depth lessons with walkthroughs for writing your first GLSL shaders and discussion of advanced usages of the ISF specification.
- The [ISF Specification Page](https://github.com/mrRay/ISF_Spec/) contains detailed information about ISF for shader and application developers, along with links to source code repositories and other useful resources.

### Software tools and sample files

Though you can create or modify ISF compositions using any standard text editor it can often be useful be able to have a specialized interface that makes it easier to see your output and debug your code.

#### The ISF Web Editor

- ISF files can be created, viewed and shared online at the [isf.video](http://interactiveshaderformat.com) website.  These are some of its basic features:
	- Browse, preview and download shaders from the online community.
	- Use a web-cam, static images, or uploaded GIFs as sources for image filters.
	- UI items are automatically created for inputs, allowing you to interact with your ISF file.

#### The ISF Editor For Mac

- An ISF Editor for Mac is available here: 
[ISF Editor.app.zip](https://www.vidvox.net/download/ISF_Editor_2.9.7.3.dmg).  These are some of its basic features:
  - Browses, renders and displays ISF files. Has a built-in video source, and can also use any QC comps, movie files, image files, Syphon video servers, or AVCapture-compatible video inputs as a video source for testing ISF-based image filters.
  - Automatically publishes the rendered output as a Syphon source.
  - UI items are automatically created for inputs, allowing you to interact with your ISF file.
  - Built-in shader editor with syntax coloring and integrated error display along with plenty of logging to facilitate creating and debugging shaders.
  - Output window can be paused, and can also be used to view the output of the individual render passes in your ISF file, which facilitates debugging by providing shader devs with a quick and easy way to visualize values being used in their shaders.
  - "Import from Shadertoy/GLSL Sandbox" feature can be used to automatically convert the vast majority of shaders found on Shadertoy and GLSL Sandbox to ISF sources and filters. Some shaders may need further modification, but it's shocking how many will "just work".

#### Sample Files

- Here are a bunch of simple test ISF files that demonstrate ISF's basic features (these are test filters, and we don't expect them to have signifcant creative use):
[ISF Test/Tutorial filters](http://vidvox.net/rays_oddsnends/ISF%20tests+tutorials.zip)
- [Here is an installer](http://www.vidvox.net/rays_oddsnends/Vidvox%20ISF%20resources.pkg.zip) for over a hundred different ISF files, both images and filters.  The installer places them in /Library/Graphics/ISF where they can be accessed by all users.

## Adding Support For ISF in 3rd Party Software

The [ISF Specification Page](https://github.com/mrRay/ISF_Spec/) contains detailed information about ISF application developers, along with links to source code repositories and other useful resources to get started with.