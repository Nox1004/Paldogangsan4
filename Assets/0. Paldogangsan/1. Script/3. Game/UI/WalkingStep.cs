using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class WalkingStep : MonoBehaviour {

    [SerializeField, Header("백의자리")]
    private Image m_Hundreds;

    [SerializeField, Header("십의자리")]
    private Image m_tens;

    [SerializeField, Header("일의자리")]
    private Image m_units;

    public int count { get; private set; }

    private void Awake()
    {
        count = 0;

        if (m_Hundreds == null || m_tens == null || m_units == null)
        {
            Debug.LogError("WalkingStep할당을 제대로 해주세요");
        }
    }

    public void Increase(List<Sprite> number)
    {
        count++;

        if (count > 999)
            return;
        else
        {
            int tempCount = count;

            int hundreds = tempCount / 100;
            tempCount -= hundreds * 100;

            int tens = tempCount / 10;
            tempCount -= tens * 10;

            // 이미지변경
            m_Hundreds.sprite = number[hundreds];
            m_tens.sprite = number[tens];
            m_units.sprite = number[tempCount];
        }
    }
}
