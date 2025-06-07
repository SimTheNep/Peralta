using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PeraltaSkills : MonoBehaviour
{

    private enum SkillType { Hover, Phase, Haunt }
    private SkillType currentSkill = SkillType.Hover;

    private bool isPerformingSkill = false;
    private bool isSkillStarted = false;

    public Image skillUIImage;
    public Sprite[] skillSprites;

    public HoverSkill hoverSkill;
    public PhaseSkill phaseSkill;
    public HauntSkill hauntSkill;

    public bool isPossessing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSkillUI();
    }

    // Update is called once per frame
    void Update()
    {
        //se estiver a possuir só deixa usar haunt e n cicla
        if (isPossessing)
        {
            if (currentSkill != SkillType.Haunt)
            {
                return;  // bloqueia todas as skills exceto Haunt

            }
        }
        else
        {
            // Ciclar skill só se não estiver possuindo
            if (Keyboard.current.yKey.wasPressedThisFrame && !Keyboard.current.bKey.isPressed)
            {
                CycleSkill();
            }
        }


        bool yPressed = Keyboard.current.yKey.isPressed;
        bool bPressed = Keyboard.current.bKey.isPressed;

        isPerformingSkill = yPressed && bPressed;


        if (isPerformingSkill)
        {
            if (!isSkillStarted)
            {
                Debug.Log("Peralta a executar skill: " + currentSkill);
                isSkillStarted = true;
                UseCurrentSkill();
            }
        }
        else
        {
            if (isSkillStarted)
            {
                Debug.Log("Peralta terminou skill: " + currentSkill);
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
        if (skillUIImage != null && skillSprites != null && skillSprites.Length > (int)currentSkill)
        {
            skillUIImage.sprite = skillSprites[(int)currentSkill];
        }
    }

    void UseCurrentSkill()
    {
        switch (currentSkill)
        {
            case SkillType.Hover:
                if (hoverSkill != null)
                    hoverSkill.Execute();
                break;
            case SkillType.Phase:
                if (phaseSkill != null)
                    phaseSkill.Execute();
                break;
            case SkillType.Haunt:
                if (hauntSkill != null)
                    hauntSkill.Execute();
                break;
        }
    }
}
