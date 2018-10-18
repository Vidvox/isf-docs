/*{
	"DESCRIPTION": "Demonstrates the use of color-type image inputs",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"Quick Start", "Examples"
	],
	"INPUTS": [
		{
			"NAME": "theColor",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.5,
				0.0,
				1.0
			]
		}
	]
}*/

void main()
{
	gl_FragColor = theColor;
}