using UnityEngine;
using UnityEngine.InputSystem;

public class MagicItemPickup : MonoBehaviour
{
    public MagicItemData itemData;
    public PeraltaInventoryManager peraltaInventoryManager;

    void Start()
    {
        GameObject peralta = GameObject.FindGameObjectWithTag("Peralta");
        if (peralta != null)
        {
            peraltaInventoryManager = peralta.GetComponentInChildren<PeraltaInventoryManager>();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        KeyCode bKeyCode = KeybindManager.GetKeyCode("Action");
        Key bKey = InputHelpers.KeyCodeToKey(bKeyCode);

        if (other.CompareTag("Peralta") && bKey != Key.None && Keyboard.current[bKey].wasPressedThisFrame)
        {
            if (peraltaInventoryManager != null && itemData != null)
            {
                MagicItem newItem = itemData.GetMagicItem();
                peraltaInventoryManager.TryPickupMagicItem(newItem);
                Destroy(gameObject);
            }
        }
    }
}
