using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzlePointerMover : MonoBehaviour
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

    //現在選択している魔法陣オブジェクト
    [SerializeField] GameObject currentSelectedCircle;

    //スティックの角度を獲る
    [SerializeField] float leftStickAngle = 0;

    //スティックの入力があるか
    bool isStickMove;

    private void Start()
    {
        s_PointControl = GetComponent<PointControl>();
    }

    private void FixedUpdate()
    {
        //現在選択してる魔法陣を取得
        GetCurrentSelecterCircle();

        //スティックの角度
        CalcAngle();

        //魔法陣間の移動を制御する
        if(isStickMove) Mover();

        //更新後の選択している魔法陣の情報を渡す
        SetCurrentSelecterCircle();

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

            soeji++;
        }
        Debug.Log("そえじ：" + soeji);

        int cycleLimit = circlesArrays[soeji].goalPorts.Length;

        

        //あらかじめ登録したやつの繰り返し
        for (int i = 0; i < cycleLimit - 1; i++) {

            if (leftStickAngle >= circlesArrays[soeji].goalPorts[i].ang &&
                leftStickAngle < circlesArrays[soeji].goalPorts[i + 1].ang)
            {
                GameObject goal = circlesArrays[soeji].goalPorts[i].goalPort;
                if (goal != null)
                    currentSelectedCircle = goal.transform.GetChild(0).gameObject;
            }
        }

        
    }

    //更新後の選択している魔法陣の情報を渡す関数
    void SetCurrentSelecterCircle() {
        s_PointControl.oldOverlapObject = currentSelectedCircle;
    }
}
