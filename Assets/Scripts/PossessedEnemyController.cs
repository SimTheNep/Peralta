using UnityEngine;
using UnityEngine.InputSystem;

public class PossessedEnemyController : MonoBehaviour
{
    private float speed = 5f;
    private HauntSkill hauntSkill;

    public void Init(HauntSkill skillRef)
    {
        hauntSkill = skillRef;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(h, v, 0).normalized;

        transform.position += move * speed * Time.deltaTime;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryInteractWithLever();
        }

        if (h != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

    }

    void TryInteractWithLever()
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
    }
}
