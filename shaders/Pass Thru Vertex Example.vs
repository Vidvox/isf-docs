//	passthru.vs
//	put your code in the main() {} function

varying vec2 translated_coord;

void main()
{
	//	make sure to call this in your custom ISF shaders to perform initial setup!
	isf_vertShaderInit();
	translated_coord = isf_FragNormCoord;
}