﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    float fadeSpeed = 0.02f;//フェードスピード
    float red, green, blue, alpha;//色:r,g,b 透明度:a
    public bool fadeOutFlag = false;// Fadeoutのフラグ
    public bool fadeInFlag = false;//FadeInのフラグ

    Text starttextsiro;
    Text starttextkuro;
    // Start is called before the first frame update
    void Start()
    {
        starttextsiro = GameObject.Find("GameStartSiro").GetComponent<Text>();
        starttextkuro = GameObject.Find("GameStartKuro").GetComponent<Text>();
        red = 1;
        green = 1;
        blue = 1;
        alpha = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(fadeOutFlag == true)
        {
            StartFadeOut();
        }
        if(fadeOutFlag == true)
        {
            alpha -= fadeSpeed * 2;
        }

        SetAlpha();

        starttextsiro.text = "BattleStart";
        starttextkuro.text = "BattleStart";
        starttextsiro.color = new Color(red, green, blue, alpha);
        starttextkuro.color = new Color(0, 0, 0, alpha);
    }

    void StartFadeOut()
    {
        alpha += fadeSpeed;
        if(alpha >= 1)
        {
            fadeOutFlag = false;
            fadeInFlag = true;
        }
    }
    void SetAlpha()
    {
        //fadeImage = new Color(red, green, blue, alpha);
        this.GetComponent<Image>().color = new Color(red, green, blue, alpha);
    }
}
