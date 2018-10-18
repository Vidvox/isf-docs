/*{
	"DESCRIPTION": "creates a simple inverting feedback effect",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"Examples"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "fxAmount",
			"TYPE": "float",
			"MIN": "0",
			"MAX": "1",
			"DEFAULT": "0.9"
		}
	],
	"PASSES": [
		{
			"TARGET": "bufferVariableNameA",
			"PERSISTENT": true
		}
	]
	
}*/

//	note that the optional 'FLOAT' tag does not work in webgl for browser based examples
//	but on desktop you can replace the PASSES section with the following...
/*
	"PASSES": [
		{
			"TARGET": "bufferVariableNameA",
			"PERSISTENT": true,
			"FLOAT": true
		}
*/

void main()
{
	vec4		freshPixel = IMG_THIS_PIXEL(inputImage);
	vec4		stalePixel = IMG_THIS_PIXEL(bufferVariableNameA);
	stalePixel.rgb = 1.0 - stalePixel.rgb;
	gl_FragColor = mix(freshPixel,stalePixel,fxAmount);
}