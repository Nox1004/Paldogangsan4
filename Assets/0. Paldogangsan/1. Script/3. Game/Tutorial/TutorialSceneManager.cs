using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSceneManager : GameSceneManager {

    [SerializeField, Header("튜토리얼 이미지")]
    private List<Tutorial> _imageList;

    [System.Serializable]
    public struct Tutorial
    {
        public string key;
        public Sprite sprite;
    }

    // 튜토리얼 성공 사운드
    [SerializeField]
    private AudioClip _clip = null;

    private TutorialUI _tutorialUI;

    private Dictionary<string, Sprite> _map;

    public void ShowImage(string key) {
        _tutorialUI.ImageDescription(_map[key]);
    }

    public void ShowText() {
        _tutorialUI.TextDescription();
    }

    public void NotShow() {
        _tutorialUI.Disabled();
    }

    public void SoundActiveAndNotShow()
    {
        NotShow();

        SoundManager.instance.PlayClip(_clip, false, _clip.length);
    }

    public void FinishTutorial()
    {
        StartCoroutine(EndTutorial());
    }

    protected override void Awake()
    {
        base.Awake();
        _tutorialUI = GetComponentInChildren<TutorialUI>();
        _map = new Dictionary<string, Sprite>(8);

        for(int i = 0; i < _imageList.Count; i++)
        {
            _map.Add(_imageList[i].key, _imageList[i].sprite);
        }
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        ShowImage("확인하기");
        
        while(!isGamePlaying)
        {
            yield return null;
        }

        ShowImage("걸어가기");
    }

    private IEnumerator EndTutorial()
    {
        ShowText();

        yield return new WaitForSeconds(4.0f);

        _tutorialUI.Disabled();

        LoadingSceneManager.LoadScene("Main");
    }
}
