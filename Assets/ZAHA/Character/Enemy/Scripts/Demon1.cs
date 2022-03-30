using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon1 : EnemyBase
{
    private void Start()
    {
        Nextpos = 4;
    }
    void FixedUpdate()
    {
        if (Enemy_anim == null) return;

        //CoreCheck();

        if (Generalenemy == null)
        {
            //Debug.Log("スポナー");
            Generalenemy = GameObject.Find("Sponer").GetComponent<GeneralEnemy>();
        }

        Vector3 target = TargetDir(this.gameObject, Generalenemy.rootpos[Startpos].transform.GetChild(Nextpos).gameObject);
        //Vector3 coredis = TargetDir(this.gameObject, Core.gameObject);
        if (Targetchangeflg) 
        {
                if (IndexCheck(Nextpos, 0))
                {
                    Nextpos++;
                    Attackflg = false;
                }
                else
                {
                    Attackflg = true;
                }
            Targetchangeflg = false;

        }
        else 
        {
            //Debug.Log("ターゲット距離" + target.magnitude);
            if (target.magnitude > Targetdistance) {//ターゲットの距離まで移動。
                Move(Startpos, Nextpos);
                status = Status.Walk;//歩き状態
                //LookTarget(false);
            }
            else //ターゲットに到達
            {
                status = Status.Idle;//止まる。
                //LookTarget(true);
                Targetchangetime += Time.deltaTime;//時間計測
                if (Targetchangetime > 5)
                {
                    Targetchangeflg = true;//目的値を変更
                    Targetchangetime = 0;
                }
            }
        }
        if (Attackflg)
        {
            ////この辺、深夜脳死でかいたので、後で修正予定。
            if (!Enemy_anim.AnimPlayBack("EnemyAttack"))
            {//再生
                Debug.Log("タイム計測");
                Attacktime += Time.deltaTime; //3秒おきに攻撃
            }

            if (Attacktime > 3)
            { //フレーム（秒）攻撃する
                Enemy_anim.TriggerAttack("Attack");//攻撃trigger
                                                   //再生中ならリセット 
                Attacktime = 0;
            }
        }

        //死亡フラグが立った時
        if (Deathflg)
        {
            if (Init_animflg) { Enemy_anim.TriggerDeath("Death"); Init_animflg = false;}; //一回のみ死亡アニメーション再生
        }

        Enemy_anim.AnimStatus(status);//アニメーション更新
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //ここにコアのdamage処理を追加する。
    //    if (other.gameObject.tag == "Player")
    //    {
    //        Debug.Log("HIt");
    //        //GameObject.Find("Sphere").GetComponent<CoreLife>().CoreDamege();
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    // 魔法が当たるとダメージ
    //    if (other.gameObject.tag == "Meteor")
    //    {
    //        Debug.Log(other.gameObject);
    //        Debug.Log("hit_triStay");
    //        //GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(other.gameObject);
    //        this.GetComponent<Demon1>().Damage(100.0f);
    //    }
    //}

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

