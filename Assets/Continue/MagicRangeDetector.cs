using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicRangeDetector : TrunManager
{
    const int stageWidth = 20;
    const int stageHeight = 20;

    public enum MagicType
    {
        FireStar = 0,
        IceStar,
        ThunderStar,
        FirePenta,
        IcePenta,
        ThunderPenta
    }

    struct MagicMassStatus {
        public int x;
        public int y;
        public GameObject obj;
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

    private void FixedUpdate()
    {
        //oldMagicType = magicType;

        //現在選択しているオーブ(魔法の種類)を取得
        GetOrInfobTick();

        //魔法の種類によって魔法の範囲を変える

        //ChangeMagicRange();
        if (s_TrunManager.trunphase != TrunManager.TrunPhase.MagicAttack) {
            s_MagicMassSelecter.BeDefaultMatOldChangeedMasses();
        }


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

    GameObject[] retRange;

    //魔法を撃つターンになった瞬間にセレクタを中心に魔法の範囲を求める。
    public void ChangeMagicRange() {

        s_MagicMassSelecter.BeDefaultMatOldChangeedMasses();

        //現在のセレクターの位置を取得 (添え字)
        int selX=0, selY=0;
        /*SelectSquares から massHとmassVを取ってきて格納する*/
        (selX, selY) = s_MagicMassSelecter.GetCurrentSelecerPos();

        MagicMassStatus[,] magicRange = null;

        //範囲の最初
        Vector2Int rangeStart;
        int num;

        switch (magicType) {
            case MagicType.FireStar:     //五芒星　炎 (選択マスを中心に n^2)
            case MagicType.ThunderPenta:

                //左上
                rangeStart = new Vector2Int();
                rangeStart.x = selX - magicLevel;
                rangeStart.y = selY - magicLevel;
                
                num = magicLevel * 2 + 1;
                magicRange = new MagicMassStatus[num,num];

                for (int i = 0; i < num; i++) {
                    for (int j = 0; j < num; j++) {
                        magicRange[i, j].y = rangeStart.y + i;
                        magicRange[i, j].x = rangeStart.x + j;
                        magicRange[i, j].obj = s_MapMass.GetGameObjectOfSpecifiedMass(rangeStart.x + j, rangeStart.y + i);
                    }
                }


                break;
            case MagicType.IceStar:      //五芒星　氷 (選択マスを中心にステージ縦(レベル)列)　ボスにも当たる

                rangeStart = new Vector2Int();
                rangeStart.x = magicLevel < 3 ? selX : selX - 1;
                rangeStart.y = 0;

                magicRange = new MagicMassStatus[stageHeight, magicLevel];

                for (int i = 0; i < stageHeight; i++)
                {
                    for (int j = 0; j < magicLevel; j++)
                    {
                        magicRange[i, j].y = i;
                        magicRange[i, j].x = rangeStart.x + j;
                        magicRange[i, j].obj = s_MapMass.GetGameObjectOfSpecifiedMass(rangeStart.x + j, i);
                    }
                }


                break;
            case MagicType.ThunderStar:  //五芒星　雷 (レベル+1体選択できる　選択した順番に雷で攻撃)

                magicRange = new MagicMassStatus[1, 1];
                magicRange[0,0].y = selY;
                magicRange[0,0].x = selX;
                magicRange[0, 0].obj = s_MapMass.GetGameObjectOfSpecifiedMass(selX, selY);

                break;

            case MagicType.FirePenta:    //五角形　炎 (地雷になる予定 個数がふえーる)

                rangeStart = new Vector2Int();
                rangeStart.x = selX - (magicLevel - 1);
                rangeStart.y = selY;

                num = 1 + (magicLevel - 1) * 2;

                magicRange = new MagicMassStatus[1, num];

                for (int j = 0; j < num; j++)
                {
                    magicRange[0, j].y = rangeStart.y;
                    magicRange[0, j].x = rangeStart.x + j;
                    magicRange[0, j].obj = s_MapMass.GetGameObjectOfSpecifiedMass(rangeStart.x + j, rangeStart.y);
                }

                break;

            case MagicType.IcePenta:     //五角形　氷 (選択マスを中心に横に広がる  )

                if (magicLevel < 3)
                {
                    rangeStart = new Vector2Int();
                    rangeStart.x = selX - magicLevel;
                    rangeStart.y = selY;

                    num = magicLevel * 2 + 1;
                    magicRange = new MagicMassStatus[1, num];

                    for (int i = 0; i < 1; i++)
                    {
                        for (int j = 0; j < num; j++)
                        {
                            magicRange[i, j].y = rangeStart.y;
                            magicRange[i, j].x = rangeStart.x + j;
                            magicRange[i, j].obj = s_MapMass.GetGameObjectOfSpecifiedMass(rangeStart.x + j, rangeStart.y);
                        }
                    }
                }
                else {

                    rangeStart = new Vector2Int();
                    rangeStart.x = selX - 1;
                    rangeStart.y = selY - 1;

                    num = magicLevel;
                    magicRange = new MagicMassStatus[num, num];

                    for (int i = 0; i < num; i++)
                    {
                        for (int j = 0; j < num; j++)
                        {
                            magicRange[i, j].y = rangeStart.y + i;
                            magicRange[i, j].x = rangeStart.x + j;
                            magicRange[i, j].obj = s_MapMass.GetGameObjectOfSpecifiedMass(rangeStart.x + j, rangeStart.y + i);
                        }
                    }

                }

                break;
        }

        if (magicRange == null) return;

        retRange = new GameObject[magicRange.Length];
        int n = 0;

        foreach (var g in magicRange) {
            s_MagicMassSelecter.ChangeMatSpecifiedMass(g.obj, (int)magicType % 3);
            retRange[n++] = g.obj;
        }
    }

    public GameObject[] GetCurrentAttackRange() {

        return retRange;
    }

    //魔法の範囲がステージ外に出ないようにするゴリラ
    public bool MagicRangeOverhangStageMap(int x,int y) {

        //魔法の種類によって行ける範囲を変える
        switch (magicType) {
            case MagicType.FireStar:     //五芒星　炎 (選択マスを中心に n^2)
            case MagicType.ThunderPenta: //五角形　雷 (選択マスにタレットを設置。　タレットの感知範囲はレベルで変わる)

                if ((x - magicLevel) < 0 || (x + magicLevel > stageWidth - 1)) return false;
                if ((y - magicLevel) < 0 || (y + magicLevel > stageHeight - 1)) return false;

                break;
            case MagicType.IceStar:      //五芒星　氷 (選択マスを中心にステージ縦(レベル)列)　ボスにも当たる

                if (magicLevel < 3)
                {
                    if (x < 0 || (x + (magicLevel - 1) > stageWidth - 1)) return false;
                }
                else
                    if ((x - 1) < 0 || (x + 1 > stageWidth - 1)) return false;

                break;
            case MagicType.ThunderStar:  //五芒星　雷 (レベル+1体選択できる　選択した順番に雷で攻撃)

                break;

            case MagicType.FirePenta:    //五角形　炎 (地雷になる予定 )

                if (x - (magicLevel - 1) < 0 || x + (magicLevel - 1) >= stageWidth) return false;
                if (y < 0 || y >= stageHeight) return false;

                break;

            case MagicType.IcePenta:     //五角形　氷 (選択マスを中心に横に広がる  )

                if (magicLevel < 3)
                {
                    if ((x - magicLevel) < 0 || (x + magicLevel > stageWidth - 1)) return false;
                    if (y < 0 || y> stageHeight - 1) return false;
                }
                else {
                    if ((x - 1) < 0 || (x + 1 > stageWidth - 1)) return false;
                    if ((y - 1) < 0 || (y + 1 > stageHeight - 1)) return false;
                }

                break;
        }

        //問題なし
        return true;
    }

    public (int, int) GetOrbInfo() {

        return ((int)magicType, magicLevel);
    }

}
