using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
                if (kb.actionName == "Action") PlayerPrefs.SetString("Action", "C");
                else if (kb.actionName == "Skill") PlayerPrefs.SetString("Skill", "V");
                else if (kb.actionName == "Item") PlayerPrefs.SetString("Item", "B");
                else if (kb.actionName == "Pause") PlayerPrefs.SetString("Pause", "Escape");
                else if (kb.actionName == "Swap") PlayerPrefs.SetString("Swap", "Tab");
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
            if (!Input.anyKey)
                waitingForKeyRelease = false;
            return;
        }

        if (waitingForKey != null)
        {
            foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    if (kc == KeyCode.Return) continue;

                    SetKey(waitingForKey, kc.ToString());
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

    public static bool GetKeyDown(string actionName)
    {
        string keyName = PlayerPrefs.GetString(actionName, "");
        if (System.Enum.TryParse(keyName, out KeyCode kc))
            return Input.GetKeyDown(kc);
        return false;
    }
}
