using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class StartUI : MonoBehaviour {

    [Header("Image Panel")]
    [SerializeField] Image m_infoPanel;
    [SerializeField] GameObject m_FoodPanel;
    List<Image> m_FoodList;
    [SerializeField] Image m_ButtonPanel;
    
    [Header("AudioClip")]
    [SerializeField] AudioClip m_NarrationClip;
    [SerializeField] AudioClip m_PressClip;
    
    Sprite[] m_buttonSprite;
    
    private bool m_isWatting;

    private void Awake()
    {
        m_FoodList = new List<Image>();
        Image[] images = m_FoodPanel.GetComponentsInChildren<Image>();
        
        for(int i = 0; i < images.Length; i++)
        {
            if (images[i].gameObject != m_FoodPanel)
            {
                m_FoodList.Add(images[i]);

                images[i].color = Color.white;
                images[i].gameObject.SetActive(false);
            }
        }
    }

    private void Start ()
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
        for (int i = 0; i < goodCount; i++)
        {
            m_FoodList[i].material = curDisease.goodFood[i];
            m_FoodList[i].gameObject.SetActive(true);
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
