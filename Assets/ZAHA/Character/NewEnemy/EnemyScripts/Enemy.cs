using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    private void Start()
    {
        Init_speed = Speed;//初期のスピード保存
        Hp = Max_hp;
        //Hpber.maxValue = Max_hp;
    }

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
        else //ターンを終了する時
        {
            Enemy_action = EnemyAction.Movement;//ターンを動きにする
            Istrun = false;//ターン終了
            Is_action = false;//アクションをオフにする
        }


        //GameObject forward_obj = Generation_enemy.rootpos[X].transform.GetChild(Y + 1).gameObject; //前方
        //GameObject right_obj = Generation_enemy.rootpos[X + 1].transform.GetChild(Y).gameObject;//右
        //GameObject left_obj = Generation_enemy.rootpos[X - 1].transform.GetChild(Y).gameObject;//左
        //GameObject forward_right_obj = Generation_enemy.rootpos[X + 1].transform.GetChild(Y + 1).gameObject; //前右
        //GameObject forward_left_obj = Generation_enemy.rootpos[X - 1].transform.GetChild(Y + 1).gameObject; //前左


        HPber();//ゲージ減らしてみるぁ＞？

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("魔法が当たりました");
        if (other.CompareTag("Magic"))//当たった相手が魔法だったら
        {
            Damage(1);//ダメージ処理
            Destroy(other.gameObject);
        }
    }

    void EnemyAttack()
    {
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

    void EnemyMovement()
    {
        int nextpos = Y + 1;//目的値設定 //次の移動先を見る。


        Oldx = X;//位置を保存
        Oldy = Y;//位置を保存

        if (IndexCheck(nextpos, (int)Mode.Y)) { return; }
        GameObject target_obj = Generation_enemy.rootpos[X].transform.GetChild(nextpos).gameObject;//次の位置にターゲットを設定

        Vector3 target = TargetDir(this.gameObject, target_obj);

        //目的値についているか?
        if (target.magnitude < Targetdistance)
        {
            status = Status.Idle;//アイドル状態
            Targetchangeflg = true;//目的値切り替え            
        }
        else
        {
            Move(X, nextpos); status = Status.Walk;//移動処理

        }

        if (Targetchangeflg) //ターゲットチェンジする時
        {
            Y++;
            Generation_enemy.rootpos[X].transform.GetChild(Y - 1).GetComponent<PseudoArray>().Whoisflg = false;//前回の位置のマスにオフフラグを立てる。
            Generation_enemy.rootpos[X].transform.GetChild(Y).GetComponent<PseudoArray>().Whoisflg = true;//現在のマスにオンフラグを立てる。
            Is_action = true;//移動した
            Targetchangeflg = false;
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