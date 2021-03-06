/*{
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Distortion Effect", "Examples"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "radius",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 5.0
		},
		{
			"NAME": "amount",
			"TYPE": "float",
			"MIN": -10.0,
			"MAX": 10.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "center",
			"TYPE": "point2D",
			"DEFAULT": [
				0.5,
				0.5
			],
			"MIN": [
				0.0,
				0.0
			],
			"MAX": [
				1.0,
				1.0
			]
		}
	]
}*/


const float pi = 3.14159265359;


void main (void)
{
	vec2 uv = isf_FragNormCoord;
	vec2 texSize = RENDERSIZE;
	vec2 tc = uv * texSize;
	float radius_sized = radius * max(RENDERSIZE.x,RENDERSIZE.y);
	tc -= center * texSize;
	float dist = length(tc);
	if (dist < radius_sized) 	{
		float percent = (radius_sized - dist) / radius_sized;
		float theta = percent * percent * amount * 2.0 * pi;
		float s = sin(theta);
		float c = cos(theta);
		tc = vec2(dot(tc, vec2(c, -s)), dot(tc, vec2(s, c)));
	}
	tc += center * texSize;
	vec2 loc = tc / texSize;
	vec4 color = IMG_NORM_PIXEL(inputImage, loc);

	if ((loc.x < 0.0)||(loc.y < 0.0)||(loc.x > 1.0)||(loc.y > 1.0))	{
		gl_FragColor = vec4(0.0);
	}
	else	{
		gl_FragColor = color;
	}
}