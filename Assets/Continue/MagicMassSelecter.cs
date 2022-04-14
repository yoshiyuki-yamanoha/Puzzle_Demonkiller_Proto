﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMassSelecter : MonoBehaviour
{
    //スクリプトたち
    [SerializeField] MapMass s_MapMass;
    [SerializeField] MagicRangeDetector s_MagicRangeDetector;
    [SerializeField] TrunManager s_TrunManager;
    [SerializeField] PlayerController s_PlayerContoller;
    [SerializeField] SelectUseOrb s_SelectUseOrb;

    //移動用
    int nowSelX = 5;
    int nowSelY = 5;

    const int rit_Interval = 5;
    [SerializeField] int moveInterval = rit_Interval;

    //マテリアル
    [SerializeField] Material defMat;
    [SerializeField] Material selMat;
    [SerializeField] Material[] eleMats = new Material[3];

    //選ぶ方法 (0:マス  1:敵)
    int selectType = 0;
    Enemy currentSelectEnemy = null;
    GameObject currentSelectEnemyObj = null;

    //複数選択用
    int selectsNum = 0;
    int selectsNumLimit = 0;
    GameObject[] selectTargets;
    int bottomCost = 99;


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

            if(selectsNum > 0)
                CalcBottomCost();

            if (selectType == 0) MoveSelecter();
            if (selectType == 1) MoveSelecterEnemy();

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

    //セレクターの対象を切り替える
    public void SwitchSelectType(int num = 99) {
        if (num == 99) selectType = 1 - selectType;
        else selectType = num;

        //敵選択にした場合
        if (selectType == 1) {
            //敵を全員検索
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            //一番近い敵を検索
            int nearX = 99;
            int nearY = 99;
            int difTortal = 99;

            foreach (var e in enemies)
            {
                int xBuf = e.GetComponent<Enemy>().X;
                int yBuf = e.GetComponent<Enemy>().Y;

                //今いる位置からの差分を求める
                int difX = Mathf.Abs(xBuf - nowSelX);
                int difY = Mathf.Abs(yBuf - nowSelY);
                int oldDifTotal = difTortal;
                difTortal = difX + difY;


                if (difTortal <= oldDifTotal)
                {

                    nearX = xBuf;
                    nearY = yBuf;
                    currentSelectEnemy = e.GetComponent<Enemy>();
                }
            }

            //座標の反映
            nowSelX = nearX;
            nowSelY = nearY;

            //新選択マスのマテリアルを変える
            s_MagicRangeDetector.ChangeMagicRange();

            //変更後のマス座標を渡す
            PassSelecterPos();
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
            if (hori >= 0.5f) nowSelX++;
            if (hori <= -0.5f) nowSelX--;
            if (vert >= 0.5f) nowSelY++;
            if (vert <= -0.5f) nowSelY--;
        }

        //範囲をはみ出るなら移動できない様にする。
        if (s_MagicRangeDetector.MagicRangeOverhangStageMap(nowSelX, nowSelY) == false){
            nowSelX = oldSelX;
            nowSelY = oldSelY;
        }

        //セレクターが移動されたら
        if(oldSelX != nowSelX || oldSelY != nowSelY){

            //新選択マスのマテリアルを変える
            s_MagicRangeDetector.ChangeMagicRange();

            //変更後のマス座標を渡す
            PassSelecterPos();

            //インターバルをリセット
            moveInterval = rit_Interval;
        }


    }

    //敵がいるセレクターを移動する
    void MoveSelecterEnemy() {

        //現在選んでいる敵の座標を取得
        int x = currentSelectEnemy.X;
        int y = currentSelectEnemy.Y;

        //コントローラーのあれ
        float hori = Input.GetAxis("Horizontal");
        float vert = -Input.GetAxis("Vertical");

        //
        int oldX = x;
        int oldY = y;

        //コントローラーで敵を変える
        //インターバルが経過しきったら
        if (moveInterval == 0)
        {
            //移動
            if (hori >= 0.5f)
                (x, y) = GetNearEnemyX(1);

            if (hori <= -0.5f)
                (x, y) = GetNearEnemyX(-1);

            if (vert >= 0.5f)
                (x, y) = GetNearEnemyY(1);

            if (vert <= -0.5f)
                (x, y) = GetNearEnemyY(-1);
        }

        //セレクターが移動されたら
        if (oldX != x || oldY != y)
        {
            //座標の反映
            nowSelX = x;
            nowSelY = y;

            //新選択マスのマテリアルを変える
            s_MagicRangeDetector.ChangeMagicRange();

            //変更後のマス座標を渡す
            PassSelecterPos();

            //インターバルをリセット
            moveInterval = rit_Interval;
        }

    }

    //x軸方向で一番近い敵を探して座標を返す (vecNum:1 右側 vecNum:-1 左側)
    (int, int) GetNearEnemyX(int vecNum) {

        //敵を全員検索
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int nearX = 99;
        int nearY = 99;

        int xLine = nowSelX + vecNum;

        var oldSelectedEnemy = currentSelectEnemy;
        currentSelectEnemy = null;

        while (currentSelectEnemy == null) {

            int oldDifY = 99;

            //コストを求める
            
            int osepX=0, osepY=0;
            if(selectsNum > 0)
                (osepX, osepY) = GetOldSelectedEnemyPos();
            

            //指定方向で近い敵を探して
            foreach (var e in enemies)
            {
                int xBuf = e.GetComponent<Enemy>().X;
                int yBuf = e.GetComponent<Enemy>().Y;

                //今いる位置からの差分を求める
                int difY = Mathf.Abs(yBuf - nowSelY);

                Enemy s_Enemy = e.GetComponent<Enemy>();
                int xCost = Mathf.Abs(osepX - s_Enemy.X);
                int yCost = Mathf.Abs(osepY - s_Enemy.Y);
                int eneCost = xCost + yCost;

                //見てるx軸にその敵が居たら
                if (xLine == xBuf && oldDifY > difY) {

                    if (selectsNum > 0 && eneCost != bottomCost) continue;
                    nearX = xBuf;
                    nearY = yBuf;
                    oldDifY = difY;
                    currentSelectEnemy = e.GetComponent<Enemy>();
                    currentSelectEnemyObj = e;
                }
            }

            //敵が居なかったら次の行を見る
            xLine += vecNum;

            //ステージ範囲外に出たら戻る
            if (xLine < -1 || xLine > 11)
            {
                currentSelectEnemy = oldSelectedEnemy;
                return (nowSelX, nowSelY);
            }
        }

        return (nearX, nearY);
    }

    //y軸方向で一番近い敵を探して座標を返す (vecNum:1 奥側 vecNum:-1 手前側)
    (int, int) GetNearEnemyY(int vecNum) {

        //敵を全員検索
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int nearX = 99;
        int nearY = 99;

        int yLine = nowSelY + vecNum;

        var oldSelectedEnemy = currentSelectEnemy;
        currentSelectEnemy = null;

        while (currentSelectEnemy == null)
        {

            int oldDifX = 99;

            //コストを求める
            int osepX=0, osepY=0;
            if (selectsNum > 0)
                (osepX, osepY) = GetOldSelectedEnemyPos();

            //指定方向で近い敵を探して
            foreach (var e in enemies)
            {
                int xBuf = e.GetComponent<Enemy>().X;
                int yBuf = e.GetComponent<Enemy>().Y;

                //今いる位置からの差分を求める
                int difX = Mathf.Abs(xBuf - nowSelX);

                Enemy s_Enemy = e.GetComponent<Enemy>();
                int xCost = Mathf.Abs(osepX - s_Enemy.X);
                int yCost = Mathf.Abs(osepY - s_Enemy.Y);
                int eneCost = xCost + yCost;

                //見てるx軸にその敵が居たら
                if (yLine == yBuf && oldDifX > difX)
                {
                    if (selectsNum > 0 && eneCost != bottomCost) continue;
                    nearX = xBuf;
                    nearY = yBuf;
                    oldDifX = difX;
                    currentSelectEnemy = e.GetComponent<Enemy>();
                    currentSelectEnemyObj = e;
                }
            }

            //敵が居なかったら次の行を見る
            yLine += vecNum;

            //ステージ範囲外に出たら戻る
            if (yLine < -1 || yLine > 15)
            {
                currentSelectEnemy = oldSelectedEnemy;
                return (nowSelX, nowSelY);
            }
        }

        return (nearX, nearY);
    }

    //今いるマスにいる敵GameObjectを返す
    GameObject GetEnemyObjectOnCurrentSelectMass() {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var e in enemies)
            if (e.GetComponent<Enemy>().X == nowSelX && e.GetComponent<Enemy>().Y == nowSelY) return e;

        return null;
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

        int typ, lev;
        (typ, lev) = s_MagicRangeDetector.GetOrbInfo();

        GameObject[] attackRange = s_MagicRangeDetector.GetCurrentAttackRange();

        //五芒星雷魔法 と 五角形炎魔法 以外
        if (typ != 2 && typ != 3)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                foreach (var g in attackRange)
                    s_PlayerContoller.ShotMagic(g, typ, lev);
            }
        }
        else {  //複数設置型
            if (Input.GetButtonDown("Fire1"))
            {
                //一番最初に選択した時
                if (selectsNum == 0) {

                    //選択上限
                    if (typ == 2) selectsNumLimit = lev;
                    if (typ == 3) selectsNumLimit = lev + 1;

                    selectTargets = new GameObject[selectsNumLimit];
                }

                //現在選択中のエネミー
                if(typ == 2)
                    selectTargets[selectsNum] = GetEnemyObjectOnCurrentSelectMass();
                if (typ == 3)
                    selectTargets[selectsNum] = s_MapMass.GetGameObjectOfSpecifiedMass(nowSelX, nowSelY);

                //カウント
                selectsNum++;

                //上限に達したら
                if (selectsNum >= selectsNumLimit) {

                    //魔法を放つ
                    if(typ == 2)
                        s_PlayerContoller.ShotMagic(selectTargets[0], typ, lev, selectTargets);
                    if(typ == 3)
                        foreach(var g in selectTargets)
                            s_PlayerContoller.ShotMagic(g, typ, lev);

                    selectsNum = 0;
                }
            }
        }
    }

    //複数選択魔法で2体目以降選択時に、最低コストを求める
    void CalcBottomCost() {

        if (selectTargets[0].GetComponent<Enemy>() == null) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject objj = selectTargets[selectsNum - 1];

        foreach (var e in enemies) {

            if (e == objj) continue;

            int oldSelectEnemyX, oldSelectEnemyY;
            (oldSelectEnemyX, oldSelectEnemyY) = GetOldSelectedEnemyPos();

            //敵との距離コスト
            Enemy s_Enemy = e.GetComponent<Enemy>();
            int xCost = Mathf.Abs(oldSelectEnemyX - s_Enemy.X);
            int yCost = Mathf.Abs(oldSelectEnemyY - s_Enemy.Y);
            int eneCost = xCost + yCost;

            //最も近い敵のコストを入れる
            if (eneCost < bottomCost) {
                bottomCost = eneCost;
            }
        }
    }

    //直近選択した敵の座標を返す
    (int, int) GetOldSelectedEnemyPos() {

        //直近選択した敵の座標
        Enemy s_EnemyOld = selectTargets[selectsNum - 1].GetComponent<Enemy>();
        int x = s_EnemyOld.X;
        int y = s_EnemyOld.Y;

        return (x, y);
    }

}
