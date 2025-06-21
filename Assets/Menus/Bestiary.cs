using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryManager : MonoBehaviour
{
    public static BestiaryManager Instance { get; private set; }

    private HashSet<string> killedEnemies = new HashSet<string>();
    private const string PlayerPrefsKey = "BestiaryKilledEnemies";

    private bool needsUpdate = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadKilledEnemies();
        needsUpdate = true; // Trigger first-time update
    }

    private void Update()
    {
        if (needsUpdate)
        {
            UpdateBestiaryEntries();
            needsUpdate = false;
        }
    }

    public void RegisterEnemyKill(string enemyID)
    {
        if (!killedEnemies.Contains(enemyID))
        {
            killedEnemies.Add(enemyID);
            SaveKilledEnemies();
            needsUpdate = true;
        }
    }

    public bool IsEnemyKilled(string enemyID)
    {
        return killedEnemies.Contains(enemyID);
    }

    private void SaveKilledEnemies()
    {
        string saveData = string.Join(",", killedEnemies);
        PlayerPrefs.SetString(PlayerPrefsKey, saveData);
        PlayerPrefs.Save();
    }

    private void LoadKilledEnemies()
    {
        killedEnemies.Clear();

        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string saveData = PlayerPrefs.GetString(PlayerPrefsKey);
            if (!string.IsNullOrEmpty(saveData))
            {
                string[] ids = saveData.Split(',');
                foreach (string id in ids)
                {
                    if (!string.IsNullOrWhiteSpace(id))
                        killedEnemies.Add(id);
                }
            }
        }
    }

    public void ShowEnemyPanel(string enemyID)
    {
        Transform panelTransform = transform.Find(enemyID + "Panel");
        if (panelTransform != null)
        {
            panelTransform.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Painel de {enemyID} não encontrado");
        }
    }

    public void UpdateBestiaryEntries()
    {
        foreach (Transform buttonTransform in transform)
        {
            string enemyID = buttonTransform.name;
            bool unlocked = IsEnemyKilled(enemyID);

            // Handle image display
            Transform imageTransform = buttonTransform.Find(enemyID + "Image");
            if (imageTransform != null)
            {
                imageTransform.gameObject.SetActive(unlocked);
            }

            // Handle button logic
            Button button = buttonTransform.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();

                string capturedID = enemyID;

                if (unlocked)
                {
                    // If discovered, show the panel
                    button.onClick.AddListener(() =>
                    {
                        ShowEnemyPanel(capturedID);
                    });
                }
                else
                {
                    // Optional: show feedback or leave empty for a disabled button
                    button.onClick.AddListener(() =>
                    {
                        Debug.Log($"Inimigo {capturedID} ainda não foi descoberto.");
                    });
                }
            }
        }
    }
}
