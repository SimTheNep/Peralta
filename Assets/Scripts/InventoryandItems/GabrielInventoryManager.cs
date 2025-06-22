using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GabrielInventoryManager : MonoBehaviour
{
    public Item[] slots = new Item[3];
    public int selectedSlot = 0;

    public GabrielSkills gabrielskills;
    public GabrielHealth gabrielHealth;
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

    public AudioSource audioSource;          
    public AudioClip itemPickupClip;         
    public AudioClip healItemConsumeClip;    

    public void Heal(float amount)
    {
        if (gabrielHealth != null)
            gabrielHealth.Heal(amount);
    }

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

        if (!Keyboard.current[actionKey].isPressed && Keyboard.current[itemKey].wasPressedThisFrame)
        {
            selectedSlot = (selectedSlot + 1) % 3;
         
            inventoryUI.UpdateUI(slots, selectedSlot);
        }

        if (Keyboard.current[actionKey].wasPressedThisFrame)
        {
            TryPickupNearbyItem();
        }

        
        if (Keyboard.current[actionKey].isPressed && Keyboard.current[itemKey].wasPressedThisFrame)
        {
            if (gabrielHealth != null && gabrielHealth.currentHealth < gabrielHealth.maxHealth)
            {
                UseHealableItem();
            }
            else
            {
                Debug.Log("Vida cheia");
            }
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

                if (audioSource != null && itemPickupClip != null)
                {
                    audioSource.PlayOneShot(itemPickupClip);
                }

                Destroy(pickup.gameObject);
                break; //faz com que apanhe só 1 de cada vez, mesmo tendo vários objetos no raio
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
            switch (item.itemName)
            {
                case "Chanfana":
                    Heal(6f);
                    consumeForHeal(selectedSlot);
                    break;

                case "Cabrito Assado":
                    Heal(1f);
                    consumeForHeal(selectedSlot);
                    break;

                case "Tigelada":
                    if (gabrielHealth != null)
                        StartCoroutine(gabrielHealth.RegenerateHealthOverTime(8f, 12f));
                    consumeForHeal(selectedSlot);
                    break;

                default:
                    Debug.Log($"Item {item.itemName} é de cura mas n tem efeito definido");
                    break;
            }
        }
    }

    void consumeForHeal(int slotIndex)
    {
        var item = slots[slotIndex];
        if (item == null) return;

        if (audioSource != null && healItemConsumeClip != null)
        {
            audioSource.PlayOneShot(healItemConsumeClip);
        }

        item.quantity--;
        if (item.quantity <= 0)
        {
            slots[slotIndex] = null;
        }
        inventoryUI.UpdateUI(slots, selectedSlot);
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
        //o código comentado acima faz desaparecer a arma de ataque, mas não é suposto isso acontecer, mas se no futuro quisermos meter durabilidade é assim que se faz
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
                Vector3 dropOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), 0);
                Vector3 dropPosition = transform.position + Vector3.right * 1f + dropOffset;
                Instantiate(prefab, dropPosition, Quaternion.identity);
            }
        }
    }
}
