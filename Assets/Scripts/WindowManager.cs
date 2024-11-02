
// ウィンドウ表示用スクリプト

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindowManager : MonoBehaviour
{
    [SerializeField] GameObject StartWindow;
    [SerializeField] GameObject ResultWindow;
    [SerializeField] GameObject InteractWindow;
    [SerializeField] TextMeshProUGUI TimeText;
    [SerializeField] TextMeshProUGUI InteractWindowText;
    private float timer;
    private bool isopenwindow = true;

    void Start()
    {
        // 開始直後にスタートウィンドウ表示
        StartWindow.SetActive(true);
        Time.timeScale = 0;
    }

    void FixedUpdate()
    {
        // タイム計測
        timer += Time.fixedDeltaTime;
    }

    public void StartWindowClose()
    {
        // スタートウィンドウを閉じるとタイム計測開始
        StartWindow.SetActive(false);
        Time.timeScale = 1;
        isopenwindow = false;
    }

    public void ResultWindowOpen()
    {
        // ゴール時の表示ウィンドウ
        ResultWindow.SetActive(true);
        Time.timeScale = 0;
        TimeText.text = timer.ToString("f2") + "s";
        isopenwindow = true;
    }

    public void InteractWindowOpen()
    {
        if (isopenwindow == false)
        {
            // インタラクト可能なオブジェクトに近づいたら表示
            InteractWindow.SetActive(true);
            InteractWindowText.text = "F: Goal";
        }
    }

    public void InteractWindowClose()
    {
        if (isopenwindow)
        {
            // インタラクト可能なオブジェクトから離れたら閉じる
            InteractWindow.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
