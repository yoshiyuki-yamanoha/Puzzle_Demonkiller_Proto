using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSwordMove : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        InitFunction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //自分(敵)のターンだったら
        if (Trun_manager.trunphase == TrunManager.TrunPhase.Enemy)
        {
            EnemyTurnStart();
        }
        else //ターンを終了する時
        {
            //魔法のターンの時に
            if (Trun_manager.GetTrunPhase() == TrunManager.TrunPhase.MagicAttack)
            {
                AbnormalStatus();
            }
            EnemyTurnEnd();
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
                    EnemyMovement(1);//動けるマス範囲
                    break;
            }
        }

        EnemyDeath();//敵が死んだときの処理
        Enemy_anim.AnimStatus(status);//アニメーション更新
    }
}
