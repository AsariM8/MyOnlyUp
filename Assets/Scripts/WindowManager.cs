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

    void Start()
    {
        Debug.Log("StartWindow: Active");
        StartWindow.SetActive(true);
        Time.timeScale = 0;
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }

    public void StartWindowClose()
    {
        StartWindow.SetActive(false);
        Time.timeScale = 1;
    }

    public void ResultWindowOpen()
    {
        ResultWindow.SetActive(true);
        Time.timeScale = 0;
        TimeText.text = timer.ToString("f2") + "s";
    }

    public void InteractWindowOpen()
    {
        InteractWindow.SetActive(true);
        InteractWindowText.text = "F: Goal";
    }

    public void InteractWindowClose()
    {
        InteractWindow.SetActive(false);
    }
}
