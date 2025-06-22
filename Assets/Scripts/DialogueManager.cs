using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
   
    public GameObject dialogueUIGroup;
    public Image dialogueBox; 
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText; 

    
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
    public Transform cabriolaTransform;

    public TutorialEvents tutorialEvents;

    public DialogueSequence initialSequence; //para inciiar


    private void Start()
    {
        if (initialSequence != null)
        {
            StartDialogue(initialSequence);
        }
    }
    void Awake()
{
    // UI
    if (dialogueUIGroup == null)
        dialogueUIGroup = GameObject.Find("DialogueUIGroup");

    if (dialogueBox == null)
        dialogueBox = GameObject.Find("DialogueBox")?.GetComponent<Image>();

    if (nameText == null)
        nameText = GameObject.Find("NameText")?.GetComponent<TextMeshProUGUI>();

    if (dialogueText == null)
        dialogueText = GameObject.Find("DialogueText")?.GetComponent<TextMeshProUGUI>();

     // Controladores
    if (gabrielController == null)
        gabrielController = FindFirstObjectByType<GabrielController>();

    if (peraltaController == null)
        peraltaController = FindFirstObjectByType<PeraltaController>();

    if (characterSwitch == null)
        characterSwitch = FindFirstObjectByType<CharacterSwitch>();

    // Inventarios

    if (gabrielInventory == null)
        gabrielInventory = FindFirstObjectByType<GabrielInventoryManager>();

    if (peraltaInventory == null)
        peraltaInventory = FindFirstObjectByType<PeraltaInventoryManager>();

    if (gabrielInventoryUI == null)
        gabrielInventoryUI = GameObject.Find("GabrielInventoryUI")?.GetComponent<InventoryUI>();

    if (peraltaInventoryUI == null)
        peraltaInventoryUI = GameObject.Find("PeraltaInventoryUI")?.GetComponent<InventoryUI>();

    // Skills
    if (gabrielSkills == null)
        gabrielSkills = FindFirstObjectByType<GabrielSkills>();

    if (peraltaSkills == null)
        peraltaSkills = FindFirstObjectByType<PeraltaSkills>();

    // HUD
    if (hudController == null)
        hudController = FindFirstObjectByType<HUDController>();

    // Camera
    if (cameraFollow == null)
        cameraFollow = FindFirstObjectByType<CameraFollow>();

    // Transforms dos personagens
    if (gabrielTransform == null)
        gabrielTransform = GameObject.Find("Gabriel")?.transform;

    if (peraltaTransform == null)
        peraltaTransform = GameObject.Find("Peralta")?.transform;
}

    void Update()
    {
        KeyCode bKeyCode = KeybindManager.GetKeyCode("Action");
        Key bKey = InputHelpers.KeyCodeToKey(bKeyCode);

        if (dialogueActive && Keyboard.current[bKey].wasPressedThisFrame)
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

        if (string.IsNullOrWhiteSpace(line.text))
        {
            dialogueUIGroup.SetActive(false);
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            HandleTutorialEvent(line.tutorialEvent);

            // Avança automaticamente após o evento (ajusta o tempo conforme o evento)
            float autoAdvanceDelay = GetAutoAdvanceDelay(line.tutorialEvent);
            StartCoroutine(AutoAdvanceLine(autoAdvanceDelay));
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        }
        else
        {
            dialogueUIGroup.SetActive(true);
            SetDialogueSide(line.isRightSide);
            nameText.text = line.speaker.ToString();
            StartCoroutine(TypeText(line.text));
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            HandleTutorialEvent(line.tutorialEvent);
        }

 
=======
=======
>>>>>>> Stashed changes
        }

            
        HandleTutorialEvent(line.tutorialEvent);
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes


        // Mudar a camara para o personagem que fala
        switch (line.speaker)
        {
            case DialogueSpeaker.Gabriel:
                cameraFollow.SetTarget(gabrielTransform);
                break;
            case DialogueSpeaker.Peralta:
                cameraFollow.SetTarget(peraltaTransform);
                break;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            case DialogueSpeaker.Cabriola:
                cameraFollow.SetTarget(cabriolaTransform);
                break;
=======
                // Adiciona aqui se quiseres para a Cabriola
>>>>>>> Stashed changes
=======
                // Adiciona aqui se quiseres para a Cabriola
>>>>>>> Stashed changes
        }
        // Adaptar isto para inimgos e npcs 
    }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
    private IEnumerator AutoAdvanceLine(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextLine();
    }

    private float GetAutoAdvanceDelay(TutorialEventType evt)
    {
        switch (evt)
        {
            case TutorialEventType.PlayFallAnimation: return 2.0f;
            case TutorialEventType.PlayGetUpAnimation: return 1.0f;
            case TutorialEventType.FlipConfusedLook: return 0.8f;
            case TutorialEventType.ShowExclamation: return 1.0f;
            case TutorialEventType.ShowInventory: return 1.5f;
            case TutorialEventType.HideInventory: return 0.5f;
            case TutorialEventType.LookAround: return 1.0f;
            case TutorialEventType.SpawnCabriola: return 0.8f;
            case TutorialEventType.FocusCameraCabriola: return 0.5f;
            case TutorialEventType.FocusCameraGabriel: return 0.5f;
            // ... outros eventos que queiras controlar o tempo
            default: return 0.8f;
        }
    }

=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    void HandleTutorialEvent(TutorialEventType evt)
    {
        switch (evt)
        {
            case TutorialEventType.PlayFallAnimation:
                tutorialEvents.PlayFallAnimation();
                break;
            case TutorialEventType.PlayGetUpAnimation:
                tutorialEvents.PlayGetUpAnimation();
                break;
            case TutorialEventType.FlipConfusedLook:
                tutorialEvents.FlipConfusedLook();
                break;
            case TutorialEventType.ShowExclamation:
                tutorialEvents.ShowExclamation();
                break;
            case TutorialEventType.ShowInventory:
                tutorialEvents.ShowGabrielInventory();
                break;
            case TutorialEventType.HideInventory:
                tutorialEvents.HideGabrielInventory();
                break;
            case TutorialEventType.PlayDangerSound:
                tutorialEvents.PlayDangerSound();
                break;
            case TutorialEventType.LookAround:
                tutorialEvents.LookAround();
                break;
            case TutorialEventType.SpawnCabriola:
                tutorialEvents.SpawnCabriola();
                break;
            case TutorialEventType.FocusCameraCabriola:
                tutorialEvents.FocusCameraOnCabriola();
                break;
            case TutorialEventType.FocusCameraGabriel:
                tutorialEvents.FocusCameraOnGabriel();
                break;

            // ...adicionem outros casos conforme precisarem
            case TutorialEventType.None:
            default:
                break;
        }
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

        // Inventario
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

        // Muda a posicao do nome
        nameText.rectTransform.anchoredPosition = isRightSide ? rightNamePos : leftNamePos;

        // Alinha o texto do nome
        nameText.alignment = isRightSide ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
    }
}