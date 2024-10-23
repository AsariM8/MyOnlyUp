using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    private float inputHorizontal;
    private float inputVertical;

    [SerializeField]
    float moveSpeedIn;//プレイヤーの移動速度を入力
    private Rigidbody rigidBody;
    Rigidbody playerRb;//プレイヤーのRigidbody

    Vector3 moveSpeed;//プレイヤーの移動速度

    Vector3 currentPos;//プレイヤーの現在の位置
    Vector3 pastPos;//プレイヤーの過去の位置

    private Vector3 cameraForward;
    private Vector3 moveForward;

    [SerializeField]
    float jumpForce = 5.0f; // ジャンプ力
    [SerializeField]
    Transform groundCheck;  // 接地判定のTransform
    [SerializeField]
    LayerMask groundLayer; // 地面レイヤー指定

    private bool isGrounded; // 接地判定

    private Animator animator;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        pastPos = transform.position;

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

        //moveVelocityを0で初期化する
        moveSpeed = Vector3.zero;


        // //移動入力
        // if (Input.GetKey(KeyCode.W))
        // {
        //     moveSpeed = moveSpeedIn * cameraForward;
        // }

        // if (Input.GetKey(KeyCode.A))
        // {
        //     moveSpeed = -moveSpeedIn * cameraRight;
        // }

        // if (Input.GetKey(KeyCode.S))
        // {
        //     moveSpeed = -moveSpeedIn * cameraForward;
        // }

        // if (Input.GetKey(KeyCode.D))
        // {
        //     moveSpeed = moveSpeedIn * cameraRight;
        // }

        // if (moveSpeed.x != 0 || moveSpeed.z != 0)
        // {
        //     animator.SetBool("Run", true);
        // }
        // else
        // {
        //     animator.SetBool("Run", false);
        // }

        // //Moveメソッドで、力加えてもらう
        // Move();

        //慣性を消す
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            //playerRb.velocity = Vector3.zero;
            // playerRb.angularVelocity = Vector3.zero;
        }

        // プレイヤーのジャンプ
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        // Debug.Log(Input.GetKeyDown(KeyCode.W));
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump");
            Jump();
        }


    }

    /// <summary>
    /// 移動方向に力を加える
    /// </summary>
    private void Move()
    {
        //playerRb.AddForce(moveSpeed, ForceMode.Force);

        Vector3 velocity = playerRb.velocity;
        velocity.x = moveSpeed.x;
        velocity.z = moveSpeed.z;
        playerRb.velocity = velocity;
    }

    // ジャンプ処理
    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        // playerRb.velocity = Vector3.up * jumpForce;
    }
    void FixedUpdate()
    {
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;  // 正面判定
        moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;    // 移動量計算
        playerRb.velocity = moveForward * moveSpeedIn + new Vector3(0, playerRb.velocity.y, 0);       // 移動

        // Vector3.zero = 移動量が0
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
        //     rigidBody.AddForce(transform.forward * speed * inputVertical, ForceMode.Acceleration);
        //     rigidBody.AddForce(transform.right * speed * inputHorizontal, ForceMode.Acceleration);
    }
}

