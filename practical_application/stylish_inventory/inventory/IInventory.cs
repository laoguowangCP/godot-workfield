using System;

namespace LGWCP.Inventory;

public interface IInventory<TItem>
{
    public TSlot GetSlot<TSlot>(TItem item)
        where TSlot : ISlot<TItem>;
    public bool Add<TSlot>(TSlot slot)
        where TSlot : ISlot<TItem>;
    public bool Remove<TSlot>(TSlot slot)
        where TSlot : ISlot<TItem>;
    public TLayout GetLayout<TLayout>()
        where TLayout : IInventoryLayout<TItem>;
}
