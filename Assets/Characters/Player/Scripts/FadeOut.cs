using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    float fadeSpeed = 0.06f;//フェードスピード
    float red, green, blue, alpha;//色:r,g,b 透明度:a
    public bool fadeOutFlag = false;// Fadeoutのフラグ
    public bool fadeInFlag = false;//FadeInのフラグ

    // Start is called before the first frame update
    void Start()
    {
        SetColor_White();
        alpha = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(fadeOutFlag == true)
        {
            StartFadeOut();
        }
        if(fadeInFlag == true)
        {
            alpha -= fadeSpeed; 
        }

        SetAlpha();

        //gageColor
    }

    void StartFadeOut()
    {
        alpha += fadeSpeed;
        if(alpha >= 1)
        {
            fadeOutFlag = false;
        }
    }
    void SetAlpha()
    {
        //fadeImage = new Color(red, green, blue, alpha);
        this.GetComponent<Image>().color = new Color(red, green, blue, alpha);
    }

    public void SetColor_Black()
    {
        red = Color.black.r;
        green = Color.black.g;
        blue = Color.black.b;
    }

    public void SetColor_White()
    {
        red = Color.white.r;
        green = Color.white.g;
        blue = Color.white.b;
    }

    public float GetAlpha()
    {
        return alpha;
    }
}
