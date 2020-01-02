using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

// <summary>
// 언어에 맞게 변경을 쉽게 하기 위한 클래스
// </summary>
public class Change : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_textAsset;
    
    protected XmlDocument m_XmlDoc;
    
    public virtual void Initialize()
    {
        EmptyText();

        LoadXml();

        ChangeLanguage();
    }

    private void LoadXml()
    {
        m_XmlDoc = new XmlDocument();
        m_XmlDoc.LoadXml(m_textAsset.text);
    }

    // <summary>
    // 로드한 XML을 토대로 언어에 맞게 변경시켜준다.
    // </summary>
    protected virtual void ChangeLanguage() { }

    protected virtual void EmptyText() { }
}