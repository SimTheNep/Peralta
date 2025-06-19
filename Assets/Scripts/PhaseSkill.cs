using UnityEngine;

public class PhaseSkill : MonoBehaviour
{
    Animator animator;
    private Vector3 Bpoint;
    private bool OnTileGrade;

    private bool isActive = false;
    public float timeRemaining = 0f;
    public float Time = 2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        timeRemaining = Time;

        /* if (== null)
         {
             GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
             if (gabriel != null)
             {
                 gabrielInventoryManager = gabriel.GetComponentInChildren<GabrielInventoryManager>();
             }
         }*/

    }

    private void Update()
    {
        if (isActive)
        {
            timeRemaining -= UnityEngine.Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                terminaFase();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.collider.CompareTag("Grade")))
        {
            print("tocou");
            OnTileGrade = true;
        }

    }
    public void Execute()
    {
        if (OnTileGrade == true)
        {
            print("tentou atravessar");
           /* if (animator != null)
            {
                animator.SetTrigger("Phase");

            }*/
            Bpoint = transform.position;
            animator.Play("Peralta_Fase_01");

            if (!isActive)
            {
                isActive = true;
                timeRemaining = Time;
            
                this.gameObject.layer = LayerMask.NameToLayer("Fase");
            }
           // transform.position = Bpoint * new Vector2(-1, 0);
            //terminaFase();
        }
        else
        {
            print("Sem grade para atravessar");
        }
    }

    void terminaFase()
    {
        animator.Play("Peralta_Fase_02");
        isActive = false;

        if (animator != null)
            animator.SetBool("IsHovering", false);

        //transform.forward = new Vector3(-1, 0, 0);

        this.gameObject.layer = LayerMask.NameToLayer("Peralta");

       
    }
}


