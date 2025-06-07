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
    public void UpdateUI(Item[] items, int selectedSlot)
    {
        
        for (int i = 0; i < 3; i++)
        {
            slotBackgrounds[i].sprite = (i == selectedSlot) ? selectedSlotSprite : normalSlotSprite;

            // muda a cor: vermelho se for o slot selecionado, branco caso contrário. só enquanto n temos o sprite final 
            slotBackgrounds[i].color = (i == selectedSlot) ? Color.red : Color.white;

            if (items[i] != null)
            {
                
                itemIcons[i].sprite = items[i].icon;
                itemIcons[i].enabled = true;

                if (items[i].isStackable && items[i].quantity > 1)
                    itemQuantities[i].text = items[i].quantity.ToString();
                else
                    itemQuantities[i].text = "";
            }
            else
            {
                
                itemIcons[i].enabled = false;
                itemQuantities[i].text = "";
            }
        }
    }
}
