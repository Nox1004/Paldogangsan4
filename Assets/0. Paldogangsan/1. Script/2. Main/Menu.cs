using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 메인씬에서 사용하는 질병선택메뉴, 스테이지메뉴는 
/// 이것을 상속받는다.
/// </summary>
public abstract class Menu : Change {

    protected Sprite[] m_Sprites;
    protected int m_iNumberOfSprites;

    [Header("Sprite Image")]
    [SerializeField] protected Image m_Image;

    // AudioClip
    protected AudioClip m_BGMClip;
    private AudioClip m_ChoiceClip;
    private AudioClip m_NextClip;
    
    // 사용하는 변수들
    protected int m_iCurrent;
    protected bool m_bSelected;
    protected float m_fTime;

    protected virtual void Start()
    {
        Initialize();

        LoadClipAssetsFromResourcesFolder();

        SpriteSetting();
    }

    protected virtual void OnEnable()
    {
        m_bSelected = false; m_fTime = 0.0f;
    }

    private void LoadClipAssetsFromResourcesFolder()
    {
        m_BGMClip = Resources.Load<AudioClip>("Audio/MenuBGM");
        m_ChoiceClip = Resources.Load<AudioClip>("Audio/MenuChoice");
        m_NextClip = Resources.Load<AudioClip>("Audio/MenuNext");
    }

    /// 순수가상함수 설정
    protected abstract void SpriteSetting();
    protected abstract void Select(int index);

    protected virtual void FixedUpdate()
    {
        m_fTime += Time.deltaTime;

        if (m_iCurrent == m_iNumberOfSprites)
            m_iCurrent = 0;

        m_Image.sprite = m_Sprites[m_iCurrent];

        if (!m_bSelected && m_fTime > 0.3f)
        {
            InputSystem.instance.HandSystemBinding(InputSystem.instance.PressLeftHandKeyDown,
                                                InputSystem.instance.PressRightHandKeyDown);

            InputActive();

            InputSystem.instance.ResetHand();
        }
    }


    /// <summary>
    /// 입력을 토대로 변경된 손 상태에 따라
    /// 처리를 해주는 함수
    /// </summary>
    private void InputActive()
    {
        if (InputSystem.instance.curHandState == InputSystem.HandState.Right)
        {
            SoundManager.instance.PlayClip(m_NextClip, false, 1.0f);

            m_iCurrent++;
            m_fTime = 0.0f;

            InputSystem.instance.HandSystemBinding();
        }
        else if (InputSystem.instance.curHandState == InputSystem.HandState.Left)
        {
            SoundManager.instance.PlayClip(m_ChoiceClip, false, 1.0f);
            m_bSelected = true;

            Select(m_iCurrent);

            InputSystem.instance.HandSystemBinding();
        }
    }
}
