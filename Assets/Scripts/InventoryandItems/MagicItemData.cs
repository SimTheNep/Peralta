using UnityEngine;

public class MagicItemData : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public MagicItemType itemType;
    public bool isStackable = false;
    public int quantity = 1;
    public int price = 0;
    public string description;

    public MagicItem GetMagicItem()
    {
        return new MagicItem
        {
            itemName = itemName,
            icon = icon,
            itemType = itemType,
            isStackable = isStackable,
            quantity = quantity,
            price = price,
            description = description
        };
    }
}
