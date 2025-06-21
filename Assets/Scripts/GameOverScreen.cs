using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    
    public TMP_Text rosasText;
    public TMP_Text checkpointText;
    public TMP_Text timerText;
    public TMP_Text mensagemText;
    public Button usarRosaButton;
    public Button voltarCheckpointButton;
    public GameObject layoutComRosa;
    public GameObject layoutSemRosa;

    
    public float tempoDecisao = 10f;

    private float timer;
    private bool decisaoTomada = false;
    private int rosasDisponiveis = 0;
    private int checkpointLevel = 1;

    public GabrielHealth gabrielHealth;

    public void Setup(int rosas, int checkpoint)
    {
        rosasDisponiveis = rosas;
        checkpointLevel = checkpoint;
        timer = tempoDecisao;
        decisaoTomada = false;

        // Atualiza textos
        rosasText.text = $"Rosas de Aragão: {rosasDisponiveis}";
        checkpointText.text = $"Último checkpoint: Nível {checkpointLevel}";
        mensagemText.text = rosasDisponiveis > 0
            ? "Deseja usar uma Rosa de Aragão para continuar?"
            : "Não há Rosas de Aragão. Voltando ao checkpoint.";

        // Layouts condicionais
        layoutComRosa.SetActive(rosasDisponiveis > 0);
        layoutSemRosa.SetActive(rosasDisponiveis == 0);

        usarRosaButton.interactable = rosasDisponiveis > 0;

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!decisaoTomada)
        {
            timer -= Time.unscaledDeltaTime;
            timerText.text = $"Tempo restante: {Mathf.Ceil(timer)}s";
            if (timer <= 0f)
            {
                decisaoTomada = true;
                VoltarAoCheckpoint();
            }
        }
    }

    public void UsarRosa()
    {
        if (decisaoTomada) return;
        decisaoTomada = true;

        if (gabrielHealth != null)
            gabrielHealth.ContinuarComRosa();

        gameObject.SetActive(false);

        // reativar os scripts de input aqui
    }

    public void VoltarAoCheckpoint()
    {
        if (decisaoTomada) return;
        decisaoTomada = true;
        //  lógica para carregar o checkpoint 
        // Por enquanto, recarrega a cena atual:
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}