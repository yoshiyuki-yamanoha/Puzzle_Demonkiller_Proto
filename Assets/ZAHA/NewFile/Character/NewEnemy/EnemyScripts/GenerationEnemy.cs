using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyGenerationInfo
{
    public EnemyGenerationInfo(int one_turn, int goblin, int woman, int bom, int flamesword)
    {
        this.One_turn_Generation = one_turn;
        this.Goblin = goblin;
        this.Woman = woman;
        this.Bom = bom;
        this.flamesword = flamesword;
    }

    int one_turn;//1ターン湧く数

    int goblin;//ゴブリン
    int woman;//女性敵
    int bom;//ボム
    int flamesword;//炎の剣を持った敵

    public int Goblin { get => goblin; set => goblin = value; }
    public int Woman { get => woman; set => woman = value; }
    public int Bom { get => bom; set => bom = value; }
    public int One_turn_Generation { get => one_turn; set => one_turn = value; }
    public int Flamesword { get => flamesword; set => flamesword = value; }
}

public class GenerationEnemy : MonoBehaviour /*PseudoArray*/
{
    List<Vector2Int> enemys_pos = new List<Vector2Int>();
    List<int> enemys_kinds = new List<int>();

    bool pos_entry_flg = false;//敵の登録フラグ

    Vector3 tmppos;
    Vector3 tmp_enemy_pos;

    //生成の位置
    bool oneturn_flg_random_spawn = true;
    //bool is_spawn_pos = false;
    Vector2Int[] spawn_pos = new Vector2Int[4];


    bool init_enemys_tmpposflg = true;

    int spawn_number = 0;//スポーン位置生成

    enum Mode
    {
        Debug,
        Game,
    }
    //炎で湧いた
    GameObject flame_obj;

    bool draw = true;

    [SerializeField] Mode game_mode = Mode.Game;

    EnemyGenerationInfo[] enemy_generation_info;
    int nowturn = 0;

    //class 参照
    EnemyBase enemy_base = null;
    [SerializeField] MapMass map = null;
    TrunManager trunmanager = null;

    //具志堅SE処理
    SEManager sePlay = null;
    //具志堅BGM処理取得
    BGMManager bgmPlay = null;

    //初期生成フラグ
    bool init_turn_generation_flg = true;//ここオフにしてしまった
    bool generation_flg = true;

    //敵生成情報
    int enemy_kinds_max = 0;//敵種類
    [SerializeField] GameObject[] enemy_prefab = null;//プレファブ格納変数
    [SerializeField] ParticleSystem[] enemy_particle = null;//敵の出現魔法陣
    int enemy_max = 48;//[1ゲーム]で生成出来る数
    float enemy_count = 0;//[1ゲーム]敵をカウント
    [SerializeField] float interval_s = 5;//生成間隔(秒)
    float time = 0;//計測時間用

    //ステージ敵検索用
    List<GameObject> activ_list_enemys = new List<GameObject>();
    List<GameObject> stage_list_enemys = new List<GameObject>();
    bool set_list_activ_flg = true;
    bool set_list_stage_flg = true;
    //bool one_turn_search_flg = true;

    Vector2Int[] debug_pos = new Vector2Int[1];
    int debug_pos_num = 0;

    //[1ターン]生成情報
    int enemy_oneturn_count = 0;

    bool init_skip = true;
    //[SerializeField] int enemy_oneturn_max = 5;  //1ターンに出る敵の最大大数

    //mapの生成情報
    [HideInInspector] public int max_x = 0;//map横最大
    [HideInInspector] public int max_y = 0;//map縦最大

    //他のスクリプトから参照してた奴
    public bool initflg = false;

    //敵を映すCameraのScript情報
    [SerializeField] EnemyCamera EC;

    public int Nowturn { get => nowturn; set => nowturn = value; }

    bool testinit = true;

    // Start is called before the first frame update
    void Start()
    {
        InitSpawnPos();
        InitEnemySpawnInfo();
        GetInstance();
        GenerationEnemy_Init();
        GenerationEnemy_BGM();

    }

    void EnemyTurn()
    {
        enemy_base.DeleteListEnemy();//敵削除

        if (init_turn_generation_flg)//最初のターン
        {
            InitTurnGeneration();//最初の生成処理
        }
        else//最初のターン以降
        {
            Generation();//生成処理
        }
    }

    void MagicAttackTurn()//魔法のターン
    {
        if (generation_flg)
        {
            while (enemy_generation_info[Nowturn].One_turn_Generation > 0)//1ターンに生成できる数分回す
            {
                Debug.Log("Nowturn" + Nowturn);
                GenerationEnemyPos();//座標登録
                GenerationEnemyKinds();//敵の種類登録
            }
        }
    }

    private void FixedUpdate()
    {
        switch (trunmanager.GetTrunPhase())
        {
            case TrunManager.TrunPhase.Enemy:
                EnemyTurn();
                break;
            case TrunManager.TrunPhase.Puzzle:
                EnemyTurnExit();
                break;
            case TrunManager.TrunPhase.MagicAttack:
                MagicAttackTurn();
                break;
        }
    }

    void InitSpawnPos()
    {
        spawn_pos[0] = new Vector2Int(5, 2);
        spawn_pos[1] = new Vector2Int(15, 2);
        spawn_pos[2] = new Vector2Int(2, 16);
        spawn_pos[3] = new Vector2Int(12, 16);
    }

    //エネミースポーン情報初期化
    private void InitEnemySpawnInfo()
    {
        if (game_mode == Mode.Game)
        {
            enemy_generation_info = new EnemyGenerationInfo[10];//配列保存

            enemy_generation_info[0] = new EnemyGenerationInfo(5, 5, 0, 0, 0);
            enemy_generation_info[1] = new EnemyGenerationInfo(3, 0, 3, 0, 0);
            enemy_generation_info[2] = new EnemyGenerationInfo(4, 2, 0, 2, 0);
            enemy_generation_info[3] = new EnemyGenerationInfo(0, 0, 0, 0, 0);
            enemy_generation_info[4] = new EnemyGenerationInfo(10, 10, 0, 0, 0);
            enemy_generation_info[5] = new EnemyGenerationInfo(0, 0, 0, 0, 0);
            enemy_generation_info[6] = new EnemyGenerationInfo(10, 0, 5, 5, 0);
            enemy_generation_info[7] = new EnemyGenerationInfo(0, 0, 0, 0, 0);
            enemy_generation_info[8] = new EnemyGenerationInfo(0, 0, 0, 0, 0);
            enemy_generation_info[9] = new EnemyGenerationInfo(16, 5, 5, 5, 1);
        }
        else
        {
            enemy_generation_info = new EnemyGenerationInfo[1];//配列保存
            enemy_generation_info[0] = new EnemyGenerationInfo(1, 1, 0, 0, 0);
            debug_pos[0] = new Vector2Int(15, 7);
        }
    }

    //初回ターンに生成する処理
    void InitTurnGeneration()
    {

        while (enemy_generation_info[Nowturn].One_turn_Generation > 0)//1ターンに生成できる数分回す
        {
            GenerationEnemyPos();//座標登録
            GenerationEnemyKinds();//敵の種類登録
        }

        if (enemys_pos.Count > enemy_oneturn_count)
        {
            //SearchGeneration();
            if (game_mode == Mode.Game) //ゲームモードがメインのみ
            {
                //生成
                InstanceEnemy(0, enemys_kinds[enemy_oneturn_count], enemys_pos[enemy_oneturn_count].x, enemys_pos[enemy_oneturn_count].y);
            }
            else //ゲームモードがデバッグのみ
            {
                InstanceEnemy(0, enemys_kinds[enemy_oneturn_count], debug_pos[0].x, debug_pos[0].y);
            }
        }

        if (EC.startFlag == false)
        {
            DebugDraw();

            enemy_oneturn_count = 0;
            enemys_pos.Clear();//一時的座標保存をclear
            enemys_kinds.Clear();
            init_turn_generation_flg = false;//初回ターンを終了
            Nowturn++;
            trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);//ターンをパズルに変更
        }

    }

    //生成処理
    void Generation()
    {
        SearchStageEnemy();//敵をステージリスト追加

        //生成フラグがオンなら
        if (generation_flg)
        {
            SkipEnemy();//ターンをスキップする処理

            //生成する
            while(enemys_pos.Count > enemy_oneturn_count)
            {
                //Debug.Log("生成フラグON" + "Nowturn" + Nowturn + "座標登録数 " + enemys_pos.Count);
                InstanceEnemy(0, enemys_kinds[enemy_oneturn_count], enemys_pos[enemy_oneturn_count].x, enemys_pos[enemy_oneturn_count].y);
            }

            if (enemy_count >= enemy_max)//超えているかみる
            {
                generation_flg = false;
            }

            if (Nowturn >= enemy_generation_info.Length - 1) { generation_flg = false; Nowturn = enemy_generation_info.Length - 1; } //現在のターンが21ターン以降は生成を止める
        }

        if (EnemyIsAction())//敵が全員行動していたら確認。
        {
            DebugDraw();

            enemy_oneturn_count = 0;
            if (stage_list_enemys.Count <= 32)
                Nowturn++;

            enemys_pos.Clear();//一時的座標保存をclear
            enemys_kinds.Clear();
            flame_obj = null;//これなに
            trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);//ターンをパズルに変更
        }
    }

    //生成位置
    void GenerationEnemyPos()
    {
        pos_entry_flg = false;

        //1～4箇所の場所をランダムで決める。
        if (oneturn_flg_random_spawn)//1ターン1回のみ
        {
            spawn_number = Random.Range(0, 4);
            oneturn_flg_random_spawn = false;
        }

        //敵の座標をインスタンス
        Vector2Int pos = new Vector2Int(spawn_pos[spawn_number].x + Random.Range(-1, 3), spawn_pos[spawn_number].y + Random.Range(-1, 3));

        //同時に処理されている。
        if (map.Map[pos.y, pos.x] == (int)MapMass.Mapinfo.NONE)
        {//mapになにもない所に
            map.Map[pos.y, pos.x] = (int)MapMass.Mapinfo.Enemy;//mapに敵の情報を渡す。
            enemys_pos.Add(pos);//エネミー座標を追加
            pos_entry_flg = true;
            Debug.Log(/*"座標登録" + "Y[" + pos.y + "]" + " X[" + pos.x + "]" + */"Nowturn " + Nowturn +"登録されている数" + enemys_pos.Count);
        }

        oneturn_flg_random_spawn = true;
    }

    //敵の種類をきめて処理を返す
    //1ターンで制御出来る処理
    void GenerationEnemyKinds()
    {
        if (pos_entry_flg)
        {//位置座標が登録されたら

            int enemy_kinds;

            if (enemy_generation_info[Nowturn].Goblin > 0)//ゴブリン
            {
                enemy_generation_info[Nowturn].One_turn_Generation--;
                enemy_generation_info[Nowturn].Goblin--;
                enemy_kinds = 1;

                //デバッグ座標の表示
                debug_pos_num++;
                if (debug_pos.Length < debug_pos_num)
                {
                    debug_pos_num = debug_pos.Length;
                }
            }
            else if (enemy_generation_info[Nowturn].Bom > 0)//ボム
            {
                enemy_generation_info[Nowturn].One_turn_Generation--;
                enemy_generation_info[Nowturn].Bom--;
                enemy_kinds = 2;
            }
            else if (enemy_generation_info[Nowturn].Woman > 0)//女性敵
            {
                enemy_generation_info[Nowturn].One_turn_Generation--;
                enemy_generation_info[Nowturn].Woman--;
                enemy_kinds = 0;
            }
            else if (enemy_generation_info[Nowturn].Flamesword > 0)//炎の敵
            {
                enemy_generation_info[Nowturn].One_turn_Generation--;
                enemy_generation_info[Nowturn].Flamesword--;
                enemy_kinds = 3;
            }
            else
            {
                enemy_kinds = 0;//ゴブリン
            }


            if (enemy_kinds == 1) //敵に応じて出現するSEの音を変える //1：ゴブリン
            {
                sePlay.Play("GoblinSpawn");
            }
            else if (enemy_kinds == 0) //2：デーモン
            {
                sePlay.Play("EnemySpawn");//SEを鳴らす
            }
            else if (enemy_kinds == 2) //3:ボム兵
            {
                sePlay.Play("BombSpawn");//SEを鳴らす  //SE変更予定
            }
            else if (enemy_kinds == 3)//4：炎の剣
            {
                sePlay.Play("FlameSpawn");
            }

            enemys_kinds.Add(enemy_kinds);
        }
    }

    void EnemyTurnExit()
    {
        //リセットフラグ
        set_list_activ_flg = true;
        set_list_stage_flg = true;
        stage_list_enemys.Clear();//ステージ状リストclear
        init_skip = true;
    }

    void SetListSearchEnemy()
    {
        if (set_list_activ_flg)//1ターンに1回取得する
        {
            GameObject[] search_enemys = null;//エネミータグで取得格納配列
            search_enemys = GameObject.FindGameObjectsWithTag("Enemy");//敵をタグで取得
            //foreach (var search_enemy in search_enemys)
            //{
            //    activ_list_enemys.Add(search_enemy);//タグで取得した敵をリストに追加
            //}

            for (int enemy_num = 0; enemy_num < search_enemys.Length; enemy_num++)
            {
                activ_list_enemys.Add(search_enemys[enemy_num]);
            }

            set_list_activ_flg = false;
        }
    }

    void SkipEnemy()
    {
        if (!init_turn_generation_flg && init_skip)
        {
            if (stage_list_enemys.Count <= 0)//0以下なら存在しない
            {
                while (enemy_generation_info[Nowturn].One_turn_Generation <= 0) //1ターン生成が0以下だったらNowターン追加
                {
                    Nowturn++;
                }

                init_skip = false;
            }
        }
    }

    void SearchStageEnemy()
    {
        SetListStageEnemy();


        for (int list_number = stage_list_enemys.Count - 1; list_number >= 0; list_number--)
        {
            if (stage_list_enemys[list_number] == null)//リストに入っていた死亡して敵が消えてた場合はリストから削除
            {
                stage_list_enemys.Remove(stage_list_enemys[list_number]);//敵をリストから消す
            }
            else
            {
                if (stage_list_enemys[list_number].GetComponent<EnemyBase>().Deathflg)
                {
                    stage_list_enemys.Remove(stage_list_enemys[list_number]);//敵をリストから消す
                }
            }
        }
    }

    void SetListStageEnemy()
    {
        if (set_list_stage_flg)//1ターンに1回取得する
        {
            GameObject[] search_enemys = null;//エネミータグで取得格納配列
            search_enemys = GameObject.FindGameObjectsWithTag("Enemy");//敵をタグで取得
            foreach (var search_enemy in search_enemys)
            {
                stage_list_enemys.Add(search_enemy);//タグで取得した敵をリストに追加
            }
            set_list_stage_flg = false;
        }
    }

    bool EnemyIsAction()
    {
        bool search_flg = false;

        SetListSearchEnemy();

        //for分に変更する //これで変更されたはず
        for (int list_number = activ_list_enemys.Count - 1; list_number >= 0; list_number--)
        {
            if (activ_list_enemys[list_number] == null)//リストに入っていた死亡して敵が消えてた場合はリストから削除
            {
                activ_list_enemys.Remove(activ_list_enemys[list_number]);//敵をリストから消す
            }
            else
            {
                if (activ_list_enemys[list_number].GetComponent<EnemyBase>().Is_action)//行動が終わったか確認
                {
                    activ_list_enemys.Remove(activ_list_enemys[list_number]);//行動が終わった敵はリストから削除。
                }
            }
        }

        if (activ_list_enemys.Count <= 0)//リストが0になぅったら
        {
            //trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
            search_flg = true;//検索終了
        }
        return search_flg;
    }

    //void Generation(Vector2Int pos)
    //{
    //    //var emptyMass = FindingEmptyMass(pos);

    //    //if (map.Map[pos.y + Random.Range(-1, 2), pos.x + Random.Range(-1, 2)] == (int)MapMass.Mapinfo.NONE)
    //    if (map.Map[pos.y, pos.x] == (int)MapMass.Mapinfo.NONE) //何もない場所だったら
    //    {
    //        time += Time.deltaTime;//カウント開始
    //        if (time > interval_s)//スパンごと出現させる
    //        {


    //            time = 0;//タイム初期化
    //        }
    //    }
    //}

    void InstanceEnemy(int magic_num, int enemy_kinds, int x, int y)//生成
    {
        //Debug.Log("敵を生成しています。" + " Y "+ y + "X" + x);
        Vector3 enemypos = new Vector3(map.Tilemas_prefab.transform.localScale.x * x, 0, -map.Tilemas_prefab.transform.localScale.z * y);//敵の出現位置

        //出現する魔法陣を生成/////////////////////////////////////////////////////////////////////////////////////////////////////
        ParticleSystem new_particle = Instantiate(enemy_particle[magic_num], enemypos, enemy_particle[magic_num].gameObject.transform.rotation);
        new_particle.Play();//再生
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Vector3 offset = new Vector3(0, 0.5f, 0);//キャラの高さ分調整用
        GameObject enemy_instantiate = Instantiate(enemy_prefab[enemy_kinds], enemypos + offset, Quaternion.identity);//敵を生成

        if (enemy_kinds == 3)//炎の敵だったら入る。
        {
            if (flame_obj == null)
            {
                flame_obj = enemy_instantiate;
            }
        }

        Vector3 dir = map.GetCore().obj[0].transform.position - enemy_instantiate.transform.position;
        dir.y = 0;
        Quaternion quaternion = Quaternion.LookRotation(dir);
        enemy_instantiate.transform.rotation = quaternion;

        enemy_instantiate.GetComponent<EnemyBase>().UIFacing();//UIが向くよーん

        enemy_instantiate.name = enemy_prefab[enemy_kinds].name + enemy_count.ToString();//敵の名前を変更
        oneturn_flg_random_spawn = true;

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

        enemy_count++;
        enemy_oneturn_count++;
    }

    void SetPos(Vector3 tmppos, Vector3 tmp_enemy_pos)
    {
        this.tmppos = tmppos;
        this.tmp_enemy_pos = tmp_enemy_pos;
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
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
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

    public GameObject FlameGameObject()
    {
        return flame_obj;
    }

    void DebugDraw()
    {
        if (draw)
        {
            string print_array = "";
            for (int y = 0; y < map.Map.GetLength(0); y++)
            {
                for (int x = 0; x < map.Map.GetLength(1); x++)
                {
                    print_array += map.Map[y, x].ToString() + ":";
                }
                print_array += "\n";
            }
            Debug.Log(print_array);
            draw = false;
        }

        draw = true;
    }
    // 敵が生成できるマスを探す
    //int[,] FindingEmptyMass(Vector2Int pos)
    //{
    //    int[,] emptyMass = new int[1, 1];
    //    // + Random.Range(-1, 2)
    //    if (map.Map[pos.y, pos.x] == (int)MapMass.Mapinfo.NONE)
    //        emptyMass = map.Map;

    //    return emptyMass;
    //}

    //foreach (var search_enemy in activ_list_enemys)//取得した敵分リストを回す
    //{
    //    if (search_enemy == null)//リストに入っていた死亡して敵が消えてた場合はリストから削除
    //    {
    //        activ_list_enemys.Remove(search_enemy);//敵をリストから消す
    //    }
    //    else
    //    {
    //        if (search_enemy.GetComponent<EnemyBase>().Is_action)//行動が終わったか確認
    //        {
    //            activ_list_enemys.Remove(search_enemy);//行動が終わった敵はリストから削除。
    //        }
    //    }
    //}

    //SetListSearchEnemy(stage__list_enemys , set_list_stage_flg);
    //if (one_turn_search_flg)//1ターンに1回取得する
    //{
    //    GameObject[] search_enemys = null;//エネミータグで取得格納配列
    //    search_enemys = GameObject.FindGameObjectsWithTag("Enemy");//敵をタグで取得
    //    Debug.Log("敵のリスト数" + search_enemys.Length);
    //    foreach (var search_enemy in search_enemys)
    //    {
    //        search_list_enemys.Add(search_enemy);//タグで取得した敵をリストに追加
    //    }
    //    one_turn_search_flg = false;
    //}

    //foreach (var search_enemy in stage_list_enemys)//取得した敵分リストを回す
    //{
    //    if (search_enemy == null)//リストに入っていた死亡して敵が消えてた場合はリストから削除
    //    {
    //        stage_list_enemys.Remove(search_enemy);//敵をリストから消す
    //    }
    //    else
    //    {
    //        if (search_enemy.GetComponent<EnemyBase>().Deathflg)//行動が終わったか確認
    //        {
    //            stage_list_enemys.Remove(search_enemy);//行動が終わった敵はリストから削除。
    //        }
    //    }
    //}


    //Debug.Log("ランダム番号 " + spawn_number);

    //
    //Generation(new Vector2Int(Random.Range(0, 19), Random.Range(0, 13)));//)//場所設定
    //Generation(new Vector2Int(Random.Range(0, 19), Random.Range(0, 13)));//)//場所設定
    //}
    //else
    //{

    // 26回呼ばれている。
    //if (oneturn_spawnpos) {
    //    MapSpawnGeneration();
    //    oneturn_spawnpos = false;
    //}

    //Generation(spawn_pos);//)//場所設定
    //}
}