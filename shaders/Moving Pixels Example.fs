/*{
	"DESCRIPTION": "Shift pixels to the left",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"Examples"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}
	]
}*/

void main() {
	vec2	loc = isf_FragNormCoord;
	float	shift = TIME;
	loc.x += shift;
	loc.x = mod(loc.x,1.0);
	gl_FragColor = IMG_NORM_PIXEL(inputImage,loc);
}