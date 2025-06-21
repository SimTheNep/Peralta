using System.Collections;
using UnityEngine;

public class GabrielHealth : MonoBehaviour
{
    public float maxHealth = 6f;
    public float currentHealth;

    public Animator animator;

    public GameObject gameOverOverlay;

    public GabrielController gabrielController;
    public PeraltaController peraltaController;
    public GabrielSkills gabrielSkills;
    public PeraltaSkills peraltaSkills;
    public CharacterSwitch characterSwitch;
    public GabrielInventoryManager gabrielInventory;
    public PeraltaInventoryManager peraltaInventory;

    public GameOverScreen gameOverScreen;

    public int checkpointLevel = 1;


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
        Debug.Log("morreu");
        if (gabrielController != null)
            gabrielController.canMove = false;
        if (animator != null)
            animator.SetTrigger("Die");
        StartCoroutine(GameOverDelay());

    }

    IEnumerator GameOverDelay()
    {
    
        yield return new WaitForSeconds(1f);

        if (gameOverOverlay != null)
            gameOverOverlay.SetActive(true);

        int rosas = peraltaInventory != null ? peraltaInventory.GetQuantidadeRosasDeAragao() : 0;
        if (gameOverScreen != null)
            gameOverScreen.Setup(rosas, checkpointLevel);

        // desativar todoos os scripts de input aqui

        Time.timeScale = 0f;

        if (peraltaController != null) peraltaController.enabled = false;
        if (gabrielController != null) gabrielController.enabled = false;
        if (peraltaSkills != null) peraltaSkills.enabled = false;
        if (gabrielSkills != null) gabrielSkills.enabled = false;
        if (characterSwitch != null) characterSwitch.enabled = false;
        if (peraltaInventory != null) peraltaInventory.canUseInventory = false;
        if (gabrielInventory != null) gabrielInventory.canUseInventory = false;
    }

    public void ContinuarComRosa()
    {
        if (peraltaInventory != null && peraltaInventory.ConsumeRosaDeAragao())
        {
            currentHealth = maxHealth;
            if (gameOverOverlay != null)
                gameOverOverlay.SetActive(false);


            //reativar todos os scripts aqui
            Time.timeScale = 1f;

            if (peraltaController != null) peraltaController.enabled = true;
            if (gabrielController != null) gabrielController.enabled = true;
            if (peraltaSkills != null) peraltaSkills.enabled = true;
            if (gabrielSkills != null) gabrielSkills.enabled = true;
            if (characterSwitch != null) characterSwitch.enabled = true;
            if (peraltaInventory != null) peraltaInventory.canUseInventory = true;
            if (gabrielInventory != null) gabrielInventory.canUseInventory = true;
        }
        else
        {
            //n tem rosas, meter qualquer cena
        }
    }

    // == Cura == 

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log($"Curou {amount} de vida");
    }

    public IEnumerator RegenerateHealthOverTime(float totalAmount, float duration)
    {
        float amountHealed = 0f;
        float healPerSecond = totalAmount / duration;

        while (amountHealed < totalAmount)
        {
            if (currentHealth >= maxHealth)
            {
                // Stacking
                yield return new WaitUntil(() => currentHealth < maxHealth);
            }

            float healThisFrame = healPerSecond * Time.deltaTime;

            float possibleHeal = Mathf.Min(healThisFrame, totalAmount - amountHealed);
            currentHealth += possibleHeal;
            amountHealed += possibleHeal;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            yield return null;
        }

        Debug.Log($"Regenerou {amountHealed} de vida sobre {duration} segundos");
    }
}
