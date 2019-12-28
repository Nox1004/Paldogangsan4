using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    public float playTime = 0.0f;
    int min = 0;
    int sec = 0;
    string timer;

    void Start()
    {
        GetComponent<GUIText>().material.color = Color.black;
    }

    void Update()
    {
        playTime += Time.deltaTime;
        PrintTime();
    }

    void PrintTime()
    {
        min = (int)(playTime / 60);
        sec = (int)(playTime % 60);

        timer = min + " : " + sec.ToString("D2");
        GetComponent<GUIText>().text = timer;
    }

    public int getMin()
    {
        return min;
    }
    public int getSec()
    {
        return sec;
    }
}
