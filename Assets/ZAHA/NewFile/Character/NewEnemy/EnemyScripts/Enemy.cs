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
            time = 0;
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
                    if (Abnormal_condition != AbnormalCondition.Ice) {
                        EnemyMovement(Enemy_move);//動けるマス範囲
                    }
                    else
                    {
                        Is_action = true;
                    }
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
        if (other.CompareTag("Fire"))//燃焼のタグ
        {
            Destroy(other.gameObject);//当たった魔法を消す

            if(Abnormal_condition == AbnormalCondition.Ice)
            {
                IceObjSetActivOff();//アイスオブジェクトオフ
                IceBreakEffeckt();
            }

            Abnormal_condition = AbnormalCondition.Fire;//ファイヤー状態付与
            FireEffectPlay();//ファイヤーeffect再生
            //Fire_Abnormal_UI();//ファイヤーUI表示
            Fire_image.gameObject.SetActive(true);
            Fire_abnormality_turncount = 0;//持続リセット

            if (Ice_del_flg)
            {
                Destroy(gameObject.GetComponentInChildren<PentaIceWall>().gameObject);
                Ice_del_flg = false;
                Ice_instance_flg = false;
            }
        }

        if (other.CompareTag("Ice"))
        {
            Destroy(other.gameObject);//当たった魔法を消すよーん
            if (Abnormal_condition == AbnormalCondition.Fire) //現在の状態異常がアイス状態なら
            {
                Fire_image.gameObject.SetActive(false);//ファイヤーUI非表示
            }

            Abnormal_condition = AbnormalCondition.Ice;//状態異常をアイス状態
            if (!Ice_instance_flg)
            {
                other.GetComponent<PentagonIce>().Tin(transform.position, this.gameObject, new Vector3(0.8f, 0.8f, 0.8f));
                Ice_instance_flg = true;
            }
            Ice_del_flg = true;//アイス消す状態
            Ice_abnormality_turncount = 0; //状態異常カウントリセット
            IceObjSetActivOn();//アイスオブジェクトオン
            
            //pentaIceEff = GameObject.Find("BreakIce_honmono");
            //Instantiate(pentaIceEff, transform.position, Quaternion.identity);
        }
    }

    public void Icerelease()
    {
        Abnormal_condition = AbnormalCondition.NONE;
    }
}