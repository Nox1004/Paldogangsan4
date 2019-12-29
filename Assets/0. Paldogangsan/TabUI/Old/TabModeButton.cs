using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabModeButton : TabButtonEvent {

    public UnityEngine.UI.Button onButton;
    public UnityEngine.UI.Button offButton;

    [SerializeField]
    public List<UnityEngine.UI.Button> buttons;

    public static bool isOn = true;

    [SerializeField]
    protected float _speed = 5;
    public Vector3 previousVector;

    private void Start()
    {
        onButton.gameObject.SetActive(false);
        offButton.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        previousVector = transform.position;
        onButton.gameObject.SetActive(true);
        offButton.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

}
