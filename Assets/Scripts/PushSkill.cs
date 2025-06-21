using UnityEngine;
using System.Collections;

public class PushSkill : MonoBehaviour
{
    public Animator animator;
    public Transform playerTransform;
    public float tileSize = 1f;
    public float moveDuration = 0.2f;

    public PeraltaInventoryManager inventoryManager;

    public void Execute()
    {
        animator.SetTrigger("Push");
        StartCoroutine(push(0.4f));
    }

    private IEnumerator push(float delay)
    {
        yield return new WaitForSeconds(delay);

        RaycastHit2D hitRight = Physics2D.Raycast(playerTransform.position, Vector2.right, 0.6f, LayerMask.GetMask("Box"));
        RaycastHit2D hitLeft = Physics2D.Raycast(playerTransform.position, Vector2.left, 0.6f, LayerMask.GetMask("Box"));

        RaycastHit2D hit = hitRight.collider != null ? hitRight :
                        hitLeft.collider != null ? hitLeft : default;

        if (hit.collider == null)
        {
            yield break;
        }

        Vector2 direction = hit == hitRight ? Vector2.right : Vector2.left;

        Box box = hit.collider.GetComponent<Box>();
        if (box == null) yield break;

        Vector3 targetPos = box.transform.position + (Vector3)direction * tileSize;


        int tilemapLayer = LayerMask.NameToLayer("Tilemap_Halls");
        int tilemapMask = 1 << tilemapLayer;

        
        Collider2D blockingCollider = Physics2D.OverlapBox(targetPos, box.GetComponent<Collider2D>().bounds.size * 0.9f, 0f, tilemapMask);

        if (blockingCollider != null)
        {

            yield break;
        }

        bool hasSerpenteEncantada = inventoryManager != null && inventoryManager.HasSerpenteEncantada();

        if (box.boxType == BoxType.Light || hasSerpenteEncantada)
        {
            yield return StartCoroutine(animation(box.transform, (Vector3)direction * tileSize));
        }
        else
        {
            if (!hasSerpenteEncantada) 
            {
                yield return StartCoroutine(animation(playerTransform, -(Vector3)direction * tileSize));
                animator.SetTrigger("Damage");
            }
        }
    }

    private IEnumerator animation(Transform target, Vector3 offset)
    {
        Vector3 start = target.position;
        Vector3 end = start + offset;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            target.position = Vector3.Lerp(start, end, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = end;
    }
}
