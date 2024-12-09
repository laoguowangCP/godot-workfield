using Godot;
using System;

namespace LGWCP.Godot.WorkField.WetStockings;

public partial class Spinner3D : Node
{
    protected Node3D Parent;
    public override void _Ready()
    {
        Parent = GetParent<Node3D>();
    }

    public override void _Process(double delta)
    {
        Parent.Rotate(Vector3.Forward, (float)delta);
    }
}
