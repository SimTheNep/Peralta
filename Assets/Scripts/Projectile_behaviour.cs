using UnityEngine;

public class Projectile_behaviour : MonoBehaviour
{
    public float Speed;
   
    void Start()
    {
        
    } 
    private void Update()
    {
        //print("pedra vai");
        transform.position += -transform.right * Time.deltaTime * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       /* foreach (var hit in hits)
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
