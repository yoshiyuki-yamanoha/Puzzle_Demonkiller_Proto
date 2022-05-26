using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    float fadeSpeed = 1f;//フェードスピード
    public float soundFadeSpeed/*fadeSpeed * 2*/;
    float red, green, blue;//色:r,g,b 透明度:a
    public float alpha;
    public bool fadeOutFlag = false;// Fadeoutのフラグ
    public bool fadeInFlag = false;//FadeInのフラグ

    public enum FadeMode
    {
        OFF = 0,
        FADE_IN,
        FADE_OUT,
    }

    // Start is called before the first frame update
    void Start()
    {
        SetColor_White();
        soundFadeSpeed = 0.2f;
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
            StartFadeIn();
        }

        SetAlpha();

        //gageColor
    }

    void StartFadeIn()
    {
        alpha -= fadeSpeed * Time.deltaTime;
        if (alpha <= 0.0f)
        {
            alpha = 0.0f;
            fadeInFlag = false;
        }
    }

    void StartFadeOut()
    {
        alpha += fadeSpeed * Time.deltaTime;
        if (alpha >= 1.0f)
        {
            alpha = 1.0f;
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

    public void SetFadeMode(FadeMode fadeMode)
    {
        if(fadeMode == FadeMode.OFF)
        {
            fadeInFlag = false;
            fadeOutFlag = false;
        }
        if (fadeMode == FadeMode.FADE_IN)
        {
            fadeInFlag = true;
            fadeOutFlag = false;
        }
        if (fadeMode == FadeMode.FADE_OUT)
        {
            fadeInFlag = false;
            fadeOutFlag = true;
        }
    }

    /// <summary>
    /// フェードアウトが完了したか確認
    /// </summary>
    public bool FinishFadeOut()
    {
        const float fadeOut = 1.0f;

        if (alpha >= fadeOut)
            return true;
        
        return false;
    }

    public void SetFadeSpeed(float speed)
    {
        fadeSpeed = speed;
    }
}
