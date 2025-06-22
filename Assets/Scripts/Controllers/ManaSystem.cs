using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    public float maxMana = 6f;
    public float currentMana = 6f;
    public float regenRate = 1f;


    public HoverSkill hoverSkill;
    public HauntSkill hauntSkill;

    void Start()
    {
        if (hoverSkill == null)
            hoverSkill = FindFirstObjectByType<HoverSkill>();
        if (hauntSkill == null)
            hauntSkill = FindFirstObjectByType<HauntSkill>();
    }

    void Update()
    {
        // S� regenera se NENHUMA skill estiver ativa
        bool anySkillActive = (hoverSkill != null && hoverSkill.isActive) || (hauntSkill != null && hauntSkill.isActive);

        if (!anySkillActive && currentMana < maxMana)
        {
            currentMana += regenRate * Time.deltaTime;
            if (currentMana > maxMana)
                currentMana = maxMana;
        }
    }

    public bool HasMana(float amount) => currentMana >= amount;

    public void SpendMana(float amount)
    {
        currentMana -= amount;
        if (currentMana < 0f) currentMana = 0f;
    }
}
