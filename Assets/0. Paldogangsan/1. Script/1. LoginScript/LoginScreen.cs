using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : Change {
    
    #region Text
    [SerializeField] protected Text m_TitleText;
    [SerializeField] protected Text m_IDText;
    [SerializeField] protected Text m_LoginText;
    [SerializeField] protected Text m_SignUpText;
    #endregion

    private InputField m_InputField;

    private bool m_bShowKeyboard;

    public override void Initialize()
    {
        base.Initialize();

        LoginManager.instance.Changechain(ChangeLanguage);

        m_InputField = GetComponentInChildren<InputField>();
    }

    private void FixedUpdate()
    {
        if(LoginManager.instance.UseToShowKeyboard)
        {
            if(m_InputField.isFocused)
            {
                if(!m_bShowKeyboard)
                {
                    m_bShowKeyboard = true;

                #if UNITY_EDITOR
                    Debug.Log("가상키보드 생성");
                #elif UNITY_STANDALONE_WIN
                    VirtualKeyboard.ShowTouchKeyboard();
                #endif 
                }
            }
            else
            {
                if (m_bShowKeyboard)
                {
                    m_bShowKeyboard = false;
                #if UNITY_EDITOR
                    Debug.Log("가상키보드 소멸");
                #elif UNITY_STANDALONE_WIN
                    VirtualKeyboard.HideTouchKeyboard();
                #endif
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(LoginManager.instance.UseToShowKeyboard)
            {
                m_bShowKeyboard = false;

            #if UNITY_EDITOR
                Debug.Log("가상키보드 소멸");
            #elif UNITY_STANDALONE_WIN
                VirtualKeyboard.HideOnScreenKeyboard();
            #endif
            }

            //로그인
        }
    }

    protected override void ChangeLanguage()
    {
        XmlNodeList nodes = m_XmlDoc.SelectNodes("LoginText/Title");
        m_TitleText.text
             = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("LoginText/ID");
        m_IDText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("LoginText/Login");
        m_LoginText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("LoginText/SignUp");
        m_SignUpText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }
    

    // 특정경로에 아이디의 텍스트파일이 있는지 확인하는 함수
    public bool IDCheck()
    {
        FileInfo file = new FileInfo(FileIO.PathForDocumentsFile("users/" + m_InputField.text + ".txt"));

        if (file.Exists)
        {
            UserData.id = m_InputField.text;
        }

        return file.Exists;
    }

}
