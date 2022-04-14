using System.Collections;
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

            //指定方向で近い敵を探して
            foreach (var e in enemies)
            {
                int xBuf = e.GetComponent<Enemy>().X;
                int yBuf = e.GetComponent<Enemy>().Y;

                //今いる位置からの差分を求める
                int difY = Mathf.Abs(yBuf - nowSelY);

                //見てるx軸にその敵が居たら
                if (xLine == xBuf && oldDifY > difY) {

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

            //指定方向で近い敵を探して
            foreach (var e in enemies)
            {
                int xBuf = e.GetComponent<Enemy>().X;
                int yBuf = e.GetComponent<Enemy>().Y;

                //今いる位置からの差分を求める
                int difX = Mathf.Abs(xBuf - nowSelX);

                //見てるx軸にその敵が居たら
                if (yLine == yBuf && oldDifX > difX)
                {

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

        //五芒星雷魔法 と 五角形雷魔法 以外
        if (typ != 2 && typ != 4)
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
                    if (typ == 4) selectsNumLimit = lev + 1;

                    selectTargets = new GameObject[selectsNumLimit];
                }

                //被って無ければその場所をマークしカウント
                //foreach(var g in selectTargets)
                //    if(g )
                


                //カウント
                selectsNum++;

                //上限に達したら
                if (selectsNum >= selectsNumLimit) {

                    //魔法を放つ


                    selectsNum = 0;
                }
            }
        }
    }

}
