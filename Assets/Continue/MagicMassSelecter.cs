using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMassSelecter : MonoBehaviour
{
    const int stageWidth = 20;
    const int stageHeight = 20;

    //スクリプトたち
    [SerializeField] MapMass s_MapMass;
    [SerializeField] MagicRangeDetector s_MagicRangeDetector;
    [SerializeField] TrunManager s_TrunManager;
    [SerializeField] PlayerController s_PlayerContoller;
    [SerializeField] SelectUseOrb s_SelectUseOrb;
    [SerializeField] OrbGage s_OrbGage;

    //移動用
    int nowSelX = 9;
    int nowSelY = 10;

    //セレクター移動のインターバル
    const int rit_Interval = 5;
    [SerializeField] int moveInterval = rit_Interval;

    //魔法を撃つインターバル
    const float reload_Interval = 0.5f;
    float nextShotInterval = reload_Interval;

    //マテリアル
    [SerializeField] Material defMat;
    [SerializeField] Material selMat;
    [SerializeField] Material[] eleMats = new Material[3];

    //選ぶ方法 (0:マス  1:敵)
    int selectType = 0;
    Enemy currentSelectEnemy = null;
    FlameSwordMove currentSelectFlameEnemy = null;
    BombEnemy currentSelectBombEnemy = null;
    GameObject currentSelectEnemyObj = null;

    //複数選択用
    int selectsNum = 0;
    int selectsNumLimit = 0;
    GameObject[] selectTargets;
    int bottomCost = 99;

    //SE用
    SEManager sePlay;
    int SETime=30;
    Magichoming magichoming;

    int defSelX, defSelY;//マスの座標用

    [SerializeField] bool isMagic;

    int[,] masses;

    private void Start()
    {
        defSelX = nowSelX;
        defSelY = nowSelY;
        s_MapMass.SetMagicMassSelector(nowSelX, nowSelY);
        Magichoming mh = GetComponent<Magichoming>();
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();//SE用
        s_OrbGage = GameObject.Find("GameObject").GetComponent<OrbGage>();
    }

    private void Update()
    {
        if (isMagic || s_TrunManager.trunphase == TrunManager.TrunPhase.MagicAttack && s_OrbGage.OrbCheckExsistens())
        {
            //マップ全体の情報をとってくう
            GetMassInfos();

            SubMoveInterval();

            if(selectsNum > 0 && bottomCost == 99)
                CalcBottomCost();

            if (selectType == 0) MoveSelecter();
            if (selectType == 1) MoveSelecterEnemy();

            //インターバルを減らす処理
            if (nextShotInterval != 0) {
                nextShotInterval -= Time.deltaTime;

                if (nextShotInterval <= 0f)
                    nextShotInterval = 0;
            }

            if(nextShotInterval == 0)
                ActivateMagic();//魔法を撃つ処理(本物)
        }
        if(s_TrunManager.trunphase == TrunManager.TrunPhase.Puzzle)
        {
            s_MapMass.SetMagicMassSelector(defSelX, defSelY);
        }
    }

    ///マス目全体の情報を取ってくる (敵の位置とか)
    void GetMassInfos() {
        masses = new int[20, 20];
        masses = s_MapMass.Map;
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
                //    int xBuf = e.GetComponent<Enemy>().X;
                //    int yBuf = e.GetComponent<Enemy>().Y;

                int xBuf = 0, yBuf = 0, difX = 0, difY = 0;
                if (e.GetComponent<Enemy>() != null)
                {
                    yBuf = e.GetComponent<Enemy>().Y;
                    xBuf = e.GetComponent<Enemy>().X;

                }
                if (e.GetComponent<BombEnemy>() != null)
                {
                    yBuf = e.GetComponent<BombEnemy>().Y;
                    xBuf = e.GetComponent<BombEnemy>().X;

                }
                if (e.GetComponent<FlameSwordMove>() != null)
                {
                    yBuf = e.GetComponent<FlameSwordMove>().Y;
                    xBuf = e.GetComponent<FlameSwordMove>().X;

                }
                //今いる位置からの差分を求める
                //int difX = Mathf.Abs(xBuf - nowSelX);
                //int difY = Mathf.Abs(yBuf - nowSelY);
                difX = Mathf.Abs(xBuf - nowSelX);
                difY = Mathf.Abs(yBuf - nowSelY);



                int oldDifTotal = difTortal;
                difTortal = difX + difY;


                if (difTortal <= oldDifTotal)
                {

                    nearX = xBuf;
                    nearY = yBuf;
                    if (e.GetComponent<Enemy>() != null)
                    {
                        currentSelectEnemy = e.GetComponent<Enemy>();
                    }else if(e.GetComponent<BombEnemy>() != null)
                    {
                        currentSelectBombEnemy = e.GetComponent<BombEnemy>();
                    }else if(e.GetComponent<FlameSwordMove>() != null)
                    {
                        currentSelectFlameEnemy = e.GetComponent<FlameSwordMove>();
                    }
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

    public int GetSelectType() {
        return selectType;
    }

    //セレクターの移動をする
    void MoveSelecter() {

        //現在の座標を取得
        (nowSelX, nowSelY) = s_MapMass.GetMAgicMassSelector();

        float hori = Input.GetAxis("Horizontal");//スティックの入力を取っている
        float vert = -Input.GetAxis("Vertical");//以下

  

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

            int typ, lev;
            (typ, lev) = s_MagicRangeDetector.GetOrbInfo();

            SETime++; //SETimeを加算していって値が30になるとSEが鳴る

            if (hori!=0 && lev <= 9 || vert!=0 && lev<=9)
            {
                if (SETime>=30) {
                    sePlay.Play("Select3");
                    SETime = 0;//長押ししていた場合等間隔になるように値を0に戻して繰り返す
                }
            }else if (hori != 0 && lev >= 10 || vert != 0 && lev >= 10)
            {
                if (SETime >= 30)
                {
                    sePlay.Play("Select3");
                    SETime = 0;//長押ししていた場合等間隔になるように値を0に戻して繰り返す
                }
            }



            nowSelX = oldSelX;
            nowSelY = oldSelY;
        }
        else
        {
            SETime = 30;//キーを離したとき、他のキーの操作をしたときに、値を初期値に戻して次の範囲外の音が鳴るようにする
        }

        //セレクターが移動されたら
        if(oldSelX != nowSelX || oldSelY != nowSelY){

  
                //sePlay.Play("Select3");///////////////////////////////////////
            

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

        int x = 0;
        int y = 0;
        if (currentSelectEnemy != null)
        {
            //現在選んでいる敵の座標を取得
            x = currentSelectEnemy.X;
            y = currentSelectEnemy.Y;
        }
        else
        if (currentSelectBombEnemy != null)
        {
            //現在選んでいる敵の座標を取得
            x = currentSelectBombEnemy.X;
            y = currentSelectBombEnemy.Y;
        }
        else
        if (currentSelectFlameEnemy != null)
        {
            //現在選んでいる敵の座標を取得
            x = currentSelectFlameEnemy.X;
            y = currentSelectFlameEnemy.Y;
        }

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

            //
            sePlay.Play("MagicCursorSelect");

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

        //var oldSelectedEnemy = currentSelectEnemy;
        Enemy oldSelectedEnemy = currentSelectEnemy;
        currentSelectEnemy = null;
        BombEnemy oldSelectedBombEnemy = currentSelectBombEnemy;
        currentSelectBombEnemy = null;
        FlameSwordMove oldSelectFlameEnemy = currentSelectFlameEnemy;
        currentSelectFlameEnemy = null;

        while (currentSelectEnemy == null && currentSelectBombEnemy == null && currentSelectFlameEnemy == null) {

            int oldDifY = 99;

            //コストを求める
            
            int osepX=0, osepY=0;
            if(selectsNum > 0)
                (osepX, osepY) = GetOldSelectedEnemyPos();
            

            //指定方向で近い敵を探して
            foreach (var e in enemies)
            {
                int xBuf = 0;
                int yBuf = 0;

                Enemy s_Enemy = null;
                BombEnemy s_bombEnemy = null;
                FlameSwordMove s_flameEnemy = null;

                int xCost;
                int yCost;
                int eneCost = 0;

                if (e.GetComponent<Enemy>() != null)
                {
                    xBuf = e.GetComponent<Enemy>().X;
                    yBuf = e.GetComponent<Enemy>().Y;

                    s_Enemy = e.GetComponent<Enemy>();

                    xCost = Mathf.Abs(osepX - s_Enemy.X);
                    yCost = Mathf.Abs(osepY - s_Enemy.Y);
                    eneCost = xCost + yCost;
                }
                else
                if (e.GetComponent<BombEnemy>() != null)
                {
                    xBuf = e.GetComponent<BombEnemy>().X;
                    yBuf = e.GetComponent<BombEnemy>().Y;

                    s_bombEnemy = e.GetComponent<BombEnemy>();
                    xCost = Mathf.Abs(osepX - s_bombEnemy.X);
                    yCost = Mathf.Abs(osepY - s_bombEnemy.Y);
                    eneCost = xCost + yCost;
                }
                else
                if (e.GetComponent<FlameSwordMove>() != null)
                {
                    xBuf = e.GetComponent<FlameSwordMove>().X;
                    yBuf = e.GetComponent<FlameSwordMove>().Y;

                    s_flameEnemy = e.GetComponent<FlameSwordMove>();

                    xCost = Mathf.Abs(osepX - s_flameEnemy.X);
                    yCost = Mathf.Abs(osepY - s_flameEnemy.Y);
                    eneCost = xCost + yCost;
                }

                //int xBuf = e.GetComponent<Enemy>().X;
                //int yBuf = e.GetComponent<Enemy>().Y;

                //今いる位置からの差分を求める
                int difY = Mathf.Abs(yBuf - nowSelY);

                //Enemy s_Enemy = e.GetComponent<Enemy>();
                //int xCost = Mathf.Abs(osepX - s_Enemy.X);
                //int yCost = Mathf.Abs(osepY - s_Enemy.Y);
                //int eneCost = xCost + yCost;

                //見てるx軸にその敵が居たら
                if (xLine == xBuf && oldDifY > difY) {

                    if (selectsNum > 0 && eneCost != bottomCost) continue;
                    nearX = xBuf;
                    nearY = yBuf;
                    oldDifY = difY;
                    //currentSelectEnemy = e.GetComponent<Enemy>();
                    if (e.GetComponent<Enemy>() != null)
                    {
                        currentSelectEnemy = e.GetComponent<Enemy>();
                    }else if(e.GetComponent<BombEnemy>() != null)
                    {
                        currentSelectBombEnemy = e.GetComponent<BombEnemy>();
                    }else if(e.GetComponent<FlameSwordMove>() != null)
                    {
                        currentSelectFlameEnemy = e.GetComponent<FlameSwordMove>();
                    }
                    currentSelectEnemyObj = e;
                }
            }

            //敵が居なかったら次の行を見る
            xLine += vecNum;

            //ステージ範囲外に出たら戻る
            if (xLine < -1 || xLine > stageWidth)
            {
                currentSelectEnemy = oldSelectedEnemy;
                currentSelectBombEnemy = oldSelectedBombEnemy;
                currentSelectFlameEnemy = oldSelectFlameEnemy;
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
                int xBuf = 0;
                int yBuf = 0;

                Enemy s_Enemy = null;
                BombEnemy s_bombEnemy = null;
                FlameSwordMove s_flameEnemy = null;

                int xCost;
                int yCost;
                int eneCost = 0;

                if (e.GetComponent<Enemy>() != null)
                {
                    xBuf = e.GetComponent<Enemy>().X;
                    yBuf = e.GetComponent<Enemy>().Y;

                    s_Enemy = e.GetComponent<Enemy>();

                    xCost = Mathf.Abs(osepX - s_Enemy.X);
                    yCost = Mathf.Abs(osepY - s_Enemy.Y);
                    eneCost = xCost + yCost;
                }
                else
                if (e.GetComponent<BombEnemy>() != null)
                {
                    xBuf = e.GetComponent<BombEnemy>().X;
                    yBuf = e.GetComponent<BombEnemy>().Y;

                    s_bombEnemy = e.GetComponent<BombEnemy>();
                    xCost = Mathf.Abs(osepX - s_bombEnemy.X);
                    yCost = Mathf.Abs(osepY - s_bombEnemy.Y);
                    eneCost = xCost + yCost;
                }
                else
                if (e.GetComponent<FlameSwordMove>() != null)
                {
                    xBuf = e.GetComponent<FlameSwordMove>().X;
                    yBuf = e.GetComponent<FlameSwordMove>().Y;

                    s_flameEnemy = e.GetComponent<FlameSwordMove>();

                    xCost = Mathf.Abs(osepX - s_flameEnemy.X);
                    yCost = Mathf.Abs(osepY - s_flameEnemy.Y);
                    eneCost = xCost + yCost;
                }
                //int xBuf = e.GetComponent<Enemy>().X;
                //int yBuf = e.GetComponent<Enemy>().Y;

                //今いる位置からの差分を求める
                int difX = Mathf.Abs(xBuf - nowSelX);

                //Enemy s_Enemy = e.GetComponent<Enemy>();
                //int xCost = Mathf.Abs(osepX - s_Enemy.X);
                //int yCost = Mathf.Abs(osepY - s_Enemy.Y);
                //eneCost = xCost + yCost;

                //見てるx軸にその敵が居たら
                if (yLine == yBuf && oldDifX > difX)
                {
                    if (selectsNum > 0 && eneCost != bottomCost) continue;
                    nearX = xBuf;
                    nearY = yBuf;
                    oldDifX = difX;
                    //currentSelectEnemy = e.GetComponent<Enemy>();
                    if (e.GetComponent<Enemy>() != null)
                    {
                        currentSelectEnemy = e.GetComponent<Enemy>();
                    }
                    else if (e.GetComponent<BombEnemy>() != null)
                    {
                        currentSelectBombEnemy = e.GetComponent<BombEnemy>();
                    }
                    else if (e.GetComponent<FlameSwordMove>() != null)
                    {
                        currentSelectFlameEnemy = e.GetComponent<FlameSwordMove>();
                    }
                    currentSelectEnemyObj = e;
                }
            }

            //敵が居なかったら次の行を見る
            yLine += vecNum;

            //ステージ範囲外に出たら戻る
            if (yLine < -1 || yLine > stageHeight)
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
            if (e.GetComponent<Enemy>() != null)
            {
                if (e.GetComponent<Enemy>().X == nowSelX && e.GetComponent<Enemy>().Y == nowSelY) return e;
            }
            else if (e.GetComponent<BombEnemy>() != null)
            {
                if (e.GetComponent<BombEnemy>().X == nowSelX && e.GetComponent<BombEnemy>().Y == nowSelY) return e;
            }
            else if (e.GetComponent<FlameSwordMove>() != null)
            {

                if (e.GetComponent<FlameSwordMove>().X == nowSelX && e.GetComponent<FlameSwordMove>().Y == nowSelY) return e;
            }

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

    //指定したマスのマテリアルを指定のものに変える  //ここ？
    public void ChangeMatSpecifiedMass(GameObject obj,int colorType=0) {

        //GameObject speci = s_MapMass.GetGameObjectOfSpecifiedMass(x, y);

        obj.GetComponent<Renderer>().material = eleMats[colorType];

        obj.tag = "ChangedMass";
        
        //sePlay.Play("Select3");///////////////////////////////////////////////////////
        
    }

    //セレクターの位置情報(添え字)をMapMassに渡す
    void PassSelecterPos() {
        sePlay.Play("MagicCursorSelect");//フィールドのマスを動かす時に音を鳴らす
        s_MapMass.SetMagicMassSelector(nowSelX,nowSelY);
    }

    //魔法セレクターが現在いるマスの添え字を返す
    public (int, int) GetCurrentSelecerPos() {
        
        return (nowSelX, nowSelY);
    }

    int currentEnemyNums = 0;

    //Aボタンで魔法を撃つ /////////ここ
    void ActivateMagic() {

        int typ, lev;
        (typ, lev) = s_MagicRangeDetector.GetOrbInfo();
        if (lev > 0)//オーブのレベルが0より上なら入る↓
        {
            GameObject[] attackRange = s_MagicRangeDetector.GetCurrentAttackRange();
            Vector2Int[] attackRangePos = s_MagicRangeDetector.GetCurrentAttackRangesPos();
            int sum = 0;

            //五芒星雷魔法 と 五角形炎魔法 以外
            if (typ != 2)
            {
                if (Input.GetButtonDown("Fire1"))
                {

                    //全ます
                    if (typ == 1 || typ == 3 || typ == 4)
                    {

                        if (typ == 4)///炎五芒星　　//魔法によって音を変える、typ1:氷五芒星 typ3:炎五角形 typ4:氷五角形  

                        {
                            sePlay.Play("IceMagicPenta");
                        }else if (typ == 3){
                            sePlay.Play("MagicShot");
                        }



                        //地雷と氷
                        if (typ == 3 || typ == 4) {
                            foreach (var rp in attackRangePos) {
                                int pos = masses[rp.y, rp.x];
                                if (pos == 4 || pos == 5)
                                    sum++;
                            }
                        }

                        //全ての範囲が障害物の中にあった場合、魔法を撃てなくする
                        if (sum >= attackRange.Length) return;

                        s_PlayerContoller.ShotMagic(attackRange[0], typ, lev, attackRange);


                        //if (typ == 4)///炎五芒星　　//魔法によって音を変える、typ1:氷五芒星 typ3:炎五角形 typ4:氷五角形  

                        //{
                        //    sePlay.Play("MagicShot");
                        //}

                        //sePlay.Play("MagicShot");
                    }

                    //
                    if (typ == 0 || typ == 5)
                    {
                        int center = attackRange.Length / 2;
                        if (lev == 10) center = 190;

                        s_PlayerContoller.ShotMagic(attackRange[center], typ, lev);
                        if (typ==5||typ==0) {
                            sePlay.Play("MagicShot");
                        }
                    }

                    //インターバルのリセット
                    nextShotInterval = reload_Interval;
                }
            }
            else
            {  //複数設置型
                if (Input.GetButtonDown("Fire1"))
                {
                    //sePlay.Play("MagicShot");
                    //一番最初に選択した時
                    if (selectsNum == 0)
                    {

                        //選択上限
                        if (typ == 2) selectsNumLimit = 1;
                        //if (typ == 3) selectsNumLimit = lev;

                        selectTargets = new GameObject[selectsNumLimit];

                        currentEnemyNums = GameObject.FindGameObjectsWithTag("Enemy").Length;
                    }

                    //一番近い敵との距離をリセット
                    bottomCost = 99;

                    //現在選択中のエネミー
                    if (typ == 2)
                        selectTargets[selectsNum] = SearchSameObjectInSelectArray(GetEnemyObjectOnCurrentSelectMass());
                    if (typ == 3)
                        selectTargets[selectsNum] = (s_MapMass.GetGameObjectOfSpecifiedMass(nowSelX, nowSelY));



                    //上限に達したら
                    if (selectsNum >= selectsNumLimit || selectsNum >= currentEnemyNums)
                    {

                        //魔法を放つ
                        if (typ == 2 || typ == 3)
                        {
                            if (typ == 2)
                            {
                                sePlay.Play("ThunderMagicFire");
                            }
                            s_PlayerContoller.ShotMagic(selectTargets[0], typ, lev, selectTargets);
                            foreach (var e in selectTargets) e.tag = "Enemy";
                        }

                        selectsNum = 0;
                    }

                    //インターバルのリセット
                    nextShotInterval = reload_Interval;
                }
            }
        }//オーブのレベルが0より上なら入る↑
    }

    //複数選択魔法で2体目以降選択時に、最低コストを求める
    void CalcBottomCost() {

        if (selectTargets[0].GetComponent<Enemy>() == null && selectTargets[0].GetComponent<BombEnemy>() == null && selectTargets[0].GetComponent<FlameSwordMove>() == null) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var e in enemies) {

            if (SearchSameObjectInSelectArray2(e) == false) continue;

            int oldSelectEnemyX, oldSelectEnemyY;
            (oldSelectEnemyX, oldSelectEnemyY) = GetOldSelectedEnemyPos();

            //敵との距離コスト
            Enemy s_Enemy = null;
            BombEnemy s_bombEnemy = null;
            FlameSwordMove s_flameEnemy = null;
            int xCost, yCost, eneCost = 0;
            //Enemy s_Enemy = e.GetComponent<Enemy>();
            //いつかBaseに変える
            if (e.GetComponent<Enemy>() != null)
            {
                s_Enemy = e.GetComponent<Enemy>();

                xCost = Mathf.Abs(oldSelectEnemyX - s_Enemy.X);
                yCost = Mathf.Abs(oldSelectEnemyY - s_Enemy.Y);
                eneCost = xCost + yCost;
            }
            else
                if (e.GetComponent<BombEnemy>() != null)
            {
                s_bombEnemy = e.GetComponent<BombEnemy>();
                xCost = Mathf.Abs(oldSelectEnemyX - s_bombEnemy.X);
                yCost = Mathf.Abs(oldSelectEnemyY - s_bombEnemy.Y);
                eneCost = xCost + yCost;
            }
            else
                if (e.GetComponent<FlameSwordMove>() != null)
            {
                s_flameEnemy = e.GetComponent<FlameSwordMove>();
                xCost = Mathf.Abs(oldSelectEnemyX - s_flameEnemy.X);
                yCost = Mathf.Abs(oldSelectEnemyY - s_flameEnemy.Y);
                eneCost = xCost + yCost;
            }
            //int xCost = Mathf.Abs(oldSelectEnemyX - s_Enemy.X);
            //int yCost = Mathf.Abs(oldSelectEnemyY - s_Enemy.Y);
            //int eneCost = xCost + yCost;

            //最も近い敵のコストを入れる
            if (eneCost < bottomCost) {
                bottomCost = eneCost;
            }
        }
    }



    //直近選択した敵の座標を返す
    (int, int) GetOldSelectedEnemyPos() {
        int x = 0, y = 0;
        //直近選択した敵の座標
        if (selectTargets[selectsNum - 1].GetComponent<Enemy>() != null)
        {
            Enemy s_EnemyOld = selectTargets[selectsNum - 1].GetComponent<Enemy>();
            x = s_EnemyOld.X;
            y = s_EnemyOld.Y;
        }
        else
        if (selectTargets[selectsNum - 1].GetComponent<BombEnemy>() != null)
        {
            BombEnemy s_EnemyOld = selectTargets[selectsNum - 1].GetComponent<BombEnemy>();
            x = s_EnemyOld.X;
            y = s_EnemyOld.Y;
        }
        else
        if (selectTargets[selectsNum - 1].GetComponent<FlameSwordMove>() != null)
        {
            FlameSwordMove s_EnemyOld = selectTargets[selectsNum - 1].GetComponent<FlameSwordMove>();
            x = s_EnemyOld.X;
            y = s_EnemyOld.Y;
        }
        return (x, y);
    }

    //選択配列の中にあるオブジェクトと同じ物が存在するかを判定すう
    GameObject SearchSameObjectInSelectArray(GameObject obj)
    {

        if (selectsNum > 0)
        {
            foreach (GameObject g in selectTargets)
            {
                if (g == obj) return null;
            }
        }

        if (selectTargets[selectsNum] != null)
            selectTargets[selectsNum].tag = "MarkedEnemy";

        //被りが無ければカウント
        selectsNum++;

        return obj;
    }

    bool SearchSameObjectInSelectArray2(GameObject obj)
    {

        if (selectsNum > 0)
        {
            foreach (GameObject g in selectTargets)
            {
                if (g == obj) return false;
            }
        }

        return true;
    }
}
