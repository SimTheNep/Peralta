using UnityEngine;

public class AttackSkill : MonoBehaviour
{
    public float duration;
    public float cooldown;

    public float baseDuration = 0.6f;
    public float baseCooldown = 0.6f;

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
        Item item = gabrielInventoryManager.slots[gabrielInventoryManager.selectedSlot];
        string itemName = item != null ? item.itemName : "";

        float multiplier = 1f;
        if (itemName == "Montante") multiplier = 2f;
        else if (itemName == "Falcata") multiplier = 0.5f;

        duration = baseDuration * multiplier;
        cooldown = baseCooldown * multiplier;

        if (animator != null)
        {
            animator.speed = 1f / multiplier; // Montante = 0.5x speed, Falcata = 2x speed
            animator.SetTrigger("Attack");
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<ControlEnemy>();
                if (enemy != null)
                {
                    float damage = item != null ? item.damage : 1f;
                    enemy.Life -= damage;
                    Debug.Log($"Gabriel atacou {enemy.name} com {itemName}, causando {damage} de dano!");
                }
            }
        }
    }
}
