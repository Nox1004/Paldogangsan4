using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public enum Language { ko = 0, en, fr }
    private int _count = 3; // Language 열거형 총 갯수를 나타낸다

    [SerializeField] protected Language current = Language.ko;
    [SerializeField] private bool isUsingOfFaivMat;

    public Language GetLanguage { get { return current; } }

    public bool GetUsingOfFaivMat { get { return isUsingOfFaivMat; } }

    public string diseaseName { get; set; }                 // 질병이름

    public int languageType { get; protected set; }         // 현재 언어타입(정수형)

    public List<string> languages { get; private set; }     // 모든 언어타입(문자열)

    protected override void Awake()
    {
        base.Awake();

        languages = new List<string>(_count);
        for(int i = 0; i < _count; i++)
        {
            languages.Add(((Language)i).ToString());
        }

#if UNITY_EDITOR
        //테스트용
        diseaseName = "dang";
#endif

        DontDestroyOnLoad(this.gameObject);
        Screen.SetResolution(1600, 900, true);
    }

    public void SetLanguage(int index)
    {
        current = (Language)index;

        languageType = index;
    }
}
