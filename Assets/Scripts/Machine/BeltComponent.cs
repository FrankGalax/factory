using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltComponent : MonoBehaviour, IInventory, IDeploy
{
    public void AddItem(int index, Item item)
    {
        GameObject worldItem = Instantiate(item.WorldItem);
        BeltManager.Instance.AddItem(worldItem, transform.position, this);
    }

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
        if (BeltManager.Instance.GetItem(this, transform.position))
        {
            return -1;
        }

        return 0;
    }

    public void OnDeploy()
    {
        BeltManager.Instance.AddBeltComponent(this);
    }

    public void OnUnDeploy()
    {
        BeltManager.Instance.RemoveBeltComponent(this);
    }

    public void OnCloseMachineDeployed() { }
}
