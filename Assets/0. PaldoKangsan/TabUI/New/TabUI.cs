using UnityEngine;

/// <summary>
/// 컴포넌트 Button OnClick을 활용하는 방식을 이용 (코드가 간결)
/// 기존 TabUI는 인터페이스를 직접구현하는 것을 활용했었다.
/// </summary>
[DisallowMultipleComponent]
public sealed class TabUI : InputModule
{
    public void Callback(string str)
    {
        switch (str)
        {
            case "LeftHand":
                _inputSystem.LeftHandBinding?.Invoke();
                break;

            case "RightHand":
                _inputSystem.RightHandBinding?.Invoke();
                break;

            case "LeftFoot":
                if(_inputSystem.playerController.isActivedFoot)
                    _inputSystem.LeftFootBinding?.Invoke();
                break;

            case "RightFoot":
                if (_inputSystem.playerController.isActivedFoot)
                    _inputSystem.RightFootBinding?.Invoke();
                break;

            case "Escape":
                _inputSystem.PressEscapeKeyDown();
                break;

            default:
#if UNITY_EDITOR
                Debug.LogWarning("MobileUI 하위 Button 컴포넌트 OnClick을 확인해주세요");
#endif
                break;
        }
    }
}
