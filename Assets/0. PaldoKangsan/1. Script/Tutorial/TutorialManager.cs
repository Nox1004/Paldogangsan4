using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {
    public GameObject[] tutoImg;
    private float time;
    public InputManager inputMgr;

    public bool[] b_checked = new bool[8];
    public bool b_checkUI = false;
    public AudioSource audioSource;
    public AudioClip clip;

    void Start()
    {
        for (int i = 0; i < tutoImg.Length; i++)
        {
            tutoImg[i].SetActive(false);
            b_checked[i] = false;
        }
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;

        if(time > 3.0f && !b_checked[0])
        {
            StartCoroutine(Check());
            b_checked[0] = true;
        }

        if (b_checkUI && !b_checked[1])
        {
            StartCoroutine(Walk());
            b_checked[1] = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.KeypadEnter))
            Application.LoadLevel("SelectStage");
    }

    IEnumerator Check()
    {
        tutoImg[0].SetActive(true);
        while(inputMgr.curHandState != InputManager.handState.LEFT)
        {
            yield return null;
        }
        AudioSource.PlayClipAtPoint(clip, audioSource.transform.position);
        b_checkUI = true;
        tutoImg[0].SetActive(false);
        time = 0;
    }

    IEnumerator Walk()
    {
        tutoImg[1].SetActive(true);
        while (inputMgr.curFootState != InputManager.footState.LEFT || inputMgr.curFootState == InputManager.footState.RIGHT)
        {
            yield return null;
        }
        AudioSource.PlayClipAtPoint(clip, audioSource.transform.position);
        tutoImg[1].SetActive(false);
        time = 0;
    }

    public IEnumerator Jump()
    {
        tutoImg[4].SetActive(true);
        while (inputMgr.curHandState != InputManager.handState.JUMP)
        {
            yield return null;
        }
        AudioSource.PlayClipAtPoint(clip, audioSource.transform.position);
        tutoImg[4].SetActive(false);
    }

    public IEnumerator LeftHand()
    {
        tutoImg[2].SetActive(true);
        while (inputMgr.curHandState != InputManager.handState.LEFT)
        {
            yield return null;
        }
        Controller.tutofoodarea = false;
        AudioSource.PlayClipAtPoint(clip, audioSource.transform.position);
        tutoImg[2].SetActive(false);
    }

    public IEnumerator RightHand()
    {
        tutoImg[3].SetActive(true);
        while (inputMgr.curHandState != InputManager.handState.RIGHT)
        {
            yield return null;
        }
        Controller.tutofoodarea = false;
        AudioSource.PlayClipAtPoint(clip, audioSource.transform.position);
        tutoImg[3].SetActive(false);
    }

    public IEnumerator DeleteItem()
    {
        tutoImg[5].SetActive(true);
        while (inputMgr.curHandState != InputManager.handState.X)
        {
            yield return null;
        }
        AudioSource.PlayClipAtPoint(clip, audioSource.transform.position);
        tutoImg[5].SetActive(false);
    }

    public IEnumerator CheckItem()
    {
        tutoImg[6].SetActive(true);
        while (inputMgr.curHandState != InputManager.handState.FOLDARM)
        {
            yield return null;
        }
        AudioSource.PlayClipAtPoint(clip, audioSource.transform.position);
        tutoImg[6].SetActive(false);
    }

    public IEnumerator ExitTutorial()
    {
        tutoImg[7].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        tutoImg[7].SetActive(false);
        Application.LoadLevel("SelectStage");
    }
}
