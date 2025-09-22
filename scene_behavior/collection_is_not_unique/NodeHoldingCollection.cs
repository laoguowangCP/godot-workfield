using Godot;
using System;

public partial class NodeHoldingCollection : Node
{
    [Export] public Godot.Collections.Array<int> HoldingArray;

    public override void _Ready()
    {
        // HoldingArray = [..HoldingArray];
    }
}
