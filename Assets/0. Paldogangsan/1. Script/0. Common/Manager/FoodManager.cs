using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// 게임씬에서 음식을 셋팅해주는 클래스
// </summary>
public class FoodManager : Singleton<FoodManager>
{
    //정적 변수
    public static int stage;

    //Editor Test
    [Range(0, 3)] public int TestStage;

    [SerializeField] Material[] m_FoodMaterials;
    [SerializeField] List<Disease> m_DiseaseList;

    [SerializeField] private Disease m_curDisease;

    [SerializeField] private Stores m_Stores;

    public int goodCount { get; set; }
    public int badCount { get; set; }

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        //테스트용
        stage = TestStage;
#endif

        m_Stores = new Stores(stage);
    }

    protected virtual void Start()
    {
        m_curDisease.diseaseName = GameManager.instance.diseaseName;

        SetTexture();
        SetFood();
        m_Stores.FoodArea();
    }

    // <summary>
    // m_FoodMaterials 머터리얼의 이미지를 설정해준다.
    // </summary>
    private void SetTexture()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>(GameManager.instance.GetLanguage
                                                            + "/food");

        if (textures.Length != m_FoodMaterials.Length)
        {
            Debug.LogError("Material배열 혹은 Resource..(en,ko,fr)/food를 확인해주세요 ");
        }
        else
        {
            for (int i = 0; i < textures.Length; i++)
            {
                m_FoodMaterials[i].mainTexture = textures[i];
            }
        }

    }

    private void SetFood()
    {
        for (int i = 0; i < m_DiseaseList.Count; i++)
        {
            if (m_DiseaseList[i].diseaseName == m_curDisease.diseaseName)
            {
                switch (stage)
                {
                    case 1: //Level 1
                        RandomFoodSetting(i, 3, 9);
                        break;

                    case 2: //Level 2
                        RandomFoodSetting(i, 4, 9);
                        break;

                    case 3: //Level 3
                        RandomFoodSetting(i, 4, 9);
                        break;

                    default:
                        RandomFoodSetting(i, 6, 9);
                        break;
                }

                break;
            }
        }
    }

    // <summary>
    // m_DiseaseList안에 있는 좋은음식과 나쁜음식을 추출해 curDisase에 할당한다.
    // </summary>
    private void RandomFoodSetting(int diseaseNum, int goodCount, int badCount)
    {
        for (int i = 0; i < goodCount; i++)
        {
            int value = UnityEngine.Random.Range(0, m_DiseaseList[diseaseNum].goodFood.Count);

            m_curDisease.goodFood.Insert(i, m_DiseaseList[diseaseNum].goodFood[value]);

            m_DiseaseList[diseaseNum].goodFood.RemoveAt(value);
        }

        for (int i = 0; i < badCount; i++)
        {
            int value = UnityEngine.Random.Range(0, m_DiseaseList[diseaseNum].badFood.Count);

            m_curDisease.badFood.Insert(i, m_DiseaseList[diseaseNum].badFood[value]);

            m_DiseaseList[diseaseNum].badFood.RemoveAt(value);
        }

        SetFoodArea(goodCount, badCount);
        GameSceneManager.instance.getStartUI.ShowGoodFoodList(goodCount, ref m_curDisease);
    }


    // <summary>
    // Stores 내부에 FoodArea배열을 갖는 변수를 토대로
    // </summarm>
    private void SetFoodArea(int maxGood, int maxBad)
    {
        int[] storeArray = SetRandomStore(stage);
        int goodCreate = 0, badCreate = 0;
        int addStoreNumber = 0;

        if (stage != 0)
        {
            if (stage == 2)
                addStoreNumber = 11;
            else
                addStoreNumber = 0;
        }

        for (int i = 0; i < storeArray.Length; i++)
        {
            //Good Food 
            if (storeArray[i] == 1)
            {
                m_Stores.storeArea[i + addStoreNumber].foodImage.GetComponent<Renderer>().material = m_curDisease.goodFood[goodCreate];
                goodCreate++;

                if (goodCreate == maxGood)
                    goodCreate = 0;
            }
            //Bad Food
            else
            {
                m_Stores.storeArea[i + addStoreNumber].foodImage.GetComponent<Renderer>().material = m_curDisease.badFood[badCreate];
                badCreate++;

                if (badCreate == maxBad)
                    badCreate = 0;
            }
        }
    }

    /// <summary>
    /// 기존에 있던 것을 그대로 사용
    /// 1 --> 좋은음식, 0 --> 좋지않은음식 을 의미한다.
    /// </summary>
    private int[] SetRandomStore(int curStage)
    {
        //Tutorial모드
        int[] mission0 = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        //Stage 1모드 / Stage 2모드 / Stage 3모드
        int[] mission1 = new int[11];
        int[] mission2 = new int[9];
        int[] mission3 = new int[8];


        int randomValue = UnityEngine.Random.Range(0, 5); // 0~4 랜덤값을 추출한다.

        switch (randomValue)
        {
            case 0:
                mission1 = new int[] { 1, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1 };
                mission2 = new int[] { 1, 1, 0, 1, 0, 1, 1, 0, 1 };
                mission3 = new int[] { 1, 0, 1, 0, 0, 1, 1, 0 };
                break;

            case 1:
                mission1 = new int[] { 0, 1, 0, 1, 1, 0, 0, 1, 0, 1, 1 };
                mission2 = new int[] { 0, 1, 0, 1, 1, 0, 1, 1, 1 };
                mission3 = new int[] { 0, 0, 1, 0, 1, 0, 1, 1 };
                break;

            case 2:
                mission1 = new int[] { 0, 1, 1, 0, 1, 0, 1, 0, 0, 1, 1 };
                mission2 = new int[] { 1, 0, 0, 1, 1, 1, 1, 1, 0 };
                mission3 = new int[] { 0, 0, 0, 1, 0, 1, 1, 1 };
                break;

            case 3:
                mission1 = new int[] { 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1 };
                mission2 = new int[] { 0, 1, 0, 1, 1, 1, 1, 0, 1 };
                mission3 = new int[] { 1, 1, 0, 0, 1, 0, 0, 1 };
                break;

            case 4:
                mission1 = new int[] { 0, 1, 1, 0, 1, 0, 1, 1, 0, 0, 1 };
                mission2 = new int[] { 0, 1, 1, 1, 0, 1, 0, 1, 1 };
                mission3 = new int[] { 1, 0, 1, 1, 0, 1, 0, 0 };
                break;
        }

        if (curStage == 0)
        {
            return mission0;
        }
        else if (curStage == 2)
        {
            return mission2;
        }
        else if (curStage == 3)
        {
            return mission3;
        }
        else
        {
            return mission1;
        }

    }


    public bool CheckFood(Material material)
    {
        //food count check
        for (int i = 0; i < m_curDisease.goodFood.Count; i++)
        {
            if (material.name == m_curDisease.goodFood[i].name)
            {
                return true;
            }
        }
        for (int i = 0; i < m_curDisease.badFood.Count; i++)
        {
            if (material.name == m_curDisease.badFood[i].name)
            {
                return false;
            }
        }
        return false;
    }
}
