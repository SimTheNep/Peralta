using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;


    public GabrielInventoryManager gabrielInventoryManager;

    void Start()
    {
        //vai procurar o inventory manager do gabriel automaticamente 
        GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
        if (gabriel != null)
        {
            gabrielInventoryManager = gabriel.GetComponentInChildren<GabrielInventoryManager>();
          
        }
       
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.CompareTag("Gabriel") && Keyboard.current.bKey.wasPressedThisFrame)
        {
            if (gabrielInventoryManager != null && itemData != null)
            {
                Item newItem = itemData.GetItem();
                gabrielInventoryManager.TryPickupItem(newItem);
                Destroy(gameObject); 
            }

        }
    }

   

}
