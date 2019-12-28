using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabEButton : TabButtonEvent {

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (m_InputSystem.RightHandBinding != null)
        {
            m_InputSystem.RightHandBinding();
            StartCoroutine(ButtonPressing());
        }
    }
}
