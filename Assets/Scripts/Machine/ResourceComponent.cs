using UnityEngine;

public class ResourceComponent : MonoBehaviour
{
    [SerializeField] private Item Item;

    public Item GetItem() { return Item; }
}