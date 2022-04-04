//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyGeneration : MonoBehaviour
//{

//    [SerializeField] GameObject[] prefab = null;//プレファブ格納変数

//    [SerializeField] int enemymax = 10;//敵最大値
//    float enemy_count = 0;//エネミーカウント
//    int max_enemy_kinds = 0;//敵種類
//    float time = 0;//時間計測

//    //生成地点に関する情報
//    [SerializeField] Transform[] generationPos = null;//生成位置オブジェクト格納
//    int spawn_number = 0;//スポーン番号
//    int spawn_max = 0;//スポーン最大値
//    int same_enemy = 1;//同時に生成する敵

//    [SerializeField] float span = 3;//敵スパン
//    float init_span = 0;
//    float span_time = 1;
//    float span_valu = 0;

//    [SerializeField] GameObject Ene_MagicCircle;//魔法陣 ゲームオブジェクト
//    [SerializeField] ParticleSystem[] enemy_particle = null;

//    float[] old_random_number = new float[2];

//    enum RandomStatus
//    {
//        Aria,
//        Prefab,
//    }

//    enum EnemyKinds //敵種類
//    {
//        Demon,
//        Demon1,
//        Boss,
//    }

//    private void Start()
//    {
//        //初期生成
//        //Generation(0);
//        //Generation(1);
//        //Generation(2);
//        max_enemy_kinds = System.Enum.GetNames(typeof(EnemyKinds)).Length;//敵種類の個数分取得
//        spawn_max = generationPos.Length;//生成位置の個数取得
//        for (int i = 0; i < old_random_number.Length; i++)
//        {
//            old_random_number[i] = 0;
//        }

//        init_span = span;//初期化値を保存
//    }

//    // Update is called once per frame
//    private void FixedUpdate()
//    {


//        if (enemy_count >= enemymax)//最大値超えていたら何もしない。
//        {
//            return;
//        }
//        else
//        {
//            time += Time.deltaTime;//時間計測 //生成するときにカウント
//        }

//        if (time > Span())//秒おきに生成
//        {
//            //Same_Enemy();
//            for (int i = 0; i < 1; i++)
//            {
//                if (enemy_count >= enemymax - 1)
//                {
//                    Generation((int)EnemyKinds.Boss, 0);//ボス生成
//                }
//                else
//                {
//                    //Generation((int)EnemyKinds.Boss, 0);
//                    Generation(Random.Range(0, max_enemy_kinds - 1), 0);//生成。
//                    /*Generation(0, 0);*/
//                    //Generation((int)EnemyKinds.Boss, 0);//ボス生成
//                }
//            }

//            //if (enemymax / 2 <= enemy_count) //最大値 15 -> の半分 7 だったらをエネミーカウントが超えたら
//            //{
//            //    span_time -= 0.1f;//スパン時間減らす
//            //}
//        }
//    }

//    //同時生成の値 関数
//    void Same_Enemy()
//    {
//        if (enemymax / 2 <= enemy_count)
//        {
//            same_enemy++;
//        }

//        if (same_enemy >= 3)//3以上超えているかcheck
//        {
//            same_enemy = 3;
//        }

//        float numver = enemy_count + same_enemy;//14 + 2 = 16
//        Debug.Log("Old" + numver);

//        if (numver >= 14)
//        {
//            Debug.Log("1にします。");
//            same_enemy = 1;
//        }
//    }

//    //間隔関数(秒)
//    float Span()
//    {
//        //if (span_time <= 0.5f)
//        //{
//        //    span_time = 0.5f;
//        //}
//        //else
//        //{
//        //    span_valu = span * span_time;
//        //}
//        //return span_valu;
//        if (enemy_count <= 0)
//        {
//            span /= 2;
//        }
//        else
//        {
//            span = init_span;
//        }

//        return span;
//    }

//    //エリア番号生成
//    int AriaSpawnNumber()
//    {
//        spawn_number = Random.Range(0, spawn_max);/*RandomRange(0, spawn_max - 2, RandomStatus.Aria);*/
//        //Debug.Log("重複なし!Random" + spawn_number);
//        //Random.Range(0, spawn_max - 2); //2枠抜いた位置を生成 0 - 3  0 1 2が生成

//        //if (enemy_count >= enemymax / 2) //敵が半分をスポーンしていたら
//        //{

//        //    spawn_number = Random.Range(spawn_max - 2, spawn_max);//後半部分も解放 3 , 4  生成 4 int型は最大値を含まない 
//        //}
//        return spawn_number;
//    }

//    //Enemy生成
//    void Generation(int num, int spawn_number)
//    {
//        //パーティクル生成
//        ParticleSystem new_particle = Instantiate(enemy_particle[num] , generationPos[spawn_number]);
//        new_particle.Play();

//        GameObject enemy = Instantiate(prefab[num], generationPos[spawn_number].transform.position, new Quaternion(0, 180.0f, 0, 1));//生成
//        //enemy.transform.position = generationPos[spawn_number].transform.position;
//        //new Vector3(generationPos[num].position.x + Random.Range(-3.0f, 3.0f), generationPos[num].position.y, generationPos[num].position.z);//位置
//        //GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Add(enemy); //NUllレファレンスエラーが出たため、2022/3/14　コメントアウトしました 追記者 ZAHA

//        Vector3 MCPos = new Vector3(enemy.transform.position.x, 2.0f, enemy.transform.position.z);//位置生成
//        GameObject MC = Instantiate(Ene_MagicCircle, MCPos, Ene_MagicCircle.transform.rotation);//プレファブ生成
//        MC.transform.parent = enemy.transform;//親子にしている
//        MC.transform.localScale = new Vector3(10f, 10f, 10f);//大きさ変更
//        MC.transform.localPosition += new Vector3(0, 2.8f, 0f);//位置変更
//        MC.SetActive(false);

//        time = 0;//時間リセット
//        enemy_count++;//敵カウント
//    }

//    int RandomRange(int Max_random, int Min_random, RandomStatus random)
//    {
//        int new_random_number = 0;
//        new_random_number = Random.Range(Min_random, Max_random); //ランダム生成
//        Debug.Log("ランダム生成 1回目" + new_random_number);

//        if (old_random_number[(int)random] == new_random_number)
//        {
//            new_random_number = Random.Range(Min_random, Max_random);
//            Debug.Log("ランダム生成 2回目" + new_random_number);
//        }

//        old_random_number[(int)random] = new_random_number;//ランダム値を保存

//        Debug.Log("ランダム値" + new_random_number);
//        return new_random_number;
//    }
//}
