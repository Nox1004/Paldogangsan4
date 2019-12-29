using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabAButton : TabButtonEvent {

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(m_InputSystem.LeftFootBinding != null)
            m_InputSystem.LeftFootBinding();
    }
}
