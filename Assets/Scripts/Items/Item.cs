using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public GameObject WorldItem;
}
