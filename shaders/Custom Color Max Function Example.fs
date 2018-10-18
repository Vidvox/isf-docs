/*
{
  "CATEGORIES" : [
    "Examples"
  ],
  "DESCRIPTION" : "Returns the brighter of two colors taking alpha into account",
  "ISFVSN" : "2",
  "INPUTS" : [
	{
      "NAME" : "color1",
      "TYPE" : "color",
      "DEFAULT" : [
        0.5,
        0.5,
        0.75,
        1.0
      ]
    },
    {
      "NAME" : "color2",
      "TYPE" : "color",
      "DEFAULT" : [
        0.25,
        0.25,
        0.95,
        1.0
      ]
    }
  ],
  "CREDIT" : "by VIDVOX"
}
*/

//	returns the average color level, using alpha
float level (vec4 c)
{
	return c.a * (c.r + c.g + c.b) / 3.0;
}

void main()
{
	//	get the level for each input color
	float	gray1 = level(color1);
	float	gray2 = level(color2);
	//	if gray1 is greater than or equal to gray2 return color1, else return color2
	gl_FragColor = (gray1 >= gray2) ? color1 : color2;
}