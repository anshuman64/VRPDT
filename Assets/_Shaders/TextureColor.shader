Shader "MyShaders/Mobile + Texture + Color" 
{
	Properties
	{
		[HDR] _Color("Main Color", Color) = (1, 1, 1, 1)    
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Tags{ "LightMode" = "Vertex" }
			Lighting Off

			SetTexture[_MainTex]
			{
				constantColor[_Color]
				combine texture * constant 
			}
		}
	}
}