using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GabrielController : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public bool Flip;
    public bool canMove = true;

    public GameObject pauseMenu;
    public GameObject canvas;

    private bool isPaused = false;

    public PeraltaInventoryManager inventoryManager;

    public AudioSource audioSource;
    public AudioClip defaultFootstepClip;
    public AudioClip waterFootstepClip;

    public Tilemap waterTile;
    public List<Collider2D> waterZones;

    public float footstepInterval = 0.35f;
    private float footstepTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    void Update()
    {
        bool HasAsaIcaro = inventoryManager != null && inventoryManager.HasAsaIcaro();
        bool hasSerpenteEncantada = inventoryManager != null && inventoryManager.HasSerpenteEncantada();

        if (hasSerpenteEncantada)
        {
            moveSpeed = 6f;
            footstepInterval = 0.25f;
        }
        else
        {
            moveSpeed = 5f;
            footstepInterval = 0.35f;
        }

        if (HasAsaIcaro)
        {
            gameObject.layer = LayerMask.NameToLayer("Floot 2");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerCharacters");
        }

        // Pause
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
        if (moveInput.magnitude > 0)
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
            if (audioSource != null)
            {
                audioSource.pitch = 1f;
            }
        }
    }

    private void PlayFootstepSound()
    {
        if (audioSource == null) return;

        AudioClip clipToPlay = defaultFootstepClip;

        foreach (var zone in waterZones)
        {
            if (zone != null && zone.bounds.Contains(transform.position))
            {
                clipToPlay = waterFootstepClip;
                break;
            }
        }

        if (clipToPlay == defaultFootstepClip && waterTile != null)
        {
            Vector3Int tilePosition = waterTile.WorldToCell(transform.position);
            TileBase tile = waterTile.GetTile(tilePosition);
            if (tile != null)
            {
                clipToPlay = waterFootstepClip;
            }
        }

        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(clipToPlay);
    }
}
