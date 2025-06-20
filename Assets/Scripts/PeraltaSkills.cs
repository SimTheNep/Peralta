using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PeraltaSkills : MonoBehaviour
{

    public enum SkillType { Haunt, Phase, Hover }
    public SkillType currentSkill = SkillType.Haunt;

    private bool isPerformingSkill = false;
    private bool isSkillStarted = false;

    public Image skillUIImage;
    public Sprite[] skillSprites;

    public HauntSkill hauntSkill;
    public PhaseSkill phaseSkill;
    public HoverSkill hoverSkill;

    public bool isPossessing = false;

    public bool canUseSkills = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSkillUI();
    }

    // Update is called once per frame
    void Update()
    {

        if (!canUseSkills) return;
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
            // Ciclar skill se se não estiver a possuir
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
                /*if (GetComponent < HoverSkill > Active == true)
                {
                    GetComponent<HoverSkill>() Return = true;
                }*/
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

    public SkillType GetCurrentSkill()
    {
        return currentSkill;
    }

}
