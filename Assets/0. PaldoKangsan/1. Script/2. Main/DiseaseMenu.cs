using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiseaseMenu : Menu {
    
    [Header("Canvas Items")]
    [SerializeField] Text m_Text;

    protected override void Start()
    {
        base.Start();

        SoundManager.instance.PlayClip(m_BGMClip, true, 1.0f);
    }

    protected override void SpriteSetting()
    {
        // Load a Sprites ( Assets/Resources/ko/ill_wu ) 경로 안에 있는 모든 Sprite를 가져온다.
        // Load, LoadAll을 사용하는 이유는 LoadAsset을 통해 가져오는 방법은 에디터에서만 유효함.
        // 빌드 시 동작이 안되기 때문에 Resources폴더 내부에 있는 것을 가져오기 위해서는 Load를 이용해준다.
        m_Sprites = Resources.LoadAll<Sprite>(GameManager.instance.GetLanguage + "/ill");

        m_iNumberOfSprites = m_Sprites.Length;
    }

    protected override void Select(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.instance.diseaseName = "chi";
                break;
            case 1:
                GameManager.instance.diseaseName = "wu";
                break;
            case 2:
                GameManager.instance.diseaseName = "gol";
                break;
            case 3:
                GameManager.instance.diseaseName = "ko";
                break;
            case 4:
                GameManager.instance.diseaseName = "dang";
                break;
        }

        MainManager.instance.MenusActive(false, true);
    }

    // Change::LoadXml() 오버라이딩
    protected override void LoadXml()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("DiseaseMenu");

        m_XmlDoc = new XmlDocument();
        m_XmlDoc.LoadXml(textAsset.text);
    }

    // Change::ChangeLanguage() 오버라이딩
    protected override void ChangeLanguage()
    {
        // Resources폴더 내부에 있는 MainMenu.Xml을 확인해보면 
        // MainMenu --> 루트노드
        //  Description --> 자식노드
        //   ko, en, fr    --> Description 자식노드안에 들어가는 속성들
        // 그렇기 때문에 m_XmlDoc.SelectNodes("MainMenu/Description") 이렇게 호출하면 
        // MainMenu자식에 있는 Description노드들을 가져오겠다 라는 의미이다.
        XmlNodeList nodes = m_XmlDoc.SelectNodes("MainMenu/Description");

        m_Text.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }
}
