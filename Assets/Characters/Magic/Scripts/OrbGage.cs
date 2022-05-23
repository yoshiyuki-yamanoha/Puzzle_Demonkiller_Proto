using System.Collections;
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

    //オーブレベルゲージ用のスライダー
    [SerializeField] Slider[] orb_Gage = new Slider[6];


    //魔方陣の線の形
    public bool starflag;
    public bool pentflag;

    //魔方陣の色
    public int colorflag;//0なし1赤2水色3黄色

    //各オーブのレベル0~3  0:赤星 1:青星 2:黄星 3:赤角 4:青角 5:黄角 
    private int[] Orb_Level = new int[6];

    private const int ORB_MAX_LEVEL = 30;

    //オーブグレーアウト用のやつ
    [SerializeField] GameObject[] grayOutMask;

    //魔法の範囲
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
    [SerializeField] SelectUseOrb s_SelectUseOrb;

    [SerializeField] MagicMassSelecter s_MagicMassSelecter;


    MagicAttackCamera mACame;

    //魔法専用シーン
    [SerializeField] bool magicOnlyMode;

    // Start is called before the first frame update
    void Start()
    {
        //オーブのスライダーゲージの初期化
        if (!magicOnlyMode)
        {

            starRed.value = 0;
            starLightBlue.value = 0;
            starYellow.value = 0;
            pentagonRed.value = 0;
            pentagonLightBlue.value = 0;
            pentagonYellow.value = 0;

            for (int i = 0; i < magicRanges.Length; i++)
                magicRanges[i].oriScale = magicRanges[i].magicRange.transform.localScale;

            magicRanges[1].magicRange.SetActive(false);
            magicRanges[2].magicRange.SetActive(false);
            magicRanges[4].magicRange.SetActive(false);
            magicRanges[5].magicRange.SetActive(false);
        }
        mACame = GameObject.Find("GameObject").GetComponent<MagicAttackCamera>();
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
        DelayPhase();

        ChangeMagicRange();

        //オーブをグレーアウト
        for (int i = 0; i < grayOutMask.Length; i++) {
            if (Orb_Level[i] == 0)
            {
                grayOutMask[i].SetActive(true);
            }
            else if (Orb_Level[i] > 0) grayOutMask[i].SetActive(false);

        }

        int num = 0, num2 = 0;
        (num, num2) = s_SelectUseOrb.GetNowSelectOrb();

        //魔法陣を消す
        if (!magicOnlyMode)
        {
            for (int i = 0; i < 6; i++)
            {
                if (num == i)
                    magicRanges[i].magicRange.SetActive(true);
                else
                    magicRanges[i].magicRange.SetActive(false);
            }
        }

    }
    public void starRedChage()
    {
        //starRed.value += 1;

        if (++Orb_Level[0] > ORB_MAX_LEVEL)
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

        if (type == (int)PointControl.MAGIC_MODE.STAR)//星型なら
        {
            starflag = true;
        }
        if (type == (int)PointControl.MAGIC_MODE.PENTAGON)//五角形なら
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

    int[] levv;

    public int[] GetMagicRanges() {

        return levv;
    }

    void ChangeMagicRange() {
        //魔法の範囲を設定するやし 今は炎だけ
        //int[] lev = Get_Orb_Level();

        levv = new int[Orb_Level.Length];
        for (int i = 0; i < Orb_Level.Length; i++)
            levv[i] = Orb_Level[i];

        {
            if (levv[0] > 0) levv[0] = 2 * levv[0] + 1;

            if (levv[3] > 0) levv[3] = 2 * levv[3] - 1;

        }

        if (!magicOnlyMode)
        {
            magicRanges[0].magicRange.transform.localScale = new Vector3(magicRanges[0].oriScale.x * levv[0], 1, magicRanges[0].oriScale.z * levv[0]);
            magicRanges[3].magicRange.transform.localScale = new Vector3(magicRanges[3].oriScale.x * levv[3], 1, 1);
        }
    }

    //使ったオーブのレベルを0にし、見た目をグレーアウトしたい関数

    float delayTime = 0.0f;
    //                     Times { 星炎, 星氷, 星雷, 五炎, 五氷, 五雷,};
    float[] Times = new float[6] { 2.0f, 4.0f, 2.0f, 2.5f, 1.5f, 2.0f, };
    public void UseOrb(int num) {

        if (num == 0)
        {
            //オーブのレベルによってうつす時間を変える
            if (Orb_Level[num] <= 3)
            {
                Times[num] = 2.0f;
            }
            else if (Orb_Level[num] <= 4)
            {
                Times[num] = 3.0f;

            }
            else if (Orb_Level[num] <= 6)
            {
                Times[num] = 4.0f;

            }
            else if (Orb_Level[num] <= 7 )
            {
                Times[num] = 4.0f;

            }
            else
            {
                Times[num] = 4.0f;
            }
        }
        Debug.Log("たいむｓ" + Times[num]);

        mACame.SetShakeTime(Times[num]);

        //オーブのレベルを0にする
        Orb_Level[num] = 0;


        //選択範囲を消す
        //magicRanges[num].magicRange.SetActive(false);
        //numが何見てるのか見たかった
        //if(num == 0 || num == 3)
        //{
        //    Debug.Log("UseOrb num " + num+" 炎 星:0 五:3");
        //}
        //if (num == 1 || num == 4)
        //{
        //    Debug.Log("UseOrb num " + num + " 氷 星:1 五:4");
        //}
        //if (num == 2 || num == 5)
        //{
        //    Debug.Log("UseOrb num " + num + " 雷 星:2 五:5");
        //}
        //Debug.Log("UseOrb num "+num);

        if (!OrbCheckExsistens())
        {
            delayTime = Times[num];
            s_MagicMassSelecter.BeDefaultMatOldChangeedMasses();
            //if(num == 0)
            //{
            //    HandOverPhase();
            //}
            //else
            //{
            //  delayTime = Times[num - 1];
            //}
            //s_TrunManager.SetTrunPhase(TrunManager.TrunPhase.Enemy);
            //for (int i = 0; i < orb_Gage.Length; i++)
            //    orb_Gage[i].value = 0;

        }
        else
        {
            delayTime = Times[num];
            orbChangeflag = false;
            //s_SelectUseOrb.ChangeUseOrb(1);
        }

        //s_SelectUseOrb.ChangeUseOrb(1);
    }
    public bool orbChangeflag = true;
    void DelayPhase()
    {
        if(delayTime > 0)
        {
            delayTime -= Time.deltaTime;

            //if (Input.GetButtonDown("Cont_L1") || Input.GetButtonDown("Cont_R1"))//魔法の切り替えを有効化する(魔法攻撃中の処理)
            //{
            //    orbChangeflag = true;
            //}
            if (delayTime <= 0)
            {
                if (!OrbCheckExsistens())
                {
                    HandOverPhase();
                }
                else
                {
                    if (orbChangeflag == false)
                    {
                        s_SelectUseOrb.ChangeUseOrb(1);
                        orbChangeflag = true;
                    }
                }
                delayTime = -1.0f;
            }
        }
    }

    void HandOverPhase()
    {
        s_TrunManager.SetTrunPhase(TrunManager.TrunPhase.Enemy);
        for (int i = 0; i < orb_Gage.Length; i++)
            orb_Gage[i].value = 0;
    }

    int totalNum;

    //オーブの存在が確認できるかの関数
    //確認出来たらtrueを返す
    public bool OrbCheckExsistens() {

        EnemyBase eb = new EnemyBase();
        List<GameObject> enemies = eb.GetEnemyList();

        //エネミーの数が０で
        if (enemies.Count == 0)
        {
            //五角形の炎が使用不可能の場合true
            if (Orb_Level[3] == 0 && Orb_Level[5] == 0)
            {
                return true;
            }
        }

        totalNum = 0;
        foreach (var n in Orb_Level)
            totalNum += n;

        return (totalNum == 0 ? false : true);
    }

    public void OrbLevelZero(int num) {
        Orb_Level[num] = 0;
    }
 }
