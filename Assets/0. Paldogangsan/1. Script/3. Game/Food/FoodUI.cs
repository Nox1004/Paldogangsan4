using UnityEngine;
using System.Collections;

public class FoodUI : Singleton<FoodUI>
{
    public int maximum_Amount;

    public Texture[] stage = new Texture[4];
    public Material[] foodArray;
    public GameObject[] foods;
    public GameObject obj;
    public FoodManager foodManager;

    public int curIndex;
    public bool eatFood = false;

    void Start()
    {
        curIndex = 0;
        SetMaximum_Amount();

        for (int i = 0; i < maximum_Amount; i++)
        {
            foods[i] = Instantiate(obj) as GameObject;
            foods[i].SetActive(false);
        }

        SetPosition();
    }

    public void SetPosition()
    {
        for (int i = 0; i < maximum_Amount; i++)
        {
            foods[i].transform.position = new Vector3(0.05f, 0.94f - (i * 0.11f), 3);
        }
    }

    void SetMaximum_Amount()
    {
        if (FoodManager.stage == 0 || FoodManager.stage == 1)
        {
            maximum_Amount = 4;
            gameObject.GetComponent<GUITexture>().texture = stage[0];
        }
        else if (FoodManager.stage == 2 || FoodManager.stage == 3)
        {
            maximum_Amount = 5;
            gameObject.GetComponent<GUITexture>().texture = stage[1];
        }
        else
        {
            maximum_Amount = 8;
        }

        foodArray = new Material[maximum_Amount];
        foods = new GameObject[maximum_Amount];
    }
}
