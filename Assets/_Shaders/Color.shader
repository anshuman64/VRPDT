Shader "MyShaders/Mobile + Color" 
{
	Properties
	{
		[HDR] _Color("Main Color", Color) = (1, 1, 1, 1)    
	}

	SubShader
	{
		Pass
		{
			Lighting Off

			Color[_Color]
		}
	}
}