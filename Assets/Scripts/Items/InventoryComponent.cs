using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum SlotIO
{
    Input = 1,
    Output = 2,
    InputOutput = Input | Output
}

[Serializable]
public class InventorySlot
{
    [SerializeField] private Item Item = null;
    [SerializeField] private int Quantity = 0;
    [SerializeField] private SlotIO SlotIO = SlotIO.InputOutput;

    public delegate void InventorySlotChanged();
    public event InventorySlotChanged OnInventorySlotChanged;

    public void AddItem(Item item)
    {
        Assert.IsTrue(Item == null || Item == item);

        if (Item == null)
        {
            Item = item;
        }

        Quantity++;

        if (OnInventorySlotChanged != null)
        {
            OnInventorySlotChanged();
        }
    }

    public Item GetItem() { return Item; }

    public int GetQuantity() { return Quantity; }

    public SlotIO GetSlotIO() { return SlotIO; }

    public void DecreaseQuantity(int amount)
    {
        Quantity -= amount;
        if (Quantity < 0)
        {
            Quantity = 0;
        }

        if (OnInventorySlotChanged != null)
        {
            OnInventorySlotChanged();
        }
    }
}

public class InventoryComponent : MonoBehaviour, IInventory
{
    [SerializeField] private List<InventorySlot> InventorySlots;

    public void AddItem(int index, Item item)
    {
        InventorySlots[index].AddItem(item);
    }

    public InventorySlot GetSlot(int index)
    {
        return InventorySlots[index];
    }

    public Item GetItem(int index)
    {
        return InventorySlots[index].GetItem();
    }

    public int GetQuantity(int index)
    {
        return InventorySlots[index].GetQuantity();
    }

    public void DecreaseQuantity(int index, int amount)
    {
        InventorySlots[index].DecreaseQuantity(amount);
    }

    public int GetAvailableSlotIndex(Item item, SlotIO slotIO)
    {
        for (int i = 0; i < InventorySlots.Count; ++i)
        {
            InventorySlot slot = InventorySlots[i];
            if (slot.GetItem() != null)
            {
                if (item != null && slot.GetItem() != item)
                {
                    continue;
                }
            }

            int slotIOMask = (int)slot.GetSlotIO() & (int)slotIO;
            if (slotIOMask == 0)
            {
                continue;
            }

            return i;
        }

        return -1;
    }
}
