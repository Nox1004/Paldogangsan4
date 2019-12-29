using UnityEngine;
using System.Collections;

[System.Serializable]
public class scoreView
{
    public int one, ten, hun;
    public bool isMinus;
    public Number[] numbers;
}

[System.Serializable]
public struct Number
{
    public Texture2D[] animation;
}