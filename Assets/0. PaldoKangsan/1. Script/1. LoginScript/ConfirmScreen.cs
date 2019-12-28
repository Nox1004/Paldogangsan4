using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmScreen : Change {

    [SerializeField] protected Text m_DescriptionText;
    [SerializeField] protected Text m_ConfirmText;

    public override void Initialize()
    {
        base.Initialize();

        LoginManager.instance.Changechain(ChangeLanguage);
    }

    protected override void LoadXml()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("ConfirmText");

        m_XmlDoc = new XmlDocument();
        m_XmlDoc.LoadXml(textAsset.text);
    }

    protected override void ChangeLanguage()
    {
        XmlNodeList nodes = m_XmlDoc.SelectNodes("ConfirmText/Description");
        //StringBuilder 사용해보기
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i< nodes.Count; i++)
        {
            sb.Append(nodes[i].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText);

            if (i != nodes.Count - 1)
                sb.Append("\n");
        }
        m_DescriptionText.text = sb.ToString();

        nodes = m_XmlDoc.SelectNodes("ConfirmText/Confirm");
        m_ConfirmText.text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }

}
