// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Planets_Shader"
{
	Properties
	{
		_MainTex("Surface", 2D) = "white" {}
		_Pattern("Pattern", 2D) = "white" {}
		_ColorA("ColorA", Color) = (1,0.4198113,0.5902162,0)
		_ColorB("ColorB", Color) = (0.9245283,0.8883796,0.6236205,0)
		_EmissiveStrength("EmissiveStrength", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Pattern;
		uniform float4 _Pattern_ST;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _EmissiveStrength;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Pattern = i.uv_texcoord * _Pattern_ST.xy + _Pattern_ST.zw;
			float4 tex2DNode5 = tex2D( _Pattern, uv_Pattern );
			float4 ColorA93 = _ColorA;
			float4 ColorB94 = _ColorB;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode6 = tex2D( _MainTex, uv_MainTex );
			o.Emission = ( ( ( ( ( tex2DNode5.r * ColorA93 ) + ( tex2DNode5.b * ColorB94 ) ) * CalculateContrast(( tex2DNode6.r * 0.87 ),tex2DNode6) ) + ( ColorB94 * float4( 0,0,0,0 ) ) ) * _EmissiveStrength ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
125;106;1366;741;2904.048;443.7565;3.233329;True;True
Node;AmplifyShaderEditor.CommentaryNode;4;-2765.656,158.3085;Inherit;False;285;442;;2;2;3;INGREDIENT B;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;3;-2715.656,388.3085;Inherit;False;Property;_ColorB;ColorB;3;0;Create;True;0;0;False;0;0.9245283,0.8883796,0.6236205,0;0.9245283,0.8883796,0.6236205,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-2703.656,206.3086;Inherit;False;Property;_ColorA;ColorA;2;0;Create;True;0;0;False;0;1,0.4198113,0.5902162,0;1,0.4198113,0.5902162,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;-2433.702,392.0477;Inherit;False;ColorB;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;100;-1243.532,80.64266;Inherit;False;1023.632;668.4438;Comment;8;70;95;71;96;72;77;85;83;PlanetColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;91;-2031.679,116.0328;Inherit;False;380.8546;568.9429;Comment;2;5;6;INGREDIENT A;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;93;-2422.702,211.0477;Inherit;False;ColorA;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;-1193.532,149.5577;Inherit;False;93;ColorA;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;6;-1970.824,454.9756;Inherit;True;Property;_MainTex;Surface;0;0;Create;False;0;0;False;0;-1;fb2261194771d99468df5da92e3f51af;f1baa92f8dd443d428965129c6148f4e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1981.678,166.0327;Inherit;True;Property;_Pattern;Pattern;1;0;Create;True;0;0;False;0;-1;697c249f13af38b4d94563e29d010465;fae2d328e91d37b4f823d061c4776ced;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;96;-1191.432,282.2577;Inherit;False;94;ColorB;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1047.886,495.0865;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;102;-570.8592,792.1722;Inherit;False;490.5748;308.3514;ExtraColor;2;88;97;ExtraColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-986.195,130.6427;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-987.1635,263.4354;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;85;-788.1197,430.8569;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0.37;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-811.2952,195.1427;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;97;-520.8592,842.1722;Inherit;False;94;ColorB;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-505.6004,339.2391;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-316.2845,846.5236;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;13.48148,612.9117;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-9.842651,831.0476;Inherit;False;Property;_EmissiveStrength;EmissiveStrength;5;0;Create;True;0;0;False;0;0;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;98;-1579.439,549.5427;Inherit;False;SurfaceAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;245.1573,572.0476;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;505.3264,470.2258;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Planets_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;False;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;4;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;94;0;3;0
WireConnection;93;0;2;0
WireConnection;83;0;6;1
WireConnection;70;0;5;1
WireConnection;70;1;95;0
WireConnection;71;0;5;3
WireConnection;71;1;96;0
WireConnection;85;1;6;0
WireConnection;85;0;83;0
WireConnection;72;0;70;0
WireConnection;72;1;71;0
WireConnection;77;0;72;0
WireConnection;77;1;85;0
WireConnection;88;0;97;0
WireConnection;89;0;77;0
WireConnection;89;1;88;0
WireConnection;98;0;6;4
WireConnection;103;0;89;0
WireConnection;103;1;104;0
WireConnection;0;2;103;0
ASEEND*/
//CHKSM=BF7413726427E75184812B7D88C8932C7D4D7158