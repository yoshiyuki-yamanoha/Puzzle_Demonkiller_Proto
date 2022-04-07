using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationEnemy : PseudoArray
{
    //List<GameObject> search_obj = new List<GameObject>();
    //List<bool> search_bool = new List<bool>();
    int search_count = 0;
    //int action_count = 0;
    //int is_action_count = 0;
    //bool init_action_flg = true;

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

    ////ターン中カメラ
    //Camera enemy_camera = null;
    //[SerializeField] Vector3 offset;//補正用

    //[SerializeField] GameObject target;
    //Vector3 distance;
    //bool cameraflg = true;


    //ターン終了フラグ
    bool turn_exit_flg = false;
    bool turn_initflg = true;

    //public List<GameObject> Search_obj { get => search_obj; set => search_obj = value; }
    //public List<bool> Search_bool { get => search_bool; set => search_bool = value; }


    //具志堅SE処理
    //SEManager sePlay;

    // Start is called before the first frame update
    void Start()
    {
        max_x = rootpos.Length;//スタートポジションの数分取得
        max_y = rootpos[0].transform.childCount;//子供の数取得
        enemy_kinds_max = enemy_prefab.Length;
        trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();

        //sePlay = GameObject.Find("Audio").GetComponent<SEManager>();

        //enemy_camera = GameObject.Find("EnemyCamera").GetComponent<Camera>();
        //distance = enemy_camera.transform.position - target.transform.position;
        //enemy_camera.depth = -2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StageSarchEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        //自分のターンの時
        if (trunmanager.trunphase == TrunManager.TrunPhase.Enemy) {
            if (turn_initflg) {
                turn_exit_flg = false;//抜けるオフ
                turn_initflg = false;
            }
        }
        

        if (!turn_exit_flg) //!抜けるフラグ
        {
            //EnemyTurnCamera();//カメラ
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
                        int randomY = Random.Range(0, max_y);

                        //生成する位置が誰もいない時 敵以外なら生成
                        if (rootpos[randomX].transform.GetChild(y).GetComponent<PseudoArray>().Mass_status != MassStatus.ENEMY)
                        {
                            Generation(Enemy_kinds_max, randomX, y);//引数(エネミーの種類 , スタートPos)生成。
                            //sePlay.Play("EnemySpawn");
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
            }
            else
            {
                
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
                        Debug.Log(search_count);
                        turn_exit_flg = true;
                    }
                    else
                    {
                        is_generation = true;
                    }
                    
                }
            }
        }

        if (turn_exit_flg)
        {

            time += Time.deltaTime;

            if (time > 2)
            {
                trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
                turn_initflg = true;
                time = 0;

                search_count = 0;
                for (int i = 0; i <  StageSarchEnemy.Length; i++) {
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


        //5は真ん中の敵
        //if (x == 5)
        //{
        //    if (cameraflg)
        //    {
        //        target.transform.parent = enemy_instantiate.transform;
        //        cameraflg = false;
        //    }
        //}

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



//カメラコメントアウト
//void EnemyTurnCamera()
//{
//    enemy_camera.transform.position = target.transform.position + distance;
//    enemy_camera.depth = 0;
//    //Vector3 pos = main_camera.transform.position;
//    //main_camera.transform.position = new Vector3(pos.x, pos.y + offset.y, pos.z);

//}



//if (enemy_last_obj != null)
//{
//    if (enemy_last_obj.GetComponent<Enemy>().Is_action)//最後に生成した敵を見る。
//    {
//        TrunManager trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
//        trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
//    }
//}