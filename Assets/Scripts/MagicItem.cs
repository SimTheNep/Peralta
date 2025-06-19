using UnityEngine;

public enum MagicItemType
{
    Soul,       // Stackable
    PowerUp     // Não stackable
}

[System.Serializable]
public class MagicItem
{
    public string itemName;
    public Sprite icon;
    public MagicItemType itemType;
    public bool isStackable;
    public int quantity;
    public int price; // Preço em almas
    public string description;

    public MagicItem() { }

    public MagicItem(MagicItem other)
    {
        itemName = other.itemName;
        icon = other.icon;
        itemType = other.itemType;
        isStackable = other.isStackable;
        quantity = other.quantity;
        price = other.price;
        description = other.description;
    }
}
