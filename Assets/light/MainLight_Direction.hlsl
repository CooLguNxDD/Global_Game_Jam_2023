void MainLight_Direction_half(float3 WorldPos, out half3 Direction)
{
#if SHADERGRAPH_PREVIEW
      Direction = half3(0.5, 0.5, 0);
#else
  #if SHADOWS_SCREEN
      half4 clipPos = TransformWorldToHClip(WorldPos);
      half4 shadowCoord = ComputeScreenPos(clipPos);
  #else
      half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
  #endif


  Light mainLight = GetMainLight(shadowCoord);
  Direction = mainLight.direction;

#endif
}
