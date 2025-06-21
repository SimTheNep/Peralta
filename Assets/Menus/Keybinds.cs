using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class KeybindManager : MonoBehaviour
{
    [System.Serializable]
    public class KeybindButton
    {
        public string actionName;
        public Button button;
        [HideInInspector] public TMP_Text buttonText;
    }

    public List<KeybindButton> keybindButtons;

    private string waitingForKey = null;
    private bool waitingForKeyRelease = false;

    void Start()
    {
        foreach (var kb in keybindButtons)
        {
            kb.buttonText = kb.button.GetComponentInChildren<TMP_Text>();

            if (!PlayerPrefs.HasKey(kb.actionName))
            {
                // Defaults
                if (kb.actionName == "Action") PlayerPrefs.SetString("Action", Key.C.ToString());
                else if (kb.actionName == "Skill") PlayerPrefs.SetString("Skill", Key.V.ToString());
                else if (kb.actionName == "Item") PlayerPrefs.SetString("Item", Key.B.ToString());
                else if (kb.actionName == "Pause") PlayerPrefs.SetString("Pause", Key.Escape.ToString());
                else if (kb.actionName == "Swap") PlayerPrefs.SetString("Swap", Key.Tab.ToString());
                PlayerPrefs.Save();
            }

            string savedKey = PlayerPrefs.GetString(kb.actionName);
            kb.buttonText.text = savedKey;

            string action = kb.actionName;
            kb.button.onClick.AddListener(() => StartKeyBinding(action));
        }
    }

    void StartKeyBinding(string actionName)
    {
        waitingForKey = actionName;
        waitingForKeyRelease = true;

        var kb = keybindButtons.Find(k => k.actionName == actionName);
        if (kb != null)
            kb.buttonText.text = "...";
    }

    void Update()
    {
        if (waitingForKeyRelease)
        {

            if (!Keyboard.current.anyKey.isPressed)
                waitingForKeyRelease = false;
            return;
        }

        if (waitingForKey != null)
        {
            foreach (Key key in System.Enum.GetValues(typeof(Key)))
            {
                if (key == Key.None || key == Key.Enter) continue;

                if (Keyboard.current[key].wasPressedThisFrame)
                {
                    SetKey(waitingForKey, key.ToString());
                    waitingForKey = null;
                    break;
                }
            }
        }
    }

    void SetKey(string actionName, string keyName)
    {
        PlayerPrefs.SetString(actionName, keyName);
        PlayerPrefs.Save();

        var kb = keybindButtons.Find(k => k.actionName == actionName);
        if (kb != null)
            kb.buttonText.text = keyName;
    }

    public static KeyCode GetKeyCode(string actionName)
    {
        string keyName = PlayerPrefs.GetString(actionName, "");
        if (System.Enum.TryParse(keyName, out KeyCode kc))
            return kc;
        return KeyCode.None;
    }


    public static bool GetKeyDown(string actionName)
    {
        string keyName = PlayerPrefs.GetString(actionName, "");
        if (System.Enum.TryParse<Key>(keyName, out Key key))
        {
            return Keyboard.current[key].wasPressedThisFrame;
        }
        return false;
    }
}
