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
    private bool onGround;
    private bool isJumping = false;  // ジャンプ中か判定
    private bool isFalling = false;
    private bool isSpaceKey = false;
    private bool isRunning = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.ResetTrigger("Landing");
        Debug.Log(onGround);
        if (OnGround() && onGround && isSpaceKey == false) playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
        //------プレイヤーの移動------

        //カメラに対して前を取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //カメラに対して右を取得
        Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // OnGround();
        // 移動処理
        if (isSpaceKey == false && onGround)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                isRunning = true;
                SetExclusiveBool(animator, "Run");
                // animator.Play("Base Layer.RUN00_F");
            }
            else
            {
                isRunning = false;
                SetExclusiveBool(animator, "Idle");
                // animator.Play("Base Layer.WAIT01");
            }
        }
        if (onGround == false)
        {
            // if (isJumping) SetExclusiveBool(animator, "Jump");
            if (playerRb.velocity.y < 0)
            {
                isFalling = true;
                SetExclusiveBool(animator, "Fall");
            }
            // if (Mathf.Abs(playerRb.velocity.y) < Ignore && isFalling)
            if (playerRb.velocity.y == 0 && isFalling)
            {
                Debug.Log("SetTrigger");
                animator.SetTrigger("Landing");
                animator.SetBool("Fall", false);
                onGround = true;
                isFalling = false;
                isJumping = false;
                isSpaceKey = false;
                // playerRb.constraints |= RigidbodyConstraints.FreezePositionY;
                // playerRb.useGravity = false;
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

        // Debug.Log("onGround:"+onGround);
        // Debug.Log("isJumping"+isJumping);
        if (isSpaceKey == false)
        {
            if (isJumping == false && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("GetSpaceKey");
                SetExclusiveBool(animator, "Jump");
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isSpaceKey = true;
                // isJumping = true;
                // onGround = false;
                // animator.Play("Base Layer.JUMP00");
            }
        }

    }

    // 移動処理 回転処理
    void FixedUpdate()
    {
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;  // 正面判定
        moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;    // 移動量計算
        playerRb.velocity = moveForward * moveSpeedIn + new Vector3(0, playerRb.velocity.y, 0);         // 移動
        // Debug.Log("Fixed: "+playerRb.velocity.y);
        // Vector3.zero = 移動量が0
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }

    private bool OnGround()
    {
        // Debug.Log("OnGround Called");
        if (rightGround.CheckGroundStatus() || leftGround.CheckGroundStatus())
        {
            return true;
        }
        else
        {
            return false;
        }
        // Debug.Log("onGround: " + onGround);
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

