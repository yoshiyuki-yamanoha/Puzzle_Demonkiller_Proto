using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{

    public GameObject[] points=null;
    private int destPoint = 0;
    private NavMeshAgent agent;
   // public GameObject target;
    private bool inArea = false;
    public float chasespeed = 0.05f;

    private int ROOT;
    private int RandR;
    private bool RimitFlg = true;
    public GameObject PrefabEnemy;
    private GameObject CloneEnem;


    // Start is called before the first frame update
    void Start()
    {
        RandR = Random.Range(0, 3);
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        //GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
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
