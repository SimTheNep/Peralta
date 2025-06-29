using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class IntermissionManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI loadingText;
    public Button continueButton;

    public float fadeDuration = 2f;
    public float totalWaitTime = 5f;

    private bool buttonReady = false;
    private bool enterPressed = false;

    void Start()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (loadingText != null)
            loadingText.enabled = false;

        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
            continueButton.onClick.AddListener(StartGame);
        }

        StartCoroutine(FadeAndShowButton());
    }

    void Update()
    {
        if (buttonReady && !enterPressed && Input.GetKeyDown(KeyCode.Return))
        {
            enterPressed = true;
            StartGame(); // Call the scene transition method
        }
    }

    IEnumerator FadeAndShowButton()
    {
        // Fade In
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        float flashTimer = 0f;
        bool visible = true;
        loadingText.enabled = true;

        while (flashTimer < totalWaitTime)
        {
            flashTimer += 0.5f;
            visible = !visible;
            loadingText.enabled = visible;
            yield return new WaitForSeconds(0.5f);
        }

        loadingText.enabled = false;

        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
            buttonReady = true;
        }
    }

    void StartGame()
    {
        // Prevent multiple triggers
        if (!enterPressed)
            enterPressed = true;

        SceneManager.LoadScene("CronologiaTutorial");
    }
}
