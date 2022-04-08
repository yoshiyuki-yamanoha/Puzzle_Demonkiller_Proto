using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationEnemy : PseudoArray
{
    int search_count = 0;

    [SerializeField] bool is_generation = false;
    [SerializeField] GameObject[] enemy_prefab = null;//プレファブ格納変数
    [SerializeField] int enemy_max = 10;//敵最大値
    float enemy_count = 0;//エネミーカウント
    int enemy_kinds_max = 0;//敵種類
    int enemy_oneturn_count = 0;
    int enemy_oneturn_max = 4;
    [SerializeField] float interval_s = 5;//生成感覚
    [SerializeField] ParticleSystem[] enemy_particle = null;
    public GameObject[] rootpos = null;//親オブジェクト
    [SerializeField] int y;//縦方向
    [HideInInspector] public int max_x = 0;//横最大
    [HideInInspector] public int max_y = 0;//縦最大
    public bool initflg = true;
    float time = 0;
    TrunManager trunmanager = null;
    GameObject[] StageSarchEnemy = null;

    bool enemy_max_flg = false;
    //bool initturnflg = false;

    float exit_time = 0;

    //ターン終了フラグ
    bool turn_exit_flg = false;
    bool turn_initflg = true;


    //具志堅SE処理
    SEManager sePlay;

    // Start is called before the first frame update
    void Start()
    {
        max_x = rootpos.Length;//スタートポジションの数分取得
        max_y = rootpos[0].transform.childCount;//子供の数取得
        enemy_kinds_max = enemy_prefab.Length;
        trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();

        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
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
                        if (rootpos[randomX].transform.GetChild(y).GetComponent<PseudoArray>().Mass_status != MassStatus.ENEMY)
                        {
                            Generation(Enemy_kinds_max, randomX, randomY);//引数(エネミーの種類 , スタートPos)生成。
                            sePlay.Play("EnemySpawn");
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
                //Debug.Log("敵の数"+StageSarchEnemy.Length);
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
                        //Debug.Log(search_count);
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
        rootpos[x].transform.GetChild(y).GetComponent<PseudoArray>().Mass_status = MassStatus.ENEMY;//mapの位置に敵を入れる。

        GameObject enemy_obj = rootpos[x].transform.GetChild(y).gameObject;

        //出現する魔法陣を生成
        ParticleSystem new_particle = Instantiate(enemy_particle[num], enemy_obj.transform);
        new_particle.Play();

        Vector3 offset = new Vector3(0,  0.398533f, 0);//キャラの高さ分調整用

        GameObject enemy_instantiate = Instantiate(enemy_prefab[num], enemy_obj.transform.position + offset, new Quaternion(0, 180.0f, 0, 1));//生成
        enemy_instantiate.name =  enemy_prefab[num].name + enemy_count.ToString();
        //Search_obj.Add(enemy_instantiate);//リストに追加

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
}