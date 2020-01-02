using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.UI;
using System.IO;
using System;

// 기존에 있던 스크립트 약간만 변형
// 비효율적인 방법으로 되있는거 같지만 수정하기에는 시간이 부족해서 그대로 둔다.
public class EndingUI : Change
{
    private FoodManager m_FoodManager;
    public GameObject GameInfoUI;

    public Timer timer;
    public FoodUI foodUI;

    int stepCount;
    int timerSec;
    int timerMin;
    int score;
    int rankToday;
    int rankTotal;
    int timeCount;
    int initTime;

    public AudioSource bgm;
    public AudioClip scoreClip;
    public AudioClip bgmClip;

    public Image[] SelectFoods;
    [SerializeField] private Sprite[] m_Foods;

    public Text prevScore;
    public Text curScore;
    public Text averageScore;

    public Text restart;
    public Text next;
    public Text count;

    public Text dayRank;
    public Text weekRank;

    public Text[] texts;

    public static bool isEnding = false;

    // 결과창에서 씬전환 가능여부를 나타내는 불변수
    private bool _IsPossibleSelect;

    // 입력을 받을 수 있게 하기 위한 불변수
    private bool _isActived;

    int m_prevScore;
    int m_num = 0;

    [SerializeField]
    public class saveGameData   //JsonUserData
    {
        public int curScore;
    }
 
    private void OnEnable()
    {
        InputSystem.instance.HandSystemBinding(InputSystem.instance.PressLeftHandKeyDown,
                                               InputSystem.instance.PressRightHandKeyDown);
    }

    protected void Start()
    {
        Initialize();

        m_FoodManager = FoodManager.instance;

        Sprite[] sprites = Resources.LoadAll<Sprite>(GameManager.instance.GetLanguage
                                                        + "/food");
        m_Foods = new Sprite[sprites.Length];

        for(int i = 0; i < m_Foods.Length; i++)
        {
            m_Foods[i] = sprites[i];
        }

        GameInfoUI.SetActive(false);
        for (int i = 0; i < foodUI.curIndex; i++)
        {
            foodUI.foods[i].SetActive(false);
        }

        initEnding();
    }

    void FixedUpdate()
    {
        if (!_IsPossibleSelect)
        {
            TextsSetActive(false, false, true);

            timeCount = initTime - (int)Time.time;

            count.text = timeCount.ToString();

            if (timeCount < 0)
            {
                _IsPossibleSelect = true;
                _isActived = true;
            }
        }
        else
        {
            TextsSetActive(true, true, false);

            InputActive();
        }
    }
    

    // 다시하기버튼 클릭이벤트
    private void RestartButton()
    {
        InputSystem.instance.PressLeftHandKeyDown();
    }

    // 메뉴선택버튼 클릭이벤트
    private void ReturnBackButton()
    {
        InputSystem.instance.PressRightHandKeyDown();
    }

    private void TextsSetActive(bool isActivedRestart
                              , bool isActivedNext
                              , bool isActivedCount)
    {
        if (restart.gameObject.activeSelf != isActivedRestart)
            restart.gameObject.SetActive(isActivedRestart);

        if (next.gameObject.activeSelf != isActivedNext)
            next.gameObject.SetActive(isActivedNext);

        if (count.gameObject.activeSelf != isActivedCount)
            count.gameObject.SetActive(isActivedCount);
    }

    // InputSystem의 Hand상태에 따라 처리
    private void InputActive()
    {
        if (_isActived) {
            if (InputSystem.instance.curHandState == InputSystem.HandState.Right)
            {
                if (CoinUI.instance != null)
                {
                    CoinUI.instance.coinCount = 0;
                    Coin.tempCoin = 0;
                }

                _isActived = false;
                FoodManager.stage = 0;

                LoadingSceneManager.LoadScene("Main");
            }
            else if (InputSystem.instance.curHandState == InputSystem.HandState.Left)
            {
                _isActived = false;

                if (CoinUI.instance != null)
                    CoinUI.instance.coinCount = Coin.tempCoin;

                LoadingSceneManager.LoadScene("inGame");
            }
        }
    }

    public void initEnding()
    {
        for (int i = 0; i < foodUI.foodArray.Length; i++)
        {
            for (int j = 0; j < 45; j++)
            {
                if (foodUI.foodArray[i] != null && foodUI.foodArray[i].name == m_Foods[j].name)
                {
                    if (FoodManager.instance.CheckFood(foodUI.foodArray[i]))
                    {
                        SelectFoods[i].sprite = m_Foods[j];
                        SelectFoods[i].GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        SelectFoods[i].sprite = m_Foods[j];
                        SelectFoods[i].GetComponent<Image>().color = Color.red;
                    }
                }
            }
        }


        CalculateScore();
        initTime = (int)Time.time + 6;
        SaveData();
        ShowScore();
        ShowRank();
    }

    private void ShowScore()
    {

        int averScore = 0;
        int averNum = 0;
        for (int i = 1; ; i++)
        {
            FileInfo file = new FileInfo(FileIO.PathForDocumentsFile("result/" + UserData.id
                                                                    + "_Stage" + FoodManager.stage
                                                                    + "_" + DateTime.FromOADate(System.DateTime.Today.ToOADate()).ToString("yyyy-MM-dd")
                                                                    + "_" + i + ".txt"));
            if (!file.Exists && i == 2)
            {
                prevScore.text = "기록 없음";
                LoadData(1);
                averScore = m_prevScore;
                break;
            }
            else if (!file.Exists)
            {
                LoadData(i - 2); 
                prevScore.text = m_prevScore.ToString();
                break;
            }
            else
            {
                LoadData(i);
                averScore += m_prevScore;
            }
        }
        curScore.text = score.ToString();
        averNum = m_num;
        averageScore.text = ((int)(averScore / averNum)).ToString();
    }

    private void ShowRank()
    {
        int rankToday = 1;
        int rankTotal = 1;
        double todayOLE = System.DateTime.Today.ToOADate();
        for (int i = 1;i<30; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                FileInfo file = new FileInfo(FileIO.PathForDocumentsFile("result/" + UserData.id 
                                                                    + "_Stage" + FoodManager.stage 
                                                                    + "_" + DateTime.FromOADate(DateTime.Today.ToOADate()).ToString("yyyy-MM-dd") 
                                                                    + "_" + i + ".txt"));
                if (!file.Exists)
                {
                    break;
                }
                else
                {
                    LoadData(i);
                    if (score < m_prevScore)
                    {
                        rankTotal++;
                        if (j == 0)
                            rankToday++;
                    }
                }
            }
        }
        dayRank.text = rankToday.ToString();
        weekRank.text = rankTotal.ToString();
    }

    private void CalculateScore()
    {
        int stepCount = Step.stepCount;
        int timerSec = timer.getSec();
        int timerMin = timer.getMin();
        int coin = (CoinUI.instance != null)? CoinUI.instance.coinCount: 0;

        score = (m_FoodManager.goodCount * 50) + (-40 * m_FoodManager.badCount) + (coin * 30);
        score += 120 - (int)Time.timeSinceLevelLoad;

        if (score > 999)
            score = 999;
        else if (score < 0)
        {
            score = 0;
        }
    }

    private void LoadData(int i)
    {
        string load = FileIO.ReadStringFromFile("result/" + UserData.id
                                                    + "_Stage" + FoodManager.stage
                                                    + "_" + DateTime.FromOADate(DateTime.Today.ToOADate()).ToString("yyyy-MM-dd") 
                                                    + "_" + i + ".txt");
        var loadData = JsonUtility.FromJson<saveGameData>(load);
        m_prevScore = loadData.curScore;
    }

    private void SaveData()
    {
        var _saveGameData = new saveGameData();
        {
            _saveGameData.curScore = score;
        }
        string _save = JsonUtility.ToJson(_saveGameData, prettyPrint: true);

        for (int i = 1; ; i++)
        {
            FileInfo file = new FileInfo(FileIO.PathForDocumentsFile("result/" + UserData.id 
                                                                    + "_Stage" + FoodManager.stage 
                                                                    + "_" + DateTime.FromOADate(DateTime.Today.ToOADate()).ToString("yyyy-MM-dd") 
                                                                    + "_" + i + ".txt"));
            if (file.Exists)
            {
                continue;
            }
            else
            {
                m_num = i;
                FileIO.WriteStringToFile(FileMode.Create, _save, "result", UserData.id
                                                                            + "_Stage" + FoodManager.stage
                                                                            + "_" + DateTime.FromOADate(DateTime.Today.ToOADate()).ToString("yyyy-MM-dd")
                                                                            + "_" + i + ".txt");
                break;
            }
        }
    }

    protected override void ChangeLanguage()
    {
        XmlNodeList nodes= m_XmlDoc.SelectNodes("EndText/Result");
        texts[0].text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("EndText/Selected");
        texts[1].text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("EndText/CurrentScore");
        texts[2].text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("EndText/LastScore");
        texts[3].text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("EndText/AverageScore");
        texts[4].text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("EndText/DailyRanking");
        texts[5].text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;

        nodes = m_XmlDoc.SelectNodes("EndText/WeeklyRanking");
        texts[6].text = nodes[0].SelectSingleNode(GameManager.instance.GetLanguage.ToString()).InnerText;
    }
}
