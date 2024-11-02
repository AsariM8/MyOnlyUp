using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float time = 0;
    public bool start = false;

    void Update()
    {
        Debug.Log("start: " + start);
        if (start)
        {
            time += Time.deltaTime;
        }
        Debug.Log("time: " + time);
    }
    public void GameStart()
    {
        start = true;
    }

    public void GameEnd()
    {
        start = false;
    }


}
