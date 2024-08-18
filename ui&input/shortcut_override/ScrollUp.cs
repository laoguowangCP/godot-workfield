using Godot;
using System;

public partial class ScrollUp : Control
{
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ScrollUp"))
        {
            GD.Print("ScrollUp from: ", GetPath());
        }
    }
}
