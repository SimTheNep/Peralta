using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PhaseSkill : MonoBehaviour
{
    private Tilemap gradeTilemap;

    Animator animator;
    private Vector3 Bpoint;
    private bool OnTileGrade;
    private Vector3 gradeTileWorldPos;

    private bool isActive = false;
    public float timeRemaining = 0f;
    public float Time = 2f;
    public float teleportOffsetY = 5f;

    private PeraltaController peraltaController;

    public AudioSource audioSource;
    public AudioClip phaseSound;

    private void Start()
    {
        peraltaController = GetComponent<PeraltaController>();
        animator = GetComponent<Animator>();
        timeRemaining = Time;
    }

    private void Update()
    {
        if (isActive)
        {
            timeRemaining -= UnityEngine.Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                terminaFase();
            }
        }
        else
        {
            if (timeRemaining < Time)
            {
                timeRemaining += UnityEngine.Time.deltaTime * 1f;
                if (timeRemaining > Time)
                    timeRemaining = Time;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Grade"))
        {
            print("tocou");
            OnTileGrade = true;

            ContactPoint2D contact = collision.GetContact(0);
            Vector3 contactPoint = contact.point;

            gradeTilemap = collision.collider.GetComponentInParent<Tilemap>();
            if (gradeTilemap != null)
            {
                Vector3Int cell = gradeTilemap.WorldToCell(contactPoint);
                gradeTileWorldPos = gradeTilemap.GetCellCenterWorld(cell);
            }
            else
            {
                print("Não encontrou grade");
                gradeTileWorldPos = collision.collider.transform.position;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Grade"))
        {
            OnTileGrade = false;
        }
    }

    public bool Execute()
    {
        if (OnTileGrade == true && !isActive)
        {
            if (audioSource != null && phaseSound != null)
                audioSource.PlayOneShot(phaseSound);

            peraltaController.canMove = false;
            print("tentou atravessar");

            Bpoint = transform.position;
            animator.Play("Peralta_Fase_01");

            isActive = true;
            timeRemaining = Time;
            this.gameObject.layer = LayerMask.NameToLayer("Fase");

            StartCoroutine(PhaseRoutine());

            return true;
        }
        else
        {
            print("Sem grade para atravessar");
            return false;
        }
    }

    IEnumerator PhaseRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        transform.position = new Vector3(gradeTileWorldPos.x, gradeTileWorldPos.y + teleportOffsetY, transform.position.z);
    }

    void terminaFase()
    {
        isActive = false;
        this.gameObject.layer = LayerMask.NameToLayer("Peralta");
        peraltaController.canMove = true;
    }
}
