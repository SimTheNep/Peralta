using Unity.VisualScripting;
using UnityEngine;

public class HoverSkill : MonoBehaviour
{
    public float Time = 6f;          // tempo m�ximo 
    public float timeRemaining = 0f; // tempo que diminui durante a execu��o
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
            // Quando n�o est� ativo, regenera mana a 1 por segundo at� o m�ximo
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



        if (isActive == false)
        {
            isActive = true;
            timeRemaining = Time;

            if (animator != null)
                animator.SetBool("IsHovering", true);


            this.gameObject.layer = LayerMask.NameToLayer("Floot");
        }
        else
        {
            if (isActive == true)
            {
                print("desligou");
                terminafloot();
            }

        }

    }

    void terminafloot()
    {
        isActive = false;

        if (animator != null)
            animator.SetBool("IsHovering", false);

        this.gameObject.layer = LayerMask.NameToLayer("Peralta");

        Debug.Log("Floot fim");
    }
}
