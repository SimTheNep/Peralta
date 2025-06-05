using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData; // Referência ao ScriptableObject do item
    public int amount = 1;


    // Para items que vão para o Gabriel mas tens de clicar B
    void OnTriggerStay2D(Collider2D other)
    {
        /*if (other.CompareTag("Gabriel") && Input.GetKeyDown(KeyCode.B))
        {
            GabrielInventory inventory = other.GetComponent<GabrielInventory>();
            if (inventory != null)
            {
                for (int i = 0; i < amount; i++)
                {
                    inventory.AddItem(itemData);
                }

                Destroy(gameObject);
            }
        }*/
        if (other.CompareTag("Gabriel"))
        {
            Debug.Log("Gabriel na trigger da pedra");

            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("B pressionado perto da pedra");

                GabrielInventory inventory = other.GetComponent<GabrielInventory>();
                if (inventory != null)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        inventory.AddItem(itemData);
                    }

                    Destroy(gameObject);
                }
            }
        }

    }

    // Para items que vão automaticamente para a Peralta
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Peralta") && itemData.person == ItemData.ItemPerson.Peralta)
        {
            /*PeraltaInventory inventory = other.GetComponent<PeraltaInventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemData);
                Destroy(gameObject);
            }*/
        }
    }
    
}
