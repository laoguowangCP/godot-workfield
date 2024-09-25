using Godot;
using System;


public partial class TestDictJson : Node
{
    public override void _Ready()
    {
        Godot.Collections.Dictionary<string, Vector2> dict = new();
        dict["pos"] = new Vector2(1, 2);
        string dictJson = Json.Stringify(dict);
        Godot.Collections.Dictionary<string, Vector2> dictFromJson = Json.ParseString(dictJson).AsGodotDictionary<string, Vector2>();
        GD.Print(dictFromJson["pos"]);
        GD.Print(dictFromJson);
    }
}
