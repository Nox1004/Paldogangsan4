using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputSystem : Singleton<InputSystem>
{
    [SerializeField] PlayerController _playerController = null;
    public PlayerController playerController { get { return _playerController; } }

    [SerializeField] private bool m_isPressingRF; //오른발버튼이 눌리는 중인가?
    public bool isPressingRF { get { return m_isPressingRF; } }

    [SerializeField] private bool m_isPressingLF; //왼발버튼이 눌리는 중인가?
    public bool isPressingLF { get { return m_isPressingLF; } }

    [SerializeField] private bool m_isPressingRH; //오른손버튼이 눌리는 중인가?
    public bool isPressingRH { get { return m_isPressingRF; } }

    [SerializeField] private bool m_isPressingLH; //왼발버튼이 눌리는 중인가?
    public bool isPressingLH { get { return m_isPressingLH; } }

    public enum FootState { Stop = 0, Right, Left }
    public enum HandState { Stop = 0, Right, Left, Jump}
    [SerializeField] private FootState m_curFootState;
    [SerializeField] private HandState m_curHandState;
    public FootState curFootState { get { return m_curFootState; } }
    public HandState curHandState { get { return m_curHandState; } }

    private bool _isPossibleEsc;

    /// <summary>
    /// 함수포인터의 개념인 델리게이트 변수를 선언
    /// 여기에 함수를 바인딩해줘서 호출하는 방식을 이용한다.
    /// </summary>
    public Action LeftHandBinding;
    public Action RightHandBinding;
    public Action LeftFootBinding;
    public Action RightFootBinding;

    private void Start()
    {
        _isPossibleEsc = true;
    }

    protected virtual void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PressEscapeKeyDown();
        }
    }

    public void HandSystemBinding(Action LHBind = null, Action RHBind = null)
    {
        LeftHandBinding = LHBind;
        RightHandBinding = RHBind;
    }

    public void FootSystemBinding(Action LFBind = null, Action RFBind = null)
    {
        LeftFootBinding = LFBind;
        RightFootBinding = RFBind;
    }

    /// <summary>
    /// 나가기
    /// </summary>
    public void PressEscapeKeyDown()
    {
        if (_isPossibleEsc)
        {
            _isPossibleEsc = false;
            if (SceneManager.GetActiveScene().name == "Login")
            {
                Application.Quit();
            }
            else if (SceneManager.GetActiveScene().name == "Main")
            {
                LoadingSceneManager.LoadScene("Login");
            }
            else if ((SceneManager.GetActiveScene().name.CompareTo("InGame") == 0
                || SceneManager.GetActiveScene().name.CompareTo("InGame2") == 0
                || SceneManager.GetActiveScene().name.CompareTo("Tutorial") == 0))
            {
                LoadingSceneManager.LoadScene("Main");
            }
        }
    }

    /// <summary>
    /// 손 상태를 관리하는 함수
    /// </summary>
    public void PressRightHandKeyDown()
    {
        m_isPressingRH = true;
        m_curHandState = HandState.Right;

        if (m_isPressingLH && m_isPressingRH)
        {
            m_curHandState = HandState.Jump;
        }
    }

    public void PressLeftHandKeyDown()
    {
        m_isPressingLH = true;
        m_curHandState = HandState.Left;

        if (m_isPressingRH && m_isPressingLH)
        {
            m_curHandState = HandState.Jump;
        }
    }

    public void ResetHand()
    {
        m_isPressingLH = false;
        m_isPressingRH = false;

        m_curHandState = HandState.Stop;
    }

    /// <summary>
    /// 발 상태를 관리하는 함수
    /// </summary>
    public void PressRightFootKeyDown()
    {
        if (m_curFootState == FootState.Stop 
                || m_curFootState == FootState.Left)
        {
            m_isPressingRF = true;
            m_isPressingLF = false;

            m_curFootState = FootState.Right;

        }
    }

    public void PressLeftFootKeyDown()
    {
        if(m_curFootState == FootState.Stop
                || m_curFootState == FootState.Right)
        {
            m_isPressingRF = false;
            m_isPressingLF = true;

            m_curFootState = FootState.Left;

        }
    }

    public void ResetFoot(bool isPressingRF = false, bool isPressingLF = false)
    {
        m_isPressingRF = isPressingRF;
        m_isPressingLF = isPressingLF;

        m_curFootState = FootState.Stop;
    }
}
