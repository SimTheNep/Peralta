using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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

    public AudioSource audioSource;       
    public AudioClip damageClip;           

    public List<AudioSource> audioSourcesToStop;
    public AudioSource audioSourceToPlay;

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

        if (audioSource != null && damageClip != null)
        {
            audioSource.PlayOneShot(damageClip);
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
        StopPlay();
        yield return new WaitForSeconds(1f);

        if (gameOverOverlay != null)
            gameOverOverlay.SetActive(true);

        int rosas = peraltaInventory != null ? peraltaInventory.GetQuantidadeRosasDeAragao() : 0;
        if (gameOverScreen != null)
            gameOverScreen.Setup(rosas, checkpointLevel);

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

            if (animator != null)
            {
                animator.ResetTrigger("Die");
                animator.SetTrigger("Idle"); 
            }

            if (gameOverOverlay != null)
                gameOverOverlay.SetActive(false);

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
            Debug.Log("No Rosa de Aragão to consume.");
        }
    }

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

    public void StopPlay()
    {
        if (audioSourcesToStop == null || audioSourcesToStop.Count == 0)
        {
            Debug.LogWarning("No AudioSources assigned to stop.");
        }
        else
        {
            foreach (AudioSource source in audioSourcesToStop)
            {
                if (source != null && source.isPlaying)
                {
                    source.Stop();
                }
            }
        }

        if (audioSourceToPlay == null)
        {
            Debug.LogWarning("audioSourceToPlay is not assigned.");
            return;
        }

        audioSourceToPlay.Play();
    }
}
