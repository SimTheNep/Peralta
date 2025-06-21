using UnityEngine;

public class HoverSkill : MonoBehaviour
{
    public ManaSystem manaSystem;
    public float manaCostPerSecond = 1f;  
    public bool Active;               
    public bool Return;                
    private Animator animator;
    public bool isActive = false;

    public AudioSource hoverAudioSource;  
    public AudioClip hoverAudioClip;      

    private PeraltaController peraltaController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (manaSystem == null)
            manaSystem = FindFirstObjectByType<ManaSystem>();

        peraltaController = GetComponent<PeraltaController>();

        if (hoverAudioSource != null)
        {
            hoverAudioSource.loop = true;
            hoverAudioSource.clip = hoverAudioClip;
        }
    }

    private void Update()
    {
        if (isActive)
        {
            manaSystem.SpendMana(manaCostPerSecond * Time.deltaTime);
            if (manaSystem.currentMana <= 0f)
            {
                manaSystem.currentMana = 0f;
                EndHover();
            }
        }
    }

    public void Execute()
    {
        if (peraltaController != null && peraltaController.canMove == false)
            return;

        var peraltaSkills = GetComponent<PeraltaSkills>();
        if (peraltaSkills != null && peraltaSkills.isPossessing)
            return;

        if (!isActive && manaSystem.HasMana(0.1f)) 
        {
            isActive = true;

            if (animator != null)
                animator.SetTrigger("StartHover");

            gameObject.layer = LayerMask.NameToLayer("Floot");

            if (peraltaController != null)
                peraltaController.audioSource.enabled = false;

            if (hoverAudioSource != null && !hoverAudioSource.isPlaying)
                hoverAudioSource.Play();
        }
        else
        {
            EndHover();
        }
    }

    void EndHover()
    {
        isActive = false;

        if (animator != null)
            animator.SetTrigger("StopHover");

        gameObject.layer = LayerMask.NameToLayer("Peralta");

        // Re-enable footsteps AudioSource
        if (peraltaController != null)
            peraltaController.audioSource.enabled = true;

        // Stop hover audio loop
        if (hoverAudioSource != null && hoverAudioSource.isPlaying)
            hoverAudioSource.Stop();
    }
}
