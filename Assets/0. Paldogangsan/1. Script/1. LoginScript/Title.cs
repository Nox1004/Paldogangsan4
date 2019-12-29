using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
    public Sprite title_1;
    public Sprite title_2;
    public SpriteRenderer spriteRenderer;
    private int state;
    private bool isUp;

    public GameObject LoginWindow;

    void Start()
    {
        isUp = true;
        
        //LoginWindow.SetActive(false);
        state = 0;
        spriteRenderer.sprite = title_1;
        spriteRenderer.color = new Color(255, 255, 255, 0);

        StartCoroutine(TitleAnim());
    }

    IEnumerator TitleAnim()
    {
        Color clr = spriteRenderer.color;
        while(true)
        {
            if(state == 0)
            {
                if(isUp)
                {
                    clr.a += Time.deltaTime;
                    spriteRenderer.color = clr;

                    if (spriteRenderer.color.a >= 1.0f)
                        isUp = false;
                }

                else if(!isUp)
                {
                    clr.a -= Time.deltaTime;
                    spriteRenderer.color = clr;

                    if (spriteRenderer.color.a <= 0.0f)
                    {
                        spriteRenderer.sprite = title_2;
                        spriteRenderer.color = new Color(255, 255, 255, 0);
                        clr = spriteRenderer.color;
                        isUp = true;
                        state = 1;
                    }
                }
            }

            else if(state == 1)
            {
                if (isUp)
                {
                    clr.a += Time.deltaTime;
                    spriteRenderer.color = clr;

                    if (spriteRenderer.color.a >= 1.0f)
                        isUp = false;
                }

                else if (!isUp)
                {
                    clr.a -= Time.deltaTime;
                    spriteRenderer.color = clr;

                    if (spriteRenderer.color.a <= 0.0f)
                    {
                        LoginWindow.SetActive(true);
                        Destroy(gameObject);
                    }
                }
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
