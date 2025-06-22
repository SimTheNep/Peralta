using UnityEngine;
public enum ItemType
{
    Healable,
    Throwable,
    Attackable,
    Key
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
    public int price;
    public string description;

    public Item() { }

    public Item(Item other) // construtor de c�pia para evitar refer�ncia direta
    {
        itemName = other.itemName;
        icon = other.icon;
        itemType = other.itemType;
        isStackable = other.isStackable;
        quantity = other.quantity;
        damage = other.damage;
        price = other.price;
        description = other.description;
    }

}
