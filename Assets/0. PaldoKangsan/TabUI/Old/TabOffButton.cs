using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabOffButton : TabButtonEvent {

    [SerializeField]
    private TabModeButton modeButton;

    protected override void Awake()
    {
        base.Awake();
        modeButton = FindObjectOfType<TabModeButton>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(TabModeButton.isOn == true)
        {
            TabModeButton.isOn = false;
            for (int i = 0; i < modeButton.buttons.Count; i++)
            {
                modeButton.buttons[i].gameObject.SetActive(false);
            }
            modeButton.onButton.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            modeButton.gameObject.SetActive(true);
        }
    }
}
