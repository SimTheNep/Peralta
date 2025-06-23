using UnityEngine;

public class PickupDialog : MonoBehaviour
{
    public DialogueSequence dialogueToTrigger;
    public DialogueManager dialogueManager;
    private bool dialoguePlayed = false;

    public void TriggerDialog()
    {
        if (!dialoguePlayed)
        {
            if (dialogueManager == null)
                dialogueManager = FindFirstObjectByType<DialogueManager>();

            if (dialogueManager != null && dialogueToTrigger != null)
            {
                dialogueManager.StartDialogue(dialogueToTrigger);
                dialoguePlayed = true;
            }
        }
    }
}
