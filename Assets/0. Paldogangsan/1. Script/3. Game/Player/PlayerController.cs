using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Jump AudioClip")]
    [SerializeField] private AudioClip[] m_JumpClip;
    [SerializeField] private AudioClip m_JumpNarrClip;

    private Animation m_Animation;

    private float m_fTurnBackFootStopTime;
    private float m_fTurnBackHandStopTime;
    [Range(0, 10)] [SerializeField] private float m_fFootWaitTime;
    [Range(0, 2)] [SerializeField] private float m_fHandWaitTime;

    public bool isActivedHand { get; set; }
    public bool isActivedFoot { get; set; }

    private bool _isMovingLeft;
    private bool _isMovingRight;
    private Controller _controller;
    private InputSystem _inputSystem;

    private void Awake()
    {
        _controller = GetComponentInParent<Controller>();
        m_Animation = GetComponent<Animation>();
        m_Animation.wrapMode = WrapMode.Once;

        m_Animation["Right"].wrapMode = WrapMode.Once;
        m_Animation["Left"].wrapMode = WrapMode.Once;
        m_Animation["jump new1"].wrapMode = WrapMode.Once;
        m_Animation["jump new2"].wrapMode = WrapMode.Once;
        m_Animation["jump new3"].wrapMode = WrapMode.Once;
        m_Animation["jump final"].wrapMode = WrapMode.Once;

        m_Animation["Right"].layer = -1;
        m_Animation["Left"].layer = -1;
        m_Animation["idle"].layer = -1;

        m_Animation["Right"].speed = 2f;
        m_Animation["Left"].speed = 2f;

        m_Animation["jump new3"].speed = 0.8f;
        m_Animation["jump final"].speed = 0.8f;
    }

    void Start()
    {
        // Awake에 두면 충돌이 일어날 수 있다.
        _inputSystem = InputSystem.instance;

        m_Animation.CrossFade("idle");

        isActivedFoot = true;
    }

    void FixedUpdate()
    {
        if (GameSceneManager.instance.isGamePlaying)
        {
            if (isActivedHand)
            {
                _inputSystem.HandSystemBinding(PlayerLeftHandEvent, PlayerRightHandEvent);
            }
            else
            {
                _inputSystem.HandSystemBinding();
            }

            CharacterAnimation();
        }
    }

    private void LateUpdate()
    {
        if (GameSceneManager.instance.isGamePlaying)
        {
            m_fTurnBackFootStopTime += Time.deltaTime;
            m_fTurnBackHandStopTime += Time.deltaTime;

            if (m_fTurnBackFootStopTime > m_fFootWaitTime)
            {
                InputSystem.instance.ResetFoot();
                _isMovingLeft = false;
                _isMovingRight = false;
            }

            if (m_fTurnBackHandStopTime > m_fHandWaitTime)
            {
                InputSystem.instance.ResetHand();
            }
        }
    }

    /// <summary>
    /// 왼손이벤트
    /// </summary>
    private void PlayerLeftHandEvent()
    {
        InputSystem.instance.PressLeftHandKeyDown();
        m_fTurnBackHandStopTime = 0.0f;
    }

    /// <summary>
    /// 오른손이벤트
    /// </summary>
    private void PlayerRightHandEvent()
    {
        InputSystem.instance.PressRightHandKeyDown();
        m_fTurnBackHandStopTime = 0.0f;
    }

    /// <summary>
    /// 왼발이벤트
    /// </summary>
    private void PlayerLeftFootEvent()
    {
        if (!_isMovingLeft
                        && !m_Animation.IsPlaying("Left"))
        {
            _inputSystem.PressLeftFootKeyDown();
            m_fTurnBackFootStopTime = 0.0f;
        }
    }

    /// <summary>
    /// 오른발이벤트
    /// </summary>
    private void PlayerRightFootEvent()
    {
        if (!_isMovingRight
                        && !m_Animation.IsPlaying("Right"))
        {
            _inputSystem.PressRightFootKeyDown();
            m_fTurnBackFootStopTime = 0.0f;
        }
    }

    private void MoveStop()
    {
        if (!_isMovingLeft && !_isMovingRight
            && _inputSystem.curFootState == InputSystem.FootState.Stop)
        {
            _inputSystem.FootSystemBinding(PlayerLeftFootEvent, PlayerRightFootEvent);
            m_Animation.CrossFade("idle");
        }
    }

    private void MoveLeft()
    {
        // Move Left
        if (!_isMovingRight
                && _inputSystem.curFootState == InputSystem.FootState.Right)
        {
            _inputSystem.FootSystemBinding(PlayerLeftFootEvent, null);

            _isMovingLeft = false;
            _isMovingRight = true;
            m_Animation.CrossFade("Left");
        }
    }

    private void MoveRight()
    {
        // Move Right
        if (!_isMovingLeft
                && _inputSystem.curFootState == InputSystem.FootState.Left)
        {
            _inputSystem.FootSystemBinding(null, PlayerRightFootEvent);

            _isMovingLeft = true;
            _isMovingRight = false;
            m_Animation.CrossFade("Right");
        }
    }

    private void Jump()
    {
        if (_controller.colliderObstacle || _controller.coinJumpPoint)
        {
            if (_inputSystem.curHandState == InputSystem.HandState.Jump)
            {
                if (!m_Animation.IsPlaying("jump new1") && !m_Animation.IsPlaying("jump new2") &&
                    !m_Animation.IsPlaying("jump new3") && !m_Animation.IsPlaying("jump final"))
                {

                    if (Controller.pathPosition < 0.9f)
                    {
                        int index = Random.Range(0, 3); //0~2 반환

                        m_Animation.CrossFade("jump new" + (index + 1));
                        SoundManager.instance.PlayClip(m_JumpClip[index]);
                        SoundManager.instance.PlayClip(m_JumpNarrClip);
                    }
                    else
                    {
                        m_Animation.CrossFade("jump final");
                        SoundManager.instance.PlayClip(m_JumpClip[2]);
                        SoundManager.instance.PlayClip(m_JumpNarrClip);
                    }
                }
            }
        }

        if (m_Animation.IsPlaying("jump new1") || m_Animation.IsPlaying("jump new2") || m_Animation.IsPlaying("jump new3"))
        {
            if (Controller.pathPosition - _controller.currentPosition < 0.02f)
            {
                Controller.pathPosition += 0.01f * Time.deltaTime;
                _controller.speedR = 0f;
                _controller.speedL = 0f;
                m_fTurnBackFootStopTime = 0;
            }
        }

        else if (m_Animation.IsPlaying("jump final"))
        {
            if (Controller.pathPosition - _controller.currentPosition < 0.02f)
            {
                Controller.pathPosition += 0.0004f;
                _controller.speedR = 0f;
                _controller.speedL = 0f;
                m_fTurnBackFootStopTime = 0;
            }
        }
    }

    private void CharacterAnimation()
    {
        MoveStop();

        MoveRight();

        MoveLeft();

        Jump();
    }
}