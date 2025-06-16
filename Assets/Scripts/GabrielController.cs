using UnityEngine;

public class GabrielController : MonoBehaviour
{
    public Animator animator; // ref ao animator s� para anima��es


    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    

    private SpriteRenderer spriteRenderer;

    private bool isFlipped = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        

    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput = moveInput.normalized;
        animator.SetBool("IsMoving", moveInput.magnitude > 0);


        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }



    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
