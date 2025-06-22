using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogueSequence dialogeToTrigger;

    public DialogueManager dialogeManager;

    private bool hasPlayed = false; 

    public string character;

    private void Start()
    {
        dialogeManager = FindFirstObjectByType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.CompareTag(character) && dialogeManager != null)
        {
            dialogeManager.StartDialogue(dialogeToTrigger);
            hasPlayed = true;
        }
    }
}
