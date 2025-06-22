using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class UISoundManager : MonoBehaviour
{
    public AudioClip selectSound;
    public AudioClip pressSound;

    public AudioMixerGroup sfxMixerGroup;

    private AudioSource audioSource;
    private GameObject lastSelected;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        lastSelected = null;
    }

    void Update()
    {
        var currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null && currentSelected != lastSelected)
        {
            PlaySound(selectSound);
            lastSelected = currentSelected;
        }

        if (currentSelected != null &&
            (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            PlaySound(pressSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
