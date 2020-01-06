using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CoinUI : MonoBehaviour {

    [SerializeField, Header("숫자 이미지")]
    private List<Sprite> m_numbers;

    [SerializeField]
    private Image m_countImage;

    public int count { get; private set; }

    public void Increase()
    {
        count++;

        if (count >= 10)
            return;
        else
        {
            if (m_countImage.sprite != m_numbers[count])
                m_countImage.sprite = m_numbers[count];
        }
    }

    private void Awake()
    {
        if (m_countImage == null)
        {
            Debug.LogError("CoinUI 할당을 제대로해주세요");
        }

        count = 0;
        m_countImage.sprite = m_numbers[0];
    }

}
