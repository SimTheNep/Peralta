using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GabrielInventory : MonoBehaviour
{
    [SerializeField] private GabrielSkills gabrielSkills;
    public Slot[] slots = new Slot[3];

    public int selectedSlot = 0;

    public Image[] slotUIBackgrounds;
    public Image[] itemIcons;
    public TextMeshProUGUI[] itemQuantities;

    public Sprite normalBackground;
    public Sprite selectedBackground;
    

    private void Start()
    {
         for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new Slot();
        }

        UpdateUI();
    }

   private void Update()
{
    if (Input.GetKeyDown(KeyCode.A)) // Mudar slot
    {
        CycleSlot();
    }

    if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.B)) // Consumir comida
    {
        ConsumeCurrentItem();
    }

    if(Input.GetKeyDown(KeyCode.Y) && !Input.GetKey(KeyCode.B)) // Usar item com skill ativa
    {
        UseSelectedItem();
    }
}


    public void AddItem(ItemData newItem)
    {
        // Tenta acumular item se já existe
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData == newItem)
            {
                slots[i].quantity++;
                UpdateUI();
                return;
            }
        }

        // Tenta adicionar a slot vazia
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData == null)
            {
                slots[i].itemData = newItem;
                slots[i].quantity = 1;
                UpdateUI();
                return;
            }
        }

        // Se chegou aqui, substitui o item do slot selecionado
        slots[selectedSlot].itemData = newItem;
        slots[selectedSlot].quantity = 1;
        UpdateUI();
    }

    public void CycleSlot()
    {
        selectedSlot = (selectedSlot + 1) % slots.Length;
        UpdateUI();
    }

    public void ConsumeCurrentItem()
    {
        var slot = slots[selectedSlot];

        if (slot.itemData != null && slot.itemData.itemType == ItemType.Healable)
        {
            MonoBehaviour behaviour = slot.itemData.itemBehaviour;

            if (behaviour != null && behaviour is IUsableItem usable)
            {
                usable.Use(this); // Executa cura
            }

            slot.quantity--;
            if (slot.quantity <= 0)
            {
                slot.itemData = null;
                slot.quantity = 0;
            }

            UpdateUI();
        }
    }

    public void UseSelectedItem()
{
    var slot = slots[selectedSlot];

    if (slot.itemData != null)
    {
        var item = slot.itemData;
        SkillType activeSkill = gabrielSkills.GetCurrentSkill();

        bool canUse = (item.itemType == ItemType.Throwable && activeSkill == SkillType.Throw) ||
                      (item.itemType == ItemType.Attackable && activeSkill == SkillType.Attack);

        if (canUse)
        {
            MonoBehaviour behaviour = item.itemBehaviour;

            if (behaviour != null && behaviour is IUsableItem usable)
            {
                usable.Use(this); // Executa ação
            }

            slot.quantity--;
            if (slot.quantity <= 0)
            {
                slot.itemData = null;
                slot.quantity = 0;
            }

            UpdateUI();
        }
        else
        {
            Debug.Log("Item não corresponde à skill ativa.");
        }
    }
}



    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            bool hasItem = slots[i].itemData != null;

            itemIcons[i].enabled = hasItem;
            itemQuantities[i].enabled = hasItem && slots[i].quantity > 1;

            if (hasItem)
            {
                itemIcons[i].sprite = slots[i].itemData.icon;
                itemQuantities[i].text = slots[i].quantity.ToString();
            }

            slotUIBackgrounds[i].sprite = (i == selectedSlot) ? selectedBackground : normalBackground;
        }
    }







}
