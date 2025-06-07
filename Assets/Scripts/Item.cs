using UnityEngine;
public enum ItemType
{
    Healable,
    Throwable,
    Attackable
}

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public bool isStackable;
    public int quantity;
    public float damage;

    public Item() { }

    public Item(Item other) // construtor de cópia para evitar referência direta
    {
        itemName = other.itemName;
        icon = other.icon;
        itemType = other.itemType;
        isStackable = other.isStackable;
        quantity = other.quantity;
        damage = other.damage;
    }

}
