using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


public class PeraltaInventoryManager : MonoBehaviour
{

    public Item[] slots = new Item[3];
    public int selectedSlot = 0;

    public PeraltaSkills peraltaSkills;
    public InventoryUI inventoryUI;

}
