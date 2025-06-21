using UnityEngine;
using UnityEngine.InputSystem;

public class PeraltaInventoryManager : MonoBehaviour
{
    public MagicItem[] slots = new MagicItem[3];
    public int selectedSlot = 0;

    public PeraltaSkills peraltaSkills;
    public InventoryUI inventoryUI;

    public GameObject Alma;
    public GameObject serpenteEncantada;
    public GameObject asaDeIcaro;
    public GameObject sorteDeNavegador;
    public GameObject rosaDeAragao;

    public bool canUseInventory = true;

    void Update()
    {
        if (!canUseInventory) return;

        KeyCode itemKeyCode = KeybindManager.GetKeyCode("Item"); 
        KeyCode actionKeyCode = KeybindManager.GetKeyCode("Action");

        Key itemKey = InputHelpers.KeyCodeToKey(itemKeyCode);
        Key actionKey = InputHelpers.KeyCodeToKey(actionKeyCode);

        if (Keyboard.current[itemKey].wasPressedThisFrame)
        {
            selectedSlot = (selectedSlot + 1) % slots.Length;
            Debug.Log("Slot selecionado: " + selectedSlot);
            inventoryUI?.UpdateUI(slots, selectedSlot);
        }

        if (Keyboard.current[actionKey].wasPressedThisFrame)
        {
            TryPickupNearbyItem();
        }
    }

    void TryPickupNearbyItem()
    {
        float pickupRadius = 1.5f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRadius);

        foreach (var hit in hits)
        {
            MagicItemPickup pickup = hit.GetComponent<MagicItemPickup>();
            if (pickup != null && pickup.itemData != null)
            {
                MagicItem newItem = pickup.itemData.GetMagicItem();
                TryPickupMagicItem(newItem);
                Destroy(pickup.gameObject);
                break;
            }
        }
    }

    public void TryPickupMagicItem(MagicItem newItem)
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

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new MagicItem(newItem);
                inventoryUI?.UpdateUI(slots, selectedSlot);
                return;
            }
        }

        DropItemToScene(slots[selectedSlot]);
        slots[selectedSlot] = new MagicItem(newItem);
        inventoryUI?.UpdateUI(slots, selectedSlot);
    }

    void DropItemToScene(MagicItem item)
    {
        if (item == null) return;

        GameObject prefab = null;

        switch (item.itemName)
        {
            case "Alma":
                prefab = Alma;
                break;
            case "Sorte de Navegador":
                prefab = sorteDeNavegador;
                break;
            case "Serpente Encantada":
                prefab = serpenteEncantada;
                break;
            case "Rosa de Aragão":
                prefab = rosaDeAragao;
                break;
            case "Asa de Ícaro":
                prefab = asaDeIcaro;
                break;
        }

        if (prefab != null)
        {
            for (int i = 0; i < item.quantity; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), 0);
                Vector3 dropPos = transform.position + Vector3.right * 1f + offset;
                Instantiate(prefab, dropPos, Quaternion.identity);
            }
        }
    }

    public int GetQuantidadeRosasDeAragao()
    {
        int total = 0;
        foreach (var item in slots)
        {
            if (item != null && item.itemName == "Rosa de Aragão")
                total += item.quantity;
        }
        return total;
    }


    public bool HasRosaDeAragao()
    {
        foreach (var item in slots)
        {
            if (item != null && item.itemName == "Rosa de Aragão" && item.quantity > 0)
                return true;
        }
        return false;
    }

    public bool HasSorteNavegador()
    {
        foreach (var item in slots)
        {
            if (item != null && item.itemName == "Sorte de Navegador" && item.quantity > 0)
                return true;
        }
        return false;
    }

    public bool HasAsaIcaro()
    {
        foreach (var item in slots)
        {
            if (item != null && item.itemName == "Asa de Ícaro" && item.quantity > 0)
                return true;
        }
        return false;
    }

    public bool HasSerpenteEncantada()
    {
        foreach (var item in slots)
        {
            if (item != null && item.itemName == "SerpenteEncantada" && item.quantity > 0)
                return true;
        }
        return false;
    }


    public bool ConsumeRosaDeAragao()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].itemName == "Rosa de Aragão" && slots[i].quantity > 0)
            {
                slots[i].quantity--;
                if (slots[i].quantity <= 0)
                    slots[i] = null;
                inventoryUI?.UpdateUI(slots, selectedSlot);
                return true;
            }
        }
        return false;
    }

}
