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

    void Start()
    {
            Velocidade = 5f;
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
            gameObject.GetComponent<Animator>().Play("Death_Inimigo01");
            Destroy(gameObject);
        }
        
        //sistema de movimento base

        if (Estate == "Idle")
        {
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
            gameObject.GetComponent<Animator>().Play("Active_Inimigo01");
            Velocidade = 5f;
            Vector3 Dif = AlvoGOB.transform.position - this.transform.position;
            Dif.Normalize();
            Dif = Time.deltaTime * Velocidade * Dif;
            this.transform.Translate(Dif, Space.World);

        }
        //sistema de rest
        if (Estate == "Tired")
        {
            gameObject.GetComponent<Animator>().Play("Tired_Inimigo01");
            Invoke("passaidle", CoolDown);
            
        }


    }
    
    void passaidle()
    {
        Estate = "Idle";
    }
    
    //Sistema de dano
    private void OnCollisionEnter2D(Collision2D collision)
    {
      //  GabrielHealthBar = GabrielHealthBar - 1;

        if (Estate == "Active")
        {
            Estate = "Tired";
        }
    }
   
   
}


