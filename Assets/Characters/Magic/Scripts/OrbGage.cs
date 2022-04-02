using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbGage : MonoBehaviour
{
    public Slider starRed;
    public Slider starLightBlue;
    public Slider starYellow;
    public Slider pentagonRed;
    public Slider pentagonLightBlue;
    public Slider pentagonYellow;

    public bool starflag;
    public bool pentflag;

    public int colorflag;//0なし1赤2水色3黄色

    // Start is called before the first frame update
    void Start()
    {
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
        if (colorflag == 1)
        {
            if (starflag)
            {
                starRedChage();
                starflag = false;
            }
            if (pentflag)
            {
                pentagonRedChage();
                pentflag = false;
            }
        }
        if (colorflag == 2)
        {
            if (starflag)
            {
                starLightBlueChage();
                starflag = false;
            }
            if (pentflag)
            {
                pentagonLightBlueChage();
                pentflag = false;
            }
        }
        if (colorflag == 3)
        {
            if (starflag)
            {
                starYellowChage();
                starflag = false;
            }
            if (pentflag)
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
    public int ChargeOrb(int type)
    {
        if(type == (int)PointControl.MAGIC_MODE.STAR)
        {
            pentflag = true;
        }
        if(type == (int)PointControl.MAGIC_MODE.PENTAGON)
        {
            starflag = true;
        }
        return type;
    }
}
