using Unity.VisualScripting;
using UnityEngine;

public class HoverSkill : MonoBehaviour
{
    public float Time = 6f;            // Tempo máximo
    public float timeRemaining = 0f;   // Tempo que diminui durante a execução
    public bool Active;               
    public bool Return;                
    private Animator animator;
    private bool isActive = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        timeRemaining = Time;
    }

    private void Update()
    {

        if (isActive)
        {
            timeRemaining -= UnityEngine.Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                terminafloot();
            }
        }
        else
        {
            // Mana regen
            if (timeRemaining < Time)
            {
                timeRemaining += UnityEngine.Time.deltaTime;
                if (timeRemaining > Time)
                    timeRemaining = Time;
            }
        }
    }

    public void Execute()
    {

        Debug.Log("Peralta executou HoverSkill");

        if (!isActive)
        {
            isActive = true;

            if (animator != null)
                animator.SetTrigger("StartHover");

            gameObject.layer = LayerMask.NameToLayer("Floot");

            Debug.Log("Floot 0");
        }
        else
        {
            Debug.Log("Desligou");
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
