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

    private bool isPossessing = false;

    private CameraFollow cameraFollow;

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
    }

    public void Execute()
    {
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
                if (dir.x > 0)
                    transform.eulerAngles = new Vector3(0, 0, 0);
                else
                    transform.eulerAngles = new Vector3(0, 180, 0);


                if (animator != null)
                    animator.SetTrigger("Possess");


                if (animator != null)
                    StartCoroutine(PerformPossessionAnimationAndHide());
                else
                {
                    HidePeralta();
                    isPossessing = true;

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
                // Reativa o controle do inimigo antigo
                if (originalEnemyControl != null)
                    originalEnemyControl.enabled = true;

                var oldPc = possessedEnemy.GetComponent<PossessedEnemyController>();
                if (oldPc != null)
                    Destroy(oldPc);

                // Atualiza para o novo inimigo
                possessedEnemy = hit.gameObject;
                originalEnemyControl = possessedEnemy.GetComponent<ControlEnemy>();
                if (originalEnemyControl != null)
                    originalEnemyControl.enabled = false;

                PossessedEnemyController pc = possessedEnemy.AddComponent<PossessedEnemyController>();
                pc.Init(this);

              

                // Muda a câmara para o novo corpo
                if (cameraFollow != null)
                    cameraFollow.SetTarget(possessedEnemy.transform);


                Debug.Log("Transferiu possessão para: " + possessedEnemy.name);
                return;
            }
        }

        // Nenhum novo inimigo encontrado → sai da possessão
        ReleasePossession();
    }

    void ReleasePossession()
    {
        if (possessedEnemy != null)
        {
            var pc = possessedEnemy.GetComponent<PossessedEnemyController>();
            if (pc != null) Destroy(pc);

            if (originalEnemyControl != null)
                originalEnemyControl.enabled = true;

            // Posiciona a Peralta perto do inimigo quando para de possuir
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

            Debug.Log("Despossuiu.");
        }
    }


    void HidePeralta()
    {
        // Desativa todos os SpriteRenderers da Peralta para sumir visualmente
        if (spriteRenderers != null)
        {
            foreach (var sr in spriteRenderers)
            {
                sr.enabled = false;
            }
        }

        // Desativa o collider e rigidbody 2D
        if (col2D != null) col2D.enabled = false;
        if (rb2D != null) rb2D.simulated = false;

        // Desativa scripts que bloqueiam movimento e habilidades
        if (peraltaController != null) peraltaController.enabled = false;
        if (inventoryManager != null) inventoryManager.enabled = false;
        if (hoverSkill != null) hoverSkill.enabled = false;
        if (phaseSkill != null) phaseSkill.enabled = false;

        // Desativa o inventário visual no canvas
        if (peraltaInventoryUI != null)
            peraltaInventoryUI.SetActive(false);

        // Diz para PeraltaSkills bloquear todas skills exceto Haunt
        if (peraltaSkills != null)
            peraltaSkills.isPossessing = true;
    }


    void ShowPeralta()
    {
        // Reativa todos os SpriteRenderers da Peralta
        if (spriteRenderers != null)
        {
            foreach (var sr in spriteRenderers)
            {
                sr.enabled = true;
            }
        }

        // Reativa collider e rigidbody 2D
        if (col2D != null) col2D.enabled = true;
        if (rb2D != null) rb2D.simulated = true;

        // Reativa scripts
        if (peraltaController != null) peraltaController.enabled = true;
        if (inventoryManager != null) inventoryManager.enabled = true;
        if (hoverSkill != null) hoverSkill.enabled = true;
        if (phaseSkill != null) phaseSkill.enabled = true;

        // Reativa inventário visual no canvas
        if (peraltaInventoryUI != null)
            peraltaInventoryUI.SetActive(true);

        // Diz para PeraltaSkills liberar todas skills
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
    IEnumerator PerformPossessionAnimationAndHide() //isto foi com o chatgpt, só para esperar antes de play e tal
    {
        if (animator != null)
            animator.SetTrigger("Possess");

        yield return new WaitForSeconds(0.5f); 

        HidePeralta();
        isPossessing = true;

        if (cameraFollow != null)
            cameraFollow.SetTarget(possessedEnemy.transform);
    }

    IEnumerator HidePeraltaAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HidePeralta();
        isPossessing = true;
    }



}
