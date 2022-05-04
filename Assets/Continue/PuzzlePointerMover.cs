using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzlePointerMover : TrunManager
{
    //角度と対象オブジェクト指定用の構造体
    [Serializable] struct GoalPorts {
        public float ang;
        public GameObject goalPort;
    }

    //それぞれの魔法陣用
    [Serializable] struct CirclesArray {
        public GameObject circle;
        public GoalPorts[] goalPorts;
    }

    [SerializeField] CirclesArray[] circlesArrays;

    //スクリプト
    PointControl s_PointControl;
    TrunManager s_TrunManager;

    //現在選択している魔法陣オブジェクト
    [SerializeField] GameObject currentSelectedCircle;

    //スティックの角度を獲る
    [SerializeField] float leftStickAngle = 0;

    //スティックの入力があるか
    bool isStickMove;

    //選択魔法陣
    [SerializeField] GameObject selectCircle;

    //音
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip moveSound;

    //パズルのみのシーンで使うbool
    [SerializeField] private bool puzzleOnlyMode;

    //ポインター移動インターバル
    const float interval = 0.1f;
    float interCount = interval;

    private void Start()
    {
        s_PointControl = GetComponent<PointControl>();

        circlesArrays[1].goalPorts[0].goalPort.transform.GetChild(0).GetComponent<GoToParent>().ShowSelectCircle(selectCircle);

        if(!puzzleOnlyMode)
            s_TrunManager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
    }

    private void FixedUpdate()
    {
        if (puzzleOnlyMode || s_TrunManager.trunphase == TrunPhase.Puzzle)
        {

            //現在選択してる魔法陣を取得
            GetCurrentSelecterCircle();

            //スティックの角度
            CalcAngle();

            //インターバルを減らす
            if (interCount != 0) {
                interCount -= Time.deltaTime;

                if (interCount <= 0f) {
                    interCount = 0;
                }
            }

            //魔法陣間の移動を制御する
            if (isStickMove && interCount == 0)
            {
                Mover();

                //更新後の選択している魔法陣の情報を渡す
                SetCurrentSelecterCircle();
            }
        }

    }

    //現在選択してる魔法陣を取得する関数
    void GetCurrentSelecterCircle() {
        currentSelectedCircle = s_PointControl.oldOverlapObject;
    }

    //魔法陣間の移動を制御する関数
    void CalcAngle()
    {

        //カーソルの基準点がずっと中央
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        if (hori + vert == 0) isStickMove = false;
        else isStickMove = true;

        //スティックの角度を求める(今の所は角度は使ってない)
        leftStickAngle = Mathf.Atan2(vert, hori) * 180 / Mathf.PI;
        if (leftStickAngle < 0) leftStickAngle = 360.0f + leftStickAngle;
        if (leftStickAngle > 360) leftStickAngle = -360.0f + leftStickAngle;


    }

    //魔法陣間の移動を制御する関数
    void Mover() {

        int soeji = 0;

        //現在選んでいる魔法陣の添え字を調べる
        foreach (var c in circlesArrays) {

            if (c.circle.transform.GetChild(0).gameObject == currentSelectedCircle) break;

            if(soeji < 4) soeji++;
        }

        int cycleLimit = circlesArrays[soeji].goalPorts.Length;
        

        //あらかじめ登録したやつの繰り返し
        for (int i = 0; i < cycleLimit - 1; i++) {

            GameObject goal = circlesArrays[soeji].goalPorts[i].goalPort;   //指定オブジェクト
            float thisAng = circlesArrays[soeji].goalPorts[i].ang;          //角度始点
            float nextAng = circlesArrays[soeji].goalPorts[i + 1].ang;      //角度終点

            if (leftStickAngle >= thisAng && leftStickAngle < nextAng)
            {
                //if (goal.transform.GetChild(0).gameObject == currentSelectedCircle) return;

                if (goal != null)
                {

                    //選択サークルを消す
                    currentSelectedCircle.GetComponent<GoToParent>().FadeSelectCircle();

                    //ポインター移動
                    if (goal.transform.childCount > 0)
                    {
                        currentSelectedCircle = goal.transform.GetChild(0).gameObject;

                        //選択サークルを出す
                        currentSelectedCircle.GetComponent<GoToParent>().ShowSelectCircle(selectCircle);
                    }

                    //移動音を鳴らす
                    audioSource.PlayOneShot(moveSound);

                    //インターバルをリセット
                    interCount = interval;

                }
            }
        }

        
    }

    //更新後の選択している魔法陣の情報を渡す関数
    void SetCurrentSelecterCircle() {
        s_PointControl.oldOverlapObject = currentSelectedCircle;
    }
}
