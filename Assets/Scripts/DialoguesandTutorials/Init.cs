using UnityEngine;

public class Init : MonoBehaviour
{
    public DialogueSequence dialogueToTrigger;

    public DialogueManager dialogueManager;

    private bool played = false; 

    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (!played && dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogueToTrigger);
            played = true;
        }
    }
}
