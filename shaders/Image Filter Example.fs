/*{
	"DESCRIPTION": "Demonstrates an invert image filter",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"Quick Start", "Examples"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}
	]
}*/

vec3 invertColor (vec3 c)
{
	return 1.0 - c;
}

void main()
{
	vec4		srcPixel = IMG_NORM_PIXEL(inputImage,isf_FragNormCoord);
	srcPixel.rgb = invertColor(srcPixel.rgb);
	gl_FragColor = srcPixel;
}
