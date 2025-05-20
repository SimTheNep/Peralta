using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData; // Referência ao ScriptableObject do item
    public int amount = 1;


    // Para items que vão para o Gabriel mas tens de clicar B
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Gabriel") && itemData.type == ItemData.ItemType.Gabriel)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                /*GabrielInventory inventory = other.GetComponent<GabrielInventory>();
                if (inventory != null)
                {
                    inventory.AddItem(itemData, amount);
                    Destroy(gameObject); // tira o objeto da cena
                }*/
            }
        }
    }

    // Para items que vão automaticamente para a Peralta
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Peralta") && itemData.type == ItemData.ItemType.Peralta)
        {
            /*PeraltaInventory inventory = other.GetComponent<PeraltaInventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemData, amount);
                Destroy(gameObject);
            }*/
        }
    }
    
}
