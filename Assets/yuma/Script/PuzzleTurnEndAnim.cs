﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTurnEndAnim : TrunManager
{
    static private bool trunEndFlg; // Trueでアニメーション開始

    // アニメーション用の変数
    float startCoolTime;    // アニメーションに入る際少し硬直させる
    [SerializeField] float rotateSpeed; //魔法陣の回転速度
    float alpha;    // 小さい魔法陣の透明度

    TrunManager turnMgr;

    // アニメーションで使用するGameObject
    [SerializeField] GameObject inCircle;
    [SerializeField] GameObject outCircle;
    [SerializeField] MeshRenderer[] circleMesh = new MeshRenderer[5];   // 小さい魔法陣の透明度を変更する変数
    [SerializeField] MeshRenderer outCircleMesh;   // 大きい魔法陣の透明度を変更する変数

    // バックアップ用のGameObject
    GameObject buf_inCircle;
    GameObject buf_outCircle;

    void Init()
    {
        alpha = 1.0f;
        startCoolTime = 60.0f;
        rotateSpeed = 5.0f;

    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        trunEndFlg = false;
        turnMgr = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        buf_inCircle = inCircle;
        buf_outCircle = outCircle;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GetPuzzleTurnEndFlg())
        {
            //inCircle.transform.Rotate(0.0f, 0.0f, rotateSpeed);
            //outCircle.transform.Rotate(-rotateSpeed, 0.0f, 0.0f);

            //Debug.Log("OK");

            if (startCoolTime-- < 0.0f)
            {
                startCoolTime = 0;
                inCircle.transform.Rotate(0.0f, 0.0f, rotateSpeed + 5.0f);
                outCircle.transform.Rotate(0.0f,rotateSpeed, 0.0f);

                if(alpha <= 0.0f)
                {
                    alpha = 0.0f;
                    turnMgr.SetTrunPhase(TrunPhase.MagicAttack);
                    SetPuzzleTurnEndFlg(false);
                    Init();

                }
                else
                {
                    alpha -= 0.03f;
                }
                
                for(int i=0;i<5; i++)
                {
                    circleMesh[i].material.color = new Color(circleMesh[i].material.color.r, circleMesh[i].material.color.g, circleMesh[i].material.color.b, alpha);
                }

                outCircleMesh.material.color = new Color(outCircleMesh.material.color.r, outCircleMesh.material.color.g, outCircleMesh.material.color.b, alpha);
            }
        }
    }

    public bool GetPuzzleTurnEndFlg()
    {
        return trunEndFlg;
    }

    public void SetPuzzleTurnEndFlg(bool flg)
    {
        trunEndFlg = flg;
    }
}
