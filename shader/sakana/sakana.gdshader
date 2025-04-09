shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	float start = 0.4;
	vec2 uv_sakana = UV;
	float u = UV.x;
	float u_add = smoothstep(start-0.1, start+0.1, UV.x) * (16.0 * (u-start) * (u-start));
	float lerp = 0.5 * (sin(TIME * 30.0) + 1.0);
	uv_sakana.x += 0.0 * (1.0 - lerp) + u_add * lerp;
	COLOR = texture(TEXTURE, uv_sakana);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
