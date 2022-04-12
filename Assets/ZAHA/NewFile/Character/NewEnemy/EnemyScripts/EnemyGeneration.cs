//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GenerationEnemy : MonoBehaviour
//{
//    //ここで
//    GameObject last_enemy = null;
//    [SerializeField] GameObject[] prefab = null;//プレファブ格納変数
//    [SerializeField] int maxenemy = 10;//敵最大値
//    float enemy_count = 0;//エネミーカウント
//    int max_enemy_kinds = 0;//敵種類

//    [SerializeField] float span = 5;//敵スパン
//    [SerializeField] ParticleSystem[] enemy_particle = null;
//    public GameObject[] rootpos = null;//親オブジェクト

//    [SerializeField] int y;
//    [HideInInspector] public int max_x = 0;
//    public int max_y = 0;
//    //public bool turnflg = true;//ターンフラグ
//    public bool initflg = true;
//    float time = 0;//

//    const int turnmaxnum = 7;//1ターンで生成する最大値
//    int trun_enemycount = 0;
//    bool turn_flg = true;//ターンが始まっているのか

//    bool countinit = true;

//    int all_enemycount;//全体のカウント

//    [SerializeField] TrunManager trunmanager = null;
//    // Start is called before the first frame update
//    void Start()
//    {
//        max_x = rootpos.Length;/*System.Enum.GetNames(typeof(StartPos)).Length;//スタートポジションの数分取得*/
//        max_y = rootpos[0].transform.childCount;//子供の数取得
//        //max_enemy_kinds = System.Enum.GetNames(typeof(EnemyKinds)).Length;//敵種類の個数分取得
//        max_enemy_kinds = prefab.Length;
//        trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
//    }

//    void Generation(int num, int x, int y)
//    {
//        GameObject enemyobj = rootpos[x].transform.GetChild(y).gameObject;
//        //パーティクル生成
//        ParticleSystem new_particle = Instantiate(enemy_particle[num], enemyobj.transform);
//        new_particle.Play();

//        Vector3 offset = new Vector3(0, prefab[num].transform.localScale.y, 0);//キャラの高さ分
//        GameObject enemy = Instantiate(prefab[num], enemyobj.transform.position + offset, new Quaternion(0, 180.0f, 0, 1));//生成
//        //スタートポジションを教えてあげる。生成したプレファブに
//        Enemy enemys = enemy.GetComponent<Enemy>();

//        switch (enemys.enemy_kinds)
//        {
//            case EnemyBase.EnemyKinds.Demon:
//                enemys.X = x; enemys.Y = y;
//                break;
//            case EnemyBase.EnemyKinds.Demon1:
//                enemys.X = x; enemys.Y = y;
//                break;
//            case EnemyBase.EnemyKinds.Boss:
//                enemys.X = x; enemys.Y = y;
//                break;
//        }

//        ++enemy_count;//敵カウント
//        ++trun_enemycount;//1ターン分の敵をカウント

//        //maxenemy - enemycount <= turnmaxnum

//        if (enemy_count >= turnmaxnum)
//        {
//            last_enemy = enemy;
//            turn_flg = false;
//            enemy_count = 0;
//        }
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        ////生成する数をしぼる。
//        //if (enemy_count >= turnmaxnum)
//        //{
//        //    turn_flg = true;
//        //}


//        if (last_enemy != null)
//        {
//            if (last_enemy.GetComponent<Enemy>().Is_action)//最後に生成した敵を見る。
//            {
//                TrunManager trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
//                trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
//            }
//        }

//        if (trun_enemycount >= maxenemy)//最大値超えていたら何もしない。
//        {
//            return;
//        }
//        else
//        {
//            time += Time.deltaTime;
//        }

//        //自分のターンが来たらターン開始
//        if (trunmanager.trunphase == TrunManager.TrunPhase.Enemy)
//        {
//            turn_flg = true;
//        }


//        if (turn_flg)
//        {//ターンが来たらON
//            if (time > span)//秒おきに生成
//            {
//                for (int i = 0; i < 1; i++) //同時生成
//                {
//                    Generation(Random.Range(0, max_enemy_kinds), Random.Range(0, max_x), y);//引数(エネミーの種類 , スタートPos)生成。
//                }
//                time = 0;
//            }
//        }

//    }
//////}
