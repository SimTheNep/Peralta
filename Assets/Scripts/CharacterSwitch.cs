using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{

    public GameObject gabriel;
    public GameObject peralta;

    public GameObject gabrielSkillUI; 
    public GameObject peraltaSkillUI;

    private GameObject currentCharacter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCharacter = gabriel;
        SetActiveCharacter(gabriel);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
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

            gabrielSkillUI.SetActive(true);
            peraltaSkillUI.SetActive(false);

            Debug.Log("Personagem ativa: Gabriel");
        }
        else
        {
            peralta.GetComponent<PeraltaController>().enabled = true;
            peraltaRb.bodyType = RigidbodyType2D.Dynamic;

            peralta.GetComponent<PeraltaSkills>().enabled = true;

            gabrielSkillUI.SetActive(false);
            peraltaSkillUI.SetActive(true);

            Debug.Log("Personagem ativa: Peralta");
        }

        // Isto é para centrar a camera na personagem ativa
        Camera.main.GetComponent<CameraFollow>().target = characterToActivate.transform;

        currentCharacter = characterToActivate;
    }
}
