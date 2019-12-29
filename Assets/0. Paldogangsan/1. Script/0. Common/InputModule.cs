using UnityEngine;

/// <summary>
/// 입력모듈은 이클래스를 상속받아야 한다.
/// </summary>
public abstract class InputModule : MonoBehaviour {

    protected InputSystem _inputSystem;
	
    protected virtual void Awake()
    {
        _inputSystem = GetComponentInParent<InputSystem>();
    }

    protected virtual void HandKeyInput() { }

    protected virtual void FootKeyInput() { } 

    protected virtual void FixedUpdate()
    {
        HandKeyInput();

        var playerController = _inputSystem.playerController;

        if (playerController != null 
            && playerController.isActivedFoot) {
            FootKeyInput();
        }
    }
}
