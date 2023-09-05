Shader "Custom/Window" {
    SubShader {

        Tags { "Queue"="Geometry-1" }
        ColorMask 0
        Zwrite off

        Pass {
            Stencil {
                Ref 1
                Comp Always
                Pass Replace
            }
        }
    }    
}
