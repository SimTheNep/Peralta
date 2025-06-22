using UnityEngine;
using UnityEngine.InputSystem;

public class LeverScript : MonoBehaviour
{
    private bool activated = false;
    private bool canBeActivated = false;
    private Animator animator;

    public AudioSource audioSource;
    public AudioClip activationSound;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (canBeActivated && !activated)
        {
            KeyCode actionKeyCode = KeybindManager.GetKeyCode("Action");
            Key actionKey = InputHelpers.KeyCodeToKey(actionKeyCode);

            if (Keyboard.current[actionKey].wasPressedThisFrame)
            {
                Activate();
            }
        }
    }

    public void Activate()
    {
        if (activated) return;

        activated = true;
        SetIndicatorActive(false);

        if (animator != null)
            animator.SetTrigger("on");

        if (audioSource != null && activationSound != null)
            audioSource.PlayOneShot(activationSound);

        Debug.Log("Lever activated!");
    }

    public bool IsActivated()
    {
        return activated;
    }

    public void SetIndicatorActive(bool active)
    {
        Transform indicator = transform.Find("indicator");
        if (indicator != null)
            indicator.gameObject.SetActive(active);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.gameObject.name.Contains("Inimigo"))
        {
            canBeActivated = true;
            SetIndicatorActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (activated) return;

        if (other.gameObject.name.Contains("Inimigo"))
        {
            canBeActivated = false;
            SetIndicatorActive(false);
        }
    }
}
