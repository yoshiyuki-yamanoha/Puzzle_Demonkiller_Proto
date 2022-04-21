using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerationEnemy : PseudoArray
{
    Vector2Int[] initpos = new Vector2Int[13];//6体分
    int random = 0;

    EnemyBase enemy_base;
    int search_count = 0;
    //インスペクターで設定用
    [SerializeField] bool is_generation = false;
    [SerializeField] GameObject[] enemy_prefab = null;//プレファブ格納変数
    [SerializeField] ParticleSystem[] enemy_particle = null;
    [SerializeField] int enemy_max = 50;//敵最大値 盤面の敵の最大値
    [SerializeField] float interval_s = 5;//生成感覚
    //mapの生成情報
    [SerializeField] MapMass map;
    [HideInInspector] public int max_x = 0;//横最大
    [HideInInspector] public int max_y = 0;//縦最大
    [SerializeField] int y;//縦方向
    float enemy_count = 0;//エネミーカウント
    int enemy_kinds_max = 0;//敵種類
    int enemy_oneturn_count = 0;
    int enemy_oneturn_max = 13;  //1ターンに出る敵の最大大数
    int Turn_Count = 1;  //1ターン目からターン数を数える（規定ターン数に敵を沸く処理を作る用
    //public GameObject[] rootpos = null;//親オブジェクト
    public bool initflg = true;
    float time = 0;
    TrunManager trunmanager = null;
    GameObject[] StageSarchEnemy = null;
    bool enemy_max_flg = false;
    float exit_time = 0;
    //ターン終了フラグ
    bool turn_exit_flg = false;
    bool turn_initflg = true;
    int[] boss_y = new int[24];
    int[] boss_x = new int[24];
    //具志堅SE処理
    SEManager sePlay = null;
    //具志堅BGM処理7
    private BGMManager bgmPlay;

    bool oneturn_generation_flg = false;//アクティブ保存
    bool general_init_flg = false;

    bool sarchinit = true;
    // Start is called before the first frame update
    void Start()
    {
        enemy_base = new EnemyBase();
        Debug.Log("enemy_base " + enemy_base);


        initpos[0] = new Vector2Int(2,7);
        initpos[1] = new Vector2Int(7,4);
        initpos[2] = new Vector2Int(8,6);
        initpos[3] = new Vector2Int(12,7);
        initpos[4] = new Vector2Int(17,5);
        initpos[5] = new Vector2Int(17,9);
        initpos[6] = new Vector2Int(2, 7);


        initpos[7] = new Vector2Int(1,1 );
        initpos[8] = new Vector2Int(2, 4);
        initpos[9] = new Vector2Int(2, 0);
        initpos[10] = new Vector2Int(7,0 );
        initpos[11] = new Vector2Int(16,2);
        initpos[12] = new Vector2Int(16,3);


        //max_x = rootpos.Length;//スタートポジションの数分取得
        //max_y = rootpos[0].transform.childCount;//子供の数取得
        max_x = map.Map.GetLength(1);
        max_y = map.Map.GetLength(0);

        enemy_kinds_max = enemy_prefab.Length;
        trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();

        if (sePlay != null) sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
        bgmPlay = GameObject.Find("BGMAudio").GetComponent<BGMManager>();//Se再生用
        /*if (bgmPlay != null)*/
        bgmPlay.Play("PLAYBGM");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //自分のターンの時
        if (trunmanager.trunphase == TrunManager.TrunPhase.Enemy)
        { 
            //DeleteListEnemy();
            enemy_base.DeleteListEnemy();//敵削除

            if (oneturn_generation_flg)//生成で全員生成した後
            {
                if (sarchinit)
                {
                    StageSarchEnemy = GameObject.FindGameObjectsWithTag("Enemy");
                    sarchinit = false;
                    //Debug.Log("敵の数" + StageSarchEnemy.LongLength);
                }
            }


            if (is_generation && general_init_flg)
            {//生成するフラグが立っている時
                if (StageSarchEnemy.Length <= 0) //敵が0人
                {
                    //if (enemy_count < enemy_max)
                    //{
                    Debug.Log("puzzleターンに移行");
                    trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
                    //}
                }
            }


            if (turn_initflg)
            {
                //Debug.Log("敵のターン");
                turn_exit_flg = false;//抜けるオフ
                turn_initflg = false;
                oneturn_generation_flg = false;
            }
        }

        if (!turn_exit_flg) //自分のターン
        {
            //生成する状態なら
            if (is_generation)
            {
                //switch (Turn_Count) //ターンごとに敵の出る量を調整します  caseを使ってターンごとの沸きを調整できます
                //{                   //使い方：case敵の量を変えたいターンの数字を追加→enemy_oneturun_maxに敵を出したい数を代入（最低1体）
                //    case 1: //ターン1
                //        enemy_oneturn_max = 3;
                //        break;
                //    case 2: //ターン2 
                //        enemy_oneturn_max = 0;
                //        break;
                //    case 3: //ターン3
                //        enemy_oneturn_max = 2;
                //        break;
                //    default:
                //        enemy_oneturn_max = Random.Range(2, 5);
                //        break;
                //}
                if (time > interval_s)//秒おきに生成
                {

                    for (int i = 0; i < 1; i++) //同時生成処理
                    {
                        //設定したエネミーMaxが
                        if (enemy_max < enemy_oneturn_max)
                        {
                            enemy_oneturn_max = enemy_max;
                        }

                        int Enemy_kinds_max = Random.Range(0, enemy_kinds_max); //////敵の位置関係？


                        
                        int randomX = initpos[random].x;//Random.Range(0, max_x);   //// //敵のx座標の位置を入れる
                        int randomY = initpos[random].y;//Random.Range(0, max_y);  ////敵のy座標の位置を入れる　右辺で、y座標のスポーン位置を調整

                        if (random < 12) {
                            random++;
                        }

                        //生成する位置が誰もいない時 空いてるマスなら生成
                        if (map.Map[randomY, randomX] == (int)MapMass.Mapinfo.NONE)
                        {
                            Generation(Enemy_kinds_max, randomX, randomY);//引数(エネミーの種類 , スタートPos)生成。
                            if (sePlay != null) sePlay.Play("EnemySpawn");
                        }
                    }
                    time = 0;
                }

                if (enemy_count >= enemy_max)//最大値超えていたら何もしない。
                {
                    enemy_max_flg = true;
                    is_generation = false;
                }
                else
                {
                    time += Time.deltaTime;
                }

            }//生成フラグがオンの時
            else//ここが敵が動いている時↓
            {
                //敵がの持ってく行動フラグを見ている。
                Debug.Log("search大きさ." + StageSarchEnemy.Length);
                for (int i = 0; i < StageSarchEnemy.Length; i++)
                {
                    if (StageSarchEnemy[i].GetComponent<Enemy>().Is_action)
                    {
                        if (StageSarchEnemy[i].GetComponent<Enemy>().Init_search_flg)
                        {

                            search_count++;
                            Debug.Log("searchカウント" + search_count);
                            StageSarchEnemy[i].GetComponent<Enemy>().Init_search_flg = false;
                        }
                    }
                }


                if (search_count >= StageSarchEnemy.Length)//searchカウントがステージのsearchカウントより大きいとき enemy max flgがターンMaxFlgが大きい時
                {
                    if (enemy_max_flg)
                    {
                        turn_exit_flg = true;
                    }
                    else
                    {
                        is_generation = true;
                    }

                }
            }
        }

        if (turn_exit_flg && trunmanager.trunphase == TrunManager.TrunPhase.Enemy)
        {

            exit_time += Time.deltaTime;

            if (exit_time > 2)
            {
                turn_initflg = true;
                exit_time = 0;
                //Debug.Log("ターンカウントとトトと十っと夫とトト:" + Turn_Count);
                Turn_Count++;  //ターンを加算していく
                trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
                search_count = 0;
                //enemy_camera.depth = -2;
                for (int i = 0; i < StageSarchEnemy.Length; i++)
                {
                    StageSarchEnemy[i].GetComponent<Enemy>().Init_search_flg = true;
                }

                sarchinit = true;
            }
        }

    }

    void Generation(int num, int x, int y)
    {
        //rootpos[x].transform.GetChild(y).GetComponent<PseudoArray>().Whoisflg = true;
        //rootpos[x].transform.GetChild(y).GetComponent<PseudoArray>().Mass_status = MassStatus.ENEMY;//mapの位置に敵を入れる。
        map.Map[y, x] = (int)MapMass.Mapinfo.Enemy;

        //GameObject enemy_obj = rootpos[x].transform.GetChild(y).gameObject;
        Vector3 enemypos = new Vector3(map.Tilemas_prefab.transform.localScale.x * x, 0, -map.Tilemas_prefab.transform.localScale.z * y);//敵の出現位置

        //出現する魔法陣を生成
        ParticleSystem new_particle = Instantiate(enemy_particle[num], enemypos, enemy_particle[num].gameObject.transform.rotation);
        new_particle.Play();//再生

        Vector3 offset = new Vector3(0, 0.5f, 0);//キャラの高さ分調整用

        GameObject enemy_instantiate = Instantiate(enemy_prefab[num], enemypos + offset, new Quaternion(0, 180.0f, 0, 1));//敵を生成
        enemy_instantiate.name = enemy_prefab[num].name + enemy_count.ToString();

        //スタートポジションを教えてあげる。生成したプレファブに
        Enemy enemy = enemy_instantiate.GetComponent<Enemy>();

        switch (enemy.enemy_kinds)
        {
            case EnemyBase.EnemyKinds.Demon:
                enemy.X = x; enemy.Y = y; enemy.Enemy_action = EnemyBase.EnemyAction.Generation; enemy.Is_action = true;
                break;
            case EnemyBase.EnemyKinds.Demon1:
                enemy.X = x; enemy.Y = y; enemy.Enemy_action = EnemyBase.EnemyAction.Generation; enemy.Is_action = true;
                break;
            case EnemyBase.EnemyKinds.Boss:
                enemy.X = x; enemy.Y = y; enemy.Enemy_action = EnemyBase.EnemyAction.Generation; enemy.Is_action = true;
                break;
        }

        enemy_count++;//敵をカウント
        enemy_oneturn_count++;//1ターンでのカウント

        if (enemy_oneturn_count >= enemy_oneturn_max)  //enemy_oneturn_maxの数敵が出てくる：ここでターン＋
        {
            //Debug.Log("生成終了");
            oneturn_generation_flg = true;
            is_generation = false;//終了
            turn_exit_flg = true;
            general_init_flg = false;
            enemy_oneturn_count = 0;
        }
    }

    bool BossGeneration(int x, int y)
    {
        map.Map[y, x] = (int)MapMass.Mapinfo.Enemy;//自分,中心値を設定                                           
        ////32方向 調べて、エネミーがないかチェック

        //時計回り
        for (int i = 0; i < 24; i++)
        {

        }
        boss_y[0] = y - 1; boss_x[0] = x;
        boss_y[1] = y; boss_x[1] = x + 1;
        boss_y[2] = y + 1; boss_x[2] = x;
        boss_y[3] = y; boss_x[3] = x - 1;
        boss_y[4] = y - 1;
        boss_y[5] = y + 1;
        boss_y[6] = y + 1;
        boss_y[7] = y - 1;

        //上 y-1, x
        //右 y, x + 1
        //下 y+1, x
        //左 y, x - 1

        //後ろ右 y - 1, x + 1
        //前右 y + 1, x + 1
        //前左 y + 1, x - 1
        //後ろ左 y - 1, x - 1



        //(16方向)
        //2段上 y - 2, x
        //2段下 y + 2, x
        //2段左 y, x - 2
        //2段右 y, x + 2
        //2段前右 y + 2, x + 2
        //2段前左 y + 2, x - 2
        //2段後ろ右 y - 2, x + 2
        //2段後ろ左 y - 2, x - 2
        // y + 2, x - 1
        // y + 2, x + 1
        // y + 1, x - 2
        // y + 1, x + 2
        // y - 1, x + 2
        // y - 1, x - 2
        // y - 2, x + 1
        // y - 2, x - 1


        for (int i = 0; i < 24; i++)
        {
            if (map.Map[boss_y[i], boss_x[i]] == (int)MapMass.Mapinfo.Enemy) { return false; }
        }
        return true; // trueがあったら生成

    }
}