﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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


    Color[] orb_Gauge_Color = new Color[6];
    [SerializeField] Color grayOutColor;

    //魔方陣の線の形
    public bool starflag;
    public bool pentflag;

    //魔方陣の色
    public int colorflag;//0なし1赤2水色3黄色

    //各オーブのレベル0~3  0:赤星 1:青星 2:黄星 3:赤角 4:青角 5:黄角 
    private int[] Orb_Level = new int[6];

    private const int ORB_MAX_LEVEL = 3;

    [Serializable]
    public struct MagicRanges {
        public GameObject magicRange;
        [NonSerialized] public Vector3 oriScale;
    }

    //炎魔法の範囲
    [SerializeField]
    MagicRanges[] magicRanges;

    

    //スクリプト
    [SerializeField] TrunManager s_TrunManager;

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

        for (int i = 0; i < magicRanges.Length; i++)
            magicRanges[i].oriScale = magicRanges[i].magicRange.transform.localScale;

        //元の色を取得しておく。　グレーアウト用
        for (int i = 0; i < orb_Gauge_Color.Length; i++)
        {
            orb_Gauge_Color[i] = orb_Gage[i].transform.GetChild(1).GetComponent<Image>().color;
            orb_Gage[i].transform.GetChild(1).GetComponent<Image>().color = grayOutColor;
        }
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

        ChangeMagicRange();
    }
    public void starRedChage()
    {
        //starRed.value += 1;
        if(++Orb_Level[0] > ORB_MAX_LEVEL)
        {
            Orb_Level[0] = ORB_MAX_LEVEL;
        }
    }
    public void starLightBlueChage()
    {
        if (++Orb_Level[1] > ORB_MAX_LEVEL)
        {
            Orb_Level[1] = ORB_MAX_LEVEL;
        }
    }
    public void starYellowChage()
    {
        if (++Orb_Level[2] > ORB_MAX_LEVEL)
        {
            Orb_Level[2] = ORB_MAX_LEVEL;
        }
    }
    public void pentagonRedChage()
    {
        if (++Orb_Level[3] > ORB_MAX_LEVEL)
        {
            Orb_Level[3] = ORB_MAX_LEVEL;
        }
    }
    public void pentagonLightBlueChage()
    {
        if (++Orb_Level[4] > ORB_MAX_LEVEL)
        {
            Orb_Level[4] = ORB_MAX_LEVEL;
        }
    }
    public void pentagonYellowChage()
    {
        if (++Orb_Level[5] > ORB_MAX_LEVEL)
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

    void ChangeMagicRange() {
        //魔法の範囲を設定するやし 今は炎だけ
        int[] lev = Get_Orb_Level();

        {
            if (lev[0] >= 0) lev[0] = 2 * lev[0] - 1;
            
            if (lev[3] >= 0) lev[3] = 2 * lev[0] - 1;
        }

        magicRanges[0].magicRange.transform.localScale = new Vector3(lev[0], 1, lev[0]);
        magicRanges[3].magicRange.transform.localScale = new Vector3(lev[1], 1, 1);
    }

    //使ったオーブのレベルを0にし、見た目をグレーアウトしたい関数
    public void UseOrb(int num) {

        //オーブのレベルを0にする
        Orb_Level[num] = 0;

        //オーブ
        orb_Gage[num].transform.GetChild(1).GetComponent<Image>().color = grayOutColor;

        //選択範囲を消す
        magicRanges[num].magicRange.SetActive(false);
        

        if (!OrbCheckExsistens())
            s_TrunManager.SetTrunPhase(TrunManager.TrunPhase.Enemy);
    }

    //オーブの存在が確認できるかの関数
    //確認出来たらtrueを返す
    bool OrbCheckExsistens() {

        int totalNum = 0;
        foreach (var n in Orb_Level)
            totalNum += n;

        return (totalNum == 0 ? false : true);
    }
 }
