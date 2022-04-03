using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbGage : MonoBehaviour
{
    //オーブゲージ
    public Slider starRed;
    public Slider starLightBlue;
    public Slider starYellow;
    public Slider pentagonRed;
    public Slider pentagonLightBlue;
    public Slider pentagonYellow;

    //魔方陣の線の形
    public bool starflag;
    public bool pentflag;

    //魔方陣の色
    public int colorflag;//0なし1赤2水色3黄色

    // Start is called before the first frame update
    void Start()
    {
        //オーブのスライダーゲージの初期化
        starRed.value = 0;
        starLightBlue.value = 0;
        starYellow.value = 0;
        pentagonRed.value = 0;
        pentagonLightBlue.value = 0;
        pentagonYellow.value = 0;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (colorflag == 1)//赤なら
        {
            if (starflag)//星型なら
            {
                starRedChage();
                starflag = false;
            }
            if (pentflag)//五角形なら
            {
                pentagonRedChage();
                pentflag = false;
            }
        }
        if (colorflag == 2)//水色なら
        {
            if (starflag)//星型なら
            {
                starLightBlueChage();
                starflag = false;
            }
            if (pentflag)//五角形なら
            {
                pentagonLightBlueChage();
                pentflag = false;
            }
        }
        if (colorflag == 3)//黄色なら
        {
            if (starflag)//星型なら
            {
                starYellowChage();
                starflag = false;
            }
            if (pentflag)//五角形なら
            {
                pentagonYellowChage();
                pentflag = false;
            }
        }
        colorflag = 0;
    }
    public void starRedChage()
    {
        starRed.value += 1;
    }
    public void starLightBlueChage()
    {
        starLightBlue.value += 1;
    }
    public void starYellowChage()
    {
        starYellow.value += 1;
    }
    public void pentagonRedChage()
    {
        pentagonRed.value += 1;
    }
    public void pentagonLightBlueChage()
    {
        pentagonLightBlue.value += 1;
    }
    public void pentagonYellowChage()
    {
        pentagonYellow.value += 1;
    }
    public int ChargeOrb(int type)//魔方陣の形
    {
        if(type == (int)PointControl.MAGIC_MODE.STAR)//星型なら
        {
            starflag = true;
        }
        if(type == (int)PointControl.MAGIC_MODE.PENTAGON)//五角形なら
        {
            pentflag = true;
        }
        return type;
    }
    public void OrbReset()//オーブのゲージリセット・主に魔法を撃った時に使う
    {
        starRed.value = 0;
        starLightBlue.value = 0;
        starYellow.value = 0;
        pentagonRed.value = 0;
        pentagonLightBlue.value = 0;
        pentagonYellow.value = 0;
    }
}
