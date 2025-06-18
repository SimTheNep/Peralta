using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    
    public Image gabrielHealthImage;
    public Sprite[] gabrielHealthSprites; // 0 (cheio) a 6 (vazio)

    
    public Image peraltaManaImage;
    public Sprite[] peraltaManaSprites; // 0 (cheio) a 6 (vazio)

    public Image peraltaPossessionImage;
    public Sprite[] peraltaPossessionSprites; // 0 (não possuído) até 6 (totalmente possuído)

    public GabrielHealth gabrielHealth;
    public HoverSkill hoverSkill;
    private HauntSkill hauntSkill;

    public void SetHUDVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    void Start()
    {
        // Referências
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
        if (hauntSkill != null && peraltaPossessionImage != null && peraltaPossessionSprites.Length > 0)
        {
            // Para este exemplo simples:
            // Possuído = index 6
            // Não possuído = index 0
            // Transições poderiam ser animadas com coroutines ou outro sistema

            if (hauntSkill.IsPossessing())
                peraltaPossessionImage.sprite = peraltaPossessionSprites[6];
            else
                peraltaPossessionImage.sprite = peraltaPossessionSprites[0];
        }
    }
}
