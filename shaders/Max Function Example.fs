/*
{
  "CATEGORIES" : [
    "Examples"
  ],
  "DESCRIPTION" : "Performs a component-wise maximum comparison",
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

void main()
{
	//	return a component-wise maximum
	gl_FragColor = max(color1,color2);
}