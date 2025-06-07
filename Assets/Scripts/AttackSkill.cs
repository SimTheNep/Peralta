using UnityEngine;

public class AttackSkill : MonoBehaviour
{
    public float attackRadius = 1.5f; // raio de detec��o de inimigos
    public GabrielInventoryManager gabrielInventoryManager; // refer�ncia ao invent�rio de Gabriel
    public Animator animator; // refer�ncia ao animator do Gabriel


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

        if (animator == null)
        {
            GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
            if (gabriel != null)
            {
                GabrielController controller = gabriel.GetComponent<GabrielController>();
                if (controller != null)
                {
                    animator = controller.animator;
                }
            }
        }
    }

    public void Execute()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }


        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<testenemy>(); //mduar para controlenemy dps
                if (enemy != null)
                {

                    Item item =gabrielInventoryManager.slots[gabrielInventoryManager.selectedSlot];
                    float damage = item != null ? item.damage : 1f;

                    enemy.Life -= damage;

                    Debug.Log($"Gabriel atacou {enemy.name} com {item.itemName}, causando {damage} de dano!");
                }
               
            }
           
        }

       

        /*float attackDistance = 2f;
        Vector2 direction = transform.up; 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackDistance);

        Debug.DrawRay(transform.position, direction * attackDistance, Color.red, 0.5f);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            var enemy = hit.collider.GetComponent<ControlEnemy>();
            if (enemy != null)
            {
                Item item = inventoryManager.slots[inventoryManager.selectedSlot];
                float damage = item != null ? item.damage : 1f;
                enemy.Life -= damage;

                Debug.Log($"Raycast atacou {enemy.name}, dano: {damage}");
            }
        }*/
    } 

}
