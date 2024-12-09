shader_type spatial;


uniform float water_height_global;
uniform vec3 dip_color : source_color;
varying vec3 model_y_to_water;

void vertex() {
	// Called for every vertex the material is visible on.
	model_y_to_water = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz
	 - vec3(0.0, water_height_global, 0.0);
}

void fragment() {
	// Called for every pixel the material is visible on.
	float s = step(0.0, model_y_to_water.y);
	ALBEDO = (1.0 - s) * dip_color + s * vec3(1.0, 1.0, 1.0);
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
