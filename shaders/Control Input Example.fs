/*{
	"DESCRIPTION": "Demonstrates a float input",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"CATEGORIES": [
		"Quick Start", "Examples"
	],
	"INPUTS": [
		{
			"NAME": "level",
			"TYPE": "float",
			"LABEL": "Gray Level",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		}
	]
}*/

void main() {
	gl_FragColor = vec4(level,level,level,1.0);
}
