using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 이제 이것을 사용하지 않습니다.
/// </summary>
public class InputManager : Singleton<InputManager>
{
	#region BoolValue
	public static bool isKinectOn = false;
	public bool isRightFootKeyDown = false;
	public bool isLeftFootKeyDown = false;
	public bool isRightHandKeyDown = false;
	public bool isLeftHandKeyDown = false;
    public bool isHandX = false;
    public bool isFoldArm = false;
    public bool isSelectFood = false;
	
	public bool isMoveRight = true;
	public bool isMoveLeft = true;
    #endregion
    public bool isEscape = false;

	public enum footState { LEFT = 0, RIGHT, STOP};
	public enum handState { LEFT = 0, RIGHT, JUMP, X, FOLDARM, ALLHANDDOWN, RIGHTHAND_ACTION}; 
	public footState curFootState;
	public handState curHandState;

    //private BodySourceView kinect;
    Transform startingInfo;

    void Start()
    {
        if (GameObject.Find("UI"))
        {
            startingInfo = GameObject.Find("UI").transform.Find("StartingInfo").transform;
        }
    }

	void Update()
    {
        GetBoolValueOnOff_Keyboard();
        CheckMotion();
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isEscape = true;
        }

        if (isEscape && SceneManager.GetActiveScene().name == "main")
        {
            Application.Quit();
        }
        else if (isEscape && SceneManager.GetActiveScene().name == "main")
        {
            CoinUI.instance.coinCount = 0;
            SceneManager.LoadScene("main");
        }
        else if (isEscape &&(SceneManager.GetActiveScene().name == "ingame"|| SceneManager.GetActiveScene().name == "Stage3"))
        {
            CoinUI.instance.coinCount = 0;
            SceneManager.LoadScene("SelectStage");
        }
    }

    void CheckMotion()
	{
        if (startingInfo == null)
        {
            // Left Foot
            if (Input.GetKey(KeyCode.D) && !isRightFootKeyDown && curFootState == footState.STOP)
            {
                isRightFootKeyDown = true;
                isLeftFootKeyDown = false;
            }

            // Right Foot
            else if (Input.GetKey(KeyCode.A) && !isLeftFootKeyDown && curFootState == footState.STOP)
            {
                isRightFootKeyDown = false;
                isLeftFootKeyDown = true;
            }
        }

		// Left Hand
        if (Input.GetKey(KeyCode.Q))
        {
            isLeftHandKeyDown = true;
        }

        else
        {
            isLeftHandKeyDown = false;
        }

        if (Input.GetKey(KeyCode.E))
        {
            isRightHandKeyDown = true;
        }
        else
        {
            isRightHandKeyDown = false;
        }
        
        //Select Menu
        if (Input.GetKeyDown(KeyCode.R))
        {
            isSelectFood = true;
        }

        else
        {
            isSelectFood = false;
        }

        // Delete BadFood
        if (Input.GetKey(KeyCode.X))
        {
            isHandX = true;
        }

        else
        {
            isHandX = false;
        }

        // Show Chance
        if (Input.GetKey(KeyCode.W))
        {
            isFoldArm = true;
        }

        else
        {
            isFoldArm = false;
        }
	}
	
	void GetBoolValueOnOff_Keyboard()
	{
        #region Foot

        if (!isRightFootKeyDown && isLeftFootKeyDown && isMoveRight)
        {
            curFootState = footState.LEFT;
            isMoveLeft = true;
            isMoveRight = false;
        }

        else if (isRightFootKeyDown && !isLeftFootKeyDown && isMoveLeft)
        {
            curFootState = footState.RIGHT;
            isMoveLeft = false;
            isMoveRight = true;
        }

        else if (!isLeftFootKeyDown && !isRightFootKeyDown)
        {
            curFootState = footState.STOP;
        }
        #endregion

        #region Hand

        if (isLeftHandKeyDown && !isRightHandKeyDown && !isHandX && !isFoldArm && !isSelectFood)
        {
            curHandState = handState.LEFT;
        }
        else if(isRightHandKeyDown && !isLeftHandKeyDown && !isHandX && !isFoldArm && !isSelectFood)
		{
			curHandState = handState.RIGHT;
		}
        else if (isRightHandKeyDown && isLeftHandKeyDown && !isHandX && !isFoldArm && !isSelectFood)
		{
            curHandState = handState.JUMP;
		}

        else if (!isRightHandKeyDown && !isLeftHandKeyDown && isHandX && !isFoldArm && !isSelectFood)
        {
            curHandState = handState.X;
        }

        else if (!isRightHandKeyDown && !isLeftHandKeyDown && !isHandX && isFoldArm && !isSelectFood)
        {
            curHandState = handState.FOLDARM;
        }

        else if (!isRightHandKeyDown && !isLeftHandKeyDown && !isHandX && !isFoldArm && isSelectFood)
        {
            curHandState = handState.RIGHTHAND_ACTION;
        }

        else
        {
            curHandState = handState.ALLHANDDOWN;
        }

        #endregion
    }
}

