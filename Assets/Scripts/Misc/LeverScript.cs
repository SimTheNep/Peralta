using UnityEngine;
using UnityEngine.InputSystem;

public class LeverScript : MonoBehaviour
{
    private bool activated = false;
    private bool canBeActivated = false;
    private Animator animator;

    public AudioSource audioSource;
    public AudioClip activationSound;

    [Tooltip("Assign the GameObject that contains the Bridge child")]
    public GameObject targetGOB;

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
            animator.SetTrigger("On");

        if (audioSource != null && activationSound != null)
            audioSource.PlayOneShot(activationSound);

        Debug.Log("Lever activated!");

        if (targetGOB != null)
        {
            Transform bridge = targetGOB.transform.Find("Bridge");
            if (bridge != null)
            {
                // Disable the collider on Bridge
                Collider2D bridgeCollider = bridge.GetComponent<Collider2D>();
                if (bridgeCollider != null)
                    bridgeCollider.enabled = false;
                else
                    Debug.LogWarning("Bridge does not have a Collider2D component.");

                // Enable all children under Bridge
                foreach (Transform child in bridge)
                {
                    child.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.LogWarning("Bridge child not found under targetGOB.");
            }
        }
        else
        {
            Debug.LogWarning("targetGOB is not assigned.");
        }
    }

    public bool IsActivated()
    {
        return activated;
    }

    public void SetIndicatorActive(bool active)
    {
        Transform indicator = transform.Find("Indicator");
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
