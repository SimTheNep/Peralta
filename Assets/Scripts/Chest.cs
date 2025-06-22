using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    public PeraltaInventoryManager inventoryManager;

    public enum ChestType
    {
        Common,
        Rare,
        Gold,
        Barrel
    }

    [System.Serializable]
    public class ChestLootEntry
    {
        public ChestType chestType;
        public List<GameObject> possibleItems;
        public int itemCount = 3;
    }

    public ChestType chestType;
    public Sprite commonSprite;
    public Sprite rareSprite;
    public Sprite goldSprite;
    public Sprite barrelSprite;

    public string playerName = "Gabriel";

    public List<ChestLootEntry> lootTables;

    public AudioSource audioSource;
    public AudioClip openSound;

    private Animator chestAnimator;
    private bool isPlayerNearby = false;
    private GameObject player;
    public bool isOpened = false;

    void Start()
    {
        chestAnimator = GetComponent<Animator>();
        chestAnimator.enabled = false;
        SetSprite(chestType);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerNearby && !isOpened && KeybindManager.GetKeyDown("Action"))
        {
            chestAnimator.enabled = true;
            player.GetComponent<Animator>().SetTrigger("Throw");

            chestAnimator.SetTrigger(GetTrigger(chestType));
            isOpened = true;

            DropItems();

            if (audioSource != null && openSound != null)
                audioSource.PlayOneShot(openSound);
        }
    }

    private void DropItems()
    {
        ChestLootEntry entry = lootTables.Find(e => e.chestType == chestType);
        if (entry == null || entry.possibleItems == null || entry.possibleItems.Count == 0) return;

        List<GameObject> availableItems = new List<GameObject>(entry.possibleItems);
        List<string> localDroppedAttackables = new List<string>();
        int droppedCount = 0;

        while (droppedCount < entry.itemCount && availableItems.Count > 0)
        {
            GameObject selectedPrefab = GetWeightedItem(availableItems);
            if (selectedPrefab == null) break;

            ItemData itemData = selectedPrefab.GetComponent<ItemData>();
            string itemName = selectedPrefab.name;
            bool isAttackable = itemData != null && itemData.itemType == ItemType.Attackable;

            // Skip if already dropped in this chest
            if (isAttackable && localDroppedAttackables.Contains(itemName))
            {
                availableItems.Remove(selectedPrefab);
                continue;
            }

            if (isAttackable)
                localDroppedAttackables.Add(itemName);

            Vector3 offset = new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), 0);
            Instantiate(selectedPrefab, transform.position + offset, Quaternion.identity);
            droppedCount++;
        }
    }

    private GameObject GetWeightedItem(List<GameObject> itemList)
    {
        bool hasSorteNavegador = inventoryManager != null && inventoryManager.HasSorteNavegador();
        float multiplier = hasSorteNavegador ? 3f : 1.5f;
        float totalWeight = 0f;

        Dictionary<GameObject, float> weights = new Dictionary<GameObject, float>();

        foreach (var prefab in itemList)
        {
            int price = Mathf.Max(GetPrice(prefab), 1);
            float weight = 1f / (price / multiplier); // Lower price = higher weight
            weights[prefab] = weight;
            totalWeight += weight;
        }

        float rand = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var kvp in weights)
        {
            cumulative += kvp.Value;
            if (rand <= cumulative)
                return kvp.Key;
        }

        return itemList[Random.Range(0, itemList.Count)];
    }

    private int GetPrice(GameObject prefab)
    {
        var itemData = prefab.GetComponent<ItemData>();
        if (itemData != null) return itemData.price;

        var magicData = prefab.GetComponent<MagicItemData>();
        if (magicData != null) return magicData.price;

        Debug.LogWarning("Missing price component: " + prefab.name);
        return 1;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == playerName)
        {
            isPlayerNearby = true;
            player = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerNearby = false;
            player = null;
        }
    }

    private string GetTrigger(ChestType type)
    {
        switch (type)
        {
            case ChestType.Rare: return "RareOpen";
            case ChestType.Gold: return "GoldOpen";
            case ChestType.Barrel: return "BarrelOpen";
            default: return "CommonOpen";
        }
    }

    private void SetSprite(ChestType type)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer not found on chest!");
            return;
        }

        switch (type)
        {
            case ChestType.Rare:
                sr.sprite = rareSprite;
                break;
            case ChestType.Gold:
                sr.sprite = goldSprite;
                break;
            case ChestType.Barrel:
                sr.sprite = barrelSprite;
                break;
            default:
                sr.sprite = commonSprite;
                break;
        }

        if (sr.sprite == null)
            Debug.LogWarning("Sprite not set for chest type: " + type);
    }
}
