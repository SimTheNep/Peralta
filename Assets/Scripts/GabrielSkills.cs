using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum SkillType { Push, Throw, Attack }

public class GabrielSkills : MonoBehaviour
{
    public GabrielController gabrielController;
    private float attackCooldownTimer = 0f;

    private SkillType currentSkill = SkillType.Push;
    private bool isPerformingSkill = false;
    private bool isSkillStarted = false;

    public Image skillUIImage;
    public Sprite[] skillSprites;

    public PushSkill pushSkill;
    public ThrowSkill throwSkill;
    public AttackSkill attackSkill;

    public GabrielInventoryManager gabrielnventory;
    public Animator animator;

    private bool isSkillExecuting = false;
    public bool canUseSkills = true;

    void Start()
    {
        UpdateSkillUI();
    }

    void Update()
    {
        if (!canUseSkills || isSkillExecuting) return;

        if (attackCooldownTimer > 0f)
            attackCooldownTimer -= Time.deltaTime;

        KeyCode skillKeyCode = KeybindManager.GetKeyCode("Skill");  
        KeyCode actionKeyCode = KeybindManager.GetKeyCode("Action");
        Key actionKey = InputHelpers.KeyCodeToKey(actionKeyCode);
        Key skillKey = InputHelpers.KeyCodeToKey(skillKeyCode);
        

        if (skillKey == Key.None || actionKey == Key.None)
            return;

        bool skillPressed = Keyboard.current[skillKey].isPressed;
        bool actionPressed = Keyboard.current[actionKey].isPressed;

        isPerformingSkill = skillPressed && actionPressed;

        if (Keyboard.current[skillKey].wasPressedThisFrame && !actionPressed)
        {
            CycleSkill();
        }

        if (isPerformingSkill && !isSkillStarted)
        {
            if (!gabrielnventory.HasItemForSkill(currentSkill)) return;

            if (currentSkill == SkillType.Attack && attackCooldownTimer > 0f)
                return;

            isSkillStarted = true;
            isSkillExecuting = true;
            gabrielController.canMove = false;

            Debug.Log("A executar skill: " + currentSkill);

            gabrielnventory.ConsumeItemForSkill(currentSkill);
            StartCoroutine(PerformSkillCoroutine(currentSkill));
        }
        else
        {
            if (isSkillStarted)
            {
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

    System.Collections.IEnumerator PerformSkillCoroutine(SkillType skill)
    {
        switch (skill)
        {
            case SkillType.Push:
                pushSkill.Execute();
                yield return new WaitForSeconds(0.5f);
                break;

            case SkillType.Throw:
                throwSkill.Execute();
                yield return new WaitForSeconds(0.5f);
                break;

            case SkillType.Attack:
                attackSkill.Execute();
                float duration = attackSkill.duration;
                float cooldown = attackSkill.cooldown;

                yield return new WaitForSeconds(duration);

                attackCooldownTimer = cooldown;
                attackSkill.animator.speed = 1f;
                break;
        }

        isSkillStarted = false;
        isSkillExecuting = false;
        gabrielController.canMove = true;
        animator.speed = 1f;
        Debug.Log("Fim de skill");
    }


    public SkillType GetCurrentSkill()
    {
        return currentSkill;
    }
}
