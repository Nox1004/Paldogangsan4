using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BasketUI : MonoBehaviour {
    
    [SerializeField, Header("올바른 선택 시 색상")]
    private Color m_rightColor;

    [SerializeField, Header("잘못된 선택 시 색상")]
    private Color m_wrongColor;

    private int m_basketCount;
    private List<Basket> m_baskets; 
    private int curIdx;
    
    public int getBasketCount { get { return m_basketCount; } }

    public bool EnableToPut()
    {
        return curIdx < m_basketCount;
    }

    public bool Equalitem(Material material)
    {
        bool result = false;

        for (int i = 0; i < curIdx; i++)
        {
            if (m_baskets[i].getImage.material.name == material.name)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    public bool Equalitem(int idx, string name)
    {
        var material = m_baskets[idx].getImage.material;
        if (material == null)
            return false;

        if (material.name == name)
            return true;
        else
            return false;
    }

    public Material GetMaterial(int idx)
    {
        return m_baskets[idx].getImage.material;
    }

    public void RightChoice(Material material)
    {
        if (curIdx >= m_baskets.Count)
            return;
        else
        {
            m_baskets[curIdx].Put(material, m_rightColor);

            curIdx++;
        }
    }

    public void WrongChoice(Material material)
    {
        if (curIdx >= m_baskets.Count)
            return;
        else
        {
            m_baskets[curIdx].Put(material, m_wrongColor);

            curIdx++;
        }
    }

    private void Start()
    {
        if (FoodManager.stage == 0 || FoodManager.stage == 1) {
            m_basketCount = 4;
        }
        else {
            m_basketCount = 5;
        }

        curIdx = 0;
        Basket[] baskets = GetComponentsInChildren<Basket>();

        if (baskets.Length < m_basketCount)
        {
            Debug.LogError("BasketUI 설정에 문제 발생");
        }
        else
        {
            // 바구니 오브젝트를 전부 비활성
            for (int i = 0; i < baskets.Length; i++)
            {
                baskets[i].gameObject.SetActive(false);
            }

            m_baskets = new List<Basket>();

            // 설정한 바구니 갯수만큼 활성화
            for (int i = 0; i < m_basketCount; i++)
            {
                baskets[i].gameObject.SetActive(true);
                m_baskets.Add(baskets[i]);
            }
        }
    }
}
