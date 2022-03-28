using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon1 : EnemyBase
{
    float dis = 0;
    float FremTime = 0;

    [SerializeField] float Distance = 0;
    [SerializeField] float Maxfremtime = 3;
    [SerializeField] EnemyAnimationBase Enemy_anim = null;
    void FixedUpdate()
    {
        if (Generalenemy == null)
        {
            Generalenemy = GameObject.Find("Sponer").GetComponent<GeneralEnemy>();
        }

        //ターンフラグ
        if (Generalenemy.turnflg)
        {
            if (Generalenemy.initflg)
            {
                NextTarget();
            }
        }

        if (TargetDir(this.gameObject, Generalenemy.rootpos[Startpos].transform.GetChild(Now_next).gameObject).magnitude > 0.01f) {//ターゲットの距離によって移動。
            Debug.Log("歩き状態");
            Move();
            status = Status.Walk;//歩き状態
        }
        else
        {
            status = Status.Idle;//止まる。//ターゲットに到達
            Debug.Log("止まる");
            ////この辺、深夜脳死でかいたので、後で修正予定。
            //if (!Enemy_anim.AnimPlayBack("EnemyAttack")) {//再生
            //    FremTime += Time.deltaTime; //3秒おきに攻撃
            //}

            //if (FremTime > Maxfremtime){ //フレーム（秒）攻撃する
            //    Enemy_anim.TriggerAttack("Attack");//攻撃trigger
            //    transform.rotation = Quaternion.LookRotation(TargetDir(this.gameObject, Target));//プレイヤーの方向を向く
            //    //再生中ならリセット 
            //    FremTime = 0;
            //}
        }

        //死亡フラグが立った時
        if (Deathflg)
        {
            if (Init_animflg) { Enemy_anim.TriggerDeath("Death"); Init_animflg = false;}; //一回のみ死亡アニメーション再生
        }

        Enemy_anim.AnimStatus(status);//アニメーション更新

    }

    private void OnTriggerEnter(Collider other)
    {
        //ここにコアのdamage処理を追加する。
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("HIt");
            //GameObject.Find("Sphere").GetComponent<CoreLife>().CoreDamege();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 魔法が当たるとダメージ
        if (other.gameObject.tag == "Meteor")
        {
            Debug.Log(other.gameObject);
            Debug.Log("hit_triStay");
            //GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(other.gameObject);
            this.GetComponent<Demon1>().Damage(100.0f);
        }
    }

}

