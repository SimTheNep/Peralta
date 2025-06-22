using UnityEngine;
using System.Collections;

public class HauntSkill : MonoBehaviour
{
    public float hauntRange = 5f;
    public GameObject peraltaInventoryUI;
    public GameObject peralta;

    private SpriteRenderer[] spriteRenderers;
    private Animator animator;
    private Collider2D col2D;
    private Rigidbody2D rb2D;
    private PeraltaController peraltaController;
    private PeraltaSkills peraltaSkills;
    private PeraltaInventoryManager inventoryManager;
    private HoverSkill hoverSkill;
    private PhaseSkill phaseSkill;

    private GameObject possessedEnemy = null;
    private ControlEnemy originalEnemyControl = null;

    public bool isPossessing = false;

    private CameraFollow cameraFollow;

    public bool isActive = false;

    public ManaSystem manaSystem;
    public float manaCostPerSecond = 1f;

    public AudioSource audioSource;
    public AudioClip hauntSound;

    public LeverScript currentLever; 

    void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        animator = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        peraltaController = GetComponent<PeraltaController>();
        peraltaSkills = GetComponent<PeraltaSkills>();
        inventoryManager = GetComponent<PeraltaInventoryManager>();
        hoverSkill = GetComponent<HoverSkill>();
        phaseSkill = GetComponent<PhaseSkill>();

        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        if (manaSystem == null)
            manaSystem = FindFirstObjectByType<ManaSystem>();
    }

    void Update()
    {
        if (isActive)
        {
            manaSystem.SpendMana(manaCostPerSecond * Time.deltaTime);
            if (manaSystem.currentMana <= 0f)
            {
                manaSystem.currentMana = 0f;
                EndSkillOrPossession();
                isActive = false;
            }
        }

        if (isPossessing && possessedEnemy != null)
        {
            DetectLeverNearPossessedEnemy();
        }
    }

    void DetectLeverNearPossessedEnemy()
    {
        float detectRadius = 1f; // Adjust as needed
        Collider2D[] hits = Physics2D.OverlapCircleAll(possessedEnemy.transform.position, detectRadius);

        // Disable previous lever indicator if any
        if (currentLever != null)
            currentLever.SetIndicatorActive(false);

        currentLever = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Lever"))
            {
                LeverScript lever = hit.GetComponent<LeverScript>();
                if (lever != null && !lever.IsActivated())
                {
                    currentLever = lever;
                    currentLever.SetIndicatorActive(true);
                    break;
                }
            }
        }
    }

    void EndSkillOrPossession()
    {
        if (isPossessing)
        {
            ReleasePossession();
        }

        isActive = false;
        currentLever = null; // Clear lever on end
    }

    public bool Execute()
    {
        if (!manaSystem.HasMana(0.1f))
            return false;

        bool success = false;

        Debug.Log("Executando HauntSkill...");

        if (audioSource != null && hauntSound != null)
            audioSource.PlayOneShot(hauntSound);

        if (isPossessing)
        {
            TrySwitchOrReleasePossession();
            success = true; 
        }
        else
        {
            success = TryPossessEnemy();
        }

        isActive = success; 

        return success;
    }

    bool TryPossessEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, hauntRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                possessedEnemy = hit.gameObject;
                originalEnemyControl = possessedEnemy.GetComponent<ControlEnemy>();
                if (originalEnemyControl != null)
                    originalEnemyControl.enabled = false;

                PossessedEnemyController pc = possessedEnemy.AddComponent<PossessedEnemyController>();
                pc.Init(this);
                pc.SetLever(currentLever);  

                Vector3 dir = possessedEnemy.transform.position - transform.position;
                transform.eulerAngles = new Vector3(0, dir.x > 0 ? 0 : 180, 0);

                if (animator != null)
                    StartCoroutine(PerformPossessionAnimationAndHide());
                else
                {
                    HidePeralta();
                    isPossessing = true;

                    if (peraltaSkills != null)
                        peraltaSkills.isPossessing = true;

                    if (cameraFollow != null)
                        cameraFollow.SetTarget(possessedEnemy.transform);
                }

                Debug.Log("Possuído: " + possessedEnemy.name);
                isPossessing = true;
                if (peraltaSkills != null)
                    peraltaSkills.isPossessing = true;
                if (cameraFollow != null)
                    cameraFollow.SetTarget(possessedEnemy.transform);

                return true; 
            }
        }
        return false; 
    }

    void TrySwitchOrReleasePossession()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(possessedEnemy.transform.position, hauntRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.gameObject != possessedEnemy)
            {
                if (originalEnemyControl != null)
                    originalEnemyControl.enabled = true;

                var oldPc = possessedEnemy.GetComponent<PossessedEnemyController>();
                if (oldPc != null)
                    Destroy(oldPc);

                possessedEnemy = hit.gameObject;
                originalEnemyControl = possessedEnemy.GetComponent<ControlEnemy>();
                if (originalEnemyControl != null)
                    originalEnemyControl.enabled = false;

                PossessedEnemyController pc = possessedEnemy.AddComponent<PossessedEnemyController>();
                pc.Init(this);
                pc.SetLever(currentLever);  

                if (cameraFollow != null)
                    cameraFollow.SetTarget(possessedEnemy.transform);

                Debug.Log("Transferiu possessão para: " + possessedEnemy.name);
                return;
            }
        }

        ReleasePossession();
    }

    void ReleasePossession()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        if (possessedEnemy != null)
        {
            var pc = possessedEnemy.GetComponent<PossessedEnemyController>();
            if (pc != null)
            {
                pc.RestoreOriginalFlip();
                Destroy(pc);
            }
            var peraltaSprite = peralta.GetComponent<SpriteRenderer>();
            if (peraltaSprite != null)
                peraltaSprite.flipX = false;

            if (originalEnemyControl != null)
                originalEnemyControl.enabled = true;

            Vector3 offset = new Vector3(1f, 0f, 0f);
            peralta.transform.position = possessedEnemy.transform.position + offset;

            ShowPeralta();

            if (animator != null)
                animator.SetTrigger("Unpossess");

            if (cameraFollow != null)
                cameraFollow.SetTarget(this.transform);

            possessedEnemy = null;
            originalEnemyControl = null;

            isPossessing = false;
            isActive = false;
            currentLever = null;  // Clear lever on release
            if (peraltaSkills != null)
                peraltaSkills.isPossessing = false;

            Debug.Log("Despossuiu.");
        }
    }

    void HidePeralta()
    {
        if (spriteRenderers != null)
        {
            foreach (var sr in spriteRenderers)
                sr.enabled = false;
        }

        if (col2D != null) col2D.enabled = false;
        if (rb2D != null) rb2D.simulated = false;

        if (peraltaController != null) peraltaController.enabled = false;
        if (inventoryManager != null) inventoryManager.enabled = false;
        if (hoverSkill != null) hoverSkill.enabled = false;
        if (phaseSkill != null) phaseSkill.enabled = false;

        if (peraltaInventoryUI != null)
            peraltaInventoryUI.SetActive(false);

        if (peraltaSkills != null)
            peraltaSkills.isPossessing = true;
    }

    void ShowPeralta()
    {
        if (spriteRenderers != null)
        {
            foreach (var sr in spriteRenderers)
                sr.enabled = true;
        }

        if (col2D != null) col2D.enabled = true;
        if (rb2D != null) rb2D.simulated = true;

        if (peraltaController != null) peraltaController.enabled = true;
        if (inventoryManager != null) inventoryManager.enabled = true;
        if (hoverSkill != null) hoverSkill.enabled = true;
        if (phaseSkill != null) phaseSkill.enabled = true;

        if (peraltaInventoryUI != null)
            peraltaInventoryUI.SetActive(true);

        if (peraltaSkills != null)
            peraltaSkills.isPossessing = false;
    }

    public bool IsPossessing()
    {
        return isPossessing;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hauntRange);

        if (possessedEnemy != null)
        {
            Gizmos.color = Color.red;
            Vector3 dir = (possessedEnemy.transform.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, dir * hauntRange);
        }
    }

    IEnumerator PerformPossessionAnimationAndHide()
    {
        if (animator != null)
            animator.SetTrigger("Possess");

        Vector3 startPosition = peralta.transform.position;
        Vector3 targetPosition = possessedEnemy.transform.position;
        float elapsed = 0f;
        float dashDuration = 0.5f;

        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;
            peralta.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / dashDuration);
            yield return null;
        }

        HidePeralta();
        isPossessing = true;

        if (cameraFollow != null)
            cameraFollow.SetTarget(possessedEnemy.transform);
    }
}
