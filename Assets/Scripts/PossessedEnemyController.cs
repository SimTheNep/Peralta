using UnityEngine;
using UnityEngine.InputSystem;

public class PossessedEnemyController : MonoBehaviour
{
    private float speed = 5f;
    private HauntSkill hauntSkill;
    private SpriteRenderer spriteRenderer;
    private bool originalFlipX;

    private float originalYRotation;

    public void Init(HauntSkill skillRef)
    {
        hauntSkill = skillRef;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();


        if (spriteRenderer != null)
            originalFlipX = spriteRenderer.flipX;

        originalYRotation = transform.eulerAngles.y;//guarda a rotation original
        transform.eulerAngles = Vector3.zero;//força a rotação para 0 para o flipx funcionar
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(h, v, 0).normalized;

        transform.position += move * speed * Time.deltaTime;

        /*if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryInteractWithLever();
        }*/

        if (h != 0 && spriteRenderer != null)
            spriteRenderer.flipX = h > 0;

    }

    public void RestoreOriginalFlip()
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = originalFlipX;
        transform.eulerAngles = new Vector3(0, originalYRotation, 0);
    }
    /*void TryInteractWithLever()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f);
        if (hit.collider != null && hit.collider.CompareTag("Lever"))
        {
            Lever lever = hit.collider.GetComponent<Lever>();
            if (lever != null)
            {
                lever.Activate();
            }
        }
    }*/
}
