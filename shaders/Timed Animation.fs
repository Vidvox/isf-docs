/*{
	"DESCRIPTION": "Test animation",
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2.0",
	"INPUTS": [],
	"CATEGORIES": [
		"Quick Start", "Examples"
	]
}*/

void main() {
	float val = (isf_FragNormCoord.x < fract(TIME)) ? 1.0 : 0.0;
	gl_FragColor = vec4(1.0,0.5,0.75,val);
}
