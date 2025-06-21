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


    }

    void EndSkillOrPossession()
    {
        if (isPossessing)
        {
            ReleasePossession();
        }

        isActive = false;
    }

    public void Execute()
    {
        if (manaSystem.HasMana(0.1f)) // valor min para ativar
        {
            isActive = true;
            Debug.Log("Executando HauntSkill...");
            if (isPossessing)
            {
                TrySwitchOrReleasePossession();
            }
            else
            {
                TryPossessEnemy();
            }
        }
    }


    void TryPossessEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, hauntRange);
        Debug.Log($"Procurando inimigos no raio {hauntRange}, encontrados: {hits.Length}");
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
                break;
            }
        }
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

    void OnDrawGizmosSelected() //serve apenas para vermos o raio
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
