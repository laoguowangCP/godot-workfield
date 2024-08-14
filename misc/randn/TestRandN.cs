using Godot;
using System;
using RandN;

public partial class TestRandN : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StandardRng rng = StandardRng.Create();
		rng.NextUInt64();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
