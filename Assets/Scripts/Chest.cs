using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    private static List<string> droppedAttackables = new List<string>();

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
    public KeyCode interactKey = KeyCode.B;

    public List<ChestLootEntry> lootTables;
    
    private Animator chestAnimator;                                 
    private bool isPlayerNearby = false;
    private GameObject player;
    public bool isOpened = false;

    void Start()
    {
        Debug.Log("start chest");
        chestAnimator = gameObject.GetComponent<Animator>();
        chestAnimator.enabled = false;
        SetSprite(chestType);
    }

    void Update()
    {
        if (isPlayerNearby && !isOpened && Input.GetKeyDown(interactKey))
        {
            chestAnimator.enabled = true;
            player.GetComponent<Animator>().SetTrigger("Throw");

            string triggerName = GetTrigger(chestType);
            chestAnimator.SetTrigger(triggerName);
            isOpened = true;
            DropItems();
        }
    }

    private void DropItems()
    {
        ChestLootEntry entry = lootTables.Find(e => e.chestType == chestType);
        if (entry == null || entry.possibleItems == null || entry.possibleItems.Count == 0) return;

        int droppedCount = 0;
        List<GameObject> availableItems = new List<GameObject>(entry.possibleItems);

        while (droppedCount < entry.itemCount && availableItems.Count > 0)
        {
            GameObject selectedPrefab = GetWeight(availableItems);
            if (selectedPrefab != null)
            {
                ItemData itemData = selectedPrefab.GetComponent<ItemData>();
                string itemName = selectedPrefab.name;

                bool isAttackable = itemData != null && itemData.itemType == ItemType.Attackable;

                if (isAttackable && droppedAttackables.Contains(itemName))
                {
                    availableItems.Remove(selectedPrefab);
                    continue; 
                }

                if (isAttackable)
                {
                    droppedAttackables.Add(itemName);
                }

                Vector3 offset = new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), 0);
                Instantiate(selectedPrefab, transform.position + offset, Quaternion.identity);
                droppedCount++;
            }
        }
    }


    private GameObject GetWeight(List<GameObject> itemList)
    {
        float totalWeight = 0f;

        foreach (var prefab in itemList)
        {
            int price = GetPrice(prefab);
            totalWeight += 1f / Mathf.Max(price, 1.5f);
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var prefab in itemList)
        {
            int price = GetPrice(prefab);
            cumulative += 1f / Mathf.Max(price, 1.5f);
            if (randomValue <= cumulative)
                return prefab;
        }

        return null;
    }

    private int GetPrice(GameObject prefab)
    {
        var itemData = prefab.GetComponent<ItemData>();
        if (itemData != null) return itemData.price;

        var magicData = prefab.GetComponent<MagicItemData>(); 
        if (magicData != null) return magicData.price;

        Debug.LogWarning("Prefab missing price component: " + prefab.name);
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
            case ChestType.Rare:
                return "RareOpen";
            case ChestType.Gold:
                return "GoldOpen";
            case ChestType.Barrel:
                return "BarrelOpen";
            case ChestType.Common:
            default:
                return "CommonOpen";
        }
    }

    private void SetSprite(ChestType type)
    {

        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("sprite renderer n encontrado");
            return;
        }

        Debug.Log("setsprite chamado paras" + type);

        switch (type)
        {
            case ChestType.Rare:
                print("rarechest_init");
                gameObject.GetComponent<SpriteRenderer>().sprite = rareSprite;
                break;
            case ChestType.Gold:
                print("goldchest_init");
                gameObject.GetComponent<SpriteRenderer>().sprite = goldSprite;
                break;
            case ChestType.Barrel:
                print("barrelchest_init");
                gameObject.GetComponent<SpriteRenderer>().sprite = barrelSprite;
                break;
            case ChestType.Common:
                print("commonchest_init");
                gameObject.GetComponent<SpriteRenderer>().sprite = commonSprite;
                break;
        }
        if (sr.sprite == null)
        {
            Debug.Log("sprite n atribuido");
        }
            
    }
}