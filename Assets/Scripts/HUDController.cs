using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    
    public Image gabrielHealthImage;
    public Sprite[] gabrielHealthSprites; // 0 (cheio) a 6 (vazio)

    
    public Image peraltaManaImage;
    public Sprite[] peraltaManaSprites; // 0 (cheio) a 6 (vazio)

    public Image peraltaPossessionImage;
    public Sprite[] peraltaPossessionSprites; // 0 (n�o possu�do) at� 6 (totalmente possu�do)

    public GabrielHealth gabrielHealth;
    public HoverSkill hoverSkill;
    private HauntSkill hauntSkill;

    private float possessionFill = 0f;  
    private float possessionTarget = 6f; 
    private float possessionSpeed = 6f; 

    public void SetHUDVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    void Start()
    {
        // Refer�ncias
        GabrielHealth gabrielHealth = FindAnyObjectByType<GabrielHealth>();

        hoverSkill = FindFirstObjectByType<HoverSkill>();
        hauntSkill = FindFirstObjectByType<HauntSkill>();
    }

    void Update()
    {
        UpdateGabrielHUD();
        UpdatePeraltaMana();
        UpdatePeraltaPossession();
    }

    void UpdateGabrielHUD()
    {
        if (gabrielHealth != null && gabrielHealthImage != null && gabrielHealthSprites.Length > 0)
        {
            int index = Mathf.Clamp(Mathf.RoundToInt((1 - gabrielHealth.currentHealth / gabrielHealth.maxHealth) * 6), 0, 6);
            gabrielHealthImage.sprite = gabrielHealthSprites[index];
        }
    }

    void UpdatePeraltaMana()
    {
        if (hoverSkill != null && peraltaManaImage != null && peraltaManaSprites.Length > 0)
        {
            float percent = Mathf.Clamp01(hoverSkill.timeRemaining / hoverSkill.Time);
            int index = Mathf.Clamp(Mathf.RoundToInt((1 - percent) * 6), 0, 6);

            peraltaManaImage.sprite = peraltaManaSprites[index];
        }
    }

    void UpdatePeraltaPossession()
    {
        if (peraltaPossessionSprites == null || peraltaPossessionSprites.Length == 0 || peraltaPossessionImage == null || hauntSkill == null)
            return;

        possessionTarget = hauntSkill.IsPossessing() ? 6f : 0f;

        possessionFill = Mathf.MoveTowards(possessionFill, possessionTarget, possessionSpeed * Time.deltaTime);

        int index = Mathf.Clamp(Mathf.RoundToInt(possessionFill), 0, 6);
        peraltaPossessionImage.sprite = peraltaPossessionSprites[index];
    }
}
