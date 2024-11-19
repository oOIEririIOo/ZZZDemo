Shader "FS/FSEffect"
{
    Properties
    {
        [Enum(Off,0,Front,1,Back,2)]_CullMode("剔除模式",float)=0
        _BaseMap ("主贴图", 2D) = "white" {}
        [Gamma][HDR]_BaseColor("主颜色",COLOR) = (1,1,1,1)
        [Toggle]_MixAlpha("RGB混合Alpha",float)=0
        [Toggle]_TillingBase("特效控制偏移",float)=0
        //遮罩
        [Enum(R,0,A,1)]_MaskChannel("遮罩通道",float) = 0
        _MaskTex("遮罩图",2D) = "white"{}
        _MaskScale("遮罩强度",Range(0,1))=1
        
        //扭曲
        _NoiseTex("扭曲图",2D) = "white"{}
        [Toggle]_NoiseU2("特效控制扭曲强度",float)=0
        _NoiseScale("扭曲强度X",float) = 1
        //溶解
        _DissolveTex("溶解贴图",2D) = "white" {}
        [Toggle]_DissolvelU2("特效控制溶解值",float)=0
        _DissolveInt("溶解值",Range(0,1)) = 0
        _DissolveHardness("边缘硬度",Range(0,1)) = 1
        _DissolveEdg("边缘宽度",Range(0,1)) = 0
        [Gamma][HDR]_DissolveColor("边缘颜色",Color) = (1,1,1,1)
        //纹理流动
        _MainMaskFlow("主贴图(xy)遮罩(zw)速度",Vector) = (0,0,0,0)
        _NoiseDissFlow("扭曲(xy)溶解(zw)速度",Vector) = (0,0,0,0)
        //软粒子平面
        _SoftDistance("软粒子距离",float)=0.5
        //其它
        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend("Src Blend", float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]_DstBlend("Dst Blend", float) = 10
        [Enum(Off,0,On,1)]_ZWriteMode("ZWrite Mode", Int) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)]_ZTestMode("ZTest Mode", Int) = 4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline""Queue"="Transparent"}
        LOD 100
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        CBUFFER_START(UnityPerMaterial)
        half4 _BaseColor;
        float4 _BaseMap_ST;
        half _MixAlpha;
        half _TillingBase;
        
        half _MaskChannel;
        float4 _MaskTex_ST;
        half _MaskScale;

        float4 _NoiseTex_ST;
        half _NoiseScale;
        half _NoiseU2;
        
        half4 _DissolveTex_ST;
        half _DissolvelU2;
        half _DissolveInt;
        half _DissolveHardness;
        half _DissolveEdg;
        half4 _DissolveColor;

        half4 _MainMaskFlow;
        half4 _NoiseDissFlow;

        half _SoftDistance;
        CBUFFER_END
        
        TEXTURE2D(_BaseMap);
        SAMPLER(sampler_BaseMap);

        #ifdef _USEMASK_ON
        TEXTURE2D(_MaskTex);
        SAMPLER(sampler_MaskTex);
        #endif

        #ifdef _USENOISE_ON
        TEXTURE2D(_NoiseTex);
        SAMPLER(sampler_NoiseTex);
        #endif

        #ifdef _USEDISSOLVE_ON
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        #endif

        #ifdef _SOFTPARTICAL_ON
        TEXTURE2D_X_FLOAT(_CameraDepthTexture);
        SAMPLER(sampler_CameraDepthTexture);
        #endif

        ENDHLSL
        
        Pass
        {
            Blend [_SrcBlend] [_DstBlend]
            Cull [_CullMode]
            ZWrite [_ZWriteMode]
            ZTest [_ZTestMode]
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature_local _USEMASK_ON
            #pragma shader_feature_local _USENOISE_ON
            #pragma shader_feature_local _USEDISSOLVE_ON
            #pragma shader_feature_local _FLOWMAP_ON
            #pragma shader_feature_local _SOFTPARTICAL_ON

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
                float4 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                #ifdef _USEMASK_ON
                float4 uv : TEXCOORD0;
                #else
                float2 uv : TEXCOORD0;
                #endif

                float4 uv2 : TEXCOORD1;

                #if defined(_USENOISE_ON) || defined(_USEDISSOLVE_ON)
                float4 noiseDissUV : TEXCOORD2;
                #endif 
                #if defined(_USEDISSOLVE_ON)
                float2 dissCoord : TEXCOORD3;
                #endif
                
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
                
                #ifdef _SOFTPARTICAL_ON
                float4 screenPos : TEXCOORD4;
                #endif
                
            };
            

            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                #ifdef _SOFTPARTICAL_ON
                o.screenPos = ComputeScreenPos(o.vertex);
                #endif
                #ifdef _FLOWMAP_ON
                float2 mainFlow = float2(_MainMaskFlow.x,_MainMaskFlow.y)*_Time.y;
                float2 maskFlow = float2(_MainMaskFlow.z,_MainMaskFlow.w)*_Time.y;
                float2 noiseFlow = float2(_NoiseDissFlow.x,_NoiseDissFlow.y)*_Time.y;
                float2 dissFlow = float2(_NoiseDissFlow.z,_NoiseDissFlow.w)*_Time.y;
                #endif

                o.uv2 = v.uv2;
                
                o.uv.xy = TRANSFORM_TEX(v.uv, _BaseMap);
                o.uv.xy += half2(o.uv2.x,o.uv2.y)*_TillingBase;
                #ifdef _FLOWMAP_ON
                o.uv.xy += mainFlow;
                #endif
                
                #ifdef _USEMASK_ON
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex);
                    #ifdef _FLOWMAP_ON
                    o.uv.zw += maskFlow;
                    #endif
                #endif
                #if defined(_USENOISE_ON) || defined(_USEDISSOLVE_ON)
                o.noiseDissUV = float4(1,1,1,1);
                #endif
                                
                #ifdef _USENOISE_ON
                o.noiseDissUV.xy = TRANSFORM_TEX(v.uv,_NoiseTex);
                    #ifdef _FLOWMAP_ON
                    o.noiseDissUV.xy += noiseFlow;
                    #endif
                #endif
                
                #ifdef _USEDISSOLVE_ON
                half hard = _DissolveHardness*0.99f;//防止1-导致分母为0
                half edg = _DissolveEdg + 1;
                _DissolveInt = lerp(_DissolveInt,o.uv2.w,_DissolvelU2);
                _DissolveInt = 1 - _DissolveInt;
                o.dissCoord.x = (_DissolveInt * edg + 1 - hard)*(2-hard) - edg;
                o.dissCoord.y = _DissolveInt * edg*(2-hard)-1;
                o.noiseDissUV.zw = TRANSFORM_TEX(v.uv,_DissolveTex);
                    #ifdef _FLOWMAP_ON
                    o.noiseDissUV.zw += dissFlow;
                    #endif
                #endif
                
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 mainUV = i.uv.xy;
                #ifdef _USENOISE_ON
                half4 noiseTex = SAMPLE_TEXTURE2D(_NoiseTex,sampler_NoiseTex,i.noiseDissUV.xy);
                half noise =  (noiseTex.r);
                _NoiseScale = lerp(_NoiseScale,i.uv2.z,_NoiseU2);
                mainUV += noiseTex.r*_NoiseScale;
                #endif
                
                half4 col = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, mainUV)*_BaseColor * i.color;
                #ifdef _USEMASK_ON
                half4 maskTex = SAMPLE_TEXTURE2D(_MaskTex,sampler_MaskTex,i.uv.zw);
                half mask = maskTex.r*(1-_MaskChannel)+maskTex.a*(_MaskChannel);
                mask = lerp(1,mask,_MaskScale);
                col.a *= mask;
                #endif

                #ifdef _USEDISSOLVE_ON
                half4 dissTex = SAMPLE_TEXTURE2D(_DissolveTex,sampler_DissolveTex,i.noiseDissUV.zw);
                half dissMask = pow(abs(dissTex.r),1/2.2f);
                half hard = _DissolveHardness * 0.99f;//防止1-导致分母为0
                
                half dissColorLerp =  saturate((dissMask + i.dissCoord.x) / (1-hard));
                col.rgb = lerp(_DissolveColor.rgb,col.rgb,dissColorLerp);

                half dissAlpha = saturate((dissMask + i.dissCoord.y) / (1-hard));
                col.a *= dissAlpha;
                #endif

                #ifdef _SOFTPARTICAL_ON
                float4 screenPosNormal = i.screenPos / i.screenPos.w;
                screenPosNormal.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNormal.z : screenPosNormal.z * 0.5 + 0.5;
                float screenDepthTexture = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex( screenPosNormal.xy)).r;
                float screenDepth = LinearEyeDepth(screenDepthTexture,_ZBufferParams);
                float distanceDepth = saturate(abs( ( screenDepth - LinearEyeDepth( screenPosNormal.z,_ZBufferParams ) ) / ( _SoftDistance + (0.1e-16) ) ));
                col.a *= distanceDepth;
                #endif
                
                
                half mixAlpha = lerp(1,col.a,_MixAlpha);
                col.rgb *= mixAlpha;
                return col;
            }
            ENDHLSL
        }
    }
    CustomEditor "EffectShaderGUI"
}
