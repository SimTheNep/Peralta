using UnityEngine;
using System.Collections;

public class Projectile_behaviour : MonoBehaviour
{
    public GabrielController gabrielController;
    public GabrielInventoryManager gabrielInventoryManager;
    public float Speed;
    public bool goingRight = true;
    public bool Flip;
    private SpriteRenderer spriteRenderer;


    public AudioSource audioSource;
    public AudioClip impactSound;
    private bool hasPlayedImpactSound = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (gabrielInventoryManager == null)
        {
            GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
            if (gabriel != null)
            {
                gabrielInventoryManager = gabriel.GetComponentInChildren<GabrielInventoryManager>();
            }
        }

        if (gabrielController == null)
        {
            GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
            if (gabriel != null)
            {
                gabrielController = gabriel.GetComponentInChildren<GabrielController>();
            }
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            Debug.LogWarning("Projétil n tem audio");
    }

    void Update()
    {
        Vector3 direcao;

        if (goingRight)
        {
            direcao = Vector3.right;
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
            direcao = Vector3.left;
        }

        transform.position += direcao * Speed * Time.deltaTime;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasPlayedImpactSound && audioSource != null && impactSound != null)
        {
            audioSource.PlayOneShot(impactSound);
            hasPlayedImpactSound = true;
        }

        gameObject.GetComponentInChildren<Animator>().SetTrigger("kaboom");

        if (collision.collider.CompareTag("Enemy"))
        {
            var enemy = collision.collider.GetComponent<ControlEnemy>();
            if (enemy != null && gabrielInventoryManager != null)
            {
                Item item = gabrielInventoryManager.slots[gabrielInventoryManager.selectedSlot];

                float damage = (item != null) ? item.damage : 1f;

                enemy.Life -= damage;
            }
        }

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
