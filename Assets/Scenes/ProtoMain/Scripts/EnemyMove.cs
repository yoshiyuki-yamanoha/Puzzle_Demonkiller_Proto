using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public static EnemyMove instance;

    public GameObject[] points=null;
    private int destPoint = 0;
    private NavMeshAgent agent;
   // public GameObject target;
    private bool inArea = false;
    public float chasespeed = 0.05f;
    public Vector3 enemyPosition;

    private int ROOT;
    private int RandR;
    private bool RimitFlg = true;
    public GameObject PrefabEnemy;
    private GameObject CloneEnem;

    private float time;
    private float interval = 0.5f;

    //敵の数を表示するためのScript
    public EnemyNumText ENT;

    bool flag;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RandR = Random.Range(0, 3);
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        //GotoNextPoint();
        ENT = GameObject.Find("EnemyNum").GetComponent<EnemyNumText>();
        flag = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject enemy = GameObject.FindWithTag("Enemy");
        GameObject enemy1 = GameObject.FindWithTag("Enemy1");
        GameObject enemy2 = GameObject.FindWithTag("Enemy2");

        if (enemy != null) enemyPosition = enemy.transform.position;
        if (enemy == null) enemyPosition = enemy1.transform.position;
        if((enemy == null) && (enemy1 == null)) enemyPosition = enemy2.transform.position;
        time += Time.deltaTime;
        //if (RimitFlg == true) {

        //     RimitFlg = false;
        //  }

        if (RandR==0)
        {
            for (int x = 0; x < points.Length; x++)//プレイヤー目線左方向のルートから敵が来る
            {
              //  if (points[x] == null)
               // {
                    points[x] = GameObject.Find("PointL" + x);
               // }
            }

        }else if (RandR==1){
            for (int x = 0; x < points.Length; x++)//プレイヤー目線真ん中方向のルートから敵が来る
            {
               // if (points[x] == null)
               // {
                    points[x] = GameObject.Find("PointM" + x);
              //  }
            }
        }else if (RandR==2){
            for (int x = 0; x < points.Length; x++)//プレイヤー目線右方向のルートから敵が来る
            {
             //   if (points[x] == null)
              //  {
                    points[x] = GameObject.Find("PointR" + x);
             //   }
            }

        }


        if (agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();

        }

        //if (target.activeInHierarchy==false)
        //{
        //    //GetComponent<Renderer>().material.color = origColor;
        //}

        //if (inArea==true&&target.activeInHierarchy==true)
        //{
        //    agent.destination = target.transform.position;
        //    EneChasing();
        //}
    }

    void GotoNextPoint()
    {
        //if (points.Length == 0) return;

        agent.destination = points[destPoint].transform.position;
        destPoint = (destPoint + 1) % points.Length;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            inArea = true;
            //target = other.gameObject;
            EneChasing();
            
        }

        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject, 0.2f);
            CloneEnem = Instantiate(PrefabEnemy, new Vector3(-0.71f, 0.69f, 26.02f), Quaternion.identity);
            CloneEnem.name = "CloneEnemy";
        }
        //0.5f待つ(デストロイするまでの時間)
        if (interval < time)
        {
            if (other.gameObject.tag == "Magic" && flag == false)
            {
                //自分を破壊
                Destroy(gameObject, 0.2f);

                //敵がでてくるりょうを制限
                if (ENT.Enemy_Count > 2)
                {
                    // 生成をする
                    CloneEnem = Instantiate(PrefabEnemy, new Vector3(-0.71f, 0.69f, 26.02f), Quaternion.identity);
                    CloneEnem.name = "CloneEnemy";
                    //ナビのエリア取得
                    inArea = true;
                    EneChasing();
                }

                ENT.Enemy_Num();
                flag = true;
            }
        }

    }

  


    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        inArea = false;
    //        GotoNextPoint();
    //    }
    //}

    public void EneChasing()
    {
        transform.position += transform.forward * chasespeed;
    }

}
