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
        KeyCode bKeyCode = KeybindManager.GetKeyCode("Action"); 
        Key bKey = InputHelpers.KeyCodeToKey(bKeyCode);

        if (other.CompareTag("Gabriel") && bKey != Key.None && Keyboard.current[bKey].wasPressedThisFrame)
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
