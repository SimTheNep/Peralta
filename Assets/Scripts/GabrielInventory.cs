using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;



public class GabrielInventory : MonoBehaviour
{
    public int maxSlots = 3;      
    public List<ItemData> items = new List<ItemData>();

    public Image[] itemIcons;   

     public int currentIndex = 0; // slot ativo
    

    private void Start()
    {
        UpdateUI();
    }

     private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) // Ciclar slot
        {
            CycleSlot();
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.B)) // Consumir item (hold A+B)
        {
            ConsumeCurrentItem();
        }
    }

    public void CycleSlot()
    {
        if (items.Count == 0) return;

        currentIndex = (currentIndex + 1) % items.Count;
        Debug.Log("Slot ativo: " + currentIndex);
        
    }

    public void AddOrReplaceItem(ItemData newItem)
    {
        if (items.Count < maxSlots)
        {
            items.Add(newItem);
        }
        else
        {
            items[currentIndex] = newItem;
        }
        UpdateUI();
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
            if (currentIndex >= items.Count)
                currentIndex = items.Count - 1; //indice fica -1 ao remover item
            UpdateUI();
        }
    }

    void ConsumeCurrentItem()
    {
        if (items.Count > 0 && currentIndex >= 0 && currentIndex < items.Count)
        {
            Debug.Log("Consumiu: " + items[currentIndex].itemName);
            RemoveItem(currentIndex);
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < itemIcons.Length; i++)
        {
            if (i < items.Count)
            {
                itemIcons[i].sprite = items[i].icon;
                itemIcons[i].enabled = true;
            }
            else
            {
                itemIcons[i].sprite = null;
                itemIcons[i].enabled = false;
            }
        }
    
    }

}
