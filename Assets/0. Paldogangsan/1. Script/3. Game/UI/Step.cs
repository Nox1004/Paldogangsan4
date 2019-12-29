using UnityEngine;
using System.Collections;

public class Step : MonoBehaviour {
    public static int stepCount = 0;
    int stepHun = 0;
    int stepTen = 0;
    int stepOne = 0;
    int number = 0;

    void Start()
    {
        stepCount = 0;
    }

    void Update()
    {
        stepHun = stepCount / 100;
        stepTen = stepCount / 10 - stepHun * 10;
        stepOne = stepCount - stepHun * 100 - stepTen * 10;

        if (GetComponent<GUIText>().name == "StepCounter01")
            number = stepHun;
        else if (GetComponent<GUIText>().name == "StepCounter02")
            number = stepTen;
        else if (GetComponent<GUIText>().name == "StepCounter03")
            number = stepOne;

        GetComponent<GUIText>().text = number.ToString();
        GetComponent<GUIText>().material.color = Color.black;
    }

}
