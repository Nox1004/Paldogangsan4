using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager> {

    [SerializeField] private DiseaseMenu m_DiseaseMenu;
    [SerializeField] private StageMenu m_StageMenu;

    // C#에서 제공해주는 System.Action 반환값이 void인 델리게이트를 이용해준다.
    public System.Action<bool, bool> MenusActive;

    protected override void Awake()
    {
        base.Awake();
     
        if(m_DiseaseMenu == null)
        {
            m_DiseaseMenu = GetComponentInChildren<DiseaseMenu>();

            if(m_DiseaseMenu == null)
            {
                Debug.LogWarning("DiseaseMenu컴포넌트를 갖는 오브젝트가 비활성이거나 없습니다.");
            }
        }
        
        if(m_StageMenu == null)
        {
            m_StageMenu = GetComponentInChildren<StageMenu>();

            if(m_StageMenu == null)
            {
                Debug.LogWarning("StageMenu컴포넌트를 갖는 오브젝트가 비활성이거나 없습니다.");
            }
        }
    }

    private void Start()
    {
        if (m_DiseaseMenu != null && m_StageMenu != null)
        {
            MenusActive = SetActive;

            MenusActive(true, false);
        }
    }

    private void SetActive(bool isActivedDM, bool isActivedSM)
    {
        m_DiseaseMenu.gameObject.SetActive(isActivedDM);
        m_StageMenu.gameObject.SetActive(isActivedSM);
    }
}
