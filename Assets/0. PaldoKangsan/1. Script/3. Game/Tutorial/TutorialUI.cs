using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : Change {

    private Image _image;
    private Text _text;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        _text = GetComponentInChildren<Text>();

        Initialize();
    }

    public void Disabled()
    {
        _image.enabled = false;
        _text.enabled = false;
    }

    public void TextDescription()
    {
        _text.enabled = true;
    }

    public void ImageDescription(Sprite sprite)
    {
        _image.enabled = true;
        _image.sprite = sprite;
    }

    protected override void LoadXml()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Tutorial");
        m_XmlDoc = new XmlDocument();
        m_XmlDoc.LoadXml(textAsset.text);
    }

    protected override void ChangeLanguage()
    {
        XmlNodeList nodes = m_XmlDoc.SelectNodes("Tutorial/Description");
        _text.text =
                    nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }
}
