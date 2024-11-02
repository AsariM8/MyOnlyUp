using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] float moveSpeedIn; // 移動スピード
    [SerializeField] float jumpForce; // ジャンプ力

    public GroundCheck3D rightGround, leftGround;   // 足オブジェクトによる接地判定
    public KeyCode interactKey = KeyCode.F; // インタラクトキー
    private InteractableObject nearbyObject = null; // インタラクト可能な付近のオブジェクト
    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody playerRb;
    private Vector3 cameraForward;
    private Vector3 moveForward;
    private Animator animator;
    private bool onGround = true;   // ゲーム内オブジェクトによらない接地判定
    private bool isJumping = false;  // ジャンプ判定
    private bool isFalling = false; // 落下判定
    private bool isSpaceKey = false;    // 空中ジャンプ防止用判定
    private bool isRunning = false; // 走行判定
    private float yAcceleration;    // y軸方向の加速度
    private Vector3 lastVelocity;   // 1フレーム前のy速度 加速度計算用

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        lastVelocity = playerRb.velocity;
    }

    void Update()
    {
        animator.ResetTrigger("Landing");

        // 走行中に発生する微小なy速度の打ち消し
        if (OnGround() && onGround && isSpaceKey == false) playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);

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
            // 走行
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                isRunning = true;
                SetExclusiveBool(animator, "Run");
            }
            // 待機
            else
            {
                isRunning = false;
                SetExclusiveBool(animator, "Idle");
            }
        }

        // 落下処理
        if (onGround == false)
        {
            // y速度が負で落下モーションに移行
            if (playerRb.velocity.y < 0)
            {
                isFalling = true;
                SetExclusiveBool(animator, "Fall");
            }

            // y速度0で着地 加速の判定も加えて挙動を安定させる
            if ((yAcceleration == 0 || playerRb.velocity.y == 0) && isFalling)
            {
                animator.SetTrigger("Landing");
                animator.SetBool("Fall", false);
                onGround = true;
                isFalling = false;
                isJumping = false;
                isSpaceKey = false;
            }
        }
        // ジャンプせずに落下した場合の処理
        else
        {
            if (playerRb.velocity.y < 0 && isJumping == false)
            {
                onGround = false;
                isRunning = false;
            }
        }

        // ジャンプ入力処理
        if (isSpaceKey == false)
        {
            if (isJumping == false && Input.GetKeyDown(KeyCode.Space))
            {
                SetExclusiveBool(animator, "Jump");
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isSpaceKey = true;
            }
        }

        // シーン内のすべてのインタラクト可能オブジェクトを検索
        // 右辺ではInteractableObjectコンポーネントを持つすべてのオブジェクトを検索
        // InteractableObject形で格納 GetComponentと同じ役割
        InteractableObject[] interactableObjects = FindObjectsOfType<InteractableObject>();
        Debug.Log(interactableObjects[0]);
        nearbyObject = null;

        foreach (var interactable in interactableObjects)
        {
            if (interactable.IsPlayerNearby(transform))
            {
                nearbyObject = interactable;
                break;
            }
        }

        // インタラクトボタンが押され，近くにオブジェクトがある場合
        if (nearbyObject != null && Input.GetKeyDown(interactKey)) nearbyObject.Interact();

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
    }

    // 接地判定
    private bool OnGround()
    {
        if (rightGround.CheckGroundStatus() || leftGround.CheckGroundStatus()) return true;
        else return false;
    }

    // アニメーション用BoolのTrue / False 設定の効率化
    // 引数に指定したBoolのみTrueにする
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

    // Jump Animation再生中に呼び出す
    private void Jump()
    {
        isRunning = false;
        onGround = false;
        isJumping = true;
    }
}

