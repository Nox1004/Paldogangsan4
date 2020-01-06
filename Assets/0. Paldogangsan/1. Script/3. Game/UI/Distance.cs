using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Distance : MonoBehaviour {

    Scrollbar m_scrollbar;

    private void SetScrollbar()
    {
        switch (FoodManager.stage)
        {
            case 2:
                m_scrollbar.value = (Controller.pathPosition-0.49f) * 2;
                break;

            case 3:
                m_scrollbar.value = Controller.pathPosition * 1.1f;
                break;

            default:
                m_scrollbar.value = Controller.pathPosition * 2;
                break;
        }
    }

    private void Awake()
    {
        m_scrollbar = GetComponentInChildren<Scrollbar>();

        m_scrollbar.value = 0;
    }

    private void FixedUpdate()
    {
        SetScrollbar();
    }

   
}
