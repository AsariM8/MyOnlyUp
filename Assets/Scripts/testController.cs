using UnityEngine;

public class testContoroller : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public Transform groundCheck;      // 足オブジェクトをgroundCheckとして使用
    public LayerMask groundLayer;      // 地面のLayer

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // キャラクターの移動
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        // 足の位置で接地判定を行う
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        // ジャンプの入力
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    // ジャンプの処理
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
