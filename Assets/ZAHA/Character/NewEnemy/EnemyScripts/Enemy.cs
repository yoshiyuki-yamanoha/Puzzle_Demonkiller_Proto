﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    //bool turnflg = true;
    bool init_abnormal_ui = true;
    bool init_abnormal = true;
    bool init_anim_flg = true;

    private void Start()
    {
        Init_speed = Speed;//初期のスピード保存
        Hp = Max_hp;
        Fire.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (Enemy_anim == null) return; //敵のアニメーション取得

        if (Generation_enemy == null)//敵を生成する取得
        {
            Generation_enemy = GameObject.Find("Sponer").GetComponent<GenerationEnemy>();
        }

        if (Trun_manager == null)//ターンマネージャーを取得
        {
            Trun_manager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        }

        if (!Istrun)//自分のターンじゃない時
        {
            //魔法のターンの時に
            if (Trun_manager.GetTrunPhase() == TrunManager.TrunPhase.MagicAttack) {
                if (init_abnormal)//1回のみ入るフラグ
                {
                    //Debug.Log("状態異常の中身" + Abnormal_condition);

                    switch (Abnormal_condition)//状態異常の中身見る
                    {
                        case AbnormalCondition.NONE:
                            //Debug.Log("状態異常じゃないです！！");
                            break;
                        case AbnormalCondition.Fire:
                            //Debug.Log("炎ダメージ");
                            Fire_Abnormal_Condition();
                            break;
                        case AbnormalCondition.Ice:
                            //Debug.Log("氷ダメージ");
                            //Ice_Abnormal_Condition();
                            break;
                    }

                    //Debug.Log("ターン終了");
                    init_abnormal = false;
                }
            }
            //ここダメージ処理書いてた場所
        }

        //自分(敵)のターンだったら
        if (Trun_manager.trunphase == TrunManager.TrunPhase.Enemy)
        {
            init_abnormal = true;//状態異常に1回のみ入るフラグ
            Istrun = true;//自分のターン(敵)開始
        }
        else //ターンを終了する時
        {
            Enemy_action = EnemyAction.Movement;//ターンを動きにする
            Istrun = false;//ターン終了
            init_anim_flg = true;
            Is_action = false;//アクションをオフにする
            init_abnormal_ui = true;
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
                    EnemyMovement();
                    break;
                case EnemyAction.Attack:
                    EnemyAttack();
                    break;
            }
        }

        //死亡フラグが立った時。
        if (Deathflg)
        {
            if (Init_animflg) { Enemy_anim.TriggerDeath("Death"); Init_animflg = false; }; //一回のみ死亡アニメーション再生
        }

        Enemy_anim.AnimStatus(status);//アニメーション更新
    }


    //魔法陣の当たり判定
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magic"))//当たった相手が魔法だったら
        {
            //Fireだったら、
            //当たった魔法が火のやつなら。(お試し、今は魔法に当たった時点)
            //燃焼のタグ。

            
                                           //Debug.Log("火を当てる");
                                           //Damage(1);//ダメージ処理
                                           //Destroy(other.gameObject);
        }


        //pullします。
        if (other.CompareTag("Fire"))//燃焼のタグ
        {
            Abnormal_condition = AbnormalCondition.Fire;
            Fire_abnormality_turncount = 0;//持続リセット
        }
    }

    //敵の攻撃
    void EnemyAttack()
    {
        //if (Attackflg)
        //{
        //    ////この辺、深夜脳死でかいたので、後で修正予定。
        //    if (!Enemy_anim.AnimPlayBack("EnemyAttack") && !Enemy_anim.AnimPlayBack("EnemyAttack"))
        //    {//再生

        //        Attacktime += Time.deltaTime; //3秒おきに攻撃
        //    }

        //    if (Attacktime > 3)
        //    {
        //        Debug.Log("攻撃");
        //        Enemy_anim.TriggerAttack("Attack");//攻撃trigger
        //        Attacktime = 0;
        //        Attackflg = false;
        //        Is_action = true;
        //    }
        //}
    }

    //敵移動
    void EnemyMovement()
    {
        if (!Endflg)//最終地点にいるのか
        {
            GameObject forward_obj = Generation_enemy.rootpos[IndexCheckX(X)].transform.GetChild(IndexCheckY(Y + 1)).gameObject; //前方
            GameObject right_obj = Generation_enemy.rootpos[IndexCheckX(X + 1)].transform.GetChild(IndexCheckY(Y)).gameObject;//右
            GameObject left_obj = Generation_enemy.rootpos[IndexCheckX(X - 1)].transform.GetChild(IndexCheckY(Y)).gameObject;//左
            GameObject forward_right_obj = Generation_enemy.rootpos[IndexCheckX(X + 1)].transform.GetChild(IndexCheckY(Y + 1)).gameObject; //前右
            GameObject forward_left_obj = Generation_enemy.rootpos[IndexCheckX(X - 1)].transform.GetChild(IndexCheckY(Y + 1)).gameObject; //前左


            if (Targetchangeflg)
            {
                if (forward_obj.GetComponent<PseudoArray>().Mass_status == (int)PseudoArray.MassStatus.NONE)//下方向
                {
                    
                    Ismove = true;
                    NextposX = X;
                    NextposY = Y + 1;
                }
                else if (forward_right_obj.GetComponent<PseudoArray>().Mass_status == (int)PseudoArray.MassStatus.NONE)//右
                {
                    //Debug.DrawLine(transform.position, forward_right_obj.transform.position, Color.green);
                    Ismove = true;
                    NextposX = X + 1;
                    NextposY = Y + 1;
                }
                else if (forward_left_obj.GetComponent<PseudoArray>().Mass_status == (int)PseudoArray.MassStatus.NONE)//左
                {
                    //Debug.DrawLine(transform.position, forward_left_obj.transform.position, Color.green);
                    Ismove = true;
                    NextposX = X - 1;
                    NextposY = Y + 1;
                }
                else if (right_obj.GetComponent<PseudoArray>().Mass_status == (int)PseudoArray.MassStatus.NONE)//右下
                {
                    //Debug.DrawLine(transform.position, right_obj.transform.position, Color.green);
                    Ismove = true;
                    NextposX = X + 1;
                    NextposY = Y;
                }
                else if (left_obj.GetComponent<PseudoArray>().Mass_status == (int)PseudoArray.MassStatus.NONE)//左下
                {
                    //Debug.DrawLine(transform.position, left_obj.transform.position, Color.green);
                    Ismove = true;
                    NextposX = X - 1;
                    NextposY = Y;
                }
                else//移動しない。
                {
                    Ismove = false;
                    NextposX = X;
                    NextposY = Y;
                }
                Targetchangeflg = false;
            }

            Debug.DrawLine(transform.position, Generation_enemy.rootpos[IndexCheckX(NextposX)].transform.GetChild(IndexCheckX(NextposY)).gameObject.transform.position, Color.green);

            //移動している時
            if (Ismove) { Generation_enemy.rootpos[IndexCheckX(NextposX)].transform.GetChild(IndexCheckY(NextposY)).GetComponent<PseudoArray>().Mass_status = PseudoArray.MassStatus.ENEMY; }

            Oldx = X;//位置を保存
            Oldy = Y;//位置を保存

            GameObject target_obj = Generation_enemy.rootpos[IndexCheckX(NextposX)].transform.GetChild(IndexCheckY(NextposY)).gameObject;//次の位置にターゲットを設定

            Vector3 target = TargetDir(this.gameObject, target_obj);

            if (Ismove)
            {
                Generation_enemy.rootpos[IndexCheckX(Oldx)].transform.GetChild(IndexCheckY(Oldy)).GetComponent<PseudoArray>().Mass_status = PseudoArray.MassStatus.NONE;//前回マスをオフにする。
                Move(IndexCheckX(NextposX), IndexCheckY(NextposY)); status = Status.Walk;//移動処理
            }



            //目的値についているか?
            if (target.magnitude < Targetdistance)
            {
                Y = IndexCheckY(NextposY);
                X = IndexCheckX(NextposX);

                if (Y == Generation_enemy.max_y - 1)
                {
                    Endflg = true; //さ
                }

                //ここで状態異常確認
                if (init_abnormal_ui)//1回のみ入るフラグ
                {
                    //Debug.Log("状態異常の中身" + Abnormal_condition);

                    switch (Abnormal_condition)//状態異常の中身見る
                    {
                        case AbnormalCondition.NONE:
                            //Debug.Log("状態異常じゃないです！！");
                            break;
                        case AbnormalCondition.Fire:
                            //Debug.Log("炎ダメージ");
                            //Fire_Abnormal_Condition();
                            Fire_Abnormal_UI();
                            break;
                        case AbnormalCondition.Ice:
                            //Debug.Log("氷ダメージ");
                            //Ice_Abnormal_Condition();
                            break;
                    }

                    //Debug.Log("ターン終了");
                    init_abnormal_ui = false;
                }


                status = Status.Idle;//アイドル状態         
                Ismove = false;//動きを止める。
                Targetchangeflg = true;
                Is_action = true;//行動した。

            }
        }
        else
        {

            Is_action = true;

            if (!Enemy_anim.AnimPlayBack("EnemyAttack"))
            {//再生

                Attacktime += Time.deltaTime; //3秒おきに攻撃
            }

            if (init_anim_flg) { Enemy_anim.TriggerAttack("Attack"); init_anim_flg = false; }//攻撃trigger

            if (Attacktime > 2.5f)
            {
                Attacktime = 0;
            }

        }

        //if (!Ismove)
        //{
        //    if (Y == 14)
        //    {
        //        Attackflg = true;
        //        if (Attackflg)
        //        {
        //            //////この辺、深夜脳死でかいたので、後で修正予定。
        //            if (!Enemy_anim.AnimPlayBack("EnemyAttack") && !Enemy_anim.AnimPlayBack("EnemyAttack"))
        //            {//再生

        //                Attacktime += Time.deltaTime; //3秒おきに攻撃
        //            }



        //            if (Attacktime > 2.5f)
        //            {
        //                Enemy_anim.TriggerAttack("Attack");//攻撃trigger
        //                Attacktime = 0;
        //                Attackflg = false;
        //                Is_action = true;
        //            }
        //        }
        //    }
        //}
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


//Rayキャスト 前方に何があるかを見る。
//Vector3 mypos = transform.position;
//mypos.y = mypos.y + 2;
//Ray ray = new Ray(mypos, transform.forward);
//Debug.DrawRay(ray.origin, ray.direction * 3, Color.green, 1, false);
//RaycastHit hit;


////
//if (Physics.Raycast(ray, out hit, 3))
//{
//    if (hit.collider.gameObject.CompareTag("Barrier"))
//    {
//        //Ismove = false;
//        //Attackflg = true;//攻撃オンにする
//        //status = Status.Idle;//アイドル状態
//    }
//    else
//    {
//        Debug.Log("移動");
//        //Y++;//目的値切り替え
//        status = Status.Walk;
//    }
//}

//if (Y == 13)
//{
//    Ismove = false;
//    Attackflg = true;//攻撃オンにする
//}