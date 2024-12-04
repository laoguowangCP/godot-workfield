using Godot;
using Godot.Collections;
using System;

public partial class NestArrayExport : Node
{
    [Export]
    protected Array<Array<Node>> NestArray;
}
