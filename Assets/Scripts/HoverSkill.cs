using Unity.VisualScripting;
using UnityEngine;

public class HoverSkill : MonoBehaviour
{
    public float Time = 6f;          // tempo máximo 
    public float timeRemaining = 0f; // tempo que diminui durante a execução

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
            // Quando não está ativo, regenera mana a 1 por segundo até o máximo
            if (timeRemaining < Time)
            {
                timeRemaining += UnityEngine.Time.deltaTime * 1f; // 1 por segundo
                if (timeRemaining > Time)
                    timeRemaining = Time;
            }
        }
    }


    public void Execute()
    {
        Debug.Log("Peralta executou HoverSkill");
        print("Floot 0");



        if (!isActive)
        {
            isActive = true;
            timeRemaining = Time;

            if (animator != null)
                animator.SetTrigger("Hover");
                animator.Play("HoverLoop");

            this.gameObject.layer = LayerMask.NameToLayer("Floot");
        }

    }

    void terminafloot()
    {
        isActive = false;

        if (animator != null)
            animator.Play("Idle_Peralta01");

        this.gameObject.layer = LayerMask.NameToLayer("Peralta");

        Debug.Log("Floot fim");
    }
}
