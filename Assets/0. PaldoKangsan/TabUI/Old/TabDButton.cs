using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabDButton : TabButtonEvent {

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(m_InputSystem.RightFootBinding != null)
            m_InputSystem.RightFootBinding();
    }
}
