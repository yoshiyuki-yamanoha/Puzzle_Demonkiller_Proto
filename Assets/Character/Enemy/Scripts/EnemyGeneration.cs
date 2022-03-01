using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{

    [SerializeField] GameObject[] prefab = null;
    [SerializeField] Transform[] generationPos = null;
    [SerializeField] int enemymax = 10;//敵最大値
    float time = 0;//時間計測
    [SerializeField] float span = 3;//敵スパン
    float enemy_count = 0;

    [SerializeField] GameObject Ene_MagicCircle;

    private void Start()
    {
        Generation(0);
        Generation(1);
        Generation(2);
    }

    // Update is called once per frame
    void Update()
    {
        //if(enemy_count >= enemymax)//最大値超えていたら何もしなーい。
        //{
        //    return;
        //}

        //time += Time.deltaTime;//時間計測 //生成するときにカウント

        //if (time > span)//秒おきに生成
        //{
        //    Generation(Random.Range(0,3));//生成。
        //}
    }

    //Enemy生成
    void Generation(int num)
    {
        GameObject enemy = Instantiate(prefab[Random.Range(0,2)]);//生成
        enemy.transform.position = new Vector3( generationPos[num].position.x + Random.Range(-3,3), generationPos[num].position.y, generationPos[num].position.z);//位置
        GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Add(enemy);

        Vector3 MCPos = new Vector3(enemy.transform.position.x, 2.0f, enemy.transform.position.z);
        GameObject MC = Instantiate(Ene_MagicCircle, MCPos, Ene_MagicCircle.transform.rotation);
        MC.transform.parent = enemy.transform;
        MC.transform.localScale = new Vector3(1.3f, 1.3f,1.3f);
        MC.transform.localPosition += new Vector3(0, 2.8f, 0f);


        time = 0;//時間リセット
        enemy_count++;//敵カウント
    }
}
