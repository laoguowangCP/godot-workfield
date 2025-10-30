using Godot;
using System;

public partial class Dead : Node
{
    [Export] public PackedScene Play;
    [Export] public String PlayPath;
    public override void _UnhandledInput(InputEvent input)
    {
        if (input is InputEventKey keyInput)
        {
            if (keyInput.Pressed)
            {
                if (keyInput.Keycode == Key.Space)
                {
                    // GetTree().ChangeSceneToPacked(Play);
                    GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>(PlayPath));
                }
            }
        }
    }
}
