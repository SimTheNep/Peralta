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

            if (collision.collider == CompareTag("Enemy"))
            {
                var enemy = collision.collider.GetComponent<ControlEnemy>();
                if (enemy != null)
                {
                    print("atingiu");
                    Item item = gabrielInventoryManager.slots[gabrielInventoryManager.selectedSlot];
                    float damage = item != null ? item.damage : 1f;

                    enemy.Life -= damage;
                   
                }

            }

        Destroy(gameObject);
    }
}
