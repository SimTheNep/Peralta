using UnityEngine;

public class GabrielController : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public bool Flip;
    private SpriteRenderer spriteRenderer;

    public bool canMove = true; // para os diálogos

    public GameObject pauseMenu;
    public GameObject canvas;

    private bool isPaused = false;

    public PeraltaInventoryManager inventoryManager;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (pauseMenu != null)
            pauseMenu.SetActive(false);  // pausa fica escondida
    }

    void Update()
    {

        bool hasSerpenteEncantada = inventoryManager != null && inventoryManager.HasSerpenteEncantada();
        if(hasSerpenteEncantada){
            moveSpeed = 6f;
        }
        else {
            moveSpeed = 5f;
        }

        // Check for pause key
        if (KeybindManager.GetKeyDown("Pause"))
        {
            TogglePause();
        }

        if (!canMove || isPaused)
        {
            moveInput = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput = moveInput.normalized;
        animator.SetBool("IsMoving", moveInput.magnitude > 0);

        if (moveInput.x != 0)
        {
            Flip = moveInput.x > 0;
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            if (pauseMenu != null)
                pauseMenu.SetActive(true);
            if (canvas != null)
                canvas.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseMenu != null)
                pauseMenu.SetActive(false);
            if (canvas != null)
                canvas.SetActive(true); 
        }
    }
}
