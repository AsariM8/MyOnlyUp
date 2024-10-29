using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator; // Animatorコンポーネント
    public float moveSpeed = 5f; // 移動速度
    public float jumpForce = 5f; // ジャンプ力
    private Rigidbody rb; // Rigidbodyコンポーネント
    private bool isGrounded; // 地面に接地しているかどうか

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/Dキーまたは矢印キー
        float moveVertical = Input.GetAxis("Vertical"); // W/Sキーまたは矢印キー

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // ジャンプ中は地面に接地していない
    }

    void HandleAnimation()
    {
        // プレイヤーの速度
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);

        if (rb.velocity.y < 0) // y軸の速度が負
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsLanding", false);
        }
        else if (isGrounded && rb.velocity.y == 0) // 着地している状態
        {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsLanding", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("IsLanding", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 地面に接触した場合、isGroundedをtrueに設定
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
