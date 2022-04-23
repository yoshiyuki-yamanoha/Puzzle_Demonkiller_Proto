using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerationEnemy : PseudoArray
{
    //class 参照
    EnemyBase enemy_base = null;
    [SerializeField] MapMass map = null;
    TrunManager trunmanager = null;

    //具志堅SE処理
    SEManager sePlay = null;
    //具志堅BGM処理取得
    BGMManager bgmPlay = null;

    //初期生成フラグ
    bool init_generation_flg = true;
    bool generation_flg = true;

    //敵生成情報
    int enemy_kinds_max = 0;//敵種類
    [SerializeField] GameObject[] enemy_prefab = null;//プレファブ格納変数
    [SerializeField] ParticleSystem[] enemy_particle = null;//敵の出現魔法陣
    [SerializeField] int enemy_max = 50;//[1ゲーム]で生成出来る数
    float enemy_count = 0;//[1ゲーム]敵をカウント
    [SerializeField] float interval_s = 5;//生成間隔(秒)
    float time = 0;//計測時間用

    //ステージ敵検索用
    List<GameObject> search_list_enemys = new List<GameObject>();
    bool one_turn_search_flg = true;

    //[1ターン]生成情報
    int enemy_oneturn_count = 0;
    int enemy_oneturn_max = 10;  //1ターンに出る敵の最大大数

    //mapの生成情報
    [HideInInspector] public int max_x = 0;//map横最大
    [HideInInspector] public int max_y = 0;//map縦最大

    //他のスクリプトから参照してた奴
    public bool initflg = false;
    // Start is called before the first frame update
    void Start()
    {
        GetInstance();
        GenerationEnemy_Init();
        GenerationEnemy_BGM();

    }

    private void FixedUpdate()
    {
        if (trunmanager.GetTrunPhase() == TrunManager.TrunPhase.Enemy)//エネミーターン時
        {
            enemy_base.DeleteListEnemy();//敵削除

            if (enemy_max < enemy_oneturn_max)
            {
                enemy_oneturn_max = enemy_max;
            }

            if (init_generation_flg)
            {//最初のターン
                if (enemy_oneturn_count < enemy_oneturn_max)
                { //1ターンに生成出来る数が最大値を超えたら
                    Debug.Log("生成中");
                    Generation(new Vector2Int(Random.Range(0, 19), Random.Range(0, 19)));
                }
                else
                {
                    enemy_oneturn_count = 0;
                    init_generation_flg = false;
                    trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);//ターンをパズルに変更
                }
            }
            else
            {
                if (SearchMoveEnemy())
                {
                    if (generation_flg)
                    {//生成フラグがオンだったら生成
                        while (enemy_oneturn_count < enemy_oneturn_max)
                        {
                            Generation(new Vector2Int(Random.Range(0, 19), Random.Range(0, 19)));
                        }
                        enemy_oneturn_count = 0;
                    }
                    trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);//ターンをパズルに変更
                }
            }
        }
        else
        {
            one_turn_search_flg = true;
        }

    }

    bool SearchMoveEnemy()//敵を検索
    {
        bool search_flg = false;
        if (one_turn_search_flg)//1ターンに1回取得する
        {
            GameObject[] search_enemys = null;//エネミータグで取得格納配列
            search_enemys = GameObject.FindGameObjectsWithTag("Enemy");//敵をタグで取得
            foreach (var search_enemy in search_enemys)
            {
                search_list_enemys.Add(search_enemy);//タグで取得した敵をリストに追加
            }
            one_turn_search_flg = false;
        }

        foreach (var search_enemy in search_list_enemys)//取得した敵分リストを回す
        {
            if (search_enemy == null)//リストに入っていた死亡して敵が消えてた場合はリストから削除
            {
                search_list_enemys.Remove(search_enemy);//敵をリストから消す
            }

            if (search_enemy.GetComponent<EnemyBase>().Is_action)//行動が終わったか確認
            {
                search_list_enemys.Remove(search_enemy);//行動が終わった敵はリストから削除。
            }
        }

        if (search_list_enemys.Count <= 0)//リストが0になぅったら
        {
            //trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
            search_flg = true;//検索終了
        }

        return search_flg;
    }

    void Generation(Vector2Int pos)
    {
        time += Time.deltaTime;//カウント開始
        if (time > interval_s)//スパンごと出現させる
        {
            int random_enem_kinds = Random.Range(0, 4);//敵の種類ランダム生成
            int random_magic = Random.Range(0, 2);//敵の種類ランダム生成
            if (map.Map[pos.y, pos.x] == (int)MapMass.Mapinfo.NONE) //何もない場所だったら
            {
                InstanceEnemy(random_magic, random_enem_kinds, pos.x, pos.y); // 生成種類 生成　X Y
                if (sePlay != null) sePlay.Play("EnemySpawn");//SEを鳴らす //具志堅
            }
            time = 0;//タイム初期化
        }

        if (enemy_count >= enemy_max)//超えているかみる
        {
            generation_flg = false;
        }
    }

    void InstanceEnemy(int magic_num, int enemy_kinds, int x, int y)//生成
    {
        map.Map[y, x] = (int)MapMass.Mapinfo.Enemy;//mapに敵の情報を渡す。

        Vector3 enemypos = new Vector3(map.Tilemas_prefab.transform.localScale.x * x, 0, -map.Tilemas_prefab.transform.localScale.z * y);//敵の出現位置

        //出現する魔法陣を生成/////////////////////////////////////////////////////////////////////////////////////////////////////
        ParticleSystem new_particle = Instantiate(enemy_particle[magic_num], enemypos, enemy_particle[magic_num].gameObject.transform.rotation);
        new_particle.Play();//再生
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Vector3 offset = new Vector3(0, 0.5f, 0);//キャラの高さ分調整用

        GameObject enemy_instantiate = Instantiate(enemy_prefab[enemy_kinds], enemypos + offset, new Quaternion(0, 180.0f, 0, 1));//敵を生成

        Debug.Log("敵情報 " + enemy_instantiate);
        enemy_instantiate.name = enemy_prefab[enemy_kinds].name + enemy_count.ToString();//敵の名前を変更


        switch (enemy_kinds)
        {
            case 0:
                SetGoblinInfo(enemy_instantiate, x, y);
                break;
            case 1:
                SetWoManEnemyInfo(enemy_instantiate, x, y);
                break;
            case 2:
                SetBomInfo(enemy_instantiate, x, y);
                break;
            case 3:
                SetFlameSwordInfo(enemy_instantiate, x, y);
                break;
        }
        //switch (num)
        //{
        //    case 0:
        //        Enemy enemy_info = enemy_instantiate.GetComponent<Enemy>();//敵情報取得
        //        break;
        //    case 1:
        //        BombEnemy enemy_info = enemy_instantiate.GetComponent<BombEnemy>();//敵情報取得
        //        break;
        //    case 2:
        //        Enemy enemy_info = enemy_instantiate.GetComponent<Enemy>();//敵情報取得
        //        break;
        //}

        //switch (enemy_info.enemy_kinds)//敵の生成した座標を渡す。
        //{
        //    case EnemyBase.EnemyKinds.Demon:
        //        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true;
        //        break;
        //    case EnemyBase.EnemyKinds.Demon1:
        //        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true;
        //        break;
        //    case EnemyBase.EnemyKinds.Boss:
        //        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true;
        //        break;
        //    case EnemyBase.EnemyKinds.Bom:
        //        Debug.Log("ボムちゃん情報");
        //        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true;
        //        break;
        //}

        enemy_count++;
        enemy_oneturn_count++;
    }

    void SetFlameSwordInfo(GameObject enemy_instantiate, int x, int y)
    {
        FlameSwordMove enemy_info = enemy_instantiate.GetComponent<FlameSwordMove>();//敵情報取得
        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true;
    }

    void SetBomInfo(GameObject enemy_instantiate, int x, int y)
    {
        BombEnemy enemy_info = enemy_instantiate.GetComponent<BombEnemy>();//敵情報取得
        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true;
    }

    void SetGoblinInfo(GameObject enemy_instantiate, int x, int y)
    {
        Enemy enemy_info = enemy_instantiate.GetComponent<Enemy>();//敵情報取得
        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true;
    }

    void SetWoManEnemyInfo(GameObject enemy_instantiate, int x, int y)
    {
        Enemy enemy_info = enemy_instantiate.GetComponent<Enemy>();//敵情報取得
        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true;
    }

    void GenerationEnemy_BGM()//BGM関係
    {
        if (sePlay != null) sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
        bgmPlay = GameObject.Find("BGMAudio").GetComponent<BGMManager>();//Se再生用
        /*if (bgmPlay != null)*/
        bgmPlay.Play("PLAYBGM");
    }

    void GetInstance()//インスタンス
    {
        //取得
        trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();//ターンマネージャ取得
        enemy_base = new EnemyBase();//エネミーベースインスタンス
    }

    void GenerationEnemy_Init()//情報取得
    {
        max_x = map.Map.GetLength(1);//mapのx軸
        max_y = map.Map.GetLength(0);//mapのy軸
        enemy_kinds_max = enemy_prefab.Length;//敵の種類数
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{

    //    //自分のターンの時
    //    if (trunmanager.trunphase == TrunManager.TrunPhase.Enemy)
    //    {
    //        //DeleteListEnemy();
    //        enemy_base.DeleteListEnemy();//敵削除

    //        if (oneturn_generation_flg)//生成で全員生成した後
    //        {
    //            if (sarchinit)
    //            {
    //                StageSarchEnemy = GameObject.FindGameObjectsWithTag("Enemy");
    //                sarchinit = false;

    //                //Debug.Log("敵の数" + StageSarchEnemy.LongLength);
    //            }
    //        }


    //        if (is_generation && general_init_flg)
    //        {//生成するフラグが立っている時
    //            if (StageSarchEnemy.Length <= 0) //敵が0人
    //            {
    //                //if (enemy_count < enemy_max)
    //                //{
    //                Debug.Log("puzzleターンに移行");
    //                trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
    //                //}
    //            }
    //        }


    //        if (turn_initflg)
    //        {
    //            //Debug.Log("敵のターン");
    //            turn_exit_flg = false;//抜けるオフ
    //            turn_initflg = false;
    //            oneturn_generation_flg = false;
    //        }
    //    }

    //    if (!turn_exit_flg) //自分のターン
    //    {
    //        //生成する状態なら
    //        if (is_generation)
    //        {
    //            //switch (Turn_Count) //ターンごとに敵の出る量を調整します  caseを使ってターンごとの沸きを調整できます
    //            //{                   //使い方：case敵の量を変えたいターンの数字を追加→enemy_oneturun_maxに敵を出したい数を代入（最低1体）
    //            //    case 1: //ターン1
    //            //        enemy_oneturn_max = 3;
    //            //        break;
    //            //    case 2: //ターン2 
    //            //        enemy_oneturn_max = 0;
    //            //        break;
    //            //    case 3: //ターン3
    //            //        enemy_oneturn_max = 2;
    //            //        break;
    //            //    default:
    //            //        enemy_oneturn_max = Random.Range(2, 5);
    //            //        break;
    //            //}
    //            if (time > interval_s)//秒おきに生成
    //            {

    //                for (int i = 0; i < 1; i++) //同時生成処理
    //                {
    //                    //設定したエネミーMaxが
    //                    if (enemy_max < enemy_oneturn_max)
    //                    {
    //                        enemy_oneturn_max = enemy_max;
    //                    }

    //                    int Enemy_kinds_max = Random.Range(0, enemy_kinds_max); //////敵の位置関係？



    //                    int randomX = initpos[random].x;//Random.Range(0, max_x);   //// //敵のx座標の位置を入れる
    //                    int randomY = initpos[random].y;//Random.Range(0, max_y);  ////敵のy座標の位置を入れる　右辺で、y座標のスポーン位置を調整

    //                    if (random < 13) {
    //                        random++;
    //                    }

    //                    //生成する位置が誰もいない時 空いてるマスなら生成
    //                    if (map.Map[randomY, randomX] == (int)MapMass.Mapinfo.NONE)
    //                    {
    //                        Generation(Enemy_kinds_max, randomX, randomY);//引数(エネミーの種類 , スタートPos)生成。
    //                        if (sePlay != null) sePlay.Play("EnemySpawn");
    //                    }
    //                }
    //                time = 0;
    //            }

    //            if (enemy_count >= enemy_max)//最大値超えていたら何もしない。
    //            {
    //                enemy_max_flg = true;
    //                is_generation = false;
    //            }
    //            else
    //            {
    //                time += Time.deltaTime;
    //            }

    //        }//生成フラグがオンの時
    //        else//ここが敵が動いている時↓
    //        {
    //            //敵がの持ってく行動フラグを見ている。
    //            Debug.Log("search大きさ." + StageSarchEnemy.Length);
    //            for (int i = 0; i < StageSarchEnemy.Length; i++)
    //            {
    //                if (StageSarchEnemy[i].GetComponent<EnemyBase>().Is_action)
    //                {
    //                    if (StageSarchEnemy[i].GetComponent<Enemy>().Init_search_flg)
    //                    {

    //                        search_count++;
    //                        Debug.Log("searchカウント" + search_count);
    //                        StageSarchEnemy[i].GetComponent<Enemy>().Init_search_flg = false;
    //                    }
    //                }
    //            }


    //            if (search_count >= StageSarchEnemy.Length)//searchカウントがステージのsearchカウントより大きいとき enemy max flgがターンMaxFlgが大きい時
    //            {
    //                if (enemy_max_flg)
    //                {
    //                    turn_exit_flg = true;
    //                }
    //                else
    //                {
    //                    is_generation = true;
    //                }

    //            }
    //        }
    //    }

    //    if (turn_exit_flg && trunmanager.trunphase == TrunManager.TrunPhase.Enemy)
    //    {

    //        exit_time += Time.deltaTime;

    //        if (exit_time > 2)
    //        {
    //            turn_initflg = true;
    //            exit_time = 0;
    //            //Debug.Log("ターンカウントとトトと十っと夫とトト:" + Turn_Count);
    //            Turn_Count++;  //ターンを加算していく
    //            trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
    //            search_count = 0;
    //            //enemy_camera.depth = -2;
    //            for (int i = 0; i < StageSarchEnemy.Length; i++)
    //            {
    //                StageSarchEnemy[i].GetComponent<Enemy>().Init_search_flg = true;
    //            }

    //            sarchinit = true;
    //        }
    //    }

    //}

    //void Generation(int num, int x, int y)
    //{
    //    //rootpos[x].transform.GetChild(y).GetComponent<PseudoArray>().Whoisflg = true;
    //    //rootpos[x].transform.GetChild(y).GetComponent<PseudoArray>().Mass_status = MassStatus.ENEMY;//mapの位置に敵を入れる。
    //    map.Map[y, x] = (int)MapMass.Mapinfo.Enemy;

    //    //GameObject enemy_obj = rootpos[x].transform.GetChild(y).gameObject;
    //    Vector3 enemypos = new Vector3(map.Tilemas_prefab.transform.localScale.x * x, 0, -map.Tilemas_prefab.transform.localScale.z * y);//敵の出現位置

    //    //出現する魔法陣を生成
    //    ParticleSystem new_particle = Instantiate(enemy_particle[num], enemypos, enemy_particle[num].gameObject.transform.rotation);
    //    new_particle.Play();//再生

    //    Vector3 offset = new Vector3(0, 0.5f, 0);//キャラの高さ分調整用

    //    GameObject enemy_instantiate = Instantiate(enemy_prefab[num], enemypos + offset, new Quaternion(0, 180.0f, 0, 1));//敵を生成
    //    enemy_instantiate.name = enemy_prefab[num].name + enemy_count.ToString();

    //    //スタートポジションを教えてあげる。生成したプレファブに
    //    Enemy enemy = enemy_instantiate.GetComponent<Enemy>();

    //    switch (enemy.enemy_kinds)
    //    {
    //        case EnemyBase.EnemyKinds.Demon:
    //            enemy.X = x; enemy.Y = y; enemy.Enemy_action = EnemyBase.EnemyAction.Generation; enemy.Is_action = true;
    //            break;
    //        case EnemyBase.EnemyKinds.Demon1:
    //            enemy.X = x; enemy.Y = y; enemy.Enemy_action = EnemyBase.EnemyAction.Generation; enemy.Is_action = true;
    //            break;
    //        case EnemyBase.EnemyKinds.Boss:
    //            enemy.X = x; enemy.Y = y; enemy.Enemy_action = EnemyBase.EnemyAction.Generation; enemy.Is_action = true;
    //            break;
    //    }

    //    enemy_count++;//敵をカウント
    //    enemy_oneturn_count++;//1ターンでのカウント

    //    if (enemy_oneturn_count >= enemy_oneturn_max)  //enemy_oneturn_maxの数敵が出てくる：ここでターン＋
    //    {
    //        //Debug.Log("生成終了");
    //        oneturn_generation_flg = true;
    //        is_generation = false;//終了
    //        turn_exit_flg = true;
    //        general_init_flg = false;
    //        enemy_oneturn_count = 0;
    //    }
    //}

    //bool BossGeneration(int x, int y)
    //{
    //    map.Map[y, x] = (int)MapMass.Mapinfo.Enemy;//自分,中心値を設定                                           
    //    ////32方向 調べて、エネミーがないかチェック

    //    //時計回り
    //    for (int i = 0; i < 24; i++)
    //    {

    //    }
    //    boss_y[0] = y - 1; boss_x[0] = x;
    //    boss_y[1] = y; boss_x[1] = x + 1;
    //    boss_y[2] = y + 1; boss_x[2] = x;
    //    boss_y[3] = y; boss_x[3] = x - 1;
    //    boss_y[4] = y - 1;
    //    boss_y[5] = y + 1;
    //    boss_y[6] = y + 1;
    //    boss_y[7] = y - 1;

    //    //上 y-1, x
    //    //右 y, x + 1
    //    //下 y+1, x
    //    //左 y, x - 1

    //    //後ろ右 y - 1, x + 1
    //    //前右 y + 1, x + 1
    //    //前左 y + 1, x - 1
    //    //後ろ左 y - 1, x - 1



    //    //(16方向)
    //    //2段上 y - 2, x
    //    //2段下 y + 2, x
    //    //2段左 y, x - 2
    //    //2段右 y, x + 2
    //    //2段前右 y + 2, x + 2
    //    //2段前左 y + 2, x - 2
    //    //2段後ろ右 y - 2, x + 2
    //    //2段後ろ左 y - 2, x - 2
    //    // y + 2, x - 1
    //    // y + 2, x + 1
    //    // y + 1, x - 2
    //    // y + 1, x + 2
    //    // y - 1, x + 2
    //    // y - 1, x - 2
    //    // y - 2, x + 1
    //    // y - 2, x - 1


    //    for (int i = 0; i < 24; i++)
    //    {
    //        if (map.Map[boss_y[i], boss_x[i]] == (int)MapMass.Mapinfo.Enemy) { return false; }
    //    }
    //    return true; // trueがあったら生成

    //}

    //1ターン生成が終わったら抜ける。
}