using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : Change {

    [SerializeField] protected Text m_DescriptionText;
    [SerializeField] protected Text m_ButtonText;

    protected virtual void Reset()
    {
        Text[] texts = GetComponentsInChildren<Text>();

        foreach(Text text in texts)
        {
            if(text.gameObject.name == "Description")
            {
                m_DescriptionText = text;
                break;
            }
        }

        m_ButtonText = GetComponentInChildren<UnityEngine.UI.Button>().gameObject.GetComponentInChildren<Text>();
    }

    protected override void EmptyText()
    {
        m_DescriptionText.text = "";
        m_ButtonText.text = "";
    }

    public override void Initialize()
    {
        base.Initialize();

        LoginManager.instance.Changechain(ChangeLanguage);
    }
}
