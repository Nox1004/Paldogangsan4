using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class Keyboard : InputModule
{
    protected override void FootKeyInput()
    {
        if (Input.GetKey(KeyCode.A))
            _inputSystem.LeftFootBinding?.Invoke();

        if (Input.GetKey(KeyCode.D))
            _inputSystem.RightFootBinding?.Invoke();
    }

    protected override void HandKeyInput()
    {
        if (Input.GetKey(KeyCode.E))
            _inputSystem.RightHandBinding?.Invoke();

        if (Input.GetKey(KeyCode.Q))
            _inputSystem.LeftHandBinding?.Invoke();
    }
}
