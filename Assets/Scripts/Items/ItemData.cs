using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;           // Nome do item
    public Sprite icon;               // Ícone do item para mostrar no UI
    public string description;        // Descrição opcional
    public ItemType type;             // Tipo do item (Gabriel ou Peralta)

    public enum ItemType
    {
        Gabriel,    // Apenas Gabriel pode usar
        Peralta     // Apenas Peralta pode usar
    }
}
