using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

// <summary>
// 언어에 맞게 변경을 쉽게 하기 위한 클래스
// </summary>
public class Change : MonoBehaviour
{
    protected XmlDocument m_XmlDoc;
    
    public virtual void Initialize()
    {
        EmptyText();

        LoadXml();

        ChangeLanguage();
    }

    // <summary>
    // 여러개의 Xml을 로드하기 때문에 별도로 재정의해주어야 한다.
    // </summary>
    protected virtual void LoadXml() { }

    // <summary>
    // 로드한 XML을 토대로 언어에 맞게 변경시켜준다.
    // </summary>
    protected virtual void ChangeLanguage() { }

    protected virtual void EmptyText() { }
}