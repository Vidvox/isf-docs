//	passthru.fs
/*{
	"DESCRIPTION": "Passes through each pixel",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
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

//	Click on the VS button to view the vertex shader

varying vec2 translated_coord;

void main() {
	//	uses the translated_coord provided from the .vs
	gl_FragColor = IMG_NORM_PIXEL(inputImage,translated_coord);
}