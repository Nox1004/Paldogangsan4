using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    public PlayerController playerController { get; private set; }

    public Transform[] controlPath;
    public Transform character;

    public Transform StartLine1;
    public Transform StartLine2;
    public AudioClip[] footClip;
    public AudioSource BGM;
    public AudioClip[] backgroundClip;
    public float speed = 0;
    public static bool tutofoodarea;
    #region Player

    public static float pathPosition = 0.0f;
    private RaycastHit hit;
    public float speedR = 0.01f;
    public float speedL = 0.01f;
    private float rayLength = 500.0f;
    private float ySpeed = 0.0f;
    private float gravity = 10.0f;
    private float lookAheadAmount = 0.01f;
    private Vector3 floorPosition;

    #endregion

    #region Object

    public float currentPosition = 0.0f;
    public bool colliderObstacle = false;
    public int colliderFood = 0;
    public bool coinJumpPoint = false;

    #endregion

    #region Camera

    private float pathPosition_C = 0;
    private float lookAheadAmount_C = 0.01f;
    private string destroyTag = "";
    public int foodCount = 0;

    #endregion

    public float translation = 0.0f;
    public float moveSpeed = 0.4f;

    [SerializeField]
    private JumpMsg jumpMsg;

    private void Awake()
    {
        playerController = GetComponentInChildren<PlayerController>();
    }

    void Start()
    {
        SetPlayerPosition();
        jumpMsg.Off();
    }

    public void ActivedPlayerController(bool isUsingHand, bool isUsingFoot)
    {
        playerController.isActivedHand = isUsingHand;
        playerController.isActivedFoot = isUsingFoot;
    }

    public void SetPlayerPosition()
    {
        if (FoodManager.stage != 2)
            pathPosition = 0.01f;
        else
            pathPosition = 0.49f;
    }

    private void OnDrawGizmosSelected()
    {
        iTween.DrawPath(controlPath, Color.blue);
    }

    private void FixedUpdate()
    {
        Character_Gravity();
        FindFloorAndRotation();
        if (GameSceneManager.instance.isGamePlaying)
        {
            Character_Move();
            SetBGM();
        }
    }

    void SetBGM()
    {
        if (FoodManager.stage != 3)
        {
            if (pathPosition < 0.085f)
            {
                if (BGM.clip != backgroundClip[0])
                {
                    BGM.clip = backgroundClip[0];
                    BGM.Play();
                }
            }
            else if (pathPosition < 0.5f)
            {
                if (BGM.clip != backgroundClip[1])
                {
                    BGM.clip = backgroundClip[1];
                    BGM.Play();
                }
            }
            else if (pathPosition > 0.4f)
            {
                if (BGM.clip != backgroundClip[2])
                {
                    BGM.clip = backgroundClip[2];
                    BGM.Play();
                }
            }
        }
        else
        {
            if (pathPosition > 0.4f)
            {
                if (BGM.clip != backgroundClip[2])
                {
                    BGM.clip = backgroundClip[2];
                    BGM.Play();
                }
            }
        }
    }

    void FindFloorAndRotation()
    {
        float pathPercent = pathPosition % 1;
        Vector3 coordinateOnPath = iTween.PointOnPath(controlPath, pathPercent);
        Vector3 lookTarget;

        if (pathPercent - lookAheadAmount >= 0 && pathPercent + lookAheadAmount <= 1)
        {
            lookTarget = iTween.PointOnPath(controlPath, pathPercent + lookAheadAmount);
            character.transform.LookAt(lookTarget);

            float yRot = character.transform.eulerAngles.y;
            character.transform.eulerAngles = new Vector3(0, yRot, 0);
        }
        if (Physics.Raycast(coordinateOnPath, -Vector3.up, out hit, rayLength))
        {
            floorPosition = hit.point;
        }
    }

    void Character_Gravity()
    {
        ySpeed += gravity * Time.deltaTime;

        character.transform.position = new Vector3(floorPosition.x, character.transform.position.y - ySpeed, floorPosition.z);

        if (character.transform.position.y < floorPosition.y)
        {
            ySpeed = 0;
            character.transform.position = new Vector3(floorPosition.x, floorPosition.y, floorPosition.z);
        }
    }

    void Character_Move()
    {
        if (InputSystem.instance.curFootState == InputSystem.FootState.Right)
        {
            if (speedR > 0)
            {
                pathPosition += Time.deltaTime * speed * speedR;
                speedR -= 0.0008f;
            }
            else if (speedR <= 0)
            {
                AudioSource.PlayClipAtPoint(footClip[0], gameObject.transform.position);
                speedR = 0;
                speedL = 0.01f;
                GameSceneManager.instance.getGameInfoUI.IncreaseStep();
                InputSystem.instance.ResetFoot(true, false);
            }
        }

        if (InputSystem.instance.curFootState == InputSystem.FootState.Left)
        {
            if (speedL > 0)
            {
                pathPosition += Time.deltaTime * speed * speedL;
                speedL -= 0.0008f;
            }
            else if (speedL <= 0)
            {
                AudioSource.PlayClipAtPoint(footClip[0], gameObject.transform.position);
                speedL = 0;
                speedR = 0.01f;
                GameSceneManager.instance.getGameInfoUI.IncreaseStep();
                InputSystem.instance.ResetFoot(false, true);
            }
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        TriggerEnterInAccordanceStage(FoodManager.stage, col);
    }

    private void OnTriggerStay(Collider col)
    {
        TriggerStayInAccordanceStage(FoodManager.stage, col);
    }

    private void OnTriggerExit(Collider col)
    {
        TrigegerExitInAccordanceStage(FoodManager.stage, col);
    }

    private void TriggerEnterInAccordanceStage(int stage, Collider collider)
    {
        if (stage == 1)
        {
            if (collider.name == "StartLine_2")
            {
                InputSystem.instance.HandSystemBinding();
                InputSystem.instance.FootSystemBinding();
                GameSceneManager.instance.EndGame();
            }
        }
        else if (stage == 2)
        {
            if (collider.name == "StartLine_1")
            {
                InputSystem.instance.HandSystemBinding();
                InputSystem.instance.FootSystemBinding();
                GameSceneManager.instance.EndGame();
            }
        }
        else if (stage == 3)
        {
            if (collider.name == "EndLine")
            {
                InputSystem.instance.HandSystemBinding();
                InputSystem.instance.FootSystemBinding();
                GameSceneManager.instance.EndGame();
            }
        }

        if (collider.CompareTag("Obstacle"))
        {
            colliderObstacle = true;
            currentPosition = pathPosition;

            ActivedPlayerController(true, false);

            if (jumpMsg != null)
                jumpMsg.On();

            return;
        }

        if (collider.CompareTag("TutorialArea"))
        {
            currentPosition = pathPosition;

            ActivedPlayerController(true, false);
        }

    }

    private void TriggerStayInAccordanceStage(int stage, Collider collider)
    {
        if (collider.CompareTag("Obstacle"))
        {
            colliderObstacle = true;

            speedR = 0;
            speedL = 0;
        }
    }

    private void TrigegerExitInAccordanceStage(int stage, Collider collider)
    {
        if (collider.CompareTag("Obstacle") || collider.CompareTag("TutorialArea"))
        {
            colliderObstacle = false;
            
            ActivedPlayerController(false, true);

            if (jumpMsg != null)
                jumpMsg.Off();

            speedR = 0;
            speedL = 0;
        }
    }
}

