using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Image[] slotBackgrounds;
    public Sprite normalSlotSprite;
    public Sprite selectedSlotSprite;
    public Image[] itemIcons;
    public TMP_Text[] itemQuantities;

    //private Item[] items = new Item[3];
    //private MagicItem[] MagicItems = new MagicItem[3];
    //private int selectedSlot = 0;

    public void SetInventoryVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    private void Start()
    {
        //UpdateUI(items, selectedSlot);
    }

    public void UpdateUI(Item[] items, int selectedSlot)
    {
        for (int i = 0; i < slotBackgrounds.Length; i++)
        {
            slotBackgrounds[i].sprite = (i == selectedSlot) ? selectedSlotSprite : normalSlotSprite;

            if (items[i] != null && items[i].icon != null)
            {
                itemIcons[i].sprite = items[i].icon;
                itemIcons[i].enabled = true;
                itemIcons[i].color = Color.white;

                if (items[i].isStackable && items[i].quantity > 1)
                    itemQuantities[i].text = items[i].quantity.ToString();
                else
                    itemQuantities[i].text = "";
            }
            else
            {
                itemIcons[i].sprite = null;
                itemIcons[i].enabled = false;
                itemQuantities[i].text = "";
            }
        }
    }

    public void UpdateUI(MagicItem[] MagicItems, int selectedSlot)
    {
        for (int i = 0; i < slotBackgrounds.Length; i++)
        {
            slotBackgrounds[i].sprite = (i == selectedSlot) ? selectedSlotSprite : normalSlotSprite;

            if (MagicItems[i] != null && MagicItems[i].icon != null)
            {
                itemIcons[i].sprite = MagicItems[i].icon;
                itemIcons[i].enabled = true;
                itemIcons[i].color = Color.white;

                if (MagicItems[i].isStackable && MagicItems[i].quantity > 1)
                    itemQuantities[i].text = MagicItems[i].quantity.ToString();
                else
                    itemQuantities[i].text = "";
            }
            else
            {
                itemIcons[i].sprite = null;
                itemIcons[i].enabled = false;
                itemQuantities[i].text = "";
            }
        }
    }
}
