using UnityEngine;

public class GabrielHealth : MonoBehaviour
{

    public float maxHealth = 6f;
    public float currentHealth;

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"[START] {gameObject.name} iniciado na layer {gameObject.layer} com tag {gameObject.tag}");
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponentInChildren<Animator>(); 
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"[GABRIEL] TakeDamage chamado. Dano: {amount}");
        currentHealth -= amount;

        Debug.Log($"Gabriel levou {amount} de dano. Vida restante: {currentHealth}");

        if (animator != null)
        {
            animator.SetTrigger("Damage"); 
        }

        if (currentHealth <= 0)
        {
           
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Gabriel morreu!");
        if (animator != null)
        {
            //animator.SetTrigger("Die"); 
        }

        
    }
}
