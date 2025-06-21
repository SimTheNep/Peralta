using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitch : MonoBehaviour
{

    public GameObject gabriel;
    public GameObject peralta;

    public GameObject gabrielUIGroup;  
    public GameObject peraltaUIGroup;  


    public GameObject currentCharacter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCharacter = gabriel;
        SetActiveCharacter(currentCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        var peraltaSkills = peralta.GetComponent<PeraltaSkills>();
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (peraltaSkills != null && peraltaSkills.isPossessing)
                return;

            if (currentCharacter == gabriel)
            {
                SetActiveCharacter(peralta);
            }
            else
            {
                SetActiveCharacter(gabriel);
            }
        }
    }
    
    // Ativa o movimento da personagem ativa e muda a personagem ativa para Dynamic
    void SetActiveCharacter(GameObject characterToActivate)
    {
        gabriel.GetComponent<GabrielController>().enabled = false;
        peralta.GetComponent<PeraltaController>().enabled = false;

        gabriel.GetComponent<GabrielSkills>().enabled = false;
        peralta.GetComponent<PeraltaSkills>().enabled = false;

        // mete as duas personagens com Rigidbody2D Kinematic (não afeta fisica)
        Rigidbody2D gabrielRb = gabriel.GetComponent<Rigidbody2D>();
        Rigidbody2D peraltaRb = peralta.GetComponent<Rigidbody2D>();
        gabrielRb.bodyType = RigidbodyType2D.Kinematic;
        peraltaRb.bodyType = RigidbodyType2D.Kinematic;


        if (characterToActivate == gabriel)
        {
            gabriel.GetComponent<GabrielController>().enabled = true;
            gabrielRb.bodyType = RigidbodyType2D.Dynamic;

            gabriel.GetComponent<GabrielSkills>().enabled = true;

            gabrielUIGroup.SetActive(true);
            peraltaUIGroup.SetActive(false);

            Debug.Log("Personagem ativa: Gabriel");
        }
        else
        {
            peralta.GetComponent<PeraltaController>().enabled = true;
            peraltaRb.bodyType = RigidbodyType2D.Dynamic;

            peralta.GetComponent<PeraltaSkills>().enabled = true;

            gabrielUIGroup.SetActive(false);
            peraltaUIGroup.SetActive(true);

            Debug.Log("Personagem ativa: Peralta");
        }

        // Isto é para centrar a camera na personagem ativa
        Camera.main.GetComponent<CameraFollow>().target = characterToActivate.transform;

        currentCharacter = characterToActivate;
    }
    //para desativar coisas durante os dialogos
    public void SetSwitchEnabled(bool enabled)
    {
        this.enabled = enabled;
        gabrielUIGroup.SetActive(enabled);
        peraltaUIGroup.SetActive(enabled);
    }

}
