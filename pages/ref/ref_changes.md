---
title: ISF JSON Reference
tags: [Reference, Changes]
keywords: PERSISTENT_BUFFERS, vv_FragNormCoord, vv_vertShaderInit(), VSN, ISFVSN, TIMEDELTA, DATE, FRAMEINDEX
last_updated: June 12, 2018
summary: "Changes to the ISF specification."
sidebar: home_sidebar
permalink: ref_changes.html
folder: ref
---

# Change Log

Currently ISF is at version 2.0 with a proposed version 3.0 in development.

## Differences from the first version of the ISF spec

The first version of the ISF spec did some confusing and silly things that the second version improves on.  If you want to write your own ISF host, and you want that host to support "old" ISF files, here's a link to [the original ISF spec.](http://www.vidvox.net/rays_oddsnends/ISF_v1.md)

...and here's a list of the specific changes that were made from ISFVSN 1 to ISFVSN 2:

- The `PERSISTENT_BUFFERS` object in the top-level dict has been removed- it was redundant and confusing. Anything describing a property of the buffer a pass renders to is in the appropriate pass dictionary (`PERSISTENT`, `FLOAT`, `WIDTH`, `HEIGHT`, etc).
- The uniform vec2 `vv_FragNormCoord` has been renamed `isf_FragNormCoord`, and the function `vv_vertShaderInit()` has been renamed `isf_vertShaderInit()`.  ISF is open-source, the use of "vv" for terms in the spec is inappropriate.
- The `INPUT` types "audio" and "audioFFT" didn't exist in the first version of the ISF spec.
- The `IMG_SIZE` function didn't exist in the first version of the ISF spec.
- The `TIMEDELTA`, `DATE`, and `FRAMEINDEX` uniforms didn't exist in the first version of the ISF spec.
- The first version of the ISF spec didn't have any sort of versioning label- `VSN` and `ISFVSN` didn't exist.
- The first version of the ISF spec didn't codify any conventions with respect to image filters.  The second version sets forth the basic requirement for ISF shaders that are expected to be used as image filters or transitions.