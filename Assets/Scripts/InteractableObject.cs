using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] GameObject windowManager;
    public float interactableDistance;   // インタラクト可能距離
    private bool isPlayerNearby = false;    // プレイヤーが近距離にいるか判定
    private WindowManager wm;
    void Start()
    {
        wm = windowManager.GetComponent<WindowManager>();
    }

    void Update()
    {
        if (isPlayerNearby)
        {
            wm.InteractWindowOpen();
        }
        else
        {
            wm.InteractWindowClose();
        }
    }

    //　プレイヤーが近づいた時に呼び出されるメソッド
    public void Interact()
    {
        Debug.Log("Player Nearby");
        wm.ResultWindowOpen();
    }

    // プレイヤーが近くにいるかどうかの判定メソッド
    public bool IsPlayerNearby(Transform playerTransform)
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
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
