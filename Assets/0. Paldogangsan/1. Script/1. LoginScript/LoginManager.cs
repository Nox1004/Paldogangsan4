using System;
using UnityEngine;

public class LoginManager : Singleton<LoginManager> {

    [SerializeField] private Title m_TitleScreen;
    [SerializeField] private LoginScreen m_LoginScreen;
    [SerializeField] private SignUpManager m_MembershipScreen;
    [SerializeField] private ConfirmScreen m_ConfirmScreen;

    [Header("Title Screen Action?")]
    [SerializeField] bool m_bUseTitle;

    [Header("가상 키보드를 사용하는가? (Window platform)")]
    [SerializeField] private bool m_bUseToShowKeyboard;
    public bool UseToShowKeyboard { get { return m_bUseToShowKeyboard; } }

    //델리게이트
    private Action m_ChangeLanguage;

    //델리게이트를 추가해주는 함수
    public void Changechain(Action action) { m_ChangeLanguage += action; }

    protected override void Awake()
    {
        base.Awake();

        Initialization();
    }

    private void Initialization()
    {
        if (m_TitleScreen == null)
        {
            m_TitleScreen = GetComponentInChildren<Title>();
        }

        if (m_LoginScreen == null)
        {
            m_LoginScreen = GetComponentInChildren<LoginScreen>();

            if (m_LoginScreen == null)
                Debug.LogError("LoginScrene컴포넌트를 갖는 오브젝트가 비활성이거나 없습니다.");
        }

        if(m_MembershipScreen == null)
        {
            m_MembershipScreen = GetComponentInChildren<SignUpManager>();

            if (m_MembershipScreen == null)
                Debug.LogError("SignUpManager컴포넌트를 갖는 오브젝트가 비활성이거나 없습니다.");
        }

        if (m_ConfirmScreen == null)
        {
            m_ConfirmScreen = GetComponentInChildren<ConfirmScreen>();

            if (m_ConfirmScreen == null)
                Debug.LogError("Confirmscreen컴포넌트를 갖는 오브젝트가 비활성이거나 없습니다.");
        }

        m_LoginScreen.Initialize();
        m_MembershipScreen.Initialize();
        m_ConfirmScreen.Initialize();
    }

    private void Start()
    {
        if(m_bUseTitle)
        {
            m_TitleScreen.gameObject.SetActive(true);
            m_LoginScreen.gameObject.SetActive(false);
            m_MembershipScreen.gameObject.SetActive(false);
            m_ConfirmScreen.gameObject.SetActive(false);
        }
        else
        {
            m_TitleScreen.gameObject.SetActive(false);
            m_LoginScreen.gameObject.SetActive(true);
            m_MembershipScreen.gameObject.SetActive(false);
            m_ConfirmScreen.gameObject.SetActive(false);
        }
    }

    // <summary>
    // 버튼 이벤트
    // </summary>
    public void LoginButtonEvent()
    {
        if(m_LoginScreen.IDCheck())
        {
            LoadingSceneManager.LoadScene("Main");
        }
        else
        {
            m_ConfirmScreen.gameObject.SetActive(true);
        }
    }
    
    public void OverlapButtonEvent()
    {
        m_MembershipScreen.IDConfirmButton();
    }

    public void CreateButtonEvent()
    {
        m_MembershipScreen.CreateButton();
    }
    
    // 모든텍스트언어를 변경시켜주는 함수 이벤트
    public void ChangeAllTextEvent()
    {
        m_ChangeLanguage();
    }
}
