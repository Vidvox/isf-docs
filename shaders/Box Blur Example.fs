/*{
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Blur", "Examples"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "blurLevel",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
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

	float mWeight = 1.0 - blurLevel;
	float nWeight = blurLevel / 8.0;
	
	vec4 color = IMG_THIS_NORM_PIXEL(inputImage);
	
	//	note that we can skip the pixel lookups here if nWeight is 0.0
	vec4 colorL = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, left_coord) : vec4(0.0);
	vec4 colorR = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, right_coord) : vec4(0.0);
	vec4 colorA = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, above_coord) : vec4(0.0);
	vec4 colorB = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, below_coord) : vec4(0.0);

	vec4 colorLA = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, lefta_coord) : vec4(0.0);
	vec4 colorRA = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, righta_coord) : vec4(0.0);
	vec4 colorLB = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, leftb_coord) : vec4(0.0);
	vec4 colorRB = (nWeight > 0.0) ? IMG_NORM_PIXEL(inputImage, rightb_coord) : vec4(0.0);

	vec3 blur = ((mWeight * color) + nWeight * (colorL + colorR + colorA + colorB + colorLA + colorRA + colorLB + colorRB)).rgb;
	
	gl_FragColor = vec4(blur,color.a);
}