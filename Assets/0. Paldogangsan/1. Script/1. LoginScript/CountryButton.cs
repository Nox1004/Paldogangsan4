using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryButton : MonoBehaviour {

    [SerializeField] private Image m_Flag;

    public Sprite GetFlagImage()
    {
        return m_Flag.sprite;
    }
}
