
using UnityEngine;


public class ControlEnemy : MonoBehaviour
{
    public float Velocidade;
    public bool direcao;
    public string Estate;
    public GameObject AlvoGOB;
    public float Life;
    public float Radius;
    public float CoolDown;
    public float VPerseguição;
    public float VIdle;
    public float Limites;
    public float Dano;
    public float DeathTime;
    public float PersueTime;

    private Animator animator;

    public GameObject soulPrefab; 
    public int soulAmount;

    void Start()
    {
        Debug.Log($"[START] {gameObject.name} iniciado na layer {gameObject.layer} com tag {gameObject.tag}");
        animator = GetComponent<Animator>(); 
        Estate = "Idle";
        Velocidade = VIdle;

    }
    

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = this.transform.position;
        Pos.x = Pos.x + Velocidade * Time.deltaTime;


        // decidir estado
        Vector3 Df = AlvoGOB.transform.position - this.transform.position;
        
        if (Df.magnitude < Radius && Estate=="Idle")
        {
            Estate = "Active";
        }

        if (Life <= 0)
        {
            animator.Play("Death_Inimigo01");
            Estate = "Dead";
            Invoke("Death", DeathTime);

           /* DropSoul();
            Destroy(gameObject);*/

        }
        
        GabrielHealth gabrielHealth = AlvoGOB.GetComponent<GabrielHealth>();
        if (gabrielHealth != null && gabrielHealth.currentHealth <= 0)
        {
            Estate = "Idle";
        }
        
        //sistema de movimento base

        if (Estate == "Idle")
        {
            
            if (Pos.x >= Limites)
            {
                this.transform.eulerAngles = new Vector3(0, 180, 0);
                direcao = true;
                Velocidade = Velocidade * -1;

            }
            if (direcao == true && Pos.x <= -Limites)
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                direcao = false;
                Velocidade = Velocidade * -1;

            }

            this.transform.position = Pos;
        }
        //sistema de ataque perto
       
        if (Estate == "Active")
        {
            if (animator != null)
            {
                animator.Play("Active_Inimigo01");
            }
            
            Velocidade = VPerseguição;
            Vector3 Dif = AlvoGOB.transform.position - this.transform.position;
            Dif.Normalize();
            Dif = Time.deltaTime * Velocidade * Dif;
            this.transform.Translate(Dif, Space.World);
            Invoke("passaTired", PersueTime);

        }
        //sistema de rest
        if (Estate == "Tired")
        {
            if (animator != null)
            {
                animator.Play("Tired_Inimigo01");
            }
  
            Invoke("passaidle", CoolDown);
            
        }


    }
    void DropSoul()
    {
        if (soulPrefab != null)
        {
            for (int i = 0; i < soulAmount; i++)
            {
                // instancia a alma na posição do inimigo 
                Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0f, 0.2f), 0);
                Instantiate(soulPrefab, transform.position + offset, Quaternion.identity);
            }
        }
    }

    void Death()
    {
        DropSoul();

        Destroy(gameObject);
    }
    void passaidle()
    {
        Estate = "Idle";
    }

    void passaTired()
    {
        Estate = "Tired";
    }

    //Sistema de dano
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[ENEMY] OnCollisionEnter2D com: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Gabriel"))
        {
            GabrielHealth gabrielHealth = collision.gameObject.GetComponent<GabrielHealth>();

            if (gabrielHealth != null)
            {
                Debug.Log("[ENEMY] Gabriel encontrado e estado ativo. Aplicando dano.");
                gabrielHealth.TakeDamage(Dano);
                Estate = "Tired";

            }
            else
            {
                Debug.Log("[ENEMY] GabrielHealth n�o encontrado ou estado n�o ativo.");
                
            }
        }
    }
   
   
}


