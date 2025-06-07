using UnityEngine;
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
            /*case SkillType.Phase:
                if (phaseSkill != null)
                    phaseSkill.Execute();
                break;
            case SkillType.Haunt:
                if (hauntSkill != null)
                    hauntSkill.Execute();
                break;*/
        }
    }
}
