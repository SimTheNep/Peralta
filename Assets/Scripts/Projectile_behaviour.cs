using UnityEngine;
using System.Collections;

public class Projectile_behaviour : MonoBehaviour
{
    public GabrielController gabrielController;
    public GabrielInventoryManager gabrielInventoryManager;
    public float Speed;
    public bool goingRight = true;

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

    void Update()
    {
        Vector3 direcao;

        if (goingRight)
        {
            direcao = Vector3.right;
        }
        else
        {
            direcao = Vector3.left;
        }

        transform.position += direcao * Speed * Time.deltaTime;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponentInChildren<Animator>().SetTrigger("kaboom");

            if (collision.collider.CompareTag("Enemy"))
            {
                
                var enemy = collision.collider.GetComponent<ControlEnemy>();
                if (enemy != null && gabrielInventoryManager != null)
                {
                    
                    Item item = gabrielInventoryManager.slots[gabrielInventoryManager.selectedSlot];
                
                    float damage;

                    if (item != null)
                    {
                        damage = item.damage;
                    }
                    else
                    {
                        damage = 1f;
                    }

                    enemy.Life -= damage;
                   
                }

            }

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
