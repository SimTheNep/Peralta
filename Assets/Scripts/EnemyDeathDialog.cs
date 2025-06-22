using UnityEngine;

public class EnemyDialogueHandler : MonoBehaviour
{
    public DialogueSequence dialogueAfterDeath;
    private DialogueManager dialogueManager;
    private ControlEnemy enemy;

    private bool dialogueTriggered = false;
    private bool deathHandled = false;
    public AudioSource musicPlayer;

    void Start()
    {
        enemy = GetComponent<ControlEnemy>();
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    void Update()
    {
        if (dialogueManager == null || enemy == null)
            return;

        if (dialogueManager.IsDialogueActive())
        {
            enemy.Estate = "Tired";
        }

        if (enemy.Life <= 0 && !deathHandled)
        {
            deathHandled = true;

            if (dialogueAfterDeath != null && dialogueManager != null)
            {
                dialogueManager.StartDialogue(dialogueAfterDeath);
                dialogueTriggered = true;
            }

            StartCoroutine(WaitToDestroy());
        }
    }

    System.Collections.IEnumerator WaitToDestroy()
    {
        PlayMusicIfNotPlaying();
        while (dialogueManager.IsDialogueActive())
        {
            yield return null;
        }

        if (enemy != null)
        {
            enemy.SendMessage("DropSoul", SendMessageOptions.DontRequireReceiver);
            Destroy(enemy.gameObject);
        }
    }

    private void PlayMusicIfNotPlaying()
    {
        if (musicPlayer != null && !musicPlayer.isPlaying)
        {
            musicPlayer.Play();
        }
    }
}
