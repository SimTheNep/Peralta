using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum SkillType { Push, Throw, Attack }
public class GabrielSkills : MonoBehaviour
{

    private SkillType currentSkill = SkillType.Push;

    private bool isPerformingSkill = false;
    private bool isSkillStarted = false;

    public Image skillUIImage;
    public Sprite[] skillSprites;


    public PushSkill pushSkill;
    public ThrowSkill throwSkill;
    public AttackSkill attackSkill;

    public GabrielInventoryManager gabrielnventory;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSkillUI();
    }

    // Update is called once per frame
    void Update()
    {

        bool yPressed = Keyboard.current.yKey.isPressed;
        bool bPressed = Keyboard.current.bKey.isPressed;

        isPerformingSkill = yPressed && bPressed;

        if (Keyboard.current.yKey.wasPressedThisFrame && !Keyboard.current.bKey.isPressed)
        {
            CycleSkill();
        }

        if (isPerformingSkill)
        {
            if (!isSkillStarted && gabrielnventory.HasItemForSkill(currentSkill))
            {
                Debug.Log("A executar skill: " + currentSkill);
                isSkillStarted = true;
                UseCurrentSkill();
                gabrielnventory.ConsumeItemForSkill(currentSkill);
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
                pushSkill.Execute();
                break;
            case SkillType.Throw:
                throwSkill.Execute();
                break;
            case SkillType.Attack:
                if (gabrielnventory.HasItemForSkill(SkillType.Attack))
                {
                    attackSkill.Execute();
                }
                break;
        }
    }



    public SkillType GetCurrentSkill()
    {
        return currentSkill;
    }
    
}
