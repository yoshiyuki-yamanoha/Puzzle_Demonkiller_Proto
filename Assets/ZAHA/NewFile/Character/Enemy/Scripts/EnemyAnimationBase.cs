using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationBase : MonoBehaviour
{
    [SerializeField] Animator enemy_animator = null;
    bool walkflg = false;
    bool deathflg = false;

    //攻撃アニメーション
    public void TriggerAttack(string state_name)
    {
        enemy_animator.SetTrigger(state_name);
    }

    //死亡アニメーション
    public void TriggerDeath(string state_name)
    {
        enemy_animator.SetTrigger(state_name);
    }

    public void AnimStatus(EnemyBase.Status status)//プレイヤーの状態を取得して、遷移する。
    {
        switch (status)
        {
            case EnemyBase.Status.Idle:
                walkflg = false;
                enemy_animator.SetBool("WalkFlg", walkflg);
                break;
            case EnemyBase.Status.Walk:
                walkflg = true;
                enemy_animator.SetBool("WalkFlg", walkflg);
                break;
        }
    }

    public void SetFloat(int valu)
    {
        enemy_animator.SetFloat("Speed", valu);
    }

    public bool AnimPlayBack(string state_name)//ステートが再生中か取得
    {
        return enemy_animator.GetCurrentAnimatorStateInfo(0).IsName(state_name);
    }

    public float AnimPlayExit()//ステートが再生中か取得
    {
        return enemy_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    public bool AnimTransition()//ステートが遷移中か取得
    {
        return enemy_animator.IsInTransition(0);
    }
}
