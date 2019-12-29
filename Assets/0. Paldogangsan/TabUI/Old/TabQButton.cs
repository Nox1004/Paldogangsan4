using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabQButton : TabButtonEvent {
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (m_InputSystem.LeftHandBinding != null)
        {
            m_InputSystem.LeftHandBinding();
            print("Test");
        }
    }
}
