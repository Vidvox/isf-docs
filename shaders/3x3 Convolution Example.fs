/*{
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Blur", "Examples"
	],
	"TAGS": [
		"Convolution"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "w00",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w10",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w20",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w01",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w11",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "w21",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w02",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w12",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "w22",
			"TYPE": "float",
			"MIN": -8.0,
			"MAX": 8.0,
			"DEFAULT": 0.0
		}
	]
}*/


varying vec2 left_coord;
varying vec2 right_coord;
varying vec2 above_coord;
varying vec2 below_coord;

varying vec2 lefta_coord;
varying vec2 righta_coord;
varying vec2 leftb_coord;
varying vec2 rightb_coord;


void main()
{
	vec4 colorLA = IMG_NORM_PIXEL(inputImage, lefta_coord);
	vec4 colorA = IMG_NORM_PIXEL(inputImage, above_coord);
	vec4 colorRA = IMG_NORM_PIXEL(inputImage, righta_coord);

	vec4 colorL = IMG_NORM_PIXEL(inputImage, left_coord);
	vec4 color = IMG_THIS_NORM_PIXEL(inputImage);
	vec4 colorR = IMG_NORM_PIXEL(inputImage, right_coord);
	
	vec4 colorLB = IMG_NORM_PIXEL(inputImage, leftb_coord);
	vec4 colorB = IMG_NORM_PIXEL(inputImage, below_coord);
	vec4 colorRB = IMG_NORM_PIXEL(inputImage, rightb_coord);

	//	make the average for the RGB values
	vec3 convolution = (w11 * color + w01 * colorL + w21 * colorR + w10 * colorA + w12 * colorB + w00 * colorLA + w20 * colorRA + w02 * colorLB + w22 * colorRB).rgb;
	
	//	keep the alpha the same as the original pixel
	gl_FragColor = vec4(convolution,color.a);
}