using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfrimCheck : Notice {

    [Header("Confirm")]
    [SerializeField] private bool m_bConfirm;

    protected override void LoadXml()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("NoticeText");

        m_XmlDoc = new XmlDocument();
        m_XmlDoc.LoadXml(textAsset.text);
    }

    protected override void ChangeLanguage()
    {
        string relativePath= m_bConfirm ? ("NoticeText/Confirm/True") : ("NoticeText/Confirm/False") ;

        XmlNodeList nodes = m_XmlDoc.SelectNodes(relativePath+ "/Description");

        for (int i = 0; i < nodes.Count; ++i)
        {
            m_DescriptionText.text += nodes[i].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

            if (i != nodes.Count - 1)
                m_DescriptionText.text += "\n";
        }

        nodes = m_XmlDoc.SelectNodes(relativePath + "/ButtonText");

        m_ButtonText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }

}
