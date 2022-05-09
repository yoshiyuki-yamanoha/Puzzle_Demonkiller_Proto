using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleColor : MonoBehaviour
{
    [Header("色の設定")]
    //public Color OrbColor;
    public Color MCC;
    private bool flag = false;

    public bool alphaChangeflag;
    public bool colorChangeflag;
    float r = 1, g = 0, b = 0;//色の変化に使う
    float colorChangeSpeed = 0.02f;//色の変化speed
    bool ccolorChangeOn;//色を変化させる時につかう
    // Start is called before the first frame update
    void Start()
    {

        //GetComponent<Renderer>().sharedMaterial.color = OrbColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alphaChangeflag == true)
        {
            AlphaChange(); //アルファ値の変化
        }
        if(colorChangeflag == true)
        {
            ColorChange();
        }

        GetComponent<Renderer>().sharedMaterial.color = MCC;
    }
    void AlphaChange()
    {
        //MagicCircle;
        if (MCC.a < 0)
        {
            flag = false;
        }
        if (MCC.a > 1)
        {
            flag = true;
        }
        if (flag == false)
        {
            MCC.a += 0.01f;
        }
        else
        {
            MCC.a -= 0.01f;
        }

    }
    void ColorChange()
    {

        //赤いとき
        if (r >= 1 && b <= 0)
        {
            g += colorChangeSpeed;
            MCC.g = g;

        }
        //黄色いとき
        if (g >= 1 && b <= 0)
        {
            r -= colorChangeSpeed;
            MCC.r = r;
        }
        //緑のとき
        if (r <= 0 && g >= 1)
        {
            b += colorChangeSpeed;
            MCC.b = b;
        }
        //水色のとき
        if (r <= 0 && b >= 1)
        {
            g -= colorChangeSpeed;
            MCC.g = g;
        }
        //青のとき
        if (g <= 0 && b >= 1)
        {
            r += colorChangeSpeed;
            MCC.r = r;
        }
        //桃色のとき
        if (r >= 1 && g <= 0)
        {
            b -= colorChangeSpeed;
            MCC.b = b;
        }
    }
}
