using UnityEngine;

public class SoulPickup : MonoBehaviour
{
    public MagicItemData magicItemData;

    void Start()
    {
        if (magicItemData == null)
            magicItemData = GetComponent<MagicItemData>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Sempre vai para o inventário do Peralta
        PeraltaInventoryManager peraltaInventory = FindFirstObjectByType<PeraltaInventoryManager>();
        if (peraltaInventory != null && magicItemData != null)
        {
            MagicItem soul = magicItemData.GetMagicItem();
            peraltaInventory.TryPickupMagicItem(soul);
            Destroy(gameObject);
        }
    }
}
