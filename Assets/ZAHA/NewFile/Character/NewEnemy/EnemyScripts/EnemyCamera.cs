﻿using System.Collections;
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
    Vector3 defaultCamerapos;//Cameraの初期位置
    Vector3 enemyLookCamepos;//敵を見ているときのCamera位置

    GameObject camera_targe /*= null*/;
    bool moveflag = false;

    PlayerCameraTest cameraMove;
    Vector3 pos = new Vector3(0, 0, 0);

    Camera enemy_camera = null;
    Enemy enemyste; //敵のステータス読み取り用
    float timer = 0;

    public bool startFlag = true;
    float x = 0, y = 0, z = 0;

    FadeOut fadeout;
    // Start is called before the first frame update
    void Start()
    {
        fadeout = GameObject.Find("FadeImage").GetComponent<FadeOut>();
        tr = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        cameraMove = GameObject.Find("GameObject").GetComponent<PlayerCameraTest>();
        enemy_camera = this.gameObject.GetComponent<Camera>();
        enemy_camera.depth = -2;
        defaultCamerapos = new Vector3(25.22f, 3.7f, -80);
        transform.position = new Vector3(0, 26, -107);
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (startFlag == true)
        {
            if (tr.GetTrunPhase() == TrunManager.TrunPhase.Enemy)
            {
                //Debug.Log(timer);
                timer += Time.deltaTime;
                enemy_camera.depth = 0;
                x += 0.5f;
                transform.position = new Vector3(x, 26, -107);
                //if(x > 95)
                //{
                //    startFlag = false;
                //    //tr.SetTrunPhase(TrunManager.TrunPhase.Puzzle);
                //}
                if(timer > 2)
                {
                    fadeout.fadeOutFlag = true;
                }
                if(timer > 5)
                {
                    startFlag = false;
                    fadeout.fadeInFlag = true;
                }
            }
            else
            {
                startFlag = false;
                enemy_camera.depth = -2;
            }
        }
        else
        {

            EnemyCameraMove();
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

    void EnemyCameraMove()
    {
        //1ターンで一番近い敵
        if (tr.GetTrunPhase() == TrunManager.TrunPhase.Enemy)
        {
            enemy_camera.depth = 0;
            targets = GameObject.FindGameObjectsWithTag("Enemy");//敵のタグがついているオブジェクト取得

            if (initflg == true)
            {
                camera_targe = CloseEnemycamera();
                if (camera_targe != null)
                {
                    enemyste = camera_targe.GetComponent<Enemy>();

                    //transform.position = MoveCamerapos;
                    if (camera_targe != null) distance = transform.position - camera_targe.transform.position;

                    initflg = false;
                }
                //if (camera_targe != null) this.transform.LookAt(camera_targe.transform);
            }
            else
            {
                if (camera_targe != null)
                {
                    enemyLookCamepos = new Vector3(/*transform.position.x*/camera_targe.transform.position.x, 4, camera_targe.transform.position.z - 5);
                    transform.position = enemyLookCamepos;

                }
                if (enemyste.Ismove != true)
                {
                    if (moveflag == true)
                    {
                        cameraMove.startTime = Time.time;
                        moveflag = false;
                    }
                    transform.position = Vector3.Lerp(enemyLookCamepos, defaultCamerapos, cameraMove.CalcMoveRatio());
                }
            }
            //if (camera_targe != null) transform.position = transform.position - distance;
        }
        else
        {
            enemy_camera.depth = -2;
            //transform.position = new Vector3(25.22f, 3.7f, -80);
            initflg = true;
            moveflag = true;
        }
    }
}
