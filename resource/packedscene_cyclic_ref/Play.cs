using Godot;
using System;

public partial class Play : Node
{
    [Export] public PackedScene Dead;
    [Export] public String DeadPath;
    public override void _UnhandledInput(InputEvent input)
    {
        if (input is InputEventKey keyInput)
        {
            if (keyInput.Pressed)
            {
                if (keyInput.Keycode == Key.Space)
                {
                    GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>(DeadPath));
                }
            }
        }
    }
}
