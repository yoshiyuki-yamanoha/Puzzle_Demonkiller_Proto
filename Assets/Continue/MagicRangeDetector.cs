using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicRangeDetector : TrunManager
{
    public enum MagicType
    {
        FireStar = 0,
        IceStar,
        ThunderStar,
        FirePenta,
        IcePenta,
        ThunderPenta
    }


    //選択中魔法の範囲を参照する用
    [SerializeField] OrbGage s_OrbGage;

    //現在選択している魔法を参照する用
    [SerializeField] SelectUseOrb s_SelectUseOrb;

    //現在のTurnを見る用
    [SerializeField] TrunManager s_TrunManager;

    //現在の位置を取得する用
    [SerializeField] MagicMassSelecter s_MagicMassSelecter;

    //指定したマスのオブジェクトを取得する用
    [SerializeField] MapMass s_MapMass;



    //オーブのレベルを格納する用のプライベートint配列と種類用の変数とフラグ
   //int[] orbLevelKeeper;
    //bool getLevelFlag = true;
    MagicType magicType;
    MagicType oldMagicType;
    int magicLevel;
    int oldTyp, oldLev;
    GameObject[,] magicRange;

    private void FixedUpdate()
    {
        //oldMagicType = magicType;

        //現在選択しているオーブ(魔法の種類)を取得
        GetOrInfobTick();

        //魔法の種類によって魔法の範囲を変える
        ChangeMagicRange();
        

    }

    //現在選択しているオーブの魔法
    void GetOrInfobTick() {

        //Getしてくるよう変数たち
        int lev, typ;

        //選択中のオーブのレベルと種類を取ってくる
        (typ,lev) = s_SelectUseOrb.GetNowSelectOrb();

        if(oldTyp != typ)
            magicType = (MagicType)Enum.ToObject(typeof(MagicType), typ);

        if(oldLev != lev)
            magicLevel = lev;

        //毎フレームキャストするのを防ぐよう
        oldTyp = typ;
        oldLev = lev;

    }

    //魔法を撃つターンになった瞬間にセレクタを中心に魔法の範囲を求める。
    void ChangeMagicRange() {

        //現在のセレクターの位置を取得 (添え字)
        int selX=0, selY=0;
        /*SelectSquares から massHとmassVを取ってきて格納する*/
        (selX, selY) = s_MagicMassSelecter.GetCurrentSelecerPos();


        switch (magicType) {
            case MagicType.FireStar:     //五芒星　炎 (選択マスを中心に n^2)

                //左上
                Vector2Int upperLeft = new Vector2Int();
                upperLeft.x = selX - magicLevel;
                upperLeft.y = selY - magicLevel;

                //右下
                Vector2Int lowerRight = new Vector2Int();
                lowerRight.x = selX + magicLevel;
                lowerRight.y = selY + magicLevel;

                
                int num = magicLevel * 2 + 1;
                magicRange = new GameObject[num,num];
                for (int i = 0; i < num; i++) {
                    magicRange[i / num, i % num] = s_MapMass.GetGameObjectOfSpecifiedMass(i % num, i / num);
                }


                break;
            case MagicType.IceStar:      //五芒星　氷 (選択マスを中心にステージ縦(レベル)列)　ボスにも当たる



                break;
            case MagicType.ThunderStar:  //五芒星　雷 (レベル+1体選択できる　選択した順番に雷で攻撃)



                break;
            case MagicType.FirePenta:    //五角形　炎 (地雷になる予定 )



                break;
            case MagicType.IcePenta:     //五角形　氷 (選択マスを中心に横に広がる  )



                break;
            case MagicType.ThunderPenta: //五角形　雷 (選択マスにタレットを設置。　タレットの感知範囲はレベルで変わる)



                break;
        }

    }

    //オーブのレベルを取ってくる
    //void GetOrbLevelSet() {

    //    //取ってくる
    //    var levels = s_OrbGage.Get_Orb_Level();

    //    //領域を確保
    //    orbLevelKeeper = new int[levels.Length];

    //    //領域にコピー
    //    for (int i = 0; i < levels.Length; i++)
    //        orbLevelKeeper[i] = levels[i];

    //    //連続で行われないようにフラグを立てる
    //    getLevelFlag = false;
    //}

}
