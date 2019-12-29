using UnityEngine;
using System.Collections;

// 기존에 있던 코드 약간만 수정
public class Coin : MonoBehaviour {

    public Controller controller;
    public Transform coinModel;
    public Transform player;
    public AudioClip biggerClip;
    public AudioClip getCoinClip;
    private bool canTouchCoin;
    public static int tempCoin = 0;

    void Update()
    {
        if (controller.coinJumpPoint && canTouchCoin)
        {
            if (InputSystem.instance.curHandState == InputSystem.HandState.Jump)
            {
                controller.coinJumpPoint = false;

                SoundManager.instance.PlayClip(getCoinClip);
                CoinUI.instance.coinCount++;
                canTouchCoin = false;
                gameObject.SetActive(false);
            }
        }

        else if (!controller.coinJumpPoint && canTouchCoin)
        {
            if (Check_CoinLocation() && InputSystem.instance.curHandState == InputSystem.HandState.Left ||
                !Check_CoinLocation() && InputSystem.instance.curHandState == InputSystem.HandState.Right)
            {
                SoundManager.instance.PlayClip(getCoinClip);
                CoinUI.instance.coinCount++;
                canTouchCoin = false;
                gameObject.SetActive(false);
            }
        }
    }



    bool Check_CoinLocation()
    {
        Vector3 gapVec = new Vector3(coinModel.position.x - player.position.x, coinModel.position.y - player.position.y, coinModel.position.z - player.position.z);
        Vector3 vec = new Vector3(gapVec.x, 0, gapVec.z);
        float angle = Vector3.Angle(vec, player.right);

        if (angle > 90)
            return true;
        else
            return false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            SoundManager.instance.PlayClip(biggerClip);

            col.gameObject.GetComponentInChildren<PlayerController>().isActivedHand = true;

            if (this.CompareTag("Coin"))
            {
                if (GameObject.Find("Canvas") != null)
                {
                    TutorialManager tutoMgr = GameObject.Find("Canvas").transform.Find("TutorialManager").GetComponent<TutorialManager>();
                    StartCoroutine(tutoMgr.Jump());
                }

                controller.coinJumpPoint = true;
                canTouchCoin = true;
                coinModel.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            }
            else if (this.CompareTag("CoinLikeStore"))
            {
                canTouchCoin = true;
                coinModel.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            }
            if(this.name=="Coin03")
            {
                if (GameObject.Find("Canvas") != null)
                {
                    TutorialManager tutoMgr = GameObject.Find("Canvas").transform.Find("TutorialManager").GetComponent<TutorialManager>();
                    StartCoroutine(tutoMgr.LeftHand());
                }
                canTouchCoin = true;
                coinModel.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            }
            if (this.name == "Coin02")
            {
                if (GameObject.Find("Canvas") != null)
                {
                    TutorialManager tutoMgr = GameObject.Find("Canvas").transform.Find("TutorialManager").GetComponent<TutorialManager>();
                    StartCoroutine(tutoMgr.RightHand());
                }
                canTouchCoin = true;
                coinModel.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponentInChildren<PlayerController>().isActivedHand = false;

            if (this.CompareTag("Coin"))
            {
                if (GameObject.Find("Canvas") != null)
                {
                    TutorialManager tutoMgr = GameObject.Find("Canvas").transform.Find("TutorialManager").GetComponent<TutorialManager>();
                    StopCoroutine(tutoMgr.Jump());
                    tutoMgr.tutoImg[4].SetActive(false);
                }
                controller.coinJumpPoint = false;
                canTouchCoin = false;
                gameObject.SetActive(false);
            }

            else if (this.CompareTag("CoinLikeStore"))
            {
                canTouchCoin = false;
                gameObject.SetActive(false);
            }
            if (this.name == "Coin03")
            {
                if (GameObject.Find("Canvas") != null)
                {
                    TutorialManager tutoMgr = GameObject.Find("Canvas").transform.Find("TutorialManager").GetComponent<TutorialManager>();
                    StopCoroutine(tutoMgr.LeftHand());
                    tutoMgr.tutoImg[2].SetActive(false);
                }
                canTouchCoin = false;
                gameObject.SetActive(false);
            }
            if (this.name == "Coin02")
            {
                if (GameObject.Find("Canvas") != null)
                {
                    TutorialManager tutoMgr = GameObject.Find("Canvas").transform.Find("TutorialManager").GetComponent<TutorialManager>();
                    StopCoroutine(tutoMgr.RightHand());
                    tutoMgr.tutoImg[3].SetActive(false);
                }
                canTouchCoin = false;
                gameObject.SetActive(false);
            }
        }
    }
}
