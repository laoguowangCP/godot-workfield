using Godot;
using System;

public partial class TestNodeDuplicate : Node
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		Node node = GetChild<Node>(0);
		Node node_d = node.Duplicate();
		node_d.PrintTree();

		await ToSignal(GetParent(), Node.SignalName.Ready);
		node = GetChild<Node>(0);
		node_d = node.Duplicate();
		node_d.PrintTree();

		AddChild(node_d);
		PrintTree();

		node_d = DuplicateShallow(node);
		AddChild(node_d);
		PrintTree();
	}

	// Function to duplicate without descendants
	public static Node DuplicateShallow(Node node, int flags = 15)
	{
		Node node_d = node.Duplicate(flags);
		foreach (var child in node_d.GetChildren())
		{
			node_d.RemoveChild(child);
		}
		return node_d;
	}
}
