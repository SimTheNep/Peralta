using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


public class PeraltaInventoryManager : MonoBehaviour
{

    public MagicItem[] slots = new MagicItem[3];
    public int selectedSlot = 0;

    public PeraltaSkills peraltaSkills;
    public InventoryUI inventoryUI;

    public bool canUseInventory = true;

    void Update()
    {
        if (!canUseInventory) return;

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            selectedSlot = (selectedSlot + 1) % 3;
            inventoryUI?.UpdateUI(slots, selectedSlot);
        }
    }

    public void TryPickupMagicItem(MagicItem newItem)
    {
        // Stack para almas
        if (newItem.isStackable)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].itemName == newItem.itemName && slots[i].isStackable)
                {
                    slots[i].quantity += newItem.quantity;
                    inventoryUI?.UpdateUI(slots, selectedSlot);
                    return;
                }
            }
        }

        // Slot vazio
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = newItem;
                inventoryUI?.UpdateUI(slots, selectedSlot);
                return;
            }
        }

        // Substitui o slot ativo se estiver cheio
        slots[selectedSlot] = newItem;
        inventoryUI?.UpdateUI(slots, selectedSlot);
    }

}
