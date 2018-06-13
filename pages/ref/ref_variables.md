---
title: ISF Variables Reference
tags: [Variables, Uniforms]
keywords: PASSINDEX, RENDERSIZE, isf_FragNormCoord, TIME, TIMEDELTA, DATE, FRAMEINDEX
last_updated: June 12, 2018
summary: "An overview of the variables available in ISF."
sidebar: home_sidebar
permalink: ref_variables.html
folder: ref
---

# Automatically Generated Variables In ISF

When writing shaders in the ISF Specification, the following variables are automatically created and available to use in your compositions.

## Automatically Declared Variables

- The uniform int `PASSINDEX` is automatically declared, and set to 0 on the first rendering pass. Subsequent passes (defined by the dicts in your `PASSES` array) increment this int.
- The uniform vec2 `RENDERSIZE` is automatically declared, and is set to the rendering size (in pixels) of the current rendering pass.
- The uniform vec2 `isf_FragNormCoord` is automatically declared. This is a convenience variable, and repesents the normalized coordinates of the current fragment ([0,0] is the bottom-left, [1,1] is the top-right).
- As part of standard GLSL vec4 `gl_FragCoord` is automatically declared. This holds the values of the fragment coordinate vector are given in the window coordinate system.  In 2D space the .xy from this can be used to get the non-normalized pixel location.
- The uniform float `TIME` is automatically declared, and is set to the current rendering time (in seconds) of the shader. This variable is updated once per rendered frame- if a frame requires multiple rendering passes, the variable is only updated once for all the passes.
- The uniform float `TIMEDELTA` is automatically declared, and is set to the time (in seconds) that have elapsed since the last frame was rendered.  This value will be 0.0 when rendering the first frame.
- The uniform vec4 `DATE` is automatically declared, and is used to pass the date and time to the shader.  The first element of the vector is the year, the second element is the month, the third element is the day, and the fourth element is the time (in seconds) within the day.
- The uniform int `FRAMEINDEX` is automatically declared, and is used to pass the index of the frame being rendered to the shader- this value is 0 when the first frame is rendered, and is incremented after each frame has finished rendering.