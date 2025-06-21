using UnityEngine;

public class GabrielHealth : MonoBehaviour
{

    public float maxHealth = 6f;
    public float currentHealth;

    public Animator animator;

    public GameObject gameOverOverlay;
    public GabrielController movementScript;

    public int rosasDeAragao = 1; //temporario

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float amount)
    {

        currentHealth -= amount;


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
        Debug.Log("morrreu");
        if (animator != null)
            animator.SetTrigger("Die");


        //Time.timeScale = 0f;
        if (gameOverOverlay != null && movementScript != null)
        {
            gameOverOverlay.SetActive(true);
            movementScript.canMove = false;
        }
        else
        {
            Debug.Log("overlay esta null");
        }


    }

    public void ContinuarComRosa()
    {
        if (rosasDeAragao > 0)
        {
            rosasDeAragao--;
            currentHealth = maxHealth;
            if (gameOverOverlay != null)
                gameOverOverlay.SetActive(false);
            //Time.timeScale = 1f;
        }
        else
        {
            //n tem rosas, meter qualquer cena
        }
    }
}