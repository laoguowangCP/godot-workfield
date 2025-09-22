using Godot;
using System;

[GlobalClass]
public partial class ResHoldingCollection : Resource
{
    [Export] public Godot.Collections.Array<int> HoldingArray;
}
