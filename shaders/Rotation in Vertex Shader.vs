//	Rotate.vs
varying vec2 translated_coord;

const float pi = 3.14159265359;

void main()	{
	isf_vertShaderInit();
	
	//	'loc' is the location in pixels of this vertex.  we're going to convert this to polar coordinates (radius/angle)
	vec2		loc = RENDERSIZE * vec2(isf_FragNormCoord[0],isf_FragNormCoord[1]);
	//	'r' is the radius- the distance in pixels from 'loc' to the center of the rendering space
	float		r = distance(RENDERSIZE/2.0, loc);
	//	'a' is the angle of the line segment from the center to loc is rotated
	float		a = atan ((loc.y-RENDERSIZE.y/2.0),(loc.x-RENDERSIZE.x/2.0));
	
	//	now modify 'a', and convert the modified polar coords (radius/angle) back to cartesian coords (x/y pixels)
	loc.x = r * cos(a + 2.0 * pi * angle);
	loc.y = r * sin(a + 2.0 * pi * angle);
	
	translated_coord = loc / RENDERSIZE + vec2(0.5);
}