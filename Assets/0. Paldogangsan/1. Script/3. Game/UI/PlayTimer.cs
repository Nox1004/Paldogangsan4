using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PlayTimer : MonoBehaviour {

    [SerializeField, Header("분")]
    private Image m_minTens;

    [SerializeField]
    private Image m_minUnits;

    [SerializeField, Header("초")]
    private Image m_secTens;

    [SerializeField]
    private Image m_secUnits;

    public float time { get; private set; }

    private void Awake()
    {
        time = 0.0f;

        if (m_minTens == null || m_minUnits == null || 
            m_secTens == null || m_secUnits == null)
        {
            Debug.LogError("PlayTimer할당을 제대로 해주세요");
        }
    }

    public void Increase(List<Sprite> number)
    {
        time += Time.deltaTime;

        if (time >= 3600)
            return;
        else
        {
            int min = (int)time / 60;
            int sec = (int)time % 60;

            int mintens = min / 10;
            min -= mintens * 10;
            
            if (m_minTens.sprite != number[mintens])
                m_minTens.sprite = number[mintens];

            if (m_minUnits.sprite != number[min])
                m_minUnits.sprite = number[min];

            int sectens = sec / 10;
            sec -= sectens * 10;
            if (m_secTens.sprite != number[sectens])
                m_secTens.sprite = number[sectens];
            
            if (m_secUnits.sprite != number[sec])
                m_secUnits.sprite = number[sec];
        }
    }
}
