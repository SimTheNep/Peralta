using UnityEngine;

public class HauntSkill : MonoBehaviour
{
    public float hauntRange = 5f;
    public GameObject peralta; 
    private GameObject possessedEnemy;
    private ControlEnemy originalEnemyControl;
    private bool isPossessing = false;

    public void Execute()
    {
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
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy") && IsInLineOfSight(hit.transform))
            {
                GameObject enemy = hit.gameObject;
                possessedEnemy = enemy;

                originalEnemyControl = enemy.GetComponent<ControlEnemy>();
                if (originalEnemyControl != null)
                    originalEnemyControl.enabled = false;

                PossessedEnemyController pc = enemy.AddComponent<PossessedEnemyController>();
                pc.Init(this);

                peralta.SetActive(false);
                isPossessing = true;
                Debug.Log("Possuído: " + enemy.name);
                break;
            }
        }
    }

    void TrySwitchOrReleasePossession()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(possessedEnemy.transform.position, hauntRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.gameObject != possessedEnemy && IsInLineOfSight(hit.transform))
            {
                ReleasePossession();

                possessedEnemy = hit.gameObject;
                originalEnemyControl = possessedEnemy.GetComponent<ControlEnemy>();
                if (originalEnemyControl != null)
                    originalEnemyControl.enabled = false;

                PossessedEnemyController pc = possessedEnemy.AddComponent<PossessedEnemyController>();
                pc.Init(this);

                Debug.Log("Transferiu possessão para: " + possessedEnemy.name);
                return;
            }
        }

        // Nenhum novo inimigo → despossuir
        ReleasePossession();
    }

    void ReleasePossession()
    {
        if (possessedEnemy != null)
        {
            var pc = possessedEnemy.GetComponent<PossessedEnemyController>();
            if (pc != null) Destroy(pc);

            if (originalEnemyControl != null) originalEnemyControl.enabled = true;

            peralta.transform.position = possessedEnemy.transform.position;
            peralta.SetActive(true);

            possessedEnemy = null;
            originalEnemyControl = null;
            isPossessing = false;

            Debug.Log("Despossuiu.");
        }
    }

    bool IsInLineOfSight(Transform target)
    {
        Vector2 direction = target.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, hauntRange);
        return hit.collider != null && hit.transform == target;
    }

    public bool IsPossessing()
    {
        return isPossessing;
    }
}
