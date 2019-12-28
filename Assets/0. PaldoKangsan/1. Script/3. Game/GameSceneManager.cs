using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : Singleton<GameSceneManager> {

    public bool isGamePlaying { get; private set; }

    [Header("UI")]
    [SerializeField] protected StartUI m_StartUI;
    [SerializeField] protected EndingUI m_EndUI;

    // 기존에 만들어둔 게임씬 UI를 그대로 사용한다.
    // ㄴ UGUI를 이용하게 변경예정
    [Header("GameInfoUI")]
    [SerializeField] protected GameObject m_GameInfoUI;

    public StartUI GetStartUI { get { return m_StartUI; } }
    public GameObject GetGameInfoUI { get { return m_GameInfoUI; } }

    protected void Reset()
    {
        m_GameInfoUI = GameObject.Find("GameInfoUI");
    }

    protected virtual void Start()
    {
        // 시작 UI를 사용할 경우에
        if (!GameManager.instance.GetUsingOfFaivMat)
        {
            m_StartUI.gameObject.SetActive(true);
            m_EndUI.gameObject.SetActive(false);
            m_GameInfoUI.gameObject.SetActive(false);
        }
        // FAIV MAT 테스트를 위함 로직 추가
        else
        {
            m_EndUI.gameObject.SetActive(false);

            StartGame();
        }
    }

    public void StartGame()
    {
        isGamePlaying = true;

        m_StartUI.gameObject.SetActive(false);
        m_GameInfoUI.gameObject.SetActive(true);
    }

    public void EndGame()
    {
        isGamePlaying = false;

        m_GameInfoUI.gameObject.SetActive(false);

        m_EndUI.gameObject.SetActive(true);
    }
}
