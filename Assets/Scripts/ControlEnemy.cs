using UnityEngine;

public class ControlEnemy : MonoBehaviour
{
    public float Velocidade;
    public bool direcao;
    public string Estate;
    public GameObject AlvoGOB;
    public float Life;
    public float maxHealth = 100f; // Ajustar a vida
    public float Radius;
    public float CoolDown;
    public float VPerseguição;
    public float VIdle;
    public float Limites;
    public float Dano;
    public float DeathTime;
    public float PersueTime;

    private Animator animator;

    public GameObject soulPrefab;
    public int soulAmount;

    public string enemyID;  // Identifica o tipo de Inimigo para o bestiário

    public SpriteRenderer healthBarRenderer;
    public Sprite[] healthBarSprites;

    public AudioSource audioSource;                 
    public AudioClip[] footstepClips;               
    public float footstepBaseInterval = 0.5f;       
    private float footstepTimer = 0f;

    public AudioClip activeSound;
    public AudioClip tiredSound;
    public AudioClip deathSound;

    private bool playedActiveSound = false;
    private bool playedTiredSound = false;
    private bool playedDeathSound = false;

    void Start()
    {
        Debug.Log($"[START] {gameObject.name} iniciado na layer {gameObject.layer} com tag {gameObject.tag}");
        animator = GetComponent<Animator>();
        Estate = "Idle";
        Velocidade = VIdle;
        Life = maxHealth; 

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            Debug.LogWarning("[Audio] No AudioSource assigned or found on enemy!");
    }

    void Update()
    {
        UpdateHealthBar();

        Vector3 Pos = transform.position;
        Pos.x += Velocidade * Time.deltaTime;

        HandleFootsteps();

        // Decide state
        Vector3 Df = AlvoGOB.transform.position - transform.position;

        if (Df.magnitude < Radius && Estate == "Idle")
        {
            Estate = "Active";
        }

        if (Life <= 0 && Estate != "Dead")
        {
            PlayDeathSoundOnce();
            animator.Play("Death_Inimigo01");
            Estate = "Dead";

            if (!string.IsNullOrEmpty(enemyID) && BestiaryManager.Instance != null)
            {
                BestiaryManager.Instance.RegisterEnemyKill(enemyID);
                Debug.Log($"[Bestiary] Registered kill: {enemyID}");
            }

            if (healthBarRenderer != null)
                healthBarRenderer.enabled = false;

            Invoke("Death", DeathTime);
        }

        GabrielHealth gabrielHealth = AlvoGOB.GetComponent<GabrielHealth>();
        if (gabrielHealth != null && gabrielHealth.currentHealth <= 0)
        {
            Estate = "Idle";
        }

        // Movement base (patrolling)
        if (Estate == "Idle")
        {
            ResetStateSounds();

            if (Pos.x >= Limites)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                direcao = true;
                Velocidade = -Mathf.Abs(VIdle);
            }
            if (direcao && Pos.x <= -Limites)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                direcao = false;
                Velocidade = Mathf.Abs(VIdle);
            }

            transform.position = Pos;
        }

        // Chase / Attack
        if (Estate == "Active")
        {
            PlayActiveSoundOnce();

            if (animator != null)
                animator.Play("Active_Inimigo01");

            Velocidade = VPerseguição;
            Vector3 Dif = AlvoGOB.transform.position - transform.position;
            Dif.Normalize();
            Dif *= Velocidade * Time.deltaTime;
            transform.Translate(Dif, Space.World);

            Invoke("passaTired", PersueTime);
        }

        // Tired State
        if (Estate == "Tired")
        {
            PlayTiredSoundOnce();

            if (animator != null)
                animator.Play("Tired_Inimigo01");

            Invoke("passaidle", CoolDown);
        }
    }

    void HandleFootsteps()
    {
        if (Estate == "Dead") return;

        float speed = Mathf.Abs(Velocidade);

        if (speed > 0.1f)
        {
            float interval = footstepBaseInterval / speed;

            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayRandomFootstep(interval);
                footstepTimer = interval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    void PlayRandomFootstep(float volumeScale)
    {
        if (audioSource == null || footstepClips == null || footstepClips.Length == 0) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        audioSource.pitch = Random.Range(0.9f, 1.1f);

        float volume = Mathf.Clamp(volumeScale, 0.1f, 1f);
        audioSource.PlayOneShot(clip, volume);
    }

    void PlayActiveSoundOnce()
    {
        if (!playedActiveSound && audioSource != null && activeSound != null)
        {
            audioSource.PlayOneShot(activeSound);
            playedActiveSound = true;
            playedTiredSound = false;
        }
    }

    void PlayTiredSoundOnce()
    {
        if (!playedTiredSound && audioSource != null && tiredSound != null)
        {
            audioSource.PlayOneShot(tiredSound);
            playedTiredSound = true;
            playedActiveSound = false;
        }
    }

    void PlayDeathSoundOnce()
    {
        if (!playedDeathSound && audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
            playedDeathSound = true;
        }
    }

    void ResetStateSounds()
    {
        playedActiveSound = false;
        playedTiredSound = false;
    }

    void UpdateHealthBar()
    {
        if (healthBarRenderer == null || healthBarSprites.Length == 0)
            return;

        if (Life <= 0)
        {
            healthBarRenderer.enabled = false;
            return;
        }

        healthBarRenderer.enabled = true;

        Life = Mathf.Clamp(Life, 0f, maxHealth);

        float healthPercent = Life / maxHealth;
        int index = Mathf.FloorToInt(healthPercent * (healthBarSprites.Length - 1));
        index = Mathf.Clamp(index, 0, healthBarSprites.Length - 1);

        Debug.Log($"[HealthBar] Life: {Life}, Percent: {healthPercent}, Sprite Index: {index}");

        healthBarRenderer.sprite = healthBarSprites[index];
    }

    // Public method to apply damage safely
    public void TakeDamage(float amount)
    {
        if (Estate == "Dead") return;

        Life -= amount;
        Life = Mathf.Clamp(Life, 0, maxHealth);

        Debug.Log($"[Enemy] Took {amount} damage, Life now {Life}");

        if (Life <= 0)
        {
            PlayDeathSoundOnce();
            animator.Play("Death_Inimigo01");
            Estate = "Dead";

            if (!string.IsNullOrEmpty(enemyID) && BestiaryManager.Instance != null)
            {
                BestiaryManager.Instance.RegisterEnemyKill(enemyID);
                Debug.Log($"[Bestiary] Registered kill: {enemyID}");
            }

            if (healthBarRenderer != null)
                healthBarRenderer.enabled = false;

            Invoke("Death", DeathTime);
        }
    }

    void DropSoul()
    {
        if (soulPrefab != null)
        {
            for (int i = 0; i < soulAmount; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0f, 0.2f), 0);
                Instantiate(soulPrefab, transform.position + offset, Quaternion.identity);
            }
        }
    }

    void Death()
    {
        DropSoul();
        Destroy(gameObject);
    }

    void passaidle()
    {
        Estate = "Idle";
        ResetStateSounds();
    }

    void passaTired()
    {
        Estate = "Tired";
        ResetStateSounds();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[ENEMY] OnCollisionEnter2D with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Gabriel"))
        {
            GabrielHealth gabrielHealth = collision.gameObject.GetComponent<GabrielHealth>();

            if (gabrielHealth != null)
            {
                Debug.Log("[ENEMY] Gabriel found and active. Applying damage.");
                gabrielHealth.TakeDamage(Dano);
                Estate = "Tired";
                ResetStateSounds();
            }
            else
            {
                Debug.Log("[ENEMY] GabrielHealth not found or inactive.");
            }
        }
    }
}
