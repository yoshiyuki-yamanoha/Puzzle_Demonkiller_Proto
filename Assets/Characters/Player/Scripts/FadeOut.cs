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
        if(fadeInFlag == true)
        {
            alpha -= fadeSpeed; 
        }

        SetAlpha();

        starttextsiro.text = "BattleStart";
        starttextkuro.text = "BattleStart";
        starttextsiro.color = new Color(red, green, blue, alpha);
        starttextkuro.color = new Color(0, 0, 0, alpha);
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
}
