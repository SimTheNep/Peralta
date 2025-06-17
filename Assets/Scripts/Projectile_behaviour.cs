using UnityEngine;

public class Projectile_behaviour : MonoBehaviour
{
    public GabrielController gabrielController;
    public GabrielInventoryManager gabrielInventoryManager;
    public float Speed;
    public bool Flip2;
   
    void Start()
    {

        if (gabrielInventoryManager == null)
        {
            GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
            if (gabriel != null)
            {
                gabrielInventoryManager = gabriel.GetComponentInChildren<GabrielInventoryManager>();
            }
        }

        if (gabrielController == null)
        {
            GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
            if (gabriel != null)
            {
                gabrielController = gabriel.GetComponentInChildren<GabrielController>();
            }
        }


    }
    private void Update()
    {
        if (gabrielController.Flip == true) {

            Flip2 = true;
        }

        if (gabrielController.Flip == false)
        {

            Flip2 = false;
        }

        //print("pedra vai");
        if (Flip2 == false)
        {
            transform.position += -transform.right * Time.deltaTime * Speed;
        }

        if (Flip2 == true)
        {
            print("Ei");
            transform.position += transform.right * Time.deltaTime * Speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<ControlEnemy>();
                if (enemy != null)
                {
                    
                    Item item = gabrielInventoryManager.slots[gabrielInventoryManager.selectedSlot];
                    float damage = item != null ? item.damage : 1f;

                    enemy.Life -= damage;

                    Debug.Log($"Gabriel atacou {enemy.name} com {item.itemName}, causando {damage} de dano!");
                }

            }

        }*/
        Destroy(gameObject);
    }
}
