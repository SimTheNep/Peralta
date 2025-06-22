using UnityEngine;

public enum IndicatorType 
{ 
    Possess, 
    PushLight, 
    PushHeavy, 
    Phase, 
    KeyDoor,
    Chest
}

public class InteractiveIndicator : MonoBehaviour
{
    public IndicatorType indicatorType;
    public SpriteRenderer indicatorSpriteRenderer;

    public Sprite[] possessAnimation;
    public Sprite[] pushLightAnimation;
    public Sprite[] pushHeavyAnimation;
    public Sprite[] phaseAnimation;
    public Sprite[] keyDoorAnimation;
    public Sprite[] chestAnimation;

    public float frameRate = 10f; // fps
    private float timer = 0f;
    private int currentFrame = 0;

    public float activationRadius = 2.5f;
    public bool isHeavyBox = false; 

    private Transform player;
    private CharacterSwitch characterSwitch;

    void Start()
    {
        characterSwitch = FindFirstObjectByType<CharacterSwitch>();
       
        indicatorSpriteRenderer.enabled = false;
    }

    void Update()
    {
        if (characterSwitch == null || characterSwitch.currentCharacter == null)
        {
            indicatorSpriteRenderer.enabled = false;
            return;
        }

        player = characterSwitch.currentCharacter.transform;

        if (player == null)
        {
            Debug.Log("player n encontrado pelo InteractiveIndicator");
            indicatorSpriteRenderer.enabled = false;
            return;
        }



        // Distância entre o objeto e o player ativo
        float dist = Vector2.Distance(transform.position, player.position);

        // Só mostra se estiver dentro do raio
        if (dist > activationRadius)
        {
            indicatorSpriteRenderer.enabled = false;
            currentFrame = 0;
            timer = 0f;
            return;
        }

        // Verifica personagem ativo e skill ativa
        bool showIndicator = false;

        Sprite[] currentAnimation = null;

        // Gabriel
        if (characterSwitch.currentCharacter == characterSwitch.gabriel)
        {
            var gabrielSkills = characterSwitch.gabriel.GetComponent<GabrielSkills>();
            switch (indicatorType)
            {
                case IndicatorType.PushLight:
                    showIndicator = gabrielSkills.GetCurrentSkill() == SkillType.Push;
                    currentAnimation = pushLightAnimation;
                    break;
                case IndicatorType.PushHeavy:
                    showIndicator = gabrielSkills.GetCurrentSkill() == SkillType.Push;
                    currentAnimation = pushHeavyAnimation;
                    break;

            }
        }
        // Peralta
        else if (characterSwitch.currentCharacter == characterSwitch.peralta)
        {
            var peraltaSkills = characterSwitch.peralta.GetComponent<PeraltaSkills>();
            switch (indicatorType)
            {
                case IndicatorType.Possess:
                    showIndicator = peraltaSkills.GetCurrentSkill() == PeraltaSkills.SkillType.Haunt;
                    currentAnimation = possessAnimation;
                    break;
                case IndicatorType.Phase:
                    showIndicator = peraltaSkills.GetCurrentSkill() == PeraltaSkills.SkillType.Phase;
                    currentAnimation = phaseAnimation;
                    break;

            }
        }

        // Porta com chave 
        if (indicatorType == IndicatorType.KeyDoor)
        {
            showIndicator = PlayerHasKey();
            currentAnimation = keyDoorAnimation;
        }

        //barris e baus
        if (indicatorType == IndicatorType.Chest)
        {
            if (characterSwitch.currentCharacter == characterSwitch.gabriel)
            {
                Chest chest = GetComponentInParent<Chest>();
                if (chest != null && !chest.isOpened)
                {
                    showIndicator = true;
                    currentAnimation = chestAnimation;
                }
            }
        }



        indicatorSpriteRenderer.enabled = showIndicator;
        //indicatorSpriteRenderer.transform.position = transform.position + Vector3.up * 1.2f;

        if (showIndicator && currentAnimation != null && currentAnimation.Length > 0)
        {
            timer += Time.deltaTime;
            if (timer >= 1f / frameRate)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % currentAnimation.Length;
            }
            indicatorSpriteRenderer.sprite = currentAnimation[currentFrame];
        }
        else
        {
           
            if (indicatorSpriteRenderer.enabled == false)
            {
                currentFrame = 0;
                timer = 0f;
            }
           
        }
    }

    bool PlayerHasKey()
    {
        if (characterSwitch.currentCharacter == characterSwitch.gabriel)
        {
            var inventory = characterSwitch.gabriel.GetComponent<GabrielInventoryManager>();
            if (inventory == null) return false;

            // Percorre todos os slots do inventário à procura de uma chave
            foreach (var item in inventory.slots)
            {
                if (item != null && item.itemType == ItemType.Key && item.quantity > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }


}
