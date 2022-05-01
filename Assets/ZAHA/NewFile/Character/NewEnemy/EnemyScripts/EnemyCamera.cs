using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCamera : MonoBehaviour
{
    private GameObject[] targets = null;//ターゲット
    GameObject closeEnemy = null;//一番近い敵を格納する変数
    GameObject hpNoneEnemy = null;//体力のない敵を格納する変数
    TrunManager tr;

    //カメラ用
    Vector3 distance;//距離
    bool initflg = true;
    Vector3 defaultCamerapos;//Cameraの初期位置
    Vector3 enemyLookCamepos;//敵を見ているときのCamera位置

    GameObject camera_targe = null;
    bool moveflag = false;

    MagicAttackCamera cameraMove;
    Vector3 pos = new Vector3(0, 0, 0);

    Camera enemy_camera = null;
    Enemy enemyste; //敵のステータス読み取り用
    float timer = 0;

    public bool startFlag = true;
    float x = 0, y = 0, z = 0;

    FadeOut fadeout;

    //ゲームクリアに使う者たち
    MainMgr mManager;
    int dieEnemyCount;
    int dieEnemyMax;
    int dieFlagEnemy;
    public bool endFlag = false;//最後の敵が倒れた時カメラの動きが終了したフラグ
    bool finalDieflag = false;//倒れた敵のカウントフラグ
    Text gameCleartext;
    // Start is called before the first frame update
    void Start()
    {
        gameCleartext = GameObject.Find("GameClearText").GetComponent<Text>();
        gameCleartext.text = "";
        mManager = GameObject.Find("MainMgr").GetComponent<MainMgr>();
        dieEnemyCount = 0;
        dieEnemyMax = mManager.GetEnemyDieCountMax();
        fadeout = GameObject.Find("FadeImage").GetComponent<FadeOut>();
        tr = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        cameraMove = GameObject.Find("GameObject").GetComponent<MagicAttackCamera>();
        enemy_camera = this.gameObject.GetComponent<Camera>();
        enemy_camera.depth = -2;
        //defaultCamerapos = new Vector3(25.22f, 3.7f, -80);
        defaultCamerapos = new Vector3(47.7f, 43.7f, -132.1f);
        transform.position = new Vector3(0, 26, -107);
    }

    // Update is called once per frame
    private void FixedUpdate() {
        dieEnemyCount = mManager.GetEnemyDieCount();
        if (tr.GetTrunPhase() == TrunManager.TrunPhase.Puzzle)
        {
            dieFlagEnemy = 0;
        }

        //敵の行動ターンの時に
        if (tr.GetTrunPhase() == TrunManager.TrunPhase.Enemy)
        {
            if (startFlag == true)
            {
                StartEnemyCameraMove();
            }
            else
            {
                EnemyCameraMove();
                //EndEnemyCameraMove();
            }
        }
        else
        {
            transform.position = defaultCamerapos;
            startFlag = false;

            enemy_camera.depth = -2;//カメラの優先度
            initflg = true;// 
            moveflag = true;
            x = 0;
            //Time.timeScale = 1f;

            //if (dieEnemyCount >= dieEnemyMax)
            //{
            //    EndEnemyCameraMove();
            //}
        }
        if (dieEnemyCount >= dieEnemyMax)
        {
            EndEnemyCameraMove();
        }
    }


    GameObject CloseEnemycamera()//近い敵
    {
        float closeDist = 1000.0f;

        foreach (var target in targets)
        {
            float targetDist = Vector3.Distance(transform.position, target.transform.position);
            if (closeDist > targetDist)
            {
                closeDist = targetDist;
                closeEnemy = target;
            }
        }

        return closeEnemy;
    }
    void HpNoneEnemy()
    {
        hpNoneEnemy = null;
        foreach (var target in targets)
        {
            enemyste = target.GetComponent<Enemy>();
            if (enemyste.Deathflg == true)
            {
                //dieEnemyCount++;
                //dieFlagEnemy++;
                hpNoneEnemy = target;
                Debug.Log(hpNoneEnemy+"HPのない敵");
            }
        }

        //dieCountflag = true;
        Debug.Log("敵のカウントフラグ" + dieFlagEnemy + "");
        Debug.Log("敵が" + dieEnemyCount + "体目倒れた");


        //return hpNoneEnemy;
    }

    void CloseEnemyCameraMove()//一番近い敵を見るカメラ
    {
        //敵の行動ターンの時に
        if (tr.GetTrunPhase() == TrunManager.TrunPhase.Enemy)
        {
            enemy_camera.depth = 0;
            targets = GameObject.FindGameObjectsWithTag("Enemy");//敵のタグがついているオブジェクト取得

            if (initflg == true)
            {
                camera_targe = CloseEnemycamera();
                if (camera_targe != null)
                {
                    enemyste = camera_targe.GetComponent<Enemy>();

                    //transform.position = MoveCamerapos;
                    if (camera_targe != null) distance = transform.position - camera_targe.transform.position;

                    initflg = false;
                }
                //if (camera_targe != null) this.transform.LookAt(camera_targe.transform);
            }
            else
            {
                if (camera_targe != null)
                {
                    enemyLookCamepos = new Vector3(/*transform.position.x*/camera_targe.transform.position.x, 4, camera_targe.transform.position.z - 5);
                    transform.position = enemyLookCamepos;

                }
                if (enemyste.Ismove != true)
                {
                    if (moveflag == true)
                    {
                        cameraMove.startTime = Time.time;
                        moveflag = false;
                    }
                    transform.position = Vector3.Lerp(enemyLookCamepos, defaultCamerapos, cameraMove.CalcMoveRatio());
                }
            }
            //if (camera_targe != null) transform.position = transform.position - distance;
        }
        else
        {
            enemy_camera.depth = -2;
            //transform.position = new Vector3(25.22f, 3.7f, -80);
            initflg = true;
            moveflag = true;
        }
    }
    void EnemyCameraMove()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");//敵のタグがついているオブジェクト取得


        if ((hpNoneEnemy == null || dieEnemyCount < dieEnemyMax-1) && finalDieflag == false)
        {
            HpNoneEnemy();
            enemy_camera.depth = 0;
            if (x < 96)
            {
                x += 0.25f;
            }
            transform.position = new Vector3(x, 40, -110);
            transform.eulerAngles = new Vector3(45, 0, 0);
            //if(x > 95)
            //{
            //    startFlag = false;
            //    //tr.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
            //}

        }
        else
        {
            EndEnemyCameraMove();
        }
        //HpNoneEnemy();
        Debug.Log(dieFlagEnemy + "たい倒れた");

    }
    void EndEnemyCameraMove()//最後の敵が倒れた時に使う
    {
        finalDieflag = true;
        //transform.eulerAngles = new Vector3(0, 0, 0);
        //Vector3 tagepos;//一番近くの敵の座標を入れる
        timer += Time.deltaTime;

        enemy_camera.depth = 0;
        //targets = GameObject.FindGameObjectsWithTag("Enemy");//敵のタグがついているオブジェクト取得
        //HpNoneEnemy();
        if (initflg == true)
        {
            camera_targe = hpNoneEnemy;//CloseEnemycamera();
            if (camera_targe != null)
            {
                enemyste = camera_targe.GetComponent<Enemy>();
                //enemyste.Hp;
                //transform.position = MoveCamerapos;
                if (camera_targe != null) distance = transform.position - camera_targe.transform.position;

                initflg = false;
            }
            //if (camera_targe != null) this.transform.LookAt(camera_targe.transform);
        }
        else
        {
            if (camera_targe != null)
            {
                enemyLookCamepos = new Vector3(/*transform.position.x*/camera_targe.transform.position.x, 4, camera_targe.transform.position.z - 15);
                //transform.position = enemyLookCamepos;

                if (moveflag == true)
                {
                    //cameraMove.startTime = Time.time;
                    cameraMove.moveflag = true;
                    moveflag = false;
                }
                transform.position = Vector3.Lerp(defaultCamerapos, enemyLookCamepos, cameraMove.CalcMoveRatio());//倒れた敵に向かう
                transform.LookAt(camera_targe.transform.position);
            }
        }
        if (timer > 3)
        {
            endFlag = true;
        }
        if(timer > 1 && timer < 2.5)
        {
            Time.timeScale = 0.5f;
        }
        else
        {
            Time.timeScale = 1;
        }
        if(hpNoneEnemy == null)
        {

            gameCleartext.text = "GameClear";//ゲームクリアの文字を出す(入れる)
        }

        //if (Time.timeScale == 1)
        //{
        //    Time.timeScale = 0.3f;
        //}


    }
    void StartEnemyCameraMove()
    {

        //Debug.Log(timer);
        timer += Time.deltaTime;
        enemy_camera.depth = 0;
        x += 0.5f;
        transform.position = new Vector3(x, 26, -107);
        //if(x > 95)
        //{
        //    startFlag = false;
        //    //tr.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
        //}
        if (timer > 3)
        {
            fadeout.fadeOutFlag = true;
        }
        if (timer > 4)
        {
            startFlag = false;
            fadeout.fadeInFlag = true;
            x = 0;
            timer = 0;
        }
    }
    //else
    //{
    //    startFlag = false;
    //    enemy_camera.depth = -2;
    //}
}
