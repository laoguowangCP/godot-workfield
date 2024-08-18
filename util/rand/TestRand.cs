using Godot;
using LGWCP.Util.Randy;
using System.Linq;

public partial class TestRand : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PCG32Fast rng = new(1);
		GD.Print(rng.GetRNGState());
		foreach(var x in Enumerable.Range(0, 1000))
		{
			GD.Print(Randy.NextSingle(rng, -10, 0));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
