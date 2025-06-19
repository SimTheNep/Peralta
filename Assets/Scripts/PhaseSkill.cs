using UnityEngine;

public class PhaseSkill : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        

    }
    public void Execute()
    {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Grade"))
        {

        }
    }
}
