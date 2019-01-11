Shader "Lines/ColorBlended"
{
    SubShader
    {
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Zwrite Off Cull Off Fog { Mode Off }
            BindChannels { Bind "vertex", vertex Bind "color", color }
        }
    }
}
