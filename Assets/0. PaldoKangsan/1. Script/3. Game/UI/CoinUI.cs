using UnityEngine;
using System.Collections;

public class CoinUI : Singleton<CoinUI> {

    public int coinCount { get; set; }
    public Texture2D coinImage;
    public Texture2D[] numberImage;

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(1390 * Screen.width / 1600, 10, Screen.width, Screen.height));
        GUI.DrawTexture(new Rect(0, 0, 100 * Screen.width / 1600, 100 * Screen.height / 900), coinImage, ScaleMode.ScaleToFit);
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(1490 * Screen.width / 1600, 10, Screen.width, Screen.height));
        GUI.DrawTexture(new Rect(0, 0, 100 * Screen.width / 1600, 100 * Screen.height / 900), numberImage[coinCount], ScaleMode.ScaleToFit);
        GUI.EndGroup();
    }
}
