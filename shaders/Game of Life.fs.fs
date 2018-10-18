/*{
	"DESCRIPTION": "Based on Conway's Game of Life",
	"CREDIT": "VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "restartNow",
			"TYPE": "event"
		},
		{
			"NAME": "startThresh",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		}
	],
	"PASSES": [
		{
			"TARGET":"lastData",
			"PERSISTENT": true
		}
	]
	
}*/

/*

Any live cell with fewer than two live neighbours dies, as if caused by under-population.
Any live cell with two or three live neighbours lives on to the next generation.
Any live cell with more than three live neighbours dies, as if by over-population.
Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

*/

varying vec2 left_coord;
varying vec2 right_coord;
varying vec2 above_coord;
varying vec2 below_coord;

varying vec2 lefta_coord;
varying vec2 righta_coord;
varying vec2 leftb_coord;
varying vec2 rightb_coord;

//	used to get the grayscale version of a pixel
float gray(vec4 n)
{
	return (n.r + n.g + n.b)/3.0;
}

//	used to randomize the start state
float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main()	{
	vec4		inputPixelColor = vec4(0.0);
	vec2		loc = gl_FragCoord.xy;
	
	//	if we are starting or the restartNow event is active, randomize
	if ((FRAMEINDEX < 1)||(restartNow))	{
		//	randomize the start conditions
		float	alive = rand(vec2(TIME+1.0,2.1*TIME+0.1)*loc);
		if (alive > 1.0 - startThresh)	{
			inputPixelColor = vec4(1.0);
		}
	}
	else	{
		vec4	color = IMG_PIXEL(lastData, loc);
		vec4	colorL = IMG_PIXEL(lastData, left_coord);
		vec4	colorR = IMG_PIXEL(lastData, right_coord);
		vec4	colorA = IMG_PIXEL(lastData, above_coord);
		vec4	colorB = IMG_PIXEL(lastData, below_coord);

		vec4	colorLA = IMG_PIXEL(lastData, lefta_coord);
		vec4	colorRA = IMG_PIXEL(lastData, righta_coord);
		vec4	colorLB = IMG_PIXEL(lastData, leftb_coord);
		vec4	colorRB = IMG_PIXEL(lastData, rightb_coord);
		
		float	neighborSum = gray(colorL + colorR + colorA + colorB + colorLA + colorRA + colorLB + colorRB);
		float	state = gray(color);
		
		//	live cell
		if (state > 0.0)	{
			if (neighborSum < 2.0)	{
				//	under population
				inputPixelColor = vec4(0.0);
			}
			else if (neighborSum < 4.0)	{
				//	status quo
				inputPixelColor = vec4(1.0);
			}
			else	{
				//	over population
				inputPixelColor = vec4(0.0);
			}
		}
		//	dead cell
		else	{
			if ((neighborSum > 2.0)&&(neighborSum < 4.0))	{
				//	reproduction
				inputPixelColor = vec4(1.0);
			}
			else if (neighborSum < 2.0)	{
				//	stay dead
			}
		}
	}
	
	gl_FragColor = inputPixelColor;
}