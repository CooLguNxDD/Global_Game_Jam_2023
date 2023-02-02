Shader "TiltBrush/UnlitSpecials/Toon"
{
    Properties
    {
		_OutlineMax ("Maximum Outline", Range (0,0.5)) = 0.01
	}

	HLSLINCLUDE
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

	CBUFFER_START(UnityPerMaterial)
		half _OutlineMax;
	CBUFFER_END

	struct Attributes
	{
		half4 positionOS	: POSITION;
		half4 color		: COLOR;
		half3 normalOS 	: NORMAL;
		half4 tangentOS    : TANGENT;
		half4 texcoord		: TEXCOORD0;

		UNITY_VERTEX_INPUT_INSTANCE_ID          
	};

	struct Varyings
	{
		half4 positionCS 	: SV_POSITION;
		half4 color		: COLOR;

		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};

	Varyings vertInflate(Attributes IN, half inflate)
	{
		Varyings OUT;

		UNITY_SETUP_INSTANCE_ID(IN);
		UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

		half outlineEnabled = inflate;

		VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
		VertexPositionInputs NDCInput = GetVertexPositionInputs(IN.positionOS.xyz + IN.normalOS.xyz * inflate);
		
		// Technically these are not yet in NDC because they haven't been divided by W, so their
		// range is currently [-W, W].
		half4 outline_NDC = NDCInput.positionCS;

		// Displacement in proper NDC coords (e.g. [-1, 1])
    	half3 disp = outline_NDC.xyz / outline_NDC.w - IN.positionOS.xyz / IN.positionOS.w;

		// Magnitude is a scaling factor to shrink large outlines down to a max width, in NDC space.
		// Notice here we're only measuring 2D displacment in X and Y.
		half mag = length(disp.xy);
		mag = min(_OutlineMax, mag) / mag;


		OUT.positionCS = vertexInput.positionCS;

		OUT.positionCS.xyz += half3(disp.xy * mag, disp.z) * IN.positionOS.w * outlineEnabled;

		// Push Z back to avoid z-fighting when scaled very small. This is not legit,
    	// mathematically speaking and likely causes crazy surface derivitives.
    	OUT.positionCS.z -= disp.z * IN.positionOS.w * outlineEnabled;
		
        // color
		VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
		OUT.color = IN.color;
		OUT.color.a = 1;
		OUT.color.rgb += normalInput.normalWS.y *.2;
		OUT.color.rgb = max(0, OUT.color.rgb);

		return OUT;
	}

	ENDHLSL

    SubShader
    {
        Tags { 
            "RenderPipeline" = "UniversalRenderPipeline"
            "IgnoreProjector"="True"
            "Queue" = "AlphaTest"
        }

        Pass
        {
			Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

			Cull Back
            ZTest LEqual
            ZWrite On
            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#pragma target 4.5

			Varyings vert (Attributes IN)
			{
				return vertInflate(IN,0.0);
			}

			half4 frag(Varyings IN) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				return IN.color;
			}

            ENDHLSL
        }

		Pass
		{
			Name "Outline"
			Cull Front
            HLSLPROGRAM

            #pragma vertex vertEdge
            #pragma fragment fragBlack
			#pragma target 4.5

			Varyings vertEdge(Attributes IN)
			{
				return vertInflate(IN,1.0);
			}


			half4 fragBlack (Varyings IN) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				return half4(0,0,0,1);
			}

            ENDHLSL
		}
    }
}