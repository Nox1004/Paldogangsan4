using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Gender : Change
{
    [SerializeField]
    Texture2D[] texture;

    string m_sex;

    void Start()
    {
        Initialize();
    }

    protected override void LoadXml()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("SignUpText");

        m_XmlDoc = new XmlDocument();
        m_XmlDoc.LoadXml(textAsset.text);
    }

    // Change를 상속받아 이용하다보니 함수명이 부적합하지만
    // 설계자체는 사용해도 무방해 상속받아 이용 --> 머터리얼 설정하는 부분이라고 생각하면 된다.
    protected override void ChangeLanguage()
    {
        string load = FileIO.ReadStringFromFile("users/" + UserData.id + ".txt");

        var loadData = JsonUtility.FromJson<SaveData>(load);
#if !UNITY_EDITOR
        m_sex = loadData.GetGender();
#endif

        XmlNodeList nodes = m_XmlDoc.SelectNodes("SignUpText/SexOption/Man");

        bool flag = false;
        for (int i = 0; i < GameManager.instance.languages.Count; i++)
        {
            if(nodes[0].SelectSingleNode(GameManager.instance.languages[i]).InnerText
                == m_sex)
            {
                flag = true;
                break;
            }
        }

        if(flag)
            this.GetComponent<Renderer>().material.SetTexture("_MainTex", texture[0]);
        else
            this.GetComponent<Renderer>().material.SetTexture("_MainTex", texture[1]);
    }
}
