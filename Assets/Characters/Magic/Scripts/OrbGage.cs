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

    [SerializeField] Slider[] orb_Gage = new Slider[6];

    //魔方陣の線の形
    public bool starflag;
    public bool pentflag;

    //魔方陣の色
    public int colorflag;//0なし1赤2水色3黄色

    //各オーブのレベル0~3  0:赤星 1:青星 2:黄星 3:赤角 4:青角 5:黄角 
    private int[] Orb_Level = new int[6];

    private const int ORB_MAX_LEVEL = 3;

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
        Gage_Draw();
    }
    public void starRedChage()
    {
        //starRed.value += 1;
        if(Orb_Level[0]++ > ORB_MAX_LEVEL)
        {
            Orb_Level[0] = ORB_MAX_LEVEL;
        }
    }
    public void starLightBlueChage()
    {
        if (Orb_Level[1]++ > ORB_MAX_LEVEL)
        {
            Orb_Level[1] = ORB_MAX_LEVEL;
        }
    }
    public void starYellowChage()
    {
        if (Orb_Level[2]++ > ORB_MAX_LEVEL)
        {
            Orb_Level[2] = ORB_MAX_LEVEL;
        }
    }
    public void pentagonRedChage()
    {
        if (Orb_Level[3]++ > ORB_MAX_LEVEL)
        {
            Orb_Level[3] = ORB_MAX_LEVEL;
        }
    }
    public void pentagonLightBlueChage()
    {
        if (Orb_Level[4]++ > ORB_MAX_LEVEL)
        {
            Orb_Level[4] = ORB_MAX_LEVEL;
        }
    }
    public void pentagonYellowChage()
    {
        if (Orb_Level[5]++ > ORB_MAX_LEVEL)
        {
            Orb_Level[5] = ORB_MAX_LEVEL;
        }
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

    private void Gage_Draw()
    {
        for (int i = 0; i < orb_Gage.Length; i++)
        {
            orb_Gage[i].value = Orb_Level[i];
        }
    }

    public int[] Get_Orb_Level()
    {
        return Orb_Level;
    }
}
