using UnityEngine;
using UnityEngine.InputSystem;

public class MagicItemPickup : MonoBehaviour
{
    public MagicItemData itemData;
    public PeraltaInventoryManager peraltaInventoryManager;

    private PickupDialog pickupDialog;

    void Start()
    {
        GameObject peralta = GameObject.FindGameObjectWithTag("Peralta");
        if (peralta != null)
        {
            peraltaInventoryManager = peralta.GetComponentInChildren<PeraltaInventoryManager>();
        }

        pickupDialog = GetComponent<PickupDialog>();
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

                if (pickupDialog != null)
                {
                    pickupDialog.TriggerDialog();
                }

                Destroy(gameObject);
            }
        }
    }
}
