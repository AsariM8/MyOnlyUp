
// インタラクト可能なオブジェクトの近くにプレイヤーがいるか判定

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] GameObject windowManager;
    private WindowManager wm;
    public float interactableDistance;   // インタラクト可能距離
    private bool isPlayerNearby = false;    // プレイヤーが近距離にいるか判定

    void Start()
    {
        wm = windowManager.GetComponent<WindowManager>();
    }

    void Update()
    {
        // プレイヤーが近くにいる場合
        if (isPlayerNearby)
        {
            wm.InteractWindowOpen();
        }
        else
        {
            wm.InteractWindowClose();
        }
    }

    //　プレイヤーが近づいた時に呼び出される
    public void Interact()
    {
        wm.ResultWindowOpen();
        wm.InteractWindowClose();
    }

    // プレイヤーが近くにいるかどうかの判定
    public bool IsPlayerNearby(Transform playerTransform)
    {
        // 該当オブジェクトとプレイヤーの距離を算出
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 距離が一定以下なら
        if (distance <= interactableDistance) isPlayerNearby = true;
        else isPlayerNearby = false;

        return isPlayerNearby;
    }

    private void OnDrawGizmosSelected()
    {
        // シーンビューでインタラクト可能距離を可視化
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactableDistance);
    }

}
