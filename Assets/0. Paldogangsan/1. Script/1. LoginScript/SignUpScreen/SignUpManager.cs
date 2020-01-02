using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpManager : Change {

    #region Must be assigned
    [Header("Must be assigned")]
    [SerializeField] private Text m_TitleText;
    [SerializeField] private Text m_OverlapText;
    [SerializeField] private Text m_BackText;
    [SerializeField] private Text m_CreateText;
    [SerializeField] private Text m_IDText;
    [SerializeField] private Text m_NameText;
    [SerializeField] private Text m_BirthText;
    [SerializeField] private Text m_CautionText;
    [SerializeField] private Text m_SexText;
    [SerializeField] private Text m_OptText;
    [SerializeField] private Dropdown m_SexOpt;

    [Header("Input Field Text Info")]
    [SerializeField] private InputField[] m_fields;
    [SerializeField] private Text m_Sex_InputText;

    [SerializeField] private Notice[] Notices;
    #endregion

    private bool m_isActivedKeyboard;
    private bool m_isConfirmedOverlap;

    private void OnDisable()
    {
        m_isConfirmedOverlap = false;
    }

    public override void Initialize()
    {
        base.Initialize();

        LoginManager.instance.Changechain(ChangeLanguage);

        for (int i = 0; i < Notices.Length; i++)
        {
            Notices[i].gameObject.SetActive(false);
            Notices[i].Initialize();
        }
    }

    private void FixedUpdate()
    {
        if(LoginManager.instance.UseToShowKeyboard)
        {
            if(!m_isActivedKeyboard)
            {
                if(m_fields[0].isFocused || m_fields[1].isFocused || m_fields[2].isFocused)
                {
                    m_isActivedKeyboard = true;

                #if UNITY_EDITOR
                    Debug.Log("가상키보드 생성");
                #elif UNITY_STANDALONE_WIN
                    VirtualKeyboard.ShowTouchKeyboard();
                #endif
                }
            }
            else
            {
                if(!m_fields[0].isFocused && !m_fields[1].isFocused && !m_fields[2].isFocused)
                {
                    m_isActivedKeyboard = false;

                #if UNITY_EDITOR
                    Debug.Log("가상키보드 소멸");
                #elif UNITY_STANDALONE_WIN
                    VirtualKeyboard.HideTouchKeyboard();
                #endif
                }
            }
        }
    }

    protected override void ChangeLanguage()
    {
        XmlNodeList nodes
#region
            = m_XmlDoc.SelectNodes("SignUpText/Title");
        m_TitleText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("SignUpText/OverLapCheck");
        m_OverlapText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("SignUpText/Back");
        m_BackText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("SignUpText/Create");
        m_CreateText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("SignUpText/ID");
        m_IDText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("SignUpText/Name");
        m_NameText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("SignUpText/Birth");
        m_BirthText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("SignUpText/Sex");
        m_SexText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
        #endregion

        nodes = m_XmlDoc.SelectNodes("SignUpText/SexOption");

        m_SexOpt.options[0].text =
            nodes[0].SelectSingleNode("Selection/" + GameManager.instance.GetLanguage.ToString()).InnerText;

        m_SexOpt.options[1].text =
            nodes[0].SelectSingleNode("Man/" + GameManager.instance.GetLanguage.ToString()).InnerText;

        m_SexOpt.options[2].text =
            nodes[0].SelectSingleNode("Woman/" + GameManager.instance.GetLanguage.ToString()).InnerText;

        m_OptText.text = m_SexOpt.options[m_SexOpt.value].text; 

        nodes = m_XmlDoc.SelectNodes("SignUpText/Caution");
        
        for(int i = 0; i<nodes.Count; i++)
        {
            m_CautionText.text =
                nodes[i].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

            if ( i != nodes.Count-1)
            {
                m_CautionText.text += "\n";
            }
        }
    }

    protected override void EmptyText()
    {
        m_CautionText.text = string.Empty;
    }

    public void IDConfirmButton()
    {
        if (m_fields[0].text == string.Empty)
        {
            Notices[2].gameObject.SetActive(true);
            m_isConfirmedOverlap = false;
            print("Empty Test");
        }
        else
        {
            FileInfo file = new FileInfo(FileIO.PathForDocumentsFile("users/" + m_fields[0].text + ".txt")); //text

            if (file.Exists) {
                Notices[1].gameObject.SetActive(true);
                m_isConfirmedOverlap = false;
            }
            else {
                Notices[0].gameObject.SetActive(true);
                m_isConfirmedOverlap = true;
            }
        }
    }

    public void CreateButton()
    {
        if(m_isConfirmedOverlap)
        {
            if (m_fields[1].text != string.Empty 
                && m_fields[2].text != string.Empty
                && m_Sex_InputText.text != m_SexOpt.options[0].text)
            {
                SaveData data = new SaveData(m_fields[0].text
                                            , m_fields[1].text
                                            , m_fields[2].text
                                            , m_Sex_InputText.text);

                print(JsonUtility.ToJson(data, prettyPrint: true));
                //계정생성
                CreateAccount(ref data);

                Notices[4].gameObject.SetActive(true);

                print("true");
            }
            else
            {
                Notices[2].gameObject.SetActive(true);
                print("false");
            }
        }
        else
        {
            if(m_fields[0].text == string.Empty)
                Notices[2].gameObject.SetActive(true);
            
            else
                Notices[3].gameObject.SetActive(true);
        }
    }

    private void CreateAccount(ref SaveData saveData)
    {
        string data = JsonUtility.ToJson(saveData, prettyPrint: true);

        FileIO.WriteStringToFile(FileMode.Create, data, "users", m_fields[0].text + ".txt");

        print(FileIO.PathForDocumentsFile("users/" + m_fields[0].text + ".txt"));

        m_fields[0].text = string.Empty;
        m_fields[1].text = string.Empty;
        m_fields[2].text = string.Empty;
        m_Sex_InputText.text = m_SexOpt.options[0].text;
    }

}
