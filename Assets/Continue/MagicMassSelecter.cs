﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMassSelecter : MonoBehaviour
{
    [SerializeField] MapMass s_MapMass;
    [SerializeField] MagicRangeDetector s_MagicRangeDetector;
    [SerializeField] TrunManager s_TrunManager;
    [SerializeField] PlayerController s_PlayerContoller;

    int nowSelX = 5;
    int nowSelY = 5;

    const int rit_Interval = 5;
    [SerializeField] int moveInterval = rit_Interval;

    [SerializeField] Material defMat;
    [SerializeField] Material selMat;
    [SerializeField] Material[] eleMats = new Material[3];

    private void Start()
    {
        s_MapMass.SetMagicMassSelector(nowSelX, nowSelY);
    }

    private void FixedUpdate()
    {
        if (s_TrunManager.trunphase == TrunManager.TrunPhase.MagicAttack)
        {
            GetMassInfos();

            SubMoveInterval();

            MoveSelecter();

            ActivateMagic();
        }
    }

    ///マス目全体の情報を取ってくる (敵の位置とか)
    void GetMassInfos() {
        var masses = s_MapMass.Map;
    }

    //移動インターバルを減らす
    void SubMoveInterval() {
        if (moveInterval != 0) {
            moveInterval--;

            if (moveInterval <= 0) {
                moveInterval = 0;
            }
        }
    }

    //セレクターの移動をする
    void MoveSelecter() {

        //現在の座標を取得
        (nowSelX, nowSelY) = s_MapMass.GetMAgicMassSelector();

        float hori = Input.GetAxis("Horizontal");
        float vert = -Input.GetAxis("Vertical");

        int oldSelX = nowSelX;
        int oldSelY = nowSelY;

        //インターバルが経過しきったら
        if (moveInterval == 0)
        {
            //移動
            if (hori >= 0.5f && nowSelX < 10) nowSelX++;
            if (hori <= -0.5f && nowSelX > 0) nowSelX--;
            if (vert >= 0.5f && nowSelY < 14) nowSelY++;
            if (vert <= -0.5f && nowSelY > 0) nowSelY--;
        }

        if (s_MagicRangeDetector.MagicRangeOverhangStageMap(nowSelX, nowSelY) == false){
            nowSelX = oldSelX;
            nowSelY = oldSelY;
        }

        //セレクターが移動されたら
        if(oldSelX != nowSelX || oldSelY != nowSelY){

            //旧選択マスのマテリアルをデフォに戻す
            //oldSelectedMass.GetComponent<Renderer>().material = defMat;
            BeDefaultMatOldChangeedMasses();

            //新選択マスのマテリアルを変える
            //ChangeMatSpecifiedMass(nowSelX, nowSelY, 0);
            s_MagicRangeDetector.ChangeMagicRange();

            //変更後のマス座標を渡す
            PassSelecterPos();

            //インターバルをリセット
            moveInterval = rit_Interval;
        }


    }

    //マテリアルを変えたオブジェクトを全て戻す
    public void BeDefaultMatOldChangeedMasses() {

        GameObject[] cMasses = GameObject.FindGameObjectsWithTag("ChangedMass");

        foreach (GameObject g in cMasses){
            g.GetComponent<Renderer>().material = defMat;
            g.tag = "Untagged";
        }

    }

    //指定したマスのマテリアルを指定のものに変える
    public void ChangeMatSpecifiedMass(GameObject obj,int colorType=0) {

        //GameObject speci = s_MapMass.GetGameObjectOfSpecifiedMass(x, y);

        obj.GetComponent<Renderer>().material = eleMats[colorType];

        obj.tag = "ChangedMass";
    }

    //セレクターの位置情報(添え字)をMapMassに渡す
    void PassSelecterPos() { 
        s_MapMass.SetMagicMassSelector(nowSelX,nowSelY);
    }

    //魔法セレクターが現在いるマスの添え字を返す
    public (int, int) GetCurrentSelecerPos() {
        return (nowSelX, nowSelY);
    }

    //Aボタンで魔法を撃つ
    void ActivateMagic() {

        GameObject[] attackRange = s_MagicRangeDetector.GetCurrentAttackRange();

        if (Input.GetButtonDown("Fire1")) {
            foreach (var g in attackRange)
                s_PlayerContoller.ShotMagic(g, 0, 1);
        }
    }

}
