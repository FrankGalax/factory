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
        BeltManager.Instance.RemoveItem(this, transform.position);
    }

    public Item GetItem(int index)
    {
        GameObject worldItem = BeltManager.Instance.GetItem(this, transform.position);
        
        if (worldItem != null)
        {
            ResourceComponent resourceComponent = worldItem.GetComponent<ResourceComponent>();
            if (resourceComponent != null)
            {
                return resourceComponent.GetItem();
            }
        }

        return null;
    }

    public int GetQuantity(int index)
    {
        GameObject worldItem = BeltManager.Instance.GetItem(this, transform.position);
        return worldItem != null ? 1 : 0;
    }

    public int GetAvailableSlotIndex(Item item)
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
