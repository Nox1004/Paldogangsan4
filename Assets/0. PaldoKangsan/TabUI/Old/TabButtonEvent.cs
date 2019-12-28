using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabButtonEvent : MonoBehaviour , IPointerDownHandler, IPointerUpHandler , IPointerEnterHandler {

    [SerializeField]
    protected InputSystem m_InputSystem;

    protected virtual void Reset()
    {
        m_InputSystem = FindObjectOfType<InputSystem>();

        if (m_InputSystem != null)
            Debug.Log("InputSystem이 할당되어있지않습니다.");
    }

    protected virtual void Awake()
    {
        m_InputSystem = FindObjectOfType<InputSystem>();
    }

    public virtual void OnPointerDown(PointerEventData eventData) { }

    public virtual void OnPointerUp(PointerEventData eventData) { }

    public virtual void OnPointerEnter(PointerEventData eventData) { }

    protected virtual IEnumerator ButtonPressing() { yield return null; }

}
