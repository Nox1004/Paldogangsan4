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
