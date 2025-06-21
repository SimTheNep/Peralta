using UnityEngine;
using System.Collections;

public class PushSkill : MonoBehaviour
{
    public Animator animator;
    public Transform playerTransform;
    public float tileSize = 1f;
    public float moveDuration = 0.2f;

    public void Execute()
    {
        animator.SetTrigger("Push");
        StartCoroutine(HandlePushAfterDelay(0.4f));
    }

    private IEnumerator HandlePushAfterDelay(float delay)
    {
        Debug.Log("Waiting for delay...");
        yield return new WaitForSeconds(delay);

        Debug.Log("Casting rays...");
        RaycastHit2D hitRight = Physics2D.Raycast(playerTransform.position, Vector2.right, 0.6f, LayerMask.GetMask("Box"));
        RaycastHit2D hitLeft = Physics2D.Raycast(playerTransform.position, Vector2.left, 0.6f, LayerMask.GetMask("Box"));

        Debug.Log($"HitRight: {hitRight.collider}, HitLeft: {hitLeft.collider}");

        RaycastHit2D hit = hitRight.collider != null ? hitRight :
                        hitLeft.collider != null ? hitLeft : default;

        if (hit.collider == null)
        {
            Debug.Log("No box detected!");
            yield break;
        }

        Vector2 direction = hit == hitRight ? Vector2.right : Vector2.left;
        Debug.Log($"Box detected to the {(direction == Vector2.right ? "right" : "left")}");

        Box box = hit.collider.GetComponent<Box>();
        if (box == null)
        {
            Debug.LogWarning("Hit collider doesn't have Box component!");
            yield break;
        }

        if (box.boxType == BoxType.Light)
        {
            Debug.Log("Moving box...");
            yield return StartCoroutine(MoveTransformOverTime(box.transform, (Vector3)direction * tileSize));
        }
        else
        {
            Debug.Log("Pushing player back...");
            yield return StartCoroutine(MoveTransformOverTime(playerTransform, -(Vector3)direction * tileSize));
            animator.SetTrigger("Damage");
        }
    }

    private IEnumerator MoveTransformOverTime(Transform target, Vector3 offset)
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
