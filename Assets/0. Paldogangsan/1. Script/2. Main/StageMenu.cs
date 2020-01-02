using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMenu : Menu {

    [Header("Canvas Items")]
    [SerializeField] private Text m_DescriptionText;
    [SerializeField] private Text m_GetBackToTitle;
    [SerializeField] private Text m_GetBackToMain;

    protected void OnDisable()
    {
        //스테이지를 선택할 수 있는 오브젝트는 항상 0을 시작으로 하기 위함
        m_iCurrent = 0;

        m_Image.sprite = m_Sprites[m_iCurrent];
    }

    protected override void SpriteSetting()
    {
        m_Sprites = Resources.LoadAll<Sprite>("StageSelect");

        m_iNumberOfSprites = m_Sprites.Length;
    }

    protected override void Select(int index)
    {
        switch (index)
        {
            case 0:
                FoodManager.stage = 0;
                LoadingSceneManager.LoadScene("Tutorial");
                break;

            case 1:
                FoodManager.stage = 1;
                LoadingSceneManager.LoadScene("InGame");
                break;

            case 2:
                FoodManager.stage = 2;
                LoadingSceneManager.LoadScene("InGame");
                break;

            case 3:
                FoodManager.stage = 3;
                LoadingSceneManager.LoadScene("InGame2");
                break;

            case 4:
                LoadingSceneManager.LoadScene("Login");
                break;

            case 5:
                MainManager.instance.MenusActive(true, false);
                break;
        }
    }

    // Change::ChangeLanguage() 오버라이딩
    protected override void ChangeLanguage()
    {
        m_DescriptionText.text = "";

        XmlNodeList nodes = m_XmlDoc.SelectNodes("StageMenu/Description");

        for(int i = 0; i < nodes.Count; i++)
        {
            m_DescriptionText.text += nodes[i].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

            if ( i != nodes.Count-1)
                m_DescriptionText.text += "\n";
        }

        nodes = m_XmlDoc.SelectNodes("StageMenu/GetBackToTitle");
        m_GetBackToTitle.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("StageMenu/GetBackToMain");
        m_GetBackToMain.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }
}
