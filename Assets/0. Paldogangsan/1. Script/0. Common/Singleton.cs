using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// 확장성을 고려하는 제네릭 싱글톤
// 오직 하나의 인스턴스를 원하는 경우에 이 제네릭싱글톤을 상속받아 구현해준다.
// </summary>
[DisallowMultipleComponent]
public class Singleton<T> : MonoBehaviour where T : Singleton<T> {

    protected static T m_Instance;

    public static T instance {
        get {
            if(m_Instance == null)
            {
                m_Instance = FindObjectOfType<T>();

                if (m_Instance == null)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("Null참조");
#endif
                }
            }
            return m_Instance;
        }

        set { m_Instance = value; }
    }

    // 외부에서 인스턴스가 존재하는지 체크해주는 정적함수
    public static bool instanceExists
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (!instanceExists)
        {
            instance = (T)this;
        }
        else
        {
            if(this != instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
