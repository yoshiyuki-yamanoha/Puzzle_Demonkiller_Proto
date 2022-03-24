using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEnemy : MonoBehaviour
{
    [SerializeField] GameObject[] prefab = null;//プレファブ格納変数
    [SerializeField] int enemymax = 10;//敵最大値
    float enemy_count = 0;//エネミーカウント
    int max_enemy_kinds = 0;//敵種類
    float time = 0;//時間計測
    [SerializeField] float span = 5;//敵スパン

    [SerializeField] ParticleSystem[] enemy_particle = null;
    //[SerializeField] EnemyBase enemybase = null;
    public GameObject[] rootpos = null;//親オブジェクト
    //public StartPos startpos = StartPos.LEFT;
    //public int startpos;
    //public int now_next = -1;
    public int max_next = 0;

    public bool turnflg = true;//ターンフラグ
    public bool initflg = true;


    int maxstartpos;
    public enum StartPos
    {
        LEFT,
        CENTER,
        RIGHT,
    }

    enum EnemyKinds //敵種類
    {
        Demon,
        Demon1,
        Boss,
    }
    // Start is called before the first frame update
    void Start()
    {
        maxstartpos = System.Enum.GetNames(typeof(StartPos)).Length;//スタートポジションの数分取得
        max_enemy_kinds = System.Enum.GetNames(typeof(EnemyKinds)).Length;//敵種類の個数分取得
        max_next = rootpos[0].transform.childCount;//子供の数取得
    }
    void Generation(int num , int startpos)
    {
        //パーティクル生成
        //ParticleSystem new_particle = Instantiate(enemy_particle[num], rootpos[startpos].transform);
        //new_particle.Play();

        Vector3 offset = new Vector3(0, prefab[num].transform.localScale.y ,0);//キャラの高さ分
        GameObject enemy = Instantiate(prefab[num], rootpos[startpos].transform.position + offset, new Quaternion(0, 180.0f, 0, 1));//生成
        time = 0;//時間リセット
        enemy_count++;//敵カウント
    }
    // Update is called once per frame
    void Update()
    {
        if (enemy_count >= enemymax)//最大値超えていたら何もしない。
        {
            return;
        }
        else
        {
            time += Time.deltaTime;//時間計測 //生成するときにカウント
        }

        //if (time > span)//秒おきに生成
        //{
        //    for (int i = 0; i < 1; i++) //同時生成
        //    {
        //        Generation(Random.Range(0, max_enemy_kinds - 1));//生成。
        //        //if (enemy_count >= enemymax - 1)
        //        //{
        //        //    Generation((int)EnemyKinds.Boss);//ボス生成
        //        //}
        //        //else
        //        //{
        //        //    Generation(Random.Range(0, max_enemy_kinds - 1));//生成。
        //        //}
        //    }
        //}

        if (turnflg)
        {
            if (initflg)
            {
                Debug.Log("処理―");
                //enemybase.Startpos = Random.Range(0, maxstartpos);
                Generation(Random.Range(0, max_enemy_kinds - 1), 0);//生成。
                initflg = false;
            }
        }
        else
        {
            initflg = true;
        }
    }
}
