using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
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

    public AudioMixer audioMixer;

    private const string MusicVolumeParam = "MusicVol";  
    private const string SFXVolumeParam = "SFXVol";      

    void Start()
    {
        ShowInicial();
        LoadSettings();
        TestMixerSet();

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
        SceneManager.LoadScene("Intermission UI");
    }

    public void RegisterSettings()
    {
        string selectedResolution = resolucaoDropdown.options[resolucaoDropdown.value].text;
        float musicVolume = musicSlider.value;
        float sfxVolume = sfxSlider.value;

        PlayerPrefs.SetString("Resolution", selectedResolution);
        PlayerPrefs.SetFloat("MusicVol", musicVolume);
        PlayerPrefs.SetFloat("SFXVol", sfxVolume);
        PlayerPrefs.Save();

        Debug.Log($"Saved Settings -> Resolution: {selectedResolution}, Music Volume: {musicVolume}, SFX Volume: {sfxVolume}");
    }

    public void LoadSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVol", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVol", 0.5f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        Debug.Log($"Loaded Settings -> Music Volume: {musicVolume}, SFX Volume: {sfxVolume}");

        SetMixerVolume(MusicVolumeParam, musicVolume);
        SetMixerVolume(SFXVolumeParam, sfxVolume);
    }

    public void ApplyResolution()
    {
        string resText = resolucaoDropdown.options[resolucaoDropdown.value].text;

        if (resText == "16:9")
            Screen.SetResolution(1366, 766, FullScreenMode.Windowed);
        else if (resText == "4:3")
            Screen.SetResolution(1366, 1025, FullScreenMode.Windowed);

        Debug.Log($"Resolution applied: {resText}");
    }

    public void ApplyMusicVolume()
    {
        float volume = musicSlider.value;
        SetMixerVolume(MusicVolumeParam, volume);

        if (audioMixer.GetFloat(MusicVolumeParam, out float mixerValue))
            Debug.Log($"Applied Music Volume: slider={volume}, mixer dB={mixerValue}");
    }

    public void ApplySFXVolume()
    {
        float volume = sfxSlider.value;
        SetMixerVolume(SFXVolumeParam, volume);

        if (audioMixer.GetFloat(SFXVolumeParam, out float mixerValue))
            Debug.Log($"Applied SFX Volume: slider={volume}, mixer dB={mixerValue}");
    }

    private void SetMixerVolume(string parameter, float sliderValue)
    {

        int steps = 8;
        int value = Mathf.Clamp(Mathf.RoundToInt(sliderValue), 0, steps);

        float dB = -80f + (value * 10f);

        dB = Mathf.Clamp(dB, -80f, 0f);

        bool success = audioMixer.SetFloat(parameter, dB);
        Debug.Log($"SetMixerVolume('{parameter}', sliderValue={sliderValue}) => dB={dB}, success={success}");
    }


    private void TestMixerSet()
    {
        bool musicSet = audioMixer.SetFloat(MusicVolumeParam, -10f);
        bool sfxSet = audioMixer.SetFloat(SFXVolumeParam, -10f);
        Debug.Log($"TestMixerSet called: Music set? {musicSet}, SFX set? {sfxSet}");
    }
}
