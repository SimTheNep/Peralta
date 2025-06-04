using UnityEngine;
using UnityEngine.UI;

public enum SkillType { Push, Attack, Throw }
public class GabrielSkills : MonoBehaviour
{

    private SkillType currentSkill = SkillType.Push;

    private bool isPerformingSkill = false;
    private bool isSkillStarted = false;

    public Image skillUIImage;
    public Sprite[] skillSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSkillUI();
    }

    // Update is called once per frame
    void Update()
    {

        bool yPressed = Input.GetKey(KeyCode.Y);
        bool bPressed = Input.GetKey(KeyCode.B);

        isPerformingSkill = yPressed && bPressed;

        if (Input.GetKeyDown(KeyCode.Y) && !Input.GetKey(KeyCode.B))
        {
            CycleSkill();
        }

        if (isPerformingSkill)
        {
            if (!isSkillStarted)
            {
                Debug.Log("A executar skill: " + currentSkill);
                isSkillStarted = true;
            }

            switch (currentSkill)
            {
                case SkillType.Push:
                    //TryPushBox();
                    break;
                case SkillType.Throw:
                    //ExecuteThrow();
                    break;
                case SkillType.Attack:
                    //ExecuteAttack();
                    break;
            }
        }
        else
        {
            if (isSkillStarted)
            {
                Debug.Log("Fim da skill: " + currentSkill);
                isSkillStarted = false;
            }
        }
    }

    void CycleSkill()
    {
        currentSkill++;
        if ((int)currentSkill >= System.Enum.GetValues(typeof(SkillType)).Length)
        {
            currentSkill = 0;
        }
        Debug.Log("Mudou para skill: " + currentSkill);
        UpdateSkillUI();
    }

    void UpdateSkillUI()
    {
        if (skillUIImage != null && skillSprites.Length == 3)
        {
            skillUIImage.sprite = skillSprites[(int)currentSkill];
        }
    }

    void UseCurrentSkill()
    {
        switch (currentSkill)
        {
            case SkillType.Push:
                Debug.Log("Gabriel usa: Empurrar");
                break;

            case SkillType.Attack:
                Debug.Log("Gabriel usa: Atacar");
                break;

            case SkillType.Throw:
                Debug.Log("Gabriel usa: Atirar");
                break;
        }
    }
    
    public SkillType GetCurrentSkill()
    {
        return currentSkill;
    }
    
}
