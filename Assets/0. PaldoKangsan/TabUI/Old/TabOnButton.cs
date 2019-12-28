using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabOnButton : TabButtonEvent {

    [SerializeField]
    private TabModeButton modeButton;

    protected override void Awake()
    {
        base.Awake();
        modeButton = FindObjectOfType<TabModeButton>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (TabModeButton.isOn == false)
        {
            TabModeButton.isOn = true;
            for (int i = 0; i < modeButton.buttons.Count; i++)
            {
                modeButton.buttons[i].gameObject.SetActive(true);
            }
            modeButton.offButton.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            modeButton.gameObject.SetActive(true);
        }
    }
}
