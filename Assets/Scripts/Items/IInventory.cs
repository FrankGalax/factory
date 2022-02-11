public interface IInventory
{
    public abstract void AddItem(int index, Item item, int amount);
    public abstract Item GetItem(int index);
    public abstract int GetQuantity(int index);
    public abstract void RemoveItem(int index, int amount);
    public abstract int GetAvailableSlotIndex(Item item, SlotIO slotIO);
}