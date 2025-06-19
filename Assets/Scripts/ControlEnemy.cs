using System.Collections;
using System.Linq.Expressions;
using UnityEditor.Rendering.Analytics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class ControlEnemy : MonoBehaviour
{
    public float Velocidade;
    public bool direcao;
    public string Estate;
    public GameObject AlvoGOB;
    public float Life;
    public float Radius;
    public float CoolDown;

    private Animator animator;

    public GameObject soulPrefab; 
    public int soulAmount = 1;

    void Start()
    {
        Debug.Log($"[START] {gameObject.name} iniciado na layer {gameObject.layer} com tag {gameObject.tag}");
        animator = GetComponent<Animator>(); 
            Estate = "Idle";
            
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
            if (animator != null)
            {
                animator.Play("Death_Inimigo01");
            }
            DropSoul();

            Destroy(gameObject);
        }
        
        //sistema de movimento base

        if (Estate == "Idle")
        {
            Velocidade = 1f;
            if (Pos.x >= 10)
            {
                this.transform.eulerAngles = new Vector3(0, 180, 0);
                direcao = true;
                Velocidade = Velocidade * -1;

            }
            if (direcao == true && Pos.x <= -10)
            {
                print("Vira");
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
            
            Velocidade = 4f;
            Vector3 Dif = AlvoGOB.transform.position - this.transform.position;
            Dif.Normalize();
            Dif = Time.deltaTime * Velocidade * Dif;
            this.transform.Translate(Dif, Space.World);

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
    void passaidle()
    {
        Estate = "Idle";
    }
    
    //Sistema de dano
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[ENEMY] OnCollisionEnter2D com: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Gabriel"))
        {
            GabrielHealth gabrielHealth = collision.gameObject.GetComponent<GabrielHealth>();

            if (gabrielHealth != null && Estate == "Active")
            {
                Debug.Log("[ENEMY] Gabriel encontrado e estado ativo. Aplicando dano.");
                gabrielHealth.TakeDamage(1f);

                Estate = "Tired";
            }
            else
            {
                Debug.Log("[ENEMY] GabrielHealth n�o encontrado ou estado n�o ativo.");
            }
        }
    }
   
   
}


