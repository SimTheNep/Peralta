using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


public class GabrielInventoryManager : MonoBehaviour
{

    public Item[] slots = new Item[3];
    public int selectedSlot = 0;

    public GabrielSkills gabrielskills;
    public InventoryUI inventoryUI;


    public GameObject stonePrefab;
    public GameObject stickPrefab;

    void Update()
    {
        // Cicla entre os 3 slots ao pressionar A
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            selectedSlot = (selectedSlot + 1) % 3;
            Debug.Log("Slot selecionado: " + selectedSlot);
            inventoryUI.UpdateUI(slots, selectedSlot);
        }
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            TryPickupNearbyItem();
        }


        // Usa item de cura com A + B
        if (Keyboard.current.aKey.isPressed && Keyboard.current.bKey.wasPressedThisFrame)
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
                break;//faz com que apanhe só 1 de cada vez, mesmo tendo varios objetos no raio
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
        //o codigo comentado acima faz desparecer a arma de ataque, mas não é suposto isso acontecer, mas se no futuro quisermos meter durabilidade é aqui
    }

    void DropItemToScene(Item item)
    {
        GameObject prefab = null;

        switch (item.itemName)
        {
            case "Pedra":
                prefab = stonePrefab;
                break;
            case "Pau":
                prefab = stickPrefab;
                break;
        }

        if (prefab != null)
        {
            for (int i = 0; i < item.quantity; i++)
            {
                // Distribui os itens com leve offset para evitar sobreposição total
                Vector3 dropOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), 0);
                Vector3 dropPosition = transform.position + Vector3.right * 1f + dropOffset;
                Instantiate(prefab, dropPosition, Quaternion.identity);
            }
        }
    }

}
