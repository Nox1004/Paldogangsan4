using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageOption : Singleton<LanguageOption> {

    [SerializeField] protected List<CountryButton> m_Buttons;
    [SerializeField] protected Image m_image;

    bool isActive;

    protected override void Awake()
    {
        base.Awake();

        Initialization();
    }

    private void Start()
    {
        SetImage(GameManager.instance.languageType);

        ButtonsSetActive(false);
    }

    public void MainButtonClick()
    {
        if (!isActive)
            ButtonsSetActive(true);
        else
            ButtonsSetActive(false);
        
        isActive = !isActive;
    }

    // 매개변수를 넘길 수 있음. (0 -> ko, 1 -> en, 2 -> fn)
    public void SubButtonClick(int index)
    {
        GameManager.instance.SetLanguage(index);

        LoginManager.instance.ChangeAllTextEvent();

        SetImage(index);
        ButtonsSetActive(false);

        isActive = !isActive;
    }

    private void Initialization()
    {
        CountryButton[] all = GetComponentsInChildren<CountryButton>();

        foreach (var button in all)
        {
            if (gameObject != button.gameObject)
            {
                m_Buttons.Add(button); //List에 버튼들을 캐싱
            }
        }
    }

    // 서브버튼들을 활성화 혹은 비활성화 해주는 함수
    private void ButtonsSetActive(bool isActive)
    {
        for (int i = 0; i < m_Buttons.Count; ++i)
        {
            m_Buttons[i].gameObject.SetActive(isActive);
        }
    }

    // Main Image 변경시켜주는 함수
    private void SetImage(int index)
    {
        m_image.sprite = m_Buttons[index].GetFlagImage();
    }
}
