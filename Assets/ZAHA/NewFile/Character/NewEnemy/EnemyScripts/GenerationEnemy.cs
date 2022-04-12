using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerationEnemy : PseudoArray
{
    int search_count = 0;
    //インスペクターで設定用
    [SerializeField] bool is_generation = false;
    [SerializeField] GameObject[] enemy_prefab = null;//プレファブ格納変数
    [SerializeField] ParticleSystem[] enemy_particle = null;
    [SerializeField] int enemy_max = 10;//敵最大値
    [SerializeField] float interval_s = 5;//生成感覚

    //mapの生成情報
    [SerializeField] MapMass map;
    [HideInInspector] public int max_x = 0;//横最大
    [HideInInspector] public int max_y = 0;//縦最大
    [SerializeField] int y;//縦方向

    float enemy_count = 0;//エネミーカウント
    int enemy_kinds_max = 0;//敵種類
    int enemy_oneturn_count = 0;
    int enemy_oneturn_max = 4;
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

    // Start is called before the first frame update
    void Start()
    {
        //max_x = rootpos.Length;//スタートポジションの数分取得
        //max_y = rootpos[0].transform.childCount;//子供の数取得
        max_x = map.Map.GetLength(1);
        max_y = map.Map.GetLength(0);

        enemy_kinds_max = enemy_prefab.Length;
        trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();

        if(sePlay != null)sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StageSarchEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        //自分のターンの時
        if (trunmanager.trunphase == TrunManager.TrunPhase.Enemy) {
            if (turn_initflg) {
                //Debug.Log("敵のターン");
                turn_exit_flg = false;//抜けるオフ
                turn_initflg = false;
            }
        }
        

        if (!turn_exit_flg) //!抜けるフラグ
        {
            //生成する状態なら
            if (is_generation)
            {
                if (time > interval_s)//秒おきに生成
                {
                    
                    for (int i = 0; i < 1; i++) //同時生成処理
                    {
                        //設定したエネミーMaxが
                        if (enemy_max < enemy_oneturn_max)
                        {
                            enemy_oneturn_max = enemy_max;
                        }

                        int Enemy_kinds_max = Random.Range(0, enemy_kinds_max);
                        int randomX = Random.Range(0, max_x);
                        int randomY = Random.Range(0, max_y - 7);

                        //生成する位置が誰もいない時 敵以外なら生成
                        if (map.Map[randomY , randomX] != (int)MapMass.Mapinfo.Enemy)
                        {
                            Generation(Enemy_kinds_max, randomX, randomY);//引数(エネミーの種類 , スタートPos)生成。
                            if (sePlay != null) sePlay.Play("EnemySpawn");
                        }


                        //if (rootpos[randomX].transform.GetChild(y).GetComponent<PseudoArray>().Mass_status != MassStatus.ENEMY)
                        //{
                        //}

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
                for (int i = 0; i < StageSarchEnemy.Length; i++)
                {
                    if (StageSarchEnemy[i].GetComponent<Enemy>().Is_action)
                    {
                        if (StageSarchEnemy[i].GetComponent<Enemy>().Init_search_flg)
                        {
                            
                            search_count++;
                            StageSarchEnemy[i].GetComponent<Enemy>().Init_search_flg = false;
                        }
                    }
                }

                if (search_count >= StageSarchEnemy.Length)
                {
                    if (enemy_max_flg) {
                        turn_exit_flg = true;
                    }
                    else
                    {
                        is_generation = true;
                    }
                    
                }
            }

        }

        if (turn_exit_flg && trunmanager.trunphase ==  TrunManager.TrunPhase.Enemy)
        {

            exit_time += Time.deltaTime;

            if (exit_time > 2)
            {
                turn_initflg = true;
                exit_time = 0;
                trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
                search_count = 0;
                //enemy_camera.depth = -2;
                for (int i = 0; i < StageSarchEnemy.Length; i++)
                {
                    StageSarchEnemy[i].GetComponent<Enemy>().Init_search_flg = true;
                }
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
        ParticleSystem new_particle = Instantiate(enemy_particle[num], enemypos,enemy_particle[num].gameObject.transform.rotation);
        new_particle.Play();//再生

        Vector3 offset = new Vector3(0,  0.5f, 0);//キャラの高さ分調整用

        GameObject enemy_instantiate = Instantiate(enemy_prefab[num], enemypos + offset, new Quaternion(0, 180.0f, 0, 1));//生成
        enemy_instantiate.name =  enemy_prefab[num].name + enemy_count.ToString();

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

        if (enemy_oneturn_count >= enemy_oneturn_max)
        {
            is_generation = false;//終了
            turn_exit_flg = true;
            enemy_oneturn_count = 0;
        }
    }

    bool BossGeneration(int x , int y)
    {
        map.Map[y, x] = (int)MapMass.Mapinfo.Enemy;//自分,中心値を設定                                           
        ////32方向 調べて、エネミーがないかチェック

        //時計回り
        for (int i= 0; i < 24; i++)
        {

        }
        boss_y[0] = y - 1; boss_x[0] = x;
        boss_y[1] = y;     boss_x[1] = x + 1;
        boss_y[2] = y + 1; boss_x[2] = x;
        boss_y[3] = y;     boss_x[3] = x - 1;
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


        for (int i = 0; i < 24; i++) {
            if (map.Map[boss_y[i], boss_x[i]] == (int)MapMass.Mapinfo.Enemy) { return false; }
        }
        return true; // trueがあったら生成

    }
}