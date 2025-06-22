using UnityEngine;

public class ItemData : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public bool isStackable = false;
    public int quantity = 1;
    public float damage = 1f;
    public int price = 0;
    public string description;

    public Item GetItem()
    {
        return new Item
        {
            itemName = itemName,
            icon = icon,
            itemType = itemType,
            isStackable = isStackable,
            quantity = quantity,
            damage = damage,
            price = price,
            description = description
        };
    }
}
