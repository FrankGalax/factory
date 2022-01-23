using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltComponent : MonoBehaviour, IInventory
{
    public void DecreaseQuantity(int index, int amount)
    {
        throw new System.NotImplementedException();
    }

    public Item GetItem(int index)
    {
        throw new System.NotImplementedException();
    }

    public int GetQuantity(int index)
    {
        throw new System.NotImplementedException();
    }

    public int GetAvailableSlotIndex()
    {
        return 0;
    }
}
