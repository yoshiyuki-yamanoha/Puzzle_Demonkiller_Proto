using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{

    void FixedUpdate()
    {
        if (Enemy_anim == null) return;

        if (Generation_enemy == null)
        {
            Generation_enemy = GameObject.Find("Sponer").GetComponent<GenerationEnemy>();
        }

        if (Trun_manager == null)
        {
            Trun_manager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        }

        if (Trun_manager.trunphase == TrunManager.TrunPhase.Enemy)
        {
            Istrun = true;//自分のターン(敵)開始
        }
        else
        {
            Istrun = false;
            Is_action = false;
        }

        //攻撃地点
        if (Istrun && !Is_action)
        {//自分のターンかつ行動していない時
            int nextpos = Y + 1;//目的値設定
            if (Y == 13)
            {
                Ismove = false;
                Attackflg = true;//攻撃オンにする
            }

            if (IndexCheck(nextpos, (int)Mode.Y)) { return; }
            GameObject target_obj = Generation_enemy.rootpos[X].transform.GetChild(nextpos).gameObject;

            Vector3 target = TargetDir(this.gameObject, target_obj);//ターゲット設定

            //目的値についているか?
            if (target.magnitude < Targetdistance)
            {
                status = Status.Idle;//アイドル状態
                Targetchangeflg = true;
                if (!Attackflg)
                {
                    Is_action = true;//行動した
                }
            }
            else
            {
                if (Ismove) { Move(X, nextpos); status = Status.Walk; }

            }

            if (Targetchangeflg)
            {
                Y++;
                Targetchangeflg = false;
            }


            if (Attackflg)
            {
                ////この辺、深夜脳死でかいたので、後で修正予定。
                if (!Enemy_anim.AnimPlayBack("EnemyAttack") && !Enemy_anim.AnimPlayBack("EnemyAttack"))
                {//再生
                 //Debug.Log("タイム計測");
                    Attacktime += Time.deltaTime; //3秒おきに攻撃
                }

                if (Attacktime > 3)
                { //フレーム（秒）攻撃する
                    Enemy_anim.TriggerAttack("Attack");//攻撃trigger
                    Attacktime = 0;
                    Attackflg = false;
                    Is_action = true;
                }
            }
        }

        //死亡フラグが立った時。
        if (Deathflg)
        {
            if (Init_animflg) { Enemy_anim.TriggerDeath("Death"); Init_animflg = false; }; //一回のみ死亡アニメーション再生
        }

        Enemy_anim.AnimStatus(status);//アニメーション更新

        //private void OnTriggerEnter(Collider other)
        //{
        //    //ここにコアのdamage処理を追加する。
        //    if (other.gameObject.tag == "Player")
        //    {
        //        Debug.Log("PlayerHIt");
        //        //GameObject.Find("Sphere").GetComponent<CoreLife>().CoreDamege();

        //    }

        //    //// 魔法が当たるとダメージ
        //    //if (other.gameObject.tag == "Magic")
        //    //{
        //    //    Debug.Log("hit_tri");

        //    //    this.GetComponent<Demon>().Damage(100.0f);
        //    //}
        //}

        //private void OnTriggerStay(Collider other)
        //{
        //    // 魔法が当たるとダメージ
        //    if (other.gameObject.tag == "Magic")
        //    {
        //        //Debug.Log(other.gameObject);
        //        Debug.Log("hit_triStay");
        //        //GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(other.gameObject);
        //        this.GetComponent<Demon>().Damage(100.0f);
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("魔法が当たりました");
        if(other.CompareTag("Magic"))//当たった相手が魔法だったら
        {
            Damage(50);//ダメージ処理
        }
    }
}

//コメント化処理

//private void OnCollisionEnter(Collision collision)
//{
//    if (collision.gameObject.tag == ("Barricade") || collision.gameObject.tag == ("Player"))
//    {
//        for (int i= 0; i< 2; i++) {
//            ParticleSystem newhiteffect = Instantiate(hiteffect.transform.GetChild(i).GetComponent<ParticleSystem>(), collision.contacts[0].point, Quaternion.identity);
//            newhiteffect.Play();
//        }
//    }
//    //Debug.Log(collision.contacts[0].point);
//    //Debug.DrawLine(transform.position, collision.contacts[0].point ,Color.red);

//}

//private void OnCollisionEnter(Collision collision)
//{
//    // 魔法が当たるとダメージ
//    if (collision.gameObject.tag == "Magic")
//    {
//        Debug.Log(collision.gameObject);
//        Debug.Log("hit_colli");
//        GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(collision.gameObject);
//        this.GetComponent<Demon>().Damage(100.0f);
//    }
//}

//if (target.magnitude > Targetdistance)
//{//ターゲットの距離によって移動。
//        status = Status.Walk;//歩き状態
//        Move(X, Y);
//}

//if (Targetchangeflg)//ターゲット切り替えだったら。
//{
//    if (IndexCheck(Y, (int)Root.Next))//ゴールしていない間 前に行く処理
//    {
//        Y++;//次の地点切り替え
//        Attackflg = false;//攻撃Off
//    }
//    else//ゴール地点到着したら
//    {
//        Attackflg = true;//攻撃
//    }

//    Targetchangeflg = false;
//}
//else
//{
//    if (target.magnitude > Targetdistance)
//    {//ターゲットの距離によって移動。
//        status = Status.Walk;//歩き状態
//        Move(X, Y);
//    }
//    else
//    {
//        Debug.Log("ターゲット到着");
//        status = Status.Idle;//止まる。//ターゲットに到達
//        Targetchangetime += Time.deltaTime;//時間計測
//        if (Targetchangetime > 5)
//        {
//            Targetchangeflg = true;//目的値を変更
//            Targetchangetime = 0;
//        }
//    }
//}