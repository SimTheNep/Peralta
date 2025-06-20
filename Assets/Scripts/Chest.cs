using UnityEngine;

public class Chest : MonoBehaviour
{
    public enum ChestType
    {
        Common,
        Rare,
        Gold,
        Barrel
    }

    public ChestType chestType;
    public Sprite commonSprite;
    public Sprite rareSprite;
    public Sprite goldSprite;
    public Sprite barrelSprite;

    public string playerName = "Gabriel";           
    public KeyCode interactKey = KeyCode.B;        
    
    private Animator chestAnimator;                                 
    private bool isPlayerNearby = false;
    private GameObject player;
    private bool isOpened = false;

    void Start()
    {
        Debug.Log("start chest");
        chestAnimator = gameObject.GetComponent<Animator>();
        SetSprite(chestType);
    }

    void Update()
    {
        if (isPlayerNearby && !isOpened && Input.GetKeyDown(interactKey))
        {
            player.GetComponent<Animator>().SetTrigger("Throw");

            string triggerName = GetTrigger(chestType);
            chestAnimator.SetTrigger(triggerName);
            isOpened = true;
        }
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
