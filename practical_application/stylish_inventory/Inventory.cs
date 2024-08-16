using Godot;
using System;
using System.Collections.Generic;


namespace LGWCP.Inventory;

public partial class Inventory : Node
{
	[Export] protected int MaxSlotLength = 5;
	[Export] protected bool IsLastSlotHangable = true;
	protected bool IsLastSlotHanged;
	protected LinkedList<Node> Items = new();
	

	public class Slot
	{

	}

	public class SlotGroup
	{
		
	}
}
