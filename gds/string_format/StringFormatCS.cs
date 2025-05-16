using Godot;
using System;

public partial class StringFormatCS : Node
{
    public override void _Ready()
    {
        GD.Print("3".PadLeft(6, '0'));
    }
}
