Shader "Custom/Solid" {
	Subshader {
		Pass {
			Stencil {
				REf 1
				comp Equal
			}
		}
	}
    
}
