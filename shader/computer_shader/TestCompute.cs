using Godot;
using Godot.Collections;
using System;

public partial class TestCompute : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create compute shader
		RenderingDevice rd = RenderingServer.CreateLocalRenderingDevice();
		RDShaderFile shaderFile = GD.Load<RDShaderFile>("res://shader/computer_shader/compute.comp.glsl");
		RDShaderSpirV shaderSpirV = shaderFile.GetSpirV();
		Rid shaderRid = rd.ShaderCreateFromSpirV(shaderSpirV);

		// Convert data to byte
		float[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		byte[] inputBytes = new byte[input.Length * sizeof(float)];
		Buffer.BlockCopy(input, 0, inputBytes, 0, inputBytes.Length);

		float[] inputAnother = { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
		byte[] inputBytesAnother = new byte[inputAnother.Length * sizeof(float)];
		Buffer.BlockCopy(inputAnother, 0, inputBytesAnother, 0, inputBytesAnother.Length);

		// Create buffer
		Rid bufferRid = rd.StorageBufferCreate((uint)inputBytes.Length, inputBytes);
		Rid bufferAnotherRid = rd.StorageBufferCreate((uint)inputBytesAnother.Length, inputBytesAnother);
		
		// Create uniform, bind buffer
		RDUniform uniform = new()
		{
			UniformType = RenderingDevice.UniformType.StorageBuffer,
			Binding = 0 // Layout binding
		};
		uniform.AddId(bufferRid);

		RDUniform uniformAnother = new()
		{
			UniformType = RenderingDevice.UniformType.StorageBuffer,
			Binding = 1 // Layout binding
		};
		uniformAnother.AddId(bufferAnotherRid);

		// Pack uniform set
		Array<RDUniform> uniforms = new() { uniform, uniformAnother };
		Rid uniformSet = rd.UniformSetCreate(uniforms, shaderRid, 2); // Layout set

		// Create pipeline
		Rid pipelineRid = rd.ComputePipelineCreate(shaderRid);

		/*
		1. Begin instruction list for GPU to execute
		2. Assign pipeline
		3. Assign uniform set (setIdx => layout set)
		4. Specify workgroup
		5. End instruction list
		*/
		long computeList = rd.ComputeListBegin();
		rd.ComputeListBindComputePipeline(computeList, pipelineRid);
		rd.ComputeListBindUniformSet(computeList, uniformSet, 0); // Layout set
		rd.ComputeListBindUniformSet(computeList, uniformSet, 2); // Layout set
		rd.ComputeListDispatch(computeList, xGroups: 6, yGroups: 1, zGroups: 1);
		rd.ComputeListEnd();

		// Submit and wait
		rd.Submit();
		rd.Sync();

		// Read buffer
		byte[] outputBytes = rd.BufferGetData(bufferRid);
		float[] output = new float[input.Length];
		Buffer.BlockCopy(outputBytes, 0, output, 0, outputBytes.Length);
		GD.Print("Output: ", string.Join(", ", output));

		
		byte[] outputBytesAnother = rd.BufferGetData(bufferAnotherRid);
		float[] outputAnother = new float[inputAnother.Length];
		Buffer.BlockCopy(outputBytesAnother, 0, outputAnother, 0, outputBytesAnother.Length);
		GD.Print("Output: ", string.Join(", ", outputAnother));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
