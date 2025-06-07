using UnityEngine;

public class AttackSkill : MonoBehaviour
{
    public float attackRadius = 1.5f; // raio de detecção de inimigos
    public GabrielInventoryManager gabrielInventoryManager; // referência ao inventário de Gabriel
    public Animator animator; // referência ao animator do Gabriel


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
        Debug.Log("tentar executar");
        if (animator != null)
        {
            Debug.Log("tentar trigger thing");
            animator.SetTrigger("Attack");
        }


        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        Debug.Log("tentar procurar");
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<ControlEnemy>(); 
                if (enemy != null)
                {
                    Debug.Log("econtrou");
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
