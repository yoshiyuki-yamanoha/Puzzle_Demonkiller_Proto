using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamera : MonoBehaviour
{
    private GameObject[] targets = null;//ターゲット
    GameObject closeEnemy = null;//一番近い敵を格納する変数
    TrunManager tr;

    //カメラ用
    Vector3 distance;//距離
    bool initflg = true;

    GameObject camera_targe = null;

    Camera enemy_camera = null;
    // Start is called before the first frame update
    void Start()
    {
        tr = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        enemy_camera = this.gameObject.GetComponent<Camera>();
        enemy_camera.depth = -2;
    }

    // Update is called once per frame
    private void FixedUpdate(){
        //1ターンで一番近い敵
        if (tr.GetTrunPhase() == TrunManager.TrunPhase.Enemy)
        {
            enemy_camera.depth = 0;
            targets = GameObject.FindGameObjectsWithTag("Enemy");//敵のタグがついているオブジェクト取得

            if (initflg)
            {
                camera_targe = CloseEnemycamera();
                if(camera_targe != null) distance = transform.position - camera_targe.transform.position;
                
                initflg = false;
                if (camera_targe != null) this.transform.LookAt(camera_targe.transform);
            }

            if (camera_targe != null) transform.position = camera_targe.transform.position + distance;
        }
        else
        {
            enemy_camera.depth = -2;
            initflg = true;
        }
    }


    GameObject CloseEnemycamera()//近い敵
    {
        float closeDist = 1000.0f;

        foreach (var target in targets)
        {
            float targetDist = Vector3.Distance(transform.position, target.transform.position);
            if (closeDist > targetDist)
            {
                closeDist = targetDist;
                closeEnemy = target;
            }
        }

        return closeEnemy;
    }
}
