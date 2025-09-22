using Godot;
using System;

public partial class TestCollectionUnique : Node
{
    [Export] public PackedScene NodeHoldingCollectionScene;
    [Export] public PackedScene NodeHoldingResHoldingCollectionScene;

    public override void _Ready()
    {
        var nodeA = NodeHoldingCollectionScene.Instantiate<NodeHoldingCollection>();
        var nodeB = NodeHoldingCollectionScene.Instantiate<NodeHoldingCollection>();
        AddChild(nodeA);
        AddChild(nodeB);
        
        nodeA.HoldingArray.Add(999);
        GD.Print(nodeB.HoldingArray);

        var nodeC = NodeHoldingResHoldingCollectionScene.Instantiate<NodeHoldingResHoldingCollection>();
        var nodeD = NodeHoldingResHoldingCollectionScene.Instantiate<NodeHoldingResHoldingCollection>();
        AddChild(nodeC);
        AddChild(nodeD);
        
        nodeC.ResHoldingCollection.HoldingArray.Add(999);
        GD.Print(nodeD.ResHoldingCollection.HoldingArray);
    }
}
