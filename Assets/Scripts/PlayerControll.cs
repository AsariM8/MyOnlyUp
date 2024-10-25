using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] float moveSpeedIn;//プレイヤーの移動速度を入力
    [SerializeField] float jumpForce = 5.0f; // ジャンプ力

    public GroundCheck3D rightGround, leftGround;
    public bool onGround;

    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody playerRb;//プレイヤーのRigidbody
    private Vector3 moveSpeed;//プレイヤーの移動速度
    private Vector3 cameraForward;
    private Vector3 moveForward;
    private bool isGrounded; // 接地判定
    private bool Jumpping = false;  // ジャンプ中か判定

    private Animator animator;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //------プレイヤーの移動------

        //カメラに対して前を取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //カメラに対して右を取得
        Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // isGrounded = Physics.CheckSphere(groundCheck_L.position, 0.1f, groundLayer) || Physics.CheckSphere(groundCheck_R.position, 0.1f, groundLayer);
        OnGround();
        // Debug.Log("Jumpping:" + Jumpping);
        Debug.Log("onGround:" + onGround);

        // ジャンプ処理
        if (onGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }


        // アニメーション処理
        if (onGround)
        {
            // animator.SetBool("Jump", false);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                // animator.SetBool("Run", true);
                // animator.SetBool("Wait", false);
                animator.Play("Base Layer.RUN00_F");

            }
            else
            {
                // animator.SetBool("Run", false);
                // animator.SetBool("Wait", true);
                animator.Play("Base Layer.WAIT01");
            }
        }
        else
        {
            // animator.SetBool("Jump", true);
            // animator.SetBool("Run", false);
            // animator.SetBool("Wait", false);
            animator.Play("Base Layer.JUMP00");
        }

        //moveVelocityを0で初期化する
        moveSpeed = Vector3.zero;

        // プレイヤーのジャンプ
        //     if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        //     {
        //         playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //     }
        //     else if (isGrounded && Jumpping)
        //     {

        //         animator.SetBool("Jump", false);
        //         Jumpping = false;
        //     }
        // }
    }

    // 移動処理 回転処理
    void FixedUpdate()
    {
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;  // 正面判定
        moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;    // 移動量計算
        playerRb.velocity = moveForward * moveSpeedIn + new Vector3(0, playerRb.velocity.y, 0);         // 移動

        // Vector3.zero = 移動量が0
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }

    private void OnGround()
    {
        if (rightGround.CheckGroundStatus() || leftGround.CheckGroundStatus())
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        // Debug.Log("onGround: " + onGround);
    }
}

