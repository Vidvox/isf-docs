/*{
	"DESCRIPTION": "Grayscale each pixel",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Color Effect"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}
	]
}*/

//	declaring the function, but not actually writing the code here...
float grayscaleAmount(vec4 color);

void main() {
	vec4	srcPixel = IMG_THIS_PIXEL(inputImage);
	float	grayLevel = grayscaleAmount(srcPixel);
	gl_FragColor = vec4(grayLevel,grayLevel,grayLevel,srcPixel.a);
}

//	actual code for the function after main()...
float grayscaleAmount(vec4 color)
{
	//	ignores alpha
	return (color.r + color.g + color.b) / 3.0;
}