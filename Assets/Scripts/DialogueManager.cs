using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string text;
    public bool isRightSide;
    public UnityEvent onLineEvent; 
}

[CreateAssetMenu(menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueLine> lines;
}

public class DialogueManager : MonoBehaviour
{
   
    public GameObject dialogueUIGroup;
    public Image dialogueBox; // DialogueBox (Image)
    public TextMeshProUGUI nameText; // NameText (TMP)
    public TextMeshProUGUI dialogueText; // DialogueText (TMP)

    
    public Vector2 leftNamePos;
    public Vector2 rightNamePos;

    
    public float typingSpeed = 0.03f;

    private DialogueSequence currentSequence;
    private int currentLineIndex;
    private bool isTyping;
    private bool dialogueActive;


    public GabrielController gabrielController;
    public PeraltaController peraltaController;
    public CharacterSwitch characterSwitch;
    public GabrielInventoryManager gabrielInventory;
    public PeraltaInventoryManager peraltaInventory;
    public GabrielSkills gabrielSkills;
    public PeraltaSkills peraltaSkills;
    public HUDController hudController;
    public InventoryUI gabrielInventoryUI;
    public InventoryUI peraltaInventoryUI;
   







    public CameraFollow cameraFollow;
    public Transform gabrielTransform;
    public Transform peraltaTransform;

    void Update()
    {
        if (dialogueActive && Keyboard.current.bKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentSequence.lines[currentLineIndex].text;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue(DialogueSequence sequence)
    {
        currentSequence = sequence;
        currentLineIndex = 0;
        dialogueActive = true;
        dialogueUIGroup.SetActive(true);
        BlockPlayerControls(true);
        ShowLine();
    }

    void ShowLine()
    {
        var line = currentSequence.lines[currentLineIndex];
        SetDialogueSide(line.isRightSide);
        nameText.text = line.speakerName;
        StartCoroutine(TypeText(line.text));
        line.onLineEvent?.Invoke();


        // Mudar a câmara para o personagem que fala
        if (line.speakerName == "Gabriel")
            cameraFollow.SetTarget(gabrielTransform);
        else if (line.speakerName == "Peralta")
            cameraFollow.SetTarget(peraltaTransform);
        // Adapta para outros personagens se necessário
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex < currentSequence.lines.Count)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueUIGroup.SetActive(false);
        dialogueActive = false;
        BlockPlayerControls(false);
    }

    void BlockPlayerControls(bool block)
    {

        // Movimento
        gabrielController.canMove = !block;
        peraltaController.canMove = !block;

        // Skills
        gabrielSkills.canUseSkills = !block;
        peraltaSkills.canUseSkills = !block;

        // Inventário
        gabrielInventory.canUseInventory = !block;
        peraltaInventory.canUseInventory = !block;

        // Troca de personagem
        characterSwitch.SetSwitchEnabled(!block);

        // UI
        hudController.SetHUDVisible(!block);
        gabrielInventoryUI.gameObject.SetActive(!block);
        peraltaInventoryUI.gameObject.SetActive(!block);

    }

    void SetDialogueSide(bool isRightSide)
    {
        // Espelha apenas a imagem de fundo
        dialogueBox.rectTransform.localScale = new Vector3(isRightSide ? -1 : 1, 1, 1);

        // Muda a posição do nome
        nameText.rectTransform.anchoredPosition = isRightSide ? rightNamePos : leftNamePos;

        // Alinha o texto do nome
        nameText.alignment = isRightSide ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
    }
}