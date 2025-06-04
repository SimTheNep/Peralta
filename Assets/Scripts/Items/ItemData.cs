using UnityEngine;

public enum ItemType { Throwable, Attackable, Healable }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public GameObject worldPrefab;  
    public ItemType itemType;             
    public MonoBehaviour itemBehaviour;
    public ItemPerson person;

    public enum ItemPerson
    {
        Gabriel,    // Apenas Gabriel pode usar
        Peralta     // Apenas Peralta pode usar
    }
}
