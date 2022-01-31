﻿public interface IInventory
{
    public abstract void AddItem(int index, Item item);
    public abstract Item GetItem(int index);
    public abstract int GetQuantity(int index);
    public abstract void DecreaseQuantity(int index, int amount);
    public abstract int GetAvailableSlotIndex();
}