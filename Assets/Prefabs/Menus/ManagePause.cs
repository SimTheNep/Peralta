using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagePause : MonoBehaviour
{
    public GameObject inicialCanvas;
    public GameObject statisticsCanvas;

    public GameObject novoJogoButton;
    public GameObject pontuacaoButton;

    void Start()
    {
        ShowInicial();
    }

    public void ShowInicial()
    {
        inicialCanvas.SetActive(true);
        statisticsCanvas.SetActive(false);
        SetSelectedButton(novoJogoButton);
    }

    public void ShowStatistics()
    {
        Debug.Log("Estatísticas");
        inicialCanvas.SetActive(false);
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
        inicialCanvas.SetActive(false);
        statisticsCanvas.SetActive(false);

        Time.timeScale = 1f; 
        Debug.Log("Continuando...");
        gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menus Peralta");
    }
}
