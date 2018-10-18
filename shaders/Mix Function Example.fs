/*{
	"DESCRIPTION": "Creates a linear gradient from one color to another",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"Examples"
	],
	"INPUTS": [
		{
			"NAME": "theColor1",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.5,
				0.0,
				1.0
			]
		},
		{
			"NAME": "theColor2",
			"TYPE": "color",
			"DEFAULT": [
				0.0,
				0.0,
				1.0,
				1.0
			]
		}
	]
}*/

void main()
{
	gl_FragColor = mix(theColor1,theColor2,isf_FragNormCoord.x);
}