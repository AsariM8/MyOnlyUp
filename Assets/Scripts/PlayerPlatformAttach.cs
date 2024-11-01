using UnityEngine;

public class PlayerPlatformAttach : MonoBehaviour
{
    private Vector3 originalScale;

    private void Start()
    {
        // プレイヤーの元のスケールを記録
        originalScale = transform.localScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = collision.transform;

            // 子オブジェクトにした後、元のスケールにリセット
            transform.localScale = originalScale;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;

            // 親子関係を解除した後も元のスケールを保持
            transform.localScale = originalScale;
        }
    }
}
