using UnityEngine;

public class PeraltaController : MonoBehaviour
{
    public Animator animator;   
    public float moveSpeed = 3f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public bool canMove = true; // para os dialogos
    public GameObject pauseMenu;
    public GameObject canvas;

    private bool isPaused = false;

    public PeraltaInventoryManager inventoryManager;

    public AudioSource audioSource;
    public AudioClip groundFootstepClip;

    public float footstepInterval = 0.45f;
    private float footstepTimer = 0f;

    // Added: flag to enable/disable footsteps externally
    public bool footstepsEnabled = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    void Update()
    {
        bool hasSerpenteEncantada = inventoryManager != null && inventoryManager.HasSerpenteEncantada();
        if(hasSerpenteEncantada){
            moveSpeed = 4f;
            footstepInterval = 0.35f;
        }
        else {
            moveSpeed = 3f;
            footstepInterval = 0.45f;
        }

        if (KeybindManager.GetKeyDown("Pause"))
        {
            TogglePause();
        }

        if (!canMove || isPaused)
        {
            moveInput = Vector2.zero;
            animator.SetBool("IsMoving", false);
            ResetFootstepAudio();
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        animator.SetBool("IsMoving", moveInput.magnitude > 0);

        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }

        HandleFootsteps();
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
            ResetFootstepAudio();
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

    private void HandleFootsteps()
    {
        // Added check for footstepsEnabled here
        if (footstepsEnabled && moveInput.magnitude > 0)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
            ResetFootstepAudio();
        }
    }

    private void PlayFootstepSound()
    {
        if (audioSource == null || groundFootstepClip == null)
            return;

        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(groundFootstepClip);
    }

    private void ResetFootstepAudio()
    {
        if (audioSource != null)
        {
            audioSource.pitch = 1f;
        }
    }
}
