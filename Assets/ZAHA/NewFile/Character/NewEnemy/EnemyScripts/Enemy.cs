using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    //public GameObject pentaIceEff;
    Magichoming magichoming;
    float time = 0;
    private void Start()
    {
        InitFunction();

    }

    void FixedUpdate()
    {
        //if (Abnormal_condition != AbnormalCondition.Ice)
        //{


        //自分(敵)のターンだったら
        if (Trun_manager.trunphase == TrunManager.TrunPhase.Enemy)
        {

            if (!AbnormalStatus())
            {//ステータスダメージが喰らったらエネミーターンにする。

                time += Time.deltaTime;
                if (time > 2)
                {
                    EnemyTurnStart();
                    time = 0;
                }
            }
        }
        else //ターンを終了する時
        {
            EnemyTurnEnd();//ターン終了 エネミーターン以外の時
        }

        HPber();//HPゲージ

        //攻撃地点
        if (Istrun && !Is_action)
        {//自分のターンかつ行動していない時
            switch (Enemy_action)
            {
                case EnemyAction.Generation:
                    break;
                case EnemyAction.Movement:
                    EnemyMovement(2);//動けるマス範囲
                    break;
            }
        }

        EnemyDeath();//敵が死んだときの処理
        Enemy_anim.AnimStatus(status);//アニメーション更新
    }
    //    else
    //    {
    //        Is_action = true;

    //        if (Trun_manager.trunphase == TrunManager.TrunPhase.Enemy)
    //        {
    //            EnemyTurnStart();
    //        }
    //        else //ターンを終了する時
    //        {
    //            //魔法のターンの時に
    //            if (Trun_manager.GetTrunPhase() == TrunManager.TrunPhase.MagicAttack)
    //            {
    //                AbnormalStatus();
    //            }
    //            EnemyTurnEnd();
    //        }

    //        HPber();//HPゲージ
    //    }
    //}
    ////魔法陣の当たり判定
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Magic"))//当たった相手が魔法だったら
    //    {
    //    }

    //    if (other.CompareTag("Fire"))//燃焼のタグ
    //    {
    //        Abnormal_condition = AbnormalCondition.Fire;
    //        Fire_abnormality_turncount = 0;//持続リセット
    //        Destroy(other.gameObject);
    //    }

    //    if (other.CompareTag("Ice"))
    //    {
    //        Abnormal_condition = AbnormalCondition.Ice;
    //        other.GetComponent<PentagonIce>().Tin(transform.position);
    //        //pentaIceEff = GameObject.Find("BreakIce_honmono");
    //        //Instantiate(pentaIceEff, transform.position, Quaternion.identity);
    //        Ice_abnormality_turncount = 0;
    //        Destroy(other.gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("魔法");
        if (other.CompareTag("Magic"))//当たった相手が魔法だったら
        {
        }

        if (other.CompareTag("Fire"))//燃焼のタグ
        {
            FireEffectPlay();
            Fire_Abnormal_UI();
            Abnormal_condition = AbnormalCondition.Fire;
            Fire_abnormality_turncount = 0;//持続リセット
            Destroy(other.gameObject);
            IceObjSetActivOff();//アイスオブジェクトオフ
            if (Ice_flg)
            {
                //ここは子供のオブジェクトが存在したら
                gameObject.GetComponentInChildren<PentaIceWall>().DestroyIce();
                //Ice_flg = false;
            }
        }

        if (other.CompareTag("Ice"))
        {
            Abnormal_condition = AbnormalCondition.Ice;//状態異常をアイス状態
            if (!Ice_instance_flg)
            {
                other.GetComponent<PentagonIce>().Tin(transform.position, this.gameObject, new Vector3(0.8f, 0.8f, 0.8f));
                Ice_instance_flg = true;
            }
            //pentaIceEff = GameObject.Find("BreakIce_honmono");
            //Instantiate(pentaIceEff, transform.position, Quaternion.identity);
            Ice_flg = true;
            Ice_abnormality_turncount = 0; //状態異常カウントリセット
            IceObjSetActivOn();//アイスオブジェクトオン
            Destroy(other.gameObject);//当たった魔法を消すよーん
        }
    }

    public void Icerelease()
    {
        Abnormal_condition = AbnormalCondition.NONE;
    }
}