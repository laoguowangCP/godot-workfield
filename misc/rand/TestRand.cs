using Godot;
using System;
using LGWCP.Randy;
using System.Linq;

public partial class TestRand : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PCG32 pcgFast = new();
		foreach(var x in Enumerable.Range(0, 1000))
		{
			GD.Print(Randy.NextDouble01(pcgFast));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
