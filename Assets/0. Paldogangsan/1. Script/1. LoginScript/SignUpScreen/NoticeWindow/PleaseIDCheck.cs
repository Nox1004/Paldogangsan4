using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PleaseIDCheck : Notice
{
    protected override void ChangeLanguage()
    {
        XmlNodeList nodes = m_XmlDoc.SelectNodes("NoticeText/IDCheck/Description");

        m_DescriptionText.text = 
            nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;


        nodes = m_XmlDoc.SelectNodes("NoticeText/ButtonText");

        m_ButtonText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }

}
