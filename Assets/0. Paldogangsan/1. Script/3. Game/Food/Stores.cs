using UnityEngine;
using System.Collections;

[System.Serializable]
public class Stores {

    const int storeCount = 20;
    [SerializeField] string[] storeNames;
    public FoodArea[] storeArea;

    public Stores(int stageLevel)
    {
        if (stageLevel == 3) {
            storeNames = new string[8];
            storeArea = new FoodArea[8];

        }
        else {
            storeNames = new string[20];
            storeArea = new FoodArea[20];
        }

        SetName(stageLevel);
        SetArea();
    }

    private void SetName(int stageLevel)
    {
        if (stageLevel == 3) 
        {
            storeNames[0] = "Food1";
            storeNames[1] = "Food2";
            storeNames[2] = "Food3";
            storeNames[3] = "Food4";
            storeNames[4] = "Food5";
            storeNames[5] = "Food6";
            storeNames[6] = "Food7";
            storeNames[7] = "Food8";
        }
        else
        {
            storeNames[0] = "Fruit1";
            storeNames[1] = "Fish1";
            storeNames[2] = "Veg2";
            storeNames[3] = "Meal2";
            storeNames[4] = "Meal3";
            storeNames[5] = "Fish3";
            storeNames[6] = "Veg3";
            storeNames[7] = "Meat2";
            storeNames[8] = "Dubu2";
            storeNames[9] = "Fruit2";
            storeNames[10] = "Meat3";
            storeNames[11] = "Dubu3";
            storeNames[12] = "Fruit3";
            storeNames[13] = "Meat4";
            storeNames[14] = "Super3";
            storeNames[15] = "Veg1";
            storeNames[16] = "Super4";
            storeNames[17] = "Fish4";
            storeNames[18] = "Veg4";
            storeNames[19] = "Super2";
        }
    }

    private void SetArea()
    {
        for (int i = 0; i < storeArea.Length; i++)
        {
            Transform parents = GameObject.Find(storeNames[i]).transform;
            GameObject trigger = parents.transform.Find("Trigger Food").gameObject;
            storeArea[i] = trigger.GetComponent<FoodArea>();
        }
    }

    // FoodManager에서 호출하는 함수
    public void FoodArea()
    {
        for(int i = 0; i < storeArea.Length; i++)
        {
            storeArea[i].InitialSetting();
        }
    }
}
