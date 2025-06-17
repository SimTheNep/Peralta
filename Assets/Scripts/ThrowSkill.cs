using UnityEngine;

public class ThrowSkill : MonoBehaviour
{
    public Projectile_behaviour Pedra_ProjectilPrefab;
    public Transform LauchOffset;
    public Animator animator;
    public GabrielInventoryManager gabrielInventoryManager;
    public GabrielController gabrielController;

    void Start()
    {
        if (gabrielController == null)
        {
            GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
            if (gabriel != null)
            {
                gabrielController = gabriel.GetComponentInChildren<GabrielController>();
            }
        }

        if (gabrielInventoryManager == null)
        {
            GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
            if (gabriel != null)
            {
                gabrielInventoryManager = gabriel.GetComponentInChildren<GabrielInventoryManager>();
            }
        }


        if (animator == null)
        {
            GameObject gabriel = GameObject.FindGameObjectWithTag("Gabriel");
            if (gabriel != null)
            {
                GabrielController controller = gabriel.GetComponent<GabrielController>();
                if (controller != null)
                {
                    animator = controller.animator;
                }
            }
        }
    }
    public void Execute()
    {
        Debug.Log("tentar executar");
        if (animator != null)
        {
            Debug.Log("tentar trigger thing");
            animator.SetTrigger("Throw");
        }

        print("Gabriel atira");

        Projectile_behaviour novoProjetil = Instantiate(Pedra_ProjectilPrefab, LauchOffset.position, LauchOffset.rotation);

        novoProjetil.goingRight = gabrielController.Flip;

        novoProjetil.gabrielInventoryManager = gabrielInventoryManager;




    }
}
