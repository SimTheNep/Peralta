using UnityEngine;
using UnityEngine.InputSystem;

public class PossessedEnemyController : MonoBehaviour
{
    private HauntSkill hauntSkill;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private float moveSpeed = 3f;

    private LeverScript currentLever;  // Added: lever passed from HauntSkill

    public void Init(HauntSkill hauntSkill)
    {
        this.hauntSkill = hauntSkill;
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetLever(LeverScript lever)
    {
        currentLever = lever;
    }

    void Update()
    {
        if (hauntSkill == null || !hauntSkill.isActive)
            return;

        Vector2 moveInput = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moveInput.x = -1;
            else if (Keyboard.current.dKey.isPressed) moveInput.x = 1;

            if (Keyboard.current.wKey.isPressed) moveInput.y = 1;
            else if (Keyboard.current.sKey.isPressed) moveInput.y = -1;
        }

        Vector2 velocity = moveInput.normalized * moveSpeed;
        if (rb2d != null)
        {
            rb2d.linearVelocity = velocity;
        }

        if (moveInput.x < 0)
            spriteRenderer.flipX = true;
        else if (moveInput.x > 0)
            spriteRenderer.flipX = false;

        KeyCode actionKeyCode = KeybindManager.GetKeyCode("Action");
        Key actionKey = InputHelpers.KeyCodeToKey(actionKeyCode);

        if (actionKey != Key.None && Keyboard.current[actionKey].wasPressedThisFrame && currentLever != null)
        {
            currentLever.Activate();
        }
    }

    public void RestoreOriginalFlip()
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = false;
    }
}
