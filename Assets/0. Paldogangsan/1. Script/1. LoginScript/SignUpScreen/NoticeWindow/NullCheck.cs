using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullCheck : Notice {

    protected override void LoadXml()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("NoticeText");

        m_XmlDoc = new XmlDocument();
        m_XmlDoc.LoadXml(textAsset.text);
    }

    protected override void ChangeLanguage()
    {
        XmlNodeList nodes = m_XmlDoc.SelectNodes("NoticeText/NullCheck/Description");

        m_DescriptionText.text =
            nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;


        nodes = m_XmlDoc.SelectNodes("NoticeText/ButtonText");

        m_ButtonText.text =
            nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }
}
