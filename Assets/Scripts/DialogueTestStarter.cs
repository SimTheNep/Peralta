using UnityEngine;

public class DialogueTestStarter : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueSequence testSequence;

    void Start()
    {
        dialogueManager.StartDialogue(testSequence);
    }
}
