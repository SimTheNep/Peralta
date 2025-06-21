using Unity.VisualScripting;
using UnityEngine;

public class HoverSkill : MonoBehaviour
{
    public ManaSystem manaSystem;
    public float manaCostPerSecond = 1f;  
    public bool Active;               
    public bool Return;                
    private Animator animator;
    public bool isActive = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (manaSystem == null)
            manaSystem = FindFirstObjectByType<ManaSystem>();
    }

    private void Update()
    {

        if (isActive)
        {
            manaSystem.SpendMana(manaCostPerSecond * Time.deltaTime);
            if (manaSystem.currentMana <= 0f)
            {
                manaSystem.currentMana = 0f;
                terminafloot();
            }
        }
    }

    public void Execute()
    {
        var peraltaSkills = GetComponent<PeraltaSkills>();
        if (peraltaSkills != null && peraltaSkills.isPossessing)
            return;

        if (!isActive && manaSystem.HasMana(0.1f)) // valor mínimo para ativar
        {
            isActive = true;
            if (animator != null)
                animator.SetTrigger("StartHover");
            gameObject.layer = LayerMask.NameToLayer("Floot");
        }
        else
        {
            terminafloot();
        }
    }

    void terminafloot()
    {
        isActive = false;

        if (animator != null)
            animator.SetTrigger("StopHover");

        gameObject.layer = LayerMask.NameToLayer("Peralta");
    }
}
