using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour {

    [Header("Image Panel")]
    [SerializeField] Image m_infoPanel;
    [SerializeField] Image m_FoodPanel;
    [SerializeField] Image m_ButtonPanel;

    [Header("AudioClip")]
    [SerializeField] AudioClip m_NarrationClip;
    [SerializeField] AudioClip m_PressClip;

    [Header("Good Food Prefab")]
    [SerializeField] GameObject m_GoodFoodPrefab;

    Sprite[] m_buttonSprite; 

    private bool m_isWatting; 

    void Start ()
    {
        LoadSprite();
    }

    private void OnEnable()
    {
        InputSystem.instance.HandSystemBinding(InputSystem.instance.PressLeftHandKeyDown);
    }

    private void FixedUpdate()
    {
        InputActive();
    }

    // <summary>
    // FoodManager에서 호출해주고 있으며, StartUI에 좋은음식리스트를 보여주기 위해 프리팹을 생성해준다.
    // </summary>
    public void ShowGoodFoodList(int goodCount,ref Disease curDisease)
    {
        for(int i = 0; i < goodCount; i++)
        {
            GameObject go = Instantiate(m_GoodFoodPrefab, m_FoodPanel.gameObject.transform);
            go.GetComponent<Image>().material = curDisease.goodFood[i];

            if (i < 3) {
                go.transform.localPosition = new Vector3(-270.0f + 270.0f * i , 175.0f);
            }
            else {
                go.transform.localPosition = new Vector3(-270.0f + 270.0f * (i - 3), -175.0f);
            }
        }
    }

    private void InputActive()
    {
        if(!m_isWatting
                && InputSystem.instance.curHandState == InputSystem.HandState.Left)
        {
            SoundManager.instance.PlayClip(m_PressClip, false, 1.0f);

            m_isWatting = true;

            StartCoroutine(SpriteActive());
        }
    }

    private IEnumerator SpriteActive()
    {
        float delayTime = 0.0f;
        bool isChangedSprite1 = false;
        bool isChangedSprite2 = false;

        while (delayTime < 0.6f)
        {
            delayTime += Time.deltaTime;
            
            if(!isChangedSprite1 && delayTime > 0.25f)
            {
                isChangedSprite1 = true;
                if (isChangedSprite1)
                    m_ButtonPanel.sprite = m_buttonSprite[1];
            }

            if(!isChangedSprite2 && delayTime > 0.5f)
            {
                isChangedSprite2 = true;
                if(isChangedSprite2)
                    m_ButtonPanel.sprite = m_buttonSprite[2];
            }
            yield return null;
        }

        if (FoodManager.stage != 3)
        {
            SoundManager.instance.PlayClip(m_NarrationClip, false, 1.0f);
        }

        GameSceneManager.instance.StartGame();
    }

    // 이미지 할당
    private void LoadSprite()
    {
        m_buttonSprite = Resources.LoadAll<Sprite>(GameManager.instance.GetLanguage
                                                    + "/Button");

        m_ButtonPanel.sprite = m_buttonSprite[0];


        int num = (FoodManager.stage != 3) ? FoodManager.stage : 2;

        m_infoPanel.sprite = Resources.Load<Sprite>(GameManager.instance.GetLanguage 
                                                    + "/Info/" 
                                                    + GameManager.instance.diseaseName
                                                    + num + "_info");
        
    }
}
