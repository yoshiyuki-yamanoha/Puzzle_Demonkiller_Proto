using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyGenerationInfo
{
    public EnemyGenerationInfo(int one_turn, int goblin, int woman, int bom, int flamesword, bool generationflg)
    {
        this.One_turn_Generation = one_turn;
        this.Goblin = goblin;
        this.Woman = woman;
        this.Bom = bom;
        this.flamesword = flamesword;
        this.Generationflg = generationflg;
    }

    int one_turn;//1ターン湧く数

    int goblin;//ゴブリン
    int woman;//女性敵
    int bom;//ボム
    int flamesword;//炎の剣を持った敵
    bool generationflg;

    public int Goblin { get => goblin; set => goblin = value; }
    public int Woman { get => woman; set => woman = value; }
    public int Bom { get => bom; set => bom = value; }
    public int One_turn_Generation { get => one_turn; set => one_turn = value; }
    public int Flamesword { get => flamesword; set => flamesword = value; }
    public bool Generationflg { get => generationflg; set => generationflg = value; }
}

public class GenerationEnemy : MonoBehaviour /*PseudoArray*/
{
    Vector2Int path;
    Vector2Int flame_map_pos;
    //たーてっと座標
    List<Vector2Int> target_pos = new List<Vector2Int>();
    //敵登録 座標種類
    List<Vector2Int> enemys_pos = new List<Vector2Int>();
    List<int> enemys_kinds = new List<int>();
    bool pos_entry_flg = false;//敵の登録フラグ

    bool oneturn_flg_random_spawn = true;
    //bool is_spawn_pos = false;
    Vector2Int[] spawn_pos_range = new Vector2Int[4];

    int spawn_number = 0;//スポーン位置生成

    enum Mode
    {
        Debug,
        Game,
    }

    [SerializeField] Mode game_mode = Mode.Game;

    int nowturn = 0;
    EnemyGenerationInfo[] enemy_generation_info;

    //初期生成フラグ
    bool init_turn_generation_flg = true;//ここオフにしてしまった
    bool generation_flg = true;

    //敵生成情報
    int total_enemy = 0;//48 [1ゲーム]で生成出来る数
    float enemy_count = 0;//[1ゲーム]敵をカウント

    //ステージ敵検索用
    List<GameObject> activ_list_enemys = new List<GameObject>();
    List<GameObject> stage_list_enemys = new List<GameObject>();
    bool set_list_activ_flg = true;
    bool set_list_stage_flg = true;
    //bool one_turn_search_flg = true;

    //デバッグ用座標
    Vector2Int[] debug_pos = new Vector2Int[1];
    int debug_pos_num = 0;
    //炎で湧いた
    GameObject flame_obj;
    bool draw = true;

    //[1ターン]生成情報
    int enemy_oneturn_count = 0;

    //敵のターンスキップ
    bool init_skip = true;


    //mapの生成情報
    [HideInInspector] public int max_x = 0;//map横最大
    [HideInInspector] public int max_y = 0;//map縦最大

    //設定等
    int enemy_kinds_max = 0;//敵種類
    [SerializeField] GameObject[] enemy_prefab = null;//プレファブ格納変数
    [SerializeField] ParticleSystem[] enemy_particle = null;//敵の出現魔法陣
    [SerializeField] float interval_s = 5;//生成間隔(秒)
    float time = 0;//計測時間用
    //class 参照
    EnemyBase enemy_base = null;
    [SerializeField] MapMass map = null;
    TrunManager trunmanager = null;
    //具志堅SE処理
    SEManager sePlay = null;
    //具志堅BGM処理取得
    BGMManager bgmPlay = null;

    //敵を映すCameraのScript情報
    [SerializeField] EnemyCamera EC;

    //プロパティ
    public int Nowturn { get => nowturn; set => nowturn = value; }

    bool testinit = true;

    //[Awake]//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        InitEnemySpawnInfo();
        TotalEnemy();
    }

    //エネミースポーン情報初期化
    private void InitEnemySpawnInfo()
    {
        if (game_mode == Mode.Game)
        {
            enemy_generation_info = new EnemyGenerationInfo[5];//配列保存

            enemy_generation_info[0] = new EnemyGenerationInfo(5, 5, 0, 0, 0, true);
            enemy_generation_info[1] = new EnemyGenerationInfo(4, 0, 4, 0, 0, true);
            enemy_generation_info[2] = new EnemyGenerationInfo(9, 3, 3, 3, 0, true);
            enemy_generation_info[3] = new EnemyGenerationInfo(4, 1, 1, 2, 0, true);
            enemy_generation_info[4] = new EnemyGenerationInfo(8, 0, 3, 4, 1, true);

            //enemy_generation_info[0] = new EnemyGenerationInfo(5, 5, 0, 0, 0, true);
            //enemy_generation_info[1] = new EnemyGenerationInfo(4, 2, 2, 0, 0, true);
            //enemy_generation_info[2] = new EnemyGenerationInfo(9, 3, 3, 3, 0, true);
            //enemy_generation_info[3] = new EnemyGenerationInfo(4, 0, 2, 2, 0, true);
            //enemy_generation_info[4] = new EnemyGenerationInfo(10, 0, 5, 5, 0, true);
            //enemy_generation_info[5] = new EnemyGenerationInfo(16, 5, 5, 5, 1, true);

        }
        else
        {
            enemy_generation_info = new EnemyGenerationInfo[1];//配列保存
            enemy_generation_info[0] = new EnemyGenerationInfo(1, 0, 0, 0, 1, true);
            debug_pos[0] = new Vector2Int(12, 19);
        }
    }

    //1ターン生成する合計
    void TotalEnemy()
    {
        for (int i = 0; i < enemy_generation_info.Length; i++)
        {
            total_enemy += enemy_generation_info[i].One_turn_Generation;
        }

        //Debug.Log("敵の生成数" + total_enemy);
    }

    public int GetTotalEnemy()
    {
        return total_enemy;
    }

    // Start is called before the first frame update//////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        InitSpawnPos();
        Instance();
        Set_GenerationEnemy_Init();
        GenerationEnemy_BGM();
    }

    void InitSpawnPos()//湧く位置範囲
    {
        spawn_pos_range[0] = new Vector2Int(5, 2);//左上
        spawn_pos_range[1] = new Vector2Int(16, 3);//右上
        spawn_pos_range[2] = new Vector2Int(3, 16);//左下
        spawn_pos_range[3] = new Vector2Int(11, 17);//右下
    }

    void Instance()//インスタンス
    {
        //取得
        trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();//ターンマネージャ取得
        enemy_base = new EnemyBase();//エネミーベースインスタンス
    }

    void Set_GenerationEnemy_Init()//情報取得
    {
        max_x = map.Map.GetLength(1);//mapのx軸
        max_y = map.Map.GetLength(0);//mapのy軸
        enemy_kinds_max = enemy_prefab.Length;//敵の種類数
    }

    void GenerationEnemy_BGM()//BGM関係
    {
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
        bgmPlay = GameObject.Find("BGMAudio").GetComponent<BGMManager>();//Se再生用
        bgmPlay.Play("PLAYBGM");
    }

    //[FixedUpdate]///////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

    void EnemyTurn()
    {
        if (enemy_base != null) {
            enemy_base.DeleteListEnemy();//敵削除
        }

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
                GenerationEnemyPos();//座標登録
                GenerationEnemyKinds();//敵の種類登録
            }
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
                if (enemy_generation_info[Nowturn].Generationflg)
                {
                    time += Time.deltaTime;//カウント開始

                    if (time > interval_s)//スパンごと出現させる
                    {
                        //生成
                        InstanceEnemy(0, enemys_kinds[enemy_oneturn_count], enemys_pos[enemy_oneturn_count].x, enemys_pos[enemy_oneturn_count].y, target_pos[enemy_oneturn_count]);
                        time = 0;//タイム初期化
                    }
                }
            }
            else //ゲームモードがデバッグのみ
            {
                //InstanceEnemy(0, enemys_kinds[enemy_oneturn_count], debug_pos[0].x, debug_pos[0].y, target_pos[enemy_oneturn_count]);
                InstanceEnemy(0, enemys_kinds[enemy_oneturn_count], enemys_pos[enemy_oneturn_count].x, enemys_pos[enemy_oneturn_count].y, target_pos[enemy_oneturn_count]);
            }
        }

        if (EC.startFlag == false)
        {
            if (enemys_pos.Count <= enemy_oneturn_count)
            {
                SearchStageEnemy();//ステージ状の敵検索
                for (int num = 0; num < stage_list_enemys.Count; num++)
                {
                    stage_list_enemys[num].GetComponent<EnemyBase>().HPBar_On();
                }

                DebugDraw();
                enemy_oneturn_count = 0;
                enemys_pos.Clear();//一時的座標保存をclear
                target_pos.Clear();
                enemys_kinds.Clear();
                Debug.Log("stageリスト数" + stage_list_enemys.Count);
                init_turn_generation_flg = false;//初回ターンを終了
                Nowturn++;
                if (Nowturn >= enemy_generation_info.Length - 1)
                {
                    Nowturn = enemy_generation_info.Length - 1;
                }

                trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);//ターンをパズルに変更
            }
        }

    }

    //生成処理
    void Generation()
    {
        //生成フラグがオンなら
        if (generation_flg)
        {
            //SkipEnemy();//ターンをスキップする処理

            //if (!init_skip)
            //{//飛ばされたら登録する
            //    while (enemy_generation_info[Nowturn].One_turn_Generation > 0)//1ターンに生成できる数分回す
            //    {
            //        GenerationEnemyPos();//座標登録
            //        GenerationEnemyKinds();//敵の種類登録/////
            //    }
            //}

            //生成する
            while (enemys_pos.Count > enemy_oneturn_count)
            {
                if (enemy_generation_info[Nowturn].Generationflg)//生成フラグがtrue
                {
                    //Debug.Log("生成フラグON" + "Nowturn" + Nowturn + "座標登録数 " + enemys_pos.Count);
                    InstanceEnemy(0, enemys_kinds[enemy_oneturn_count], enemys_pos[enemy_oneturn_count].x, enemys_pos[enemy_oneturn_count].y, target_pos[enemy_oneturn_count]);
                }
            }

            enemy_generation_info[Nowturn].Generationflg = false;

            if (enemy_count >= total_enemy)//超えているかみる
            {
                generation_flg = false;//生成フラグをオフにする
            }
        }

        SearchStageEnemy();//敵をステージリスト追加

        if (EnemyIsAction())//敵が全員行動していたら確認。
        {
            for (int num = 0; num < stage_list_enemys.Count; num++)
            {
                stage_list_enemys[num].GetComponent<EnemyBase>().HPBar_On();
            }

            DebugDraw();

            enemy_oneturn_count = 0;

            if (stage_list_enemys.Count <= 10)
            {
                Nowturn++;
                if (Nowturn >= enemy_generation_info.Length - 1)
                {
                    Nowturn = enemy_generation_info.Length - 1;
                }
            }
            enemys_pos.Clear();//一時的座標保存をclear
            target_pos.Clear();
            enemys_kinds.Clear();
            flame_obj = null;//これなに
            trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);//ターンをパズルに変更
        }
        else
        {
            for (int num = 0; num < stage_list_enemys.Count; num++)
            {
                stage_list_enemys[num].GetComponent<EnemyBase>().HPBar_Off();
            }
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
        Vector2Int pos = new Vector2Int(spawn_pos_range[spawn_number].x + Random.Range(-1, 3), spawn_pos_range[spawn_number].y + Random.Range(-1, 3));

        //同時に処理されている。
        if (map.Map[pos.y, pos.x] == (int)MapMass.Mapinfo.NONE)
        {//mapになにもない所に
            //ここで敵のフェンス座標を割出す。
            SetTarget();
            map.Map[pos.y, pos.x] = (int)MapMass.Mapinfo.Enemy;//mapに敵の情報を渡す。
            enemys_pos.Add(pos);//エネミー座標を追加
            pos_entry_flg = true;
            path = pos;//仮追加
            //Debug.Log(/*"座標登録" + "Y[" + pos.y + "]" + " X[" + pos.x + "]" + */"Nowturn " + Nowturn + "登録されている数" + enemys_pos.Count);
        }

        oneturn_flg_random_spawn = true;
    }

    public List<Vector2Int> GetTarget()
    {
        return target_pos;
    }

    void SetTarget()
    {
        switch (spawn_number)
        {
            case 0://左上 バリケード座標
                //target_pos.Add(map.GetBari().pos[Random.Range(0, 2)]);//0~1
                CheckBari(map.GetBari().pos[Random.Range(0, 2)]);
                break;
            case 1://右上 バリケード座標 2~7
                //target_pos.Add(map.GetBari().pos[Random.Range(2, 8)]);//2~7
                CheckBari(map.GetBari().pos[Random.Range(2, 8)]);
                break;
            case 2://左下 バリケード座標 8~13
                //target_pos.Add(map.GetBari().pos[Random.Range(8, 14)]);//0~1
                CheckBari(map.GetBari().pos[Random.Range(8, 14)]);
                break;
            case 3://右下 コア座標
                target_pos.Add(map.GetCore().pos[Random.Range(0, 1)]);
                break;
        }
    }

    void CheckBari(Vector2Int baripos)
    {
        if (map.Map[baripos.y, baripos.x] == (int)MapMass.Mapinfo.bari)
        {
            target_pos.Add(baripos);
        }
        else
        {
            target_pos.Add(map.GetCore().pos[Random.Range(0, 1)]);
        }
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
                SetFlame(path);
            }
            else
            {
                enemy_kinds = 0;//ゴブリン
            }
            enemys_kinds.Add(enemy_kinds);
        }
    }

    void InstanceEnemy(int magic_num, int enemy_kinds, int x, int y, Vector2Int target_pos)//生成
    {
        //Debug.Log("敵を生成しています。" + " Y "+ y + "X" + x);
        Vector3 enemypos = new Vector3(map.Tilemas_prefab.transform.localScale.x * x, 0, -map.Tilemas_prefab.transform.localScale.z * y);//敵の出現位置

        //出現する魔法陣を生成/////////////////////////////////////////////////////////////////////////////////////////////////////
        ParticleSystem new_particle = Instantiate(enemy_particle[magic_num], enemypos, enemy_particle[magic_num].gameObject.transform.rotation);
        new_particle.Play();//再生
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        Vector3 offset = new Vector3(0, 0.5f, 0);//キャラの高さ分調整用
        GameObject enemy_instantiate = Instantiate(enemy_prefab[enemy_kinds], enemypos + offset, Quaternion.identity);//敵を生成
        sePlay.Play("EnemySpawn");

        if (enemy_kinds == 3)//炎の敵だったら入る。
        {
            if (flame_obj == null)
            {
                flame_obj = enemy_instantiate;
            }
        }
        //生成時の方向
        Vector3 v3_target_pos = new Vector3(target_pos.x * map.Tilemas_prefab.transform.localScale.x, 0, target_pos.y * -map.Tilemas_prefab.transform.localScale.z);
        Vector3 dir = /*map.GetCore().obj[0].transform.position*/v3_target_pos - enemy_instantiate.transform.position;
        dir.y = 0;
        Quaternion quaternion = Quaternion.LookRotation(dir);

        enemy_instantiate.transform.rotation = quaternion;

        enemy_instantiate.GetComponent<EnemyBase>().UIFacing();//UIが向く

        enemy_instantiate.name = enemy_prefab[enemy_kinds].name + enemy_count.ToString();//敵の名前を変更
        oneturn_flg_random_spawn = true;

        switch (enemy_kinds)
        {
            case 0:
                SetGoblinInfo(enemy_instantiate, x, y, target_pos);
                break;
            case 1:
                SetWoManEnemyInfo(enemy_instantiate, x, y, target_pos);
                break;
            case 2:
                SetBomInfo(enemy_instantiate, x, y, target_pos);
                break;
            case 3:
                SetFlameSwordInfo(enemy_instantiate, x, y, target_pos);
                break;
        }

        enemy_count++;
        enemy_oneturn_count++;
    }

    void SetFlameSwordInfo(GameObject enemy_instantiate, int x, int y, Vector2Int target_pos)
    {
        FlameSwordMove enemy_info = enemy_instantiate.GetComponent<FlameSwordMove>();//敵情報取得
        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true; enemy_info.SetTargetPos(target_pos); enemy_info.SetPos_Register();
    }

    void SetBomInfo(GameObject enemy_instantiate, int x, int y, Vector2Int target_pos)
    {
        BombEnemy enemy_info = enemy_instantiate.GetComponent<BombEnemy>();//敵情報取得
        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true; enemy_info.SetTargetPos(target_pos); enemy_info.SetPos_Register();
    }

    void SetGoblinInfo(GameObject enemy_instantiate, int x, int y, Vector2Int target_pos)
    {
        Enemy enemy_info = enemy_instantiate.GetComponent<Enemy>();//敵情報取得
        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true; enemy_info.SetTargetPos(target_pos); enemy_info.SetPos_Register();
    }

    void SetWoManEnemyInfo(GameObject enemy_instantiate, int x, int y, Vector2Int target_pos)
    {
        Enemy enemy_info = enemy_instantiate.GetComponent<Enemy>();//敵情報取得
        enemy_info.X = x; enemy_info.Y = y; enemy_info.Enemy_action = EnemyBase.EnemyAction.Generation; enemy_info.Is_action = true; enemy_info.SetTargetPos(target_pos); enemy_info.SetPos_Register();
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

    //void SkipEnemy()
    //{
    //    if (!init_turn_generation_flg && init_skip)
    //    {
    //        if (stage_list_enemys.Count <= 0)//0以下なら存在しない
    //        {
    //            while (enemy_generation_info[Nowturn].One_turn_Generation <= 0) //1ターン生成が0以下だったらNowターン追加
    //            {
    //                Nowturn++;
    //            }
    //            init_skip = false;
    //        }
    //    }
    //}

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
            search_flg = true;//検索終了
        }
        return search_flg;
    }

    public bool fireEnemyGetFlag = false;//炎の剣を持った敵の座標を受け取ったフラグ
    void SetFlame(Vector2Int pos)
    {
        flame_map_pos = pos;
        fireEnemyGetFlag = true;
    }

    public Vector3 FlameGameObject()
    {
        Vector3 pos = new Vector3(flame_map_pos.x * map.Tilemas_prefab.transform.localScale.x, 0, flame_map_pos.y * -map.Tilemas_prefab.transform.localScale.z);
        return pos;
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

    void EnemyTurnExit()
    {
        //リセットフラグ
        set_list_activ_flg = true;
        set_list_stage_flg = true;
        stage_list_enemys.Clear();//ステージ状リストclear
        init_skip = true;
    }
}