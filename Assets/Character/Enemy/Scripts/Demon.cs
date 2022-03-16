using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : EnemyBase
{
    
    [SerializeField] float distance = 0;
    float fremTime = 0;
    [SerializeField] float maxfremtime = 3;
    [SerializeField] EnemyAnimationBase enemy_anim = null;
    [SerializeField] GameObject target = null;
    
    //追加
    [SerializeField] float Offset = 0;
    bool init_anim = true;
    int oldrand = 0;
    
    bool enemy_start = false;
    [SerializeField] GameObject hiteffect = null;
    void FixedUpdate()
    {
        if (enemy_anim == null) return;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Barricade");
        }

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

            if (TargetDir(this.gameObject, target).magnitude > distance) {//ターゲットの距離によって移動。
                Move();
                status = Status.Walk;//歩き状態

                if (TargetDir(this.gameObject, target).magnitude < distance + Offset)//フィリップキックの攻撃!! 一回のみ
                {
                    if (init_anim)
                    {
                        if (target.tag == "Barricade")
                        {
                            Debug.Log("攻撃");
                            enemy_anim.TriggerAttack("FlipKick");
                        }
                        else
                        {
                            enemy_anim.TriggerAttack("SwordAttack");
                        }

                        init_anim = false;
                    }
                }
            }
            else
            {
                status = Status.Idle;//止まる。//ターゲットに到達

                //この辺、深夜脳死でかいたので、後で修正予定。
                if (!enemy_anim.AnimPlayBack("EnemyAttack")) {//再生
                    fremTime += Time.deltaTime; //3秒おきに攻撃
                }

                if (fremTime > maxfremtime) {//フレーム（秒）攻撃する

                    if (GetRandom(0, 1) == 0)
                    {
                        enemy_anim.TriggerAttack("Attack");//攻撃trigger
                    }
                    else
                    {
                        enemy_anim.TriggerAttack("AttackKick");//攻撃trigger
                    }

                    transform.rotation = Quaternion.LookRotation(TargetDir(this.gameObject, target));//プレイヤーの方向を向く
                                                                                                     //再生中ならリセット 
                    fremTime = 0;
                }
            }

        //死亡フラグが立った時。
        if (Deathflg)
        {
            if (Init_animflg) { enemy_anim.TriggerDeath("Death"); Init_animflg = false;}; //一回のみ死亡アニメーション再生
        }

        enemy_anim.AnimStatus(status);//アニメーション更新
    }

    int GetRandom(int begin, int end)
    {
        //int walk = 0;//作業用変数
        int newrand = 0;

        //生成した値が同じだったら生成して
        do
        {
            newrand = Random.Range(begin, end + 1);
        } while (newrand == oldrand);

        oldrand = newrand;

        return newrand;
    }

    //入力受付
    private void Update()
    {

    }

    private void Move()
    {
        transform.position += TargetDir(this.gameObject, target).normalized * Speed * Time.deltaTime;//移動
        transform.rotation = Quaternion.LookRotation(TargetDir(this.gameObject, target));//ターゲットの方向を向く
    }

    private void OnTriggerEnter(Collider other)
    {
        //ここにコアのdamage処理を追加する。
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("PlayerHIt");
            GameObject.Find("Sphere").GetComponent<CoreLife>().CoreDamege();

        }

        if (other.gameObject.tag == "Barricade") //バリケードのやつ
        {
            Debug.Log("BarricadeHIt");
            other.GetComponent<Barricade>().Damage(Attack);
            other.GetComponent<Barricade>().ColorChange();
        }

        if (other.gameObject.tag == ("Barricade") || other.gameObject.tag == ("Player"))
        {
            for (int i = 0; i < 2; i++)
            {
                ParticleSystem newhiteffect = Instantiate(hiteffect.transform.GetChild(i).GetComponent<ParticleSystem>(), other.gameObject.transform.position, Quaternion.identity);
                newhiteffect.Play();
            }
        }

        //// 魔法が当たるとダメージ
        //if (other.gameObject.tag == "Magic")
        //{
        //    Debug.Log("hit_tri");

        //    this.GetComponent<Demon>().Damage(100.0f);
        //}
    }

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

    private void OnTriggerStay(Collider other)
    {
        // 魔法が当たるとダメージ
        if (other.gameObject.tag == "Magic")
        {
            Debug.Log(other.gameObject);
            Debug.Log("hit_triStay");
            GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(other.gameObject);
            this.GetComponent<Demon>().Damage(100.0f);
        }
    }
}
