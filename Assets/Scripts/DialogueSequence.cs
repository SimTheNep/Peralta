using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public enum DialogueSpeaker
{
    Gabriel,
    Peralta,
    Cabriola
}


public enum TutorialEventType
{
    None,               
    PlayFallAnimation,
    PlayGetUpAnimation,
    FlipConfusedLook,
    ShowExclamation,    
    ShowInventory,      
    HideInventory,      
    PlayDangerSound,
    LookAround,         
    SpawnCabriola,      
    FocusCameraCabriola,
    FocusCameraGabriel, 
    ShowTooltip,


    // adicionem aqui outros eventos conforme forem precisando
}


[System.Serializable]
public class DialogueLine
{
    public DialogueSpeaker speaker;
    [TextArea(2, 5)]
    public string text;
    public bool isRightSide;
    //public UnityEvent onLineEvent;
    public TutorialEventType tutorialEvent;
}

[CreateAssetMenu(menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueLine> lines;
}
