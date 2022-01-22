using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public enum InventoryIO
{
    Input,
    Output
}

[Serializable]
public class InventorySlot
{
    public InventoryIO InventoryIO = InventoryIO.Input;
    public Item Item = null;
    public int Quantity = 0;

    public void AddItem(Item item)
    {
        Assert.IsTrue(Item == null || Item == item);

        if (Item == null)
        {
            Item = item;
        }

        Quantity++;
    }
}

public class InventoryComponent : MonoBehaviour
{
    public List<InventorySlot> InventorySlots;

    public void AddItem(int slotIndex, Item item)
    {
        InventorySlots[slotIndex].AddItem(item);
    }
}
