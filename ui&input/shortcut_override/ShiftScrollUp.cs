using Godot;
using System;

public partial class ShiftScrollUp : Control
{
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ShiftScrollUp"))
        {
            GD.Print("ShiftScrollUp from: ", GetPath());
            AcceptEvent();
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ScrollUp"))
        {
            GD.Print("ScrollUp from: ", GetPath());
        }
    }
}
