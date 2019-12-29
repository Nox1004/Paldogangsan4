using UnityEngine;
using System.Collections;

public class DistancePercent : MonoBehaviour {
    float distance = 0.0f;

    void Update()
    {
        CalculateDistance();
        SetImagePosition();
    }

    void CalculateDistance()
    {
        switch (FoodManager.stage)
        {
            case 1:
                distance = Mathf.Round(Controller.pathPosition * 2000);
                distance /= 10;
                break;

            case 2:
                distance = Mathf.Round((Controller.pathPosition - 0.5f) * 2000);
                distance /= 10;
                break;

            case 3:
                distance = Mathf.Round(Controller.pathPosition * 1000);
                distance /= 10;
                break;
        }
    }

    void SetImagePosition()
    {
        GameObject.Find("DistanceCursor").transform.position = new Vector3(0.41f + distance / 1000 * 1.85f, 0.083f, 3);
    }
}
