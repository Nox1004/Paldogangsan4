using UnityEngine;
using System.Xml;
using System.Collections;

public class JumpMsg : Change {

    //protected override void LoadXml()
    //{
    //    TextAsset textAsset = Resources.Load<TextAsset>("Jump");
    //    m_XmlDoc = new XmlDocument();
    //    m_XmlDoc.LoadXml(textAsset.text);
    //}

    protected override void ChangeLanguage()
    {
        XmlNodeList nodes = m_XmlDoc.SelectNodes("local/STRING_JUMP");
        GetComponent<TextMesh>().text = 
                    nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }

    public void On()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void Off()
    {
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
