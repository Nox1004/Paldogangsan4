using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabESCButton : TabButtonEvent {

    public override void OnPointerDown(PointerEventData eventData)
    {
        m_InputSystem.PressEscapeKeyDown();
    }
}
