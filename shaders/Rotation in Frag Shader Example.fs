/*
{
  "CATEGORIES" : [
    "Geometry Adjustment", "Examples"
  ],
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "angle",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : 0
    }
  ],
  "CREDIT" : "by VIDVOX"
}
*/

const float pi = 3.14159265359;

void main()	{
	
	//	'loc' is the location in pixels of this vertex.  we're going to convert this to polar coordinates (radius/angle)
	vec2		loc = RENDERSIZE * vec2(isf_FragNormCoord.x,isf_FragNormCoord.y);
	//	'r' is the radius- the distance in pixels from 'loc' to the center of the rendering space
	float		r = distance(RENDERSIZE/2.0, loc);
	//	'a' is the angle of the line segment from the center to loc is rotated
	float		a = atan((loc.y-RENDERSIZE.y/2.0),(loc.x-RENDERSIZE.x/2.0));
	
	//	now modify 'a', and convert the modified polar coords (radius/angle) back to cartesian coords (x/y pixels)
	loc.x = r * cos(a + 2.0 * pi * angle);
	loc.y = r * sin(a + 2.0 * pi * angle);
	
	loc = loc / RENDERSIZE + vec2(0.5);
	if ((loc.x < 0.0)||(loc.y < 0.0)||(loc.x > 1.0)||(loc.y > 1.0))	{
		gl_FragColor = vec4(0.0);
	}
	else	{
		gl_FragColor = IMG_NORM_PIXEL(inputImage,loc);
	}
}