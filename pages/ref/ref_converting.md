---
title: ISF JSON Reference
tags: [Reference, Converting Shaders]
keywords: texture2D(), texture2DRect(), IMG_NORM_PIXEL(), IMG_PIXEL(), GLSL, gl_FragCoord.xy, translate, convert
last_updated: June 12, 2018
summary: "Tips for converting GLSL shaders into the ISF specification."
sidebar: home_sidebar
permalink: ref_converting.html
folder: ref
---

# Converting Non-ISF GLSL shaders to ISF

- You should probably replace any calls in your shader to `texture2D()` or `texture2DRect()` with `IMG_NORM_PIXEL()` or `IMG_PIXEL()`, respectively. Images in ISF- inputs, persistent buffers, etc- can be accessed by either `IMG_NORM_PIXEL()` or `IMG_PIXEL()`, depending on whether you want to use normalized or non-normalized coordinates to access the colors of the image. If your shader isn't using these- if it's using `texture2D()` or `texture2DRect()`- it won't compile if the host application tries to send it a different type of texture.
- Many shaders pass in the resolution of the image being rendered (knowing where the fragment being evaluated is located within the output image is frequently useful). By default, ISF automatically declares a uniform vec2 named `RENDERSIZE` which is passed the dimensions of the image being rendered.
- If the shader you're converting requires a time value, note that the uniform float `TIME` is declared, and passed the duration (in seconds) which the shader's been runing when the shader's rendered.
- Many shaders don't use (or even acknowledge) the alpha channel of the image being rendered. There's nothing wrong with this- but when the shader's loaded in an application that uses the alpha channel, the output of the shader can look bizarre and unpredictable (though it usually involves something being darker than it should be). If you run into this, try setting gl_FragColor.a to 1.0 at the end of your shader.
- `gl_FragCoord.xy` contains the coordinates of the fragment being evaluated. `isf_FragNormCoord.xy` contains the normalized coordinates of the fragment being evaluated.  
- While ISF files are fragment shaders, and the host environment automatically generates a vertex shader, you can use your own vertex shader if you'd like. If you go this route, your vertex shader should have the same base name as your ISF file (just use the extension .vs), and the first thing you do in your vertex shader's main function is call `isf_vertShaderInit();`.
- If the shader you're converting requires imported graphic resources, note that the ISF format defines the ability to import image files by adding objects to your JSON dict under the `IMPORTED` key. The imported images are accessed via the usual `IMG_PIXEL()` or `IMG_NORM_PIXEL()` methods. Details on how to do this are listed below, and examples are included.
- If your texture doesn't look right, make sure your texture coordinates are ranged properly (textures are typically "clamped" by the host implementation, if you specify an out-of-range texture coordinate it may look funny).