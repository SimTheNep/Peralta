using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class ControlEnemy : MonoBehaviour
{
    public float Velocidade;
    public bool direcao;
    public string Estate;

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

        if (Estate == "Idle")
        {
            if (Pos.x >= 10)
            {
                this.transform.eulerAngles = new Vector3(0, 0, 180);
                Velocidade = Velocidade * -1;

            }
          /*  if (Pos.x <= -10)
            {
                this.transform.eulerAngles = new Vector3(0, 0, 180);
                Velocidade = Velocidade * -1;
            }*/
                this.transform.position = Pos;
        }
        else
        {
            /*if (Pos.x <= -10)
            {
                Pos.x = 10;
            }
            this.transform.position = Pos;
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            this.transform.eulerAngles = new Vector3(0, 0, -90);
            this.transform.Translate(Incremento, 0, 0, Space.World);
            */
        }
    }
}
