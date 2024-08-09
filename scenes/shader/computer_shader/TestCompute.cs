using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class TestCompute : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create compute shader
		RenderingDevice rd = RenderingServer.CreateLocalRenderingDevice();
		RDShaderFile shaderFile = GD.Load<RDShaderFile>("res://scenes/shader/computer_shader/compute.comp.glsl");
		RDShaderSpirV shaderSpirV = shaderFile.GetSpirV();
		Rid shaderRid = rd.ShaderCreateFromSpirV(shaderSpirV);

		// Convert data to byte
		float[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		byte[] inputBytes = new byte[input.Length * sizeof(float)];
		Buffer.BlockCopy(input, 0, inputBytes, 0, inputBytes.Length);

		// Create buffer
		Rid bufferRid = rd.StorageBufferCreate((uint)inputBytes.Length, inputBytes);
		
		// Create uniform, bind buffer
		RDUniform uniform = new()
		{
			UniformType = RenderingDevice.UniformType.StorageBuffer,
			Binding = 0
		};
		uniform.AddId(bufferRid);

		// Pack uniform set
		Array<RDUniform> uniforms = new() { uniform };
		Rid uniformSet = rd.UniformSetCreate(uniforms, shaderRid, 2);

		// Create pipeline
		Rid pipelineRid = rd.ComputePipelineCreate(shaderRid);

		/*
		1. Begin instruction list for GPU to execute
		2. Bind inst list to pipeline
		3. Bind uniform set to pipeline
		4. Specify workgroup
		5. End inst list
		*/
		long computeList = rd.ComputeListBegin();
		rd.ComputeListBindComputePipeline(computeList, pipelineRid);
		rd.ComputeListBindUniformSet(computeList, uniformSet, 2);
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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
