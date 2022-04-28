using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : EnemyBase
{
    Magichoming magichoming;
    float time = 0;

    public bool IsDamege;
    private void Start()
    {
        //InitFunction();
        IsDamege = false;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsDamege = true;
        }
        if (IsDamege)
        {
            DamegeAnim();
            return;
        }

        //if (Abnormal_condition != AbnormalCondition.Ice)
        //{
        //    //自分(敵)のターンだったら
        //    if (Trun_manager.trunphase == TrunManager.TrunPhase.Enemy)
        //    {
        //        if (!AbnormalStatus())
        //        {//ステータスダメージが喰らったらエネミーターンにする。
        //            time += Time.deltaTime;
        //            if (time > 3)
        //            {
        //                EnemyTurnStart();
        //                time = 0;
        //            }
        //        }
        //    }
        //    else //ターンを終了する時
        //    {
        //        EnemyTurnEnd();
        //    }

        //    HPber();//HPゲージ

        //    //攻撃地点
        //    if (Istrun && !Is_action)
        //    {//自分のターンかつ行動していない時
        //        switch (Enemy_action)
        //        {
        //            case EnemyAction.Generation:
        //                break;
        //            case EnemyAction.Movement:
        //                EnemyMovement(2);//動けるマス範囲
        //                break;
        //        }
        //    }

        //    EnemyDeath();//敵が死んだときの処理
        //    if (Enemy_anim != null) {
        //        Enemy_anim.AnimStatus(status);//アニメーション更新
        //    }
        //}
        //else
        //{
        //    Is_action = true;

        //    if (Trun_manager.trunphase == TrunManager.TrunPhase.Enemy)
        //    {
        //        EnemyTurnStart();
        //    }
        //    else //ターンを終了する時
        //    {
        //        //魔法のターンの時に
        //        if (Trun_manager.GetTrunPhase() == TrunManager.TrunPhase.MagicAttack)
        //        {
        //            AbnormalStatus();
        //        }
        //        EnemyTurnEnd();
        //    }

        //    HPber();//HPゲージ
        //}

        

    }
    //魔法陣の当たり判定
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magic"))//当たった相手が魔法だったら
        {
        }

        if (other.CompareTag("Fire"))//燃焼のタグ
        {
            Abnormal_condition = AbnormalCondition.Fire;
            Fire_abnormality_turncount = 0;//持続リセット
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Ice"))
        {
            Abnormal_condition = AbnormalCondition.Ice;
            Ice_abnormality_turncount = 0;
            Destroy(other.gameObject);
        }
    }
    public void Icerelease()
    {
        Abnormal_condition = AbnormalCondition.NONE;
    }

    public new void EnemyAttack()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("attack01");

        Core.ReceiveDamage();// コアのｈｐ減らす

        Destroy(gameObject, 0.5f);
    }


    float rotSpeed = 0.0f;
    public void DamegeAnim()
    {
        Transform bom = transform.GetChild(2).GetChild(1).GetChild(0);

        Debug.Log("****" + bom.name);
        Vector3 rot = bom.localRotation.eulerAngles;
        rot += new Vector3(-360, 0, 0) * Time.deltaTime;
        bom.localRotation = Quaternion.Euler(rot);
    }
}
