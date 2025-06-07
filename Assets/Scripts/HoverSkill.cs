using Unity.VisualScripting;
using UnityEngine;

public class HoverSkill : MonoBehaviour
{
    public float Time;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
       
    }


    public void Execute()
    {
        Debug.Log("Peralta executou HoverSkill");
        print("Floot 0");
        if (Time > 0)
        {
            if (animator != null)
            {
                animator.Play("Hover_Peralta01");
                

            }
            this.gameObject.layer = LayerMask.NameToLayer("Floot");
            print("Floot 1");
            Invoke("terminafloot", Time);
            
        }
        
    }

    void terminafloot()
    {
        animator.Play("Idle_Peralta01");
        print("Floot fim");
        this.gameObject.layer = LayerMask.NameToLayer("Peralta");

    }
}
