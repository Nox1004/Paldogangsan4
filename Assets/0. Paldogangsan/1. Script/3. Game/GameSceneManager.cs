using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : Singleton<GameSceneManager> {

    public bool isGamePlaying { get; private set; }

    [Header("UI")]
    [SerializeField] private StartUI m_StartUI;
    [SerializeField] private GameInfoUI m_GameInfoUI;
    [SerializeField] private EndingUI m_EndUI;

    public StartUI getStartUI { get { return m_StartUI; } }
    public GameInfoUI getGameInfoUI { get { return m_GameInfoUI; } }

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
