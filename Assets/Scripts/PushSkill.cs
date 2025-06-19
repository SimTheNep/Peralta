using UnityEngine;
using UnityEngine.UIElements;

public class PushSkill : MonoBehaviour
{

    public Animator animator;
    public Transform playerTransform;
    public float tileSize = 1f;



    public void Execute()
    {
        //primeiro dá sempre a animação de push, mesmo sendo pesada
        animator.SetTrigger("Push");


        RaycastHit2D hitRight = Physics2D.Raycast(playerTransform.position, Vector2.right, 0.6f, LayerMask.GetMask("Box"));
        RaycastHit2D hitLeft = Physics2D.Raycast(playerTransform.position, Vector2.left, 0.6f, LayerMask.GetMask("Box"));

        RaycastHit2D hit = hitRight.collider != null ? hitRight :
                           hitLeft.collider != null ? hitLeft : default;

        if (hit.collider == null)
        {
            // Nenhuma caixa nos lados
            return;
        }

        Vector2 direction = hit == hitRight ? Vector2.right : Vector2.left;

        Box box = hit.collider.GetComponent<Box>();
        if (box == null) return;

        if (box.boxType == BoxType.Light)
        {
            // Move a caixa 1 tile na direção
            box.transform.position += (Vector3)direction * tileSize;
        }
        else
        {
            // Empurra Gabriel para trás
            playerTransform.position -= (Vector3)direction * tileSize;
            animator.SetTrigger("Damage");
        }
    }
}
