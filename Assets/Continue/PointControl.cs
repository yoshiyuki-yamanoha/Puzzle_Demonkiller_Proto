﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointControl : MonoBehaviour
{

    Transform tf;
    [SerializeField] private float power;

    [SerializeField] private GameObject selectCircle;
    [SerializeField] GameObject selectCircle2;

    //元の位置
    private Vector3 oriPos;

    //選択用
    [SerializeField] private Transform c_Select;
    [SerializeField] private GameObject selA;
    [SerializeField] private GameObject selB;
    private bool isSelect = false;

    //SE
    private SEPlayer sePlay;

    //変更の親保存用
    [SerializeField] private Transform selTf;

    //ぽいんたーが重なってるオブジェ
    //[SerializeField] private GameObject olObj;

    //角度
    [SerializeField] private float ang;

    //吸いつき範囲
    [SerializeField, Range(0, 100)] private float dist = 1.5f;

    //ゲームオブジェクト用
    private GameObject[] circles;

    //誘導用透明オブジェクトたち
    private GameObject[] porters;

    //前回選択してたオブジェクト(カーソル位置固定用)
    GameObject oldOverlapObject;

    //ポジション移動インターバル
    [SerializeField, Range(0, 60)]
    float interval;
    float interCount;

    // Start is called before the first frame update
    void Start()
    {
        sePlay = GameObject.Find("SePlayer").GetComponent<SEPlayer>();

        tf = transform;

        oriPos = tf.position;

        RegisterCircles();

        oldOverlapObject = circles[0];
    }

    GameObject circleA = null;

    void Update()
    {
        //カーソルの基準点がずっと中央
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 ppos = oriPos + new Vector3(hori * power, vert * power, 0);


        Debug.DrawLine(tf.position, ppos);
        if(oldOverlapObject)
            tf.position = oldOverlapObject.transform.position;

        //スティックの角度を求める(今の所は角度は使ってない)
        ang = Mathf.Atan2(vert, hori) * 180 / Mathf.PI;
        if (ang < 0) ang = 360.0f + ang;


        if (interCount == 0)
        {
            //当たり判定を伸ばすやつ
            foreach (GameObject o in porters)
            {
                float per = 0.1f;
                Vector3 currentPerPos;

                while (per < 1.0f)
                {
                    currentPerPos = Vector3.Lerp(oriPos, ppos, per);

                    if (Vector3.Distance(currentPerPos, o.transform.position) < dist && oldOverlapObject != o)
                    {
                        oriPos = o.GetComponent<TransportToParent>().GetGoalPos();

                        interCount = interval;

                        break;
                    }

                    per += 0.1f;
                }
            }

            //ポインターと魔法陣の当たり判定
            foreach (GameObject o in circles)
            {
                GoToParent gp = o.GetComponent<GoToParent>();

                float per = 0.1f;
                Vector3 currentPerPos;
                while (per < 1.0f)
                {
                    currentPerPos = Vector3.Lerp(oriPos, ppos, per);

                    if (Vector3.Distance(currentPerPos, o.transform.position) < dist && oldOverlapObject != o)
                    {
                        //最近選択していたオブジェクト
                        oldOverlapObject = o;

                        //選択した親オブジェクトの位置にいく
                        oriPos = o.transform.parent.position;

                        interCount = interval;

                        break;
                    }

                    per += 0.1f;
                }

                //魔法陣の中心からdist分の範囲内に入ったら
                if (Vector3.Distance(tf.position, o.transform.position) < dist)
                {

                    //選択サークルを出させる
                    gp.ShowSelectCircle(selectCircle);

                    //Aボタン選択
                    SelectCircle(o);


                }
                else
                {  //入って無ければ
                    gp.FadeSelectCircle();
                }

            }
        }

        //インターバルカウント
        if (interCount != 0) {
            interCount--;
            if (interCount < 0) interCount = 0;
        }

    }


    public void RegisterCircles() {
        circles = GameObject.FindGameObjectsWithTag("My");

        porters = GameObject.FindGameObjectsWithTag("Porter");
    }

    public void SelectCircle(GameObject obj) {
        //されてない状態
        if (!isSelect)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                //sePlay.Play("DECIDE");//SEを鳴らす（魔方陣を選択した音）
                selA = obj;                                   //選択したオブジェ保存
                selTf = selA.transform.parent;              //1個目の親オブジェ
                                                            //selA.transform.parent = c_Select;           //選択位置に移動

                isSelect = true;                            //選択フラグを立てる
                circleA = Instantiate(selectCircle2, selA.transform.position, Quaternion.identity);

            }
        }
        else //されてる状態
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                //sePlay.Play("SWITCH");//SEを鳴らす（魔方陣の位置が入れ替わる）
                selB = obj;
                selA.transform.parent = selB.transform.parent;
                selB.transform.parent = selTf;
                selA.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
                selA = null;
                selB = null;
                selTf = null;
                isSelect = false;
                Destroy(circleA);
            }
        }
    }

}
