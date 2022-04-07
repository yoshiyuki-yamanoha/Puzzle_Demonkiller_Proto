﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationEnemy : MonoBehaviour
{
    [SerializeField] bool is_generation = false;

    [SerializeField] GameObject[] enemy_prefab = null;//プレファブ格納変数
    [SerializeField] int enemy_max = 10;//敵最大値
    float enemy_count = 0;//エネミーカウント
    int enemy_kinds_max = 0;//敵種類
    int enemy_oneturn_count = 0;
    int enemy_oneturn_max = 7;
    [SerializeField] float interval_s = 5;//生成感覚
    [SerializeField] ParticleSystem[] enemy_particle = null;
    public GameObject[] rootpos = null;//親オブジェクト
    [SerializeField] int y;//縦方向
    [HideInInspector] public int max_x = 0;//横最大
    public int max_y = 0;//縦最大
    public bool initflg = true;
    float time = 0;

    TrunManager trunmanager = null;
    GameObject[] StageSarchEnemy = null;

    int is_action_count = 0;

    bool enemy_max_flg = false;

    bool turnflg = false;

    private SEManager sePlay;
    // Start is called before the first frame update
    void Start()
    {
        max_x = rootpos.Length;//スタートポジションの数分取得
        max_y = rootpos[0].transform.childCount;//子供の数取得
        enemy_kinds_max = enemy_prefab.Length;
        trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();//Se再生用
    }

    void Generation(int num, int x, int y)
    {
        rootpos[x].transform.GetChild(y).GetComponent<PseudoArray>().Whoisflg = true;

        GameObject enemy_obj = rootpos[x].transform.GetChild(y).gameObject;

        //出現する魔法陣を生成
        ParticleSystem new_particle = Instantiate(enemy_particle[num], enemy_obj.transform);
        new_particle.Play();

        Vector3 offset = new Vector3(0, enemy_prefab[num].transform.localScale.y, 0);//キャラの高さ分調整用

        GameObject enemy_instantiate = Instantiate(enemy_prefab[num], enemy_obj.transform.position + offset, new Quaternion(0, 180.0f, 0, 1));//生成



        //敵の出現の音を入れる

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
            trunmanager.trunphase = TrunManager.TrunPhase.Puzzle;//ターン移動
            
            enemy_oneturn_count = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //自分のターンが来たらターン開始
        if (trunmanager.trunphase == TrunManager.TrunPhase.Enemy)
        {

            //生成する状態なら
            if (is_generation)
            {
                if (enemy_count >= enemy_max)//最大値超えていたら何もしない。
                {
                    Debug.Log("Puzulle移行");
                    enemy_max_flg = true;
                    is_generation = false;
                    sePlay.Play("TurnChange"); //ターンチェンジの音を鳴らす
                    trunmanager.trunphase = TrunManager.TrunPhase.Puzzle;//パズルターンに移行
                }
                else
                {
                    time += Time.deltaTime;
                }

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

                        //生成する位置が誰もいない時
                        if (!rootpos[randomX].transform.GetChild(y).GetComponent<PseudoArray>().Whoisflg)
                        {
                            Debug.Log("flg" + rootpos[randomX].transform.GetChild(randomY).GetComponent<PseudoArray>().Whoisflg);
                            Generation(Enemy_kinds_max, randomX, y);//引数(エネミーの種類 , スタートPos)生成。
                            sePlay.Play("EnemySpawn");//敵が出現した時の音
                        }

                    }
                    time = 0;
                }

            }
            else
            {
                IsMoveSearch();//ステージ内の敵検索
                
                if (turnflg)
                {
                    time += Time.deltaTime;
                    if (time > 0.5)
                    {
                        trunmanager.trunphase = TrunManager.TrunPhase.Puzzle;
                        turnflg = false;
                        time = 0;
                    }
                }
            }
        }
        else
        {
            is_action_count = 0;
        }
    }

    void IsMoveSearch()
    {
        Debug.Log("検索しまーす");
        GameObject[] search_obj = GameObject.FindGameObjectsWithTag("Enemy");//タグを取得
        foreach (var hit_search_obj in search_obj)
        {
            Debug.Log("search大きさ" + search_obj.Length);
            //移動が終わるまで
            if (hit_search_obj.GetComponent<Enemy>().Is_action && hit_search_obj.GetComponent<Enemy>().Enemy_action == EnemyBase.EnemyAction.Movement)
            {
                Debug.Log(is_action_count < search_obj.Length);

                if (is_action_count < search_obj.Length)
                { //動いている数分
                    is_action_count++;
                    if (is_action_count == search_obj.Length)
                    {
                        if (!enemy_max_flg)
                        {
                            is_generation = true;
                        }
                        else
                        {
                            //ターン終了
                            turnflg = true;
                        }
                    }
                }
            }
        }
    }
}



//if (enemy_last_obj != null)
//{
//    if (enemy_last_obj.GetComponent<Enemy>().Is_action)//最後に生成した敵を見る。
//    {
//        TrunManager trunmanager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
//        trunmanager.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
//    }
//}