using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//炎上エフェクトによる処理スクリプト
//ターンが1周するごとに範囲内のエネミーに1ダメージ与える

public class FireMagic : TrunManager
{
    //トゥルンマネージャー
    TrunManager s_TrunManager;

    //ダメージ与えるフラグ
    bool damageFlag = true;

    //魔法の範囲
    float magicRange = 10.0f;

    //魔法のライフ
    int magicLife = 3;


    

    private void Start()
    {
        s_TrunManager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
    }

    private void FixedUpdate()
    {
        //現在のターンを取得
        TrunPhase currentPhase = s_TrunManager.GetTrunPhase();


        //ダメージフラグが倒れているかつ、エネミーフェイズの最初に
        if (currentPhase == TrunPhase.Enemy && damageFlag) {

            //一定範囲内のエネミーを検索
            GameObject[] enes = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var e in enes) {
                if (Vector3.Distance(transform.position, e.transform.position) <= magicRange)
                    e.GetComponent<EnemyBase>().Damage(30);
            }

            damageFlag = false; //魔法を発動したフラグを立てる
            magicLife--;        //魔法のライフを減らす

            //魔法のライフが0になったら
            if (magicLife <= 0) {
                Destroy(gameObject, 1.0f);
            }
        }

        //エネミーふぇいず以外でフラグを倒す
        if (currentPhase != TrunPhase.Enemy)
            damageFlag = true;
    }

    public void SetMagicRange(int level) {
        magicRange *= (float)level;
        transform.localScale *= level;
    }
}

