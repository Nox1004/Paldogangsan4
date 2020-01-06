using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour {

    public static string loadingSceneName = "LoadingScene";
    public static string previousSceneName = "";

    [Header("UI Information")]
    public Text loadingText;

    public CanvasGroup loadingProgressBar;

    public CanvasGroup loadingAnimation;

    [Header("Time")]
    public float startFadeDuration = 0.2f;
    public float progressBarSpeed = 2.0f;
    public float exitFadeDuration = 0.2f;
    public float loadCompleteDelay = 0.5f;

    [SerializeField] protected float m_fillTarget;

    protected AsyncOperation m_asyncOperation;   //비동기 씬전환을 하기 위해서 필요객체
    protected static string m_sceneToLoad = "";
    protected float m_fadeDuration = 0.5f;
    protected string m_loadingTextValue;

    //정적멤버함수로 설정해 이함수로 씬을 로드한다.
    //로드할 때 로딩화면을 나오게 한다.
    public static void LoadScene(string sceneToLoad)
    {
        previousSceneName = SceneManager.GetActiveScene().name;

        // 비동기로 로드할 씬의 이름 저장
        m_sceneToLoad = sceneToLoad;
        Application.backgroundLoadingPriority = ThreadPriority.High;

        if (loadingSceneName != null)
        {
            SceneManager.LoadSceneAsync(loadingSceneName,LoadSceneMode.Additive);
        }
    }

    public static string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    protected virtual void Start()
    {
        m_loadingTextValue = loadingText.text;

        if (m_sceneToLoad != "")
            StartCoroutine(LoadAsynchronously());
    }

    protected virtual void Update()
    {
        loadingProgressBar.GetComponent<Image>().fillAmount = m_fillTarget;
    }

    private IEnumerator LoadAsynchronously()
    {
        LoadingSetup();

        //LoadSceneAsync는 2가지 방식의 로드를 지원 
        //1. LoadSceneModeSingle --> 로드된 씬을 닫고 씬을 로드한다.
        //2. LoadSceneAdditive --> 현재 로드된 씬에 씬을 추가한다.
        //로딩창같은 경우에는 Single을 이용하는 편이 좋아보임.
        m_asyncOperation = SceneManager.LoadSceneAsync(m_sceneToLoad, LoadSceneMode.Single);
        m_asyncOperation.allowSceneActivation = false;

        while (m_asyncOperation.progress < 0.9f)
        {
            m_fillTarget = m_asyncOperation.progress;
            yield return null;
        }

        m_fillTarget = 1f;

        while (loadingProgressBar.GetComponent<Image>().fillAmount != m_fillTarget)
        {
            yield return null;
        }

        LoadingComplete();
        yield return new WaitForSeconds(loadCompleteDelay);

        yield return new WaitForSeconds(exitFadeDuration);

        // new Scene
        m_asyncOperation.allowSceneActivation = true;
    }

    private void LoadingSetup()
    {
        //Fade
        loadingProgressBar.GetComponent<Image>().fillAmount = 0f;
        loadingText.text = m_loadingTextValue;
    }

    private void LoadingComplete()
    {
        StartCoroutine(FadeCanvasGroup(loadingProgressBar, 0.1f, 0f));
        StartCoroutine(FadeCanvasGroup(loadingAnimation, 0.1f, 0f));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup target, float duration, float targetAlpha)
    {
        if (target == null)
            yield break;

        float currentAlpha = target.alpha;

        float t = 0f;

        while (t < 1.0f)
        {
            if (target == null)
                yield break;

            float newAlpha = Mathf.SmoothStep(currentAlpha, targetAlpha, t);
            target.alpha = newAlpha;

            t += Time.deltaTime / duration;

            yield return null;

        }

        target.alpha = targetAlpha;
    }
}
