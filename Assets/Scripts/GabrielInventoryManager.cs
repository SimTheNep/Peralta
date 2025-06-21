using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class GabrielInventoryManager : MonoBehaviour
{
    public Item[] slots = new Item[3];
    public int selectedSlot = 0;

    public GabrielSkills gabrielskills;
    public InventoryUI inventoryUI;

    public GameObject pedra;
    public GameObject chanfana;
    public GameObject frascoDeAgua;
    public GameObject tigelada;
    public GameObject cabritoAssado;
    public GameObject espadaDeAntenas;
    public GameObject espadaEnferrujada;
    public GameObject falcata;
    public GameObject montante;
    public GameObject cranio;
    public GameObject chave;

    public bool canUseInventory = true;

    void Update()
    {
        if (!canUseInventory) return; //para bloquear no dialogo

        KeyCode itemKeyCode = KeybindManager.GetKeyCode("Item"); 
        KeyCode actionKeyCode = KeybindManager.GetKeyCode("Action");

        Key itemKey = InputHelpers.KeyCodeToKey(itemKeyCode);
        Key actionKey = InputHelpers.KeyCodeToKey(actionKeyCode);

        if (itemKey == Key.None || actionKey == Key.None)
            return;

        if (Keyboard.current[itemKey].wasPressedThisFrame)
        {
            selectedSlot = (selectedSlot + 1) % 3;
            Debug.Log("Slot selecionado: " + selectedSlot);
            inventoryUI.UpdateUI(slots, selectedSlot);
        }

        if (Keyboard.current[actionKey].wasPressedThisFrame)
        {
            TryPickupNearbyItem();
        }

        // Usa item de cura com A + B
        if (Keyboard.current[itemKey].isPressed && Keyboard.current[actionKey].wasPressedThisFrame)
        {
            UseHealableItem();
        }
    }

    void TryPickupNearbyItem()
    {
        float pickupRadius = 1f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRadius);

        foreach (var hit in hits)
        {
            ItemPickup pickup = hit.GetComponent<ItemPickup>();
            if (pickup != null && pickup.itemData != null)
            {
                Item newItem = pickup.itemData.GetItem();
                TryPickupItem(newItem);
                Destroy(pickup.gameObject);
                break; //faz com que apanhe s� 1 de cada vez, mesmo tendo varios objetos no raio
            }
        }
    }

    public void TryPickupItem(Item newItem)
    {
        // tentar encontrar um slot com o mesmo item (stackable)
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].itemName == newItem.itemName && slots[i].isStackable)
            {
                slots[i].quantity += newItem.quantity;
                inventoryUI?.UpdateUI(slots, selectedSlot);
                return;
            }
        }

        // tentar encontrar um slot vazio
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = newItem;
                inventoryUI?.UpdateUI(slots, selectedSlot);
                return;
            }
        }

        // se todos os slots estiverem ocupados, substituir o slot ativo
        DropItemToScene(slots[selectedSlot]); // dropa o que estava
        slots[selectedSlot] = newItem;
        inventoryUI?.UpdateUI(slots, selectedSlot);
    }

    void UseHealableItem()
    {
        var item = slots[selectedSlot];
        if (item != null && item.itemType == ItemType.Healable)
        {
            slots[selectedSlot] = null;
            inventoryUI.UpdateUI(slots, selectedSlot);
        }
    }

    public bool HasItemForSkill(SkillType skill)
    {
        var item = slots[selectedSlot];
        if (item == null) return false;

        switch (skill)
        {
            case SkillType.Throw:
                return item.itemType == ItemType.Throwable && item.quantity > 0;
            case SkillType.Attack:
                return item.itemType == ItemType.Attackable;
            default:
                return true;
        }
    }

    public void ConsumeItemForSkill(SkillType skill)
    {
        var item = slots[selectedSlot];
        if (item == null) return;

        if (skill == SkillType.Throw)
        {
            item.quantity--;
            if (item.quantity <= 0) slots[selectedSlot] = null;
            inventoryUI?.UpdateUI(slots, selectedSlot);
        }
        /*else if (skill == SkillType.Attack)
        {
            slots[selectedSlot] = null;
        }*/
        //o codigo comentado acima faz desparecer a arma de ataque, mas n�o � suposto isso acontecer, mas se no futuro quisermos meter durabilidade � aqui
    }

    void DropItemToScene(Item item)
    {
        GameObject prefab = null;

        switch (item.itemName)
        {
            case "Pedra":
                prefab = pedra;
                break;
            case "Chanfana":
                prefab = chanfana;
                break;
            case "Crânio":
                prefab = cranio;
                break;
            case "Espada de Antenas":
                prefab = espadaDeAntenas;
                break;
            case "Espada Enferrujada":
                prefab = espadaEnferrujada;
                break;
            case "Falcata":
                prefab = falcata;
                break;
            case "Montante":
                prefab = montante;
                break;
            case "Frasco de Àgua":
                prefab = frascoDeAgua;
                break;
            case "Chave":
                prefab = chave;
                break;
            case "Cabrito Assado":
                prefab = cabritoAssado;
                break;
            case "Tigelada":
                prefab = tigelada;
                break;
        }

        if (prefab != null)
        {
            for (int i = 0; i < item.quantity; i++)
            {
                // Distribui os itens com leve offset para evitar sobreposi��o total
                Vector3 dropOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), 0);
                Vector3 dropPosition = transform.position + Vector3.right * 1f + dropOffset;
                Instantiate(prefab, dropPosition, Quaternion.identity);
            }
        }
    }
}
