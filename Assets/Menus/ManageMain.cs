using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuNavigation : MonoBehaviour
{
    public GameObject inicialCanvas;
    public GameObject definitionsCanvas;
    public GameObject statisticsCanvas;

    public GameObject novoJogoButton;
    public GameObject geralButton;
    public GameObject pontuacaoButton;

    public TMP_Dropdown resolucaoDropdown;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        ShowInicial();
        RegisterSettings();

        resolucaoDropdown.onValueChanged.AddListener(delegate { ApplyResolution(); RegisterSettings(); });
        musicSlider.onValueChanged.AddListener(delegate { ApplyMusicVolume(); RegisterSettings(); });
        sfxSlider.onValueChanged.AddListener(delegate { ApplySFXVolume(); RegisterSettings(); });
    }

    public void ShowInicial()
    {
        inicialCanvas.SetActive(true);
        definitionsCanvas.SetActive(false);
        statisticsCanvas.SetActive(false);
        SetSelectedButton(novoJogoButton);
    }

    public void ShowDefinitions()
    {
        inicialCanvas.SetActive(false);
        definitionsCanvas.SetActive(true);
        statisticsCanvas.SetActive(false);
        SetSelectedButton(geralButton);
    }

    public void ShowStatistics()
    {
        inicialCanvas.SetActive(false);
        definitionsCanvas.SetActive(false);
        statisticsCanvas.SetActive(true);
        SetSelectedButton(pontuacaoButton);
    }

    private void SetSelectedButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void StartNewGame()
    {
        RegisterSettings();
        Time.timeScale = 1f; 
        SceneManager.LoadScene("TestRoom");
    }

    public void RegisterSettings()
    {
        string selectedResolution = resolucaoDropdown.options[resolucaoDropdown.value].text;
        float musicVolume = musicSlider.value;
        float sfxVolume = sfxSlider.value;

        PlayerPrefs.SetString("Resolution", selectedResolution);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();

        Debug.Log($"Res: {selectedResolution}, Music: {musicVolume}, SFX: {sfxVolume}");
    }

    public void ApplyResolution()
    {
        string resText = resolucaoDropdown.options[resolucaoDropdown.value].text;

        if (resText == "4:3")
            Screen.SetResolution(1366, 766, FullScreenMode.Windowed);
        else if (resText == "16:9")
            Screen.SetResolution(1366, 1025, FullScreenMode.Windowed);

        Debug.Log($"Resolution applied: {resText}");
    }

    public void ApplyMusicVolume()
    {
        AudioListener.volume = musicSlider.value;
        Debug.Log($"Music volume applied: {musicSlider.value}");
    }

    public void ApplySFXVolume()
    {
        // Placeholder: use separate mixer for SFX in a real setup
        Debug.Log($"SFX volume applied: {sfxSlider.value}");
    }
}
