---
title: An Introduction To ISF
tags: [Primer, Introduction]
keywords: getting_started
last_updated: June 12, 2018
summary: "An overview of the topics covered in this Primer and an introduction to the concepts behind Interactive Shader Format."
sidebar: home_sidebar
permalink: primer_chapter_1.html
folder: primer
---

# What is ISF?

ISF, aka Interactive Shader Format, is a file format for creating video generators and FX plugins that can run on desktop, mobile and WebGL.  ISF is built on top of GLSL, a powerful cross-platform language that runs on the GPU.  By writing shaders that meet the ISF specification you can easily re-use your creations across different software applications without having to make environment specific modifications.

If you already know some GLSL, learning how to take advantage of ISF will be very easy as it is merely a set of simple basic conventions to follow.  Having already encountered some of the limitations of the language on its own will hopefully make you appreciate some of the ideas introduced by ISF.  Additionally you still may enjoy some of the beginner GLSL lessons in this tutorial as a refresher and to help better understand some of the thought process behind the ISF specification.

If you don't already know any GLSL, get excited, because this is some crazy powerful stuff that you are about to add to your visual toolkit.  There is a rich community of developers who are sharing their experience online for you to learn from and the underlying concepts are widely applicable to a wide variety of platforms.

## About this Guide

One of the most powerful techniques in modern digital art using code to create and process imagery.  When lots of these images are put together, we call it video.  This guide is an introduction to one of the emerging standards for rendering images and video known as Interactive Shader Format, or ISF for short.

The core concept of writing generators and FX in ISF is that each idea is designed to be a small self contained file that can be run on its own or combined with other ISF compositions within a variety of different host environments.  Artists and new developers can focus on creating specific visual effects without having to write an entire application around them.  By writing shaders against the ISF standard, the same rendering code can often be re-used on desktop, mobile and web platforms.  In most cases ISF compositions are designed to be run in real-time, accelerated by the computer graphics card.

### Who is this guide for?

Visual artists, motion graphics designers, VJs, game developers and anyone else interested in using code to work with images and pixel data while taking advantage of the power of the GPU will find that ISF can be a great tool to work with.  In many cases you may use it to extend the capabilities of a piece of software that you are already using.  In others, you may want to create a single interactive graphical composition that runs embedded as part of a webpage.

Software developers looking to support ISF in their own applications can visit the [ISF Specification Page](https://github.com/mrRay/ISF_Spec/) for links to opensource codebases and other useful information.

### What does this guide cover?

ISF is itself built on top of another language known as GLSL.  No advanced knowledge of GLSL or any other programming is needed to read through these chapters as we will begin with some of the very basics.  Though we will demonstrate some GLSL in these chapters, it will usually be just enough to see how ISF extends the underlying concepts of the language.  There are many other amazing resources for learning GLSL that are readily applicable to working with ISF and we'll mention a few of them later in this chapter.

## The difference between ISF and GLSL

As mentioned, ISF is a specification built on top of an existing popular language known as GLSL.  This means that you can take advantage of other resources for learning how to write GLSL shaders and in many cases adapt other GLSL projects to meet the ISF specification.

### What is GLSL?

GLSL is also commonly known as the OpenGL Shading Language.  It is a language that has a C-like syntax that runs on the GPU of a computer.  The advantage of using GLSL for image processing is that it takes advantage of parallel processing – the code that you write describes an algorithm that is designed to be executed for each pixel (or vertex) simultaneously, rather than writing code that requires individually stepping through each pixel sequentially.  While there are several different kinds of shaders, in this introduction we'll begin by focusing on fragment shaders which can be used to generate or process pixel information.

GLSL shaders are widely used in places like gaming engines, photo manipulation tools, CGI, and anywhere else that real-time capable visual graphics and effects are needed.

### What is ISF?

While GLSL is on its own extremely powerful and flexible, it is a very low level language designed for writing small snippits of code that often require some additional work to be used within a host application or to connect several shaders together to create complex multi-stage algorithms.  GLSL also lacks a standard way for shaders to provide information about the properties that can be adjusted and other useful meta-data that is often useful for host applications to work with plugin format in a standardized way.

ISF is simply a set of agreed upon conventions for writing GLSL shaders that makes it possible for different software to use them without having to make custom versions for each environment.

Some of the specific ways that ISF extends GLSL is by providing:
- An interface for shaders to describe their parameters to host applications.  This is done by providing a commented out JSON-blob in the space above the GLSL source code.
- A set of standard uniform variables, such as TIME, FRAMEINDEX, RENDERSIZE and isf_FragNormCoord.
- A convention for working with multiple passes for creating compound shader chains within a single file.
- The ability to create persistent buffers that can be used to store data in between render passes.
- The ability to resize texture inputs to specific sizes before rendering.
- A standard input convention for working with raw multichannel audio and audio FFT data.
- A set of standard functions for accessing texture size and color information that work with 2D or Rect textures.
- Though ISF allows for an optional vertex shader, in most cases they are not needed and when not included a host application will automatically provide one.

Currently the ISF Specification is used to describe generators, filters and transitions for 2D images. A proposed future version adds support for working with 3D geometry.  Developers writing GLSL shaders for their own applications may still find it useful to follow the ISF specification where possible as it was designed to be extensible for their own custom use cases, but should make it clear to end-users that these shaders may not work as expected in other host applications.

### Why use GLSL / ISF instead of other languages for creative coding?

Though GLSL and ISF themselves are limited in their overall capabilities - you are limited to what is possible by using vertex and fragment shaders and it often requires a host application to load the documents - whenever possible it can be beneficial to write your generators and FX in this format for a variety of reasons:
- By its very nature ISF encourages writing small, re-usable code files that can be used in a variety of different environments.
- Because it is an open format, it will be less likely that the ISF files you make will need to be recreated in a different language should you need to transition to different software in the future.
- The same generator / FX files can be made to run on desktop, mobile and webgl platforms, often with little or no modifications.

In these ways, ISF files are more flexible than using a language like Processing or OpenFrameworks, which are overall can create vastly more complex projects, but are difficult to re-use in other environments.  As GLSL itself is supported in many creative coding languages, learning how to write shaders and using them whenever possible in those environments can work to your advantage in many ways.

## Getting Started Writing ISF Shaders

In these tutorials we'll look at how to write GLSL shaders that meet the ISF specification and explore some of the advantages over using traditional GLSL.

### Tools and Downloads

Before starting creating your own shaders, you may want to have the following on hand:
- You can write ISF generators and FX using any standard text editor, or you can use a dedicated tool such as the free [ISF Editor for Mac](https://www.vidvox.net/download/ISF_Editor_2.9.7.3.dmg), or use the [isf.video](http://interactiveshaderformat.com) web interface to create your sketches.
- The [ISF Test/Tutorial filters](http://vidvox.net/rays_oddsnends/ISF%20tests+tutorials.zip) is a useful collection of reference and test shaders.  They aren't particularly interesting but they do a good job of demonstrating specific functionality.

### Additional GLSL Resources

Beyond the lessons and examples provided here and on the [ISF website](http://interactiveshaderformat.com), there are many other great websites dedicated to teaching and sharing of GLSL shaders.  Here are a few of our favorites:

- [The Book Of Shaders](https://thebookofshaders.com/): The current defacto standard introduction to GLSL for the arts.
- [Fragment Foundry](http://hughsk.io/fragment-foundry/chapters/01-hello-world.html): Great interactive walkthrough of basic concepts in shader writing.
- [ShaderToy](http://shadertoy.com): Popular website for sharing shaders.
- [GLSL Sandbox](http://glslsandbox.com/): Sandbox for writing and publishing shaders.

### Next steps...

Moving right along, in chapter 2 we'll write our first GLSL shaders in ISF and take a closer look at the anatomy of an ISF document.
