using Godot;
using System;

public partial class AreaInsideEnter2D : Node
{
	[Export] protected PackedScene AreaInsidePS;
	[Export] protected PackedScene StaticInsidePS;
	[Export] protected PackedScene CBInsidePS;
	[Export] protected PackedScene RigidInsidePS;
	protected Area2D Area;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area = GetNode<Area2D>("Area2D");
		Area.Connect(Area2D.SignalName.AreaEntered, Callable.From<Area2D>(OnAreaEntered));
		Area.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node2D>(OnBodyEntered));
	}

	public override void _Input(InputEvent e)
	{
		if (e is InputEventKey key && key.Keycode == Key.Space && key.IsPressed())
		{
			Area2D areaInside = AreaInsidePS.Instantiate<Area2D>();
			AddChild(areaInside);
			StaticBody2D staticInside = StaticInsidePS.Instantiate<StaticBody2D>();
			AddChild(staticInside);
			CharacterBody2D cbInside = CBInsidePS.Instantiate<CharacterBody2D>();
			AddChild(cbInside);
			RigidBody2D rigidInside = RigidInsidePS.Instantiate<RigidBody2D>();
			AddChild(rigidInside);
		}
	}

	public void OnAreaEntered(Area2D area)
	{
		GD.Print("Inside instantiated area detected!");
		GD.Print(Area.HasOverlappingBodies());
	}

	public void OnBodyEntered(Node2D node)
	{
		GD.Print("Inside instantiated body detected: ", node.Name);
	}
}
