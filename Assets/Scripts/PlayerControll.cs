using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] float moveSpeedIn;//プレイヤーの移動速度を入力
    [SerializeField] float jumpForce = 5.0f; // ジャンプ力

    public GroundCheck3D rightGround, leftGround;
    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody playerRb; //プレイヤーのRigidbody
    private Vector3 cameraForward;
    private Vector3 moveForward;
    private Animator animator;
    private bool onGround = true;
    private bool isJumping = false;  // ジャンプ中か判定
    private bool isFalling = false;
    private bool isSpaceKey = false;
    private bool isRunning = false;
    private float yAcceleration;
    private Vector3 lastVelocity;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.sleepThreshold = 0.1f; // スリープ閾値を調整（デフォルトは0.005f)
        lastVelocity = playerRb.velocity;
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        animator.ResetTrigger("Landing");
        // Debug.Log(playerRb.velocity.y);

        // 走行中に発生する微小なy速度の打ち消し
        if (OnGround() && onGround && isSpaceKey == false) playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
        if (playerRb.velocity.y < 0) Debug.Log("y < 0");
        Debug.Log("onGround: " + onGround);

        //------プレイヤーの移動------
        //カメラに対して前を取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //カメラに対して右を取得
        Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // 移動処理
        if (isSpaceKey == false && onGround)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                isRunning = true;
                SetExclusiveBool(animator, "Run");
            }
            else
            {
                isRunning = false;
                SetExclusiveBool(animator, "Idle");
            }
        }
        if (onGround == false)
        {
            if (playerRb.velocity.y < 0)
            {
                isFalling = true;
                SetExclusiveBool(animator, "Fall");
            }
            // if (playerRb.velocity.y == 0 && isFalling)
            if (yAcceleration == 0 && isFalling)
            {
                Debug.Log("SetTrigger");
                animator.SetTrigger("Landing");
                animator.SetBool("Fall", false);
                onGround = true;
                isFalling = false;
                isJumping = false;
                isSpaceKey = false;
            }
        }
        else
        {
            if (playerRb.velocity.y < 0 && isJumping == false)
            {
                Debug.Log("Just Fall");
                onGround = false;
                isRunning = false;
            }
        }

        if (isSpaceKey == false)
        {
            if (isJumping == false && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("GetSpaceKey");
                SetExclusiveBool(animator, "Jump");
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isSpaceKey = true;
            }
        }

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

        // 現在のy方向の速度を取得
        float currentYVelocity = playerRb.velocity.y;

        // y方向の加速度を計算（加速度 = 速度の変化量 / 時間）
        yAcceleration = (currentYVelocity - lastVelocity.y) / Time.fixedDeltaTime;

        // 前フレームの速度を更新
        lastVelocity = playerRb.velocity;

        // デバッグ用に加速度を表示
        // Debug.Log("Y方向の加速度: " + yAcceleration);
    }

    private bool OnGround()
    {
        if (rightGround.CheckGroundStatus() || leftGround.CheckGroundStatus())
        {
            Debug.Log("OnGround Called: True");
            return true;
        }
        else
        {
            Debug.Log("OnGround Called: False");
            return false;
        }
    }

    void SetExclusiveBool(Animator animator, string targetBoolName)
    {
        // 全てのパラメーターを取得
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            // パラメーターがBool型か確認
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                // ターゲットのBool以外はfalseに設定
                animator.SetBool(parameter.name, parameter.name == targetBoolName);
            }
        }
    }

    private void Jump()
    {
        Debug.Log("Jump Called");
        isRunning = false;
        onGround = false;
        isJumping = true;
    }
}

