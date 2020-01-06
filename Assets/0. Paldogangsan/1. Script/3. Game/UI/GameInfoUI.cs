using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameInfoUI : MonoBehaviour {

    [SerializeField, Header("숫자이미지")]
    private List<Sprite> m_number;

    private WalkingStep m_walkingStep;
    private PlayTimer m_playTimer;
    private BasketUI m_basketUI;

    public CoinUI coinUI { get; private set; }

    // ----- walkingStep 기능 호출 -----

    public void IncreaseStep()
    {
        m_walkingStep.Increase(m_number);
    }

    public int getStep()
    {
        return m_walkingStep.count;
    }

    // ----- playTimer 기능 호출 -----
    
    public int getMin()
    {
        return (int)m_playTimer.time / 60;
    }

    public int getSec()
    {
        return (int)m_playTimer.time % 60;
    }

    // ----- coinUI 기능 호출 -----

    public void IncreaseCoin()
    {
        coinUI.Increase();
    }

    public int getCoin()
    {
        return coinUI.count;
    }

    // ----- BasketUI 기능 호출 -----
    
    public void RightPurchase(Material material)
    {
        m_basketUI.RightChoice(material);
    }

    public void WrongPurchase(Material material)
    {
        m_basketUI.WrongChoice(material);
    }

    public bool EnablePurchase()
    {
        return m_basketUI.EnableToPut();
    }

    public bool CheckSameFood(Material material)
    {
        return m_basketUI.Equalitem(material);
    }

    public bool CheckSameFood(int idx, string name)
    {
        return m_basketUI.Equalitem(idx, name);
    }

    public int GetBasketCount()
    {
        return m_basketUI.getBasketCount;
    }

    public Material GetBasketMaterial(int idx)
    {
        return m_basketUI.GetMaterial(idx);
    }

    // ----- 이벤트 함수 -----

    private void Awake()
    {
        m_walkingStep = GetComponentInChildren<WalkingStep>();
        m_playTimer = GetComponentInChildren<PlayTimer>();
        m_basketUI = GetComponentInChildren<BasketUI>();
        coinUI = GetComponentInChildren<CoinUI>();
    }

    private void FixedUpdate()
    {
        if (GameSceneManager.instance.isGamePlaying)
        {
            m_playTimer.Increase(m_number);
        }
    }
}
