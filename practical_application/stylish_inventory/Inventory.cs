using Godot;
using System;
using System.Collections;
using System.Collections.Generic;


namespace LGWCP.Inventory;

public partial class Inventory
{
	[Export] protected int MaxSlotLength = 5;
	[Export] protected bool IsLastSlotHangable = true;
	protected bool IsLastSlotHanged;
	protected LinkedList<Node> Items = new();

	
}
