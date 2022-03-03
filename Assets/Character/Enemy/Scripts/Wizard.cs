using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : EnemyBase
{
    [SerializeField] float distance = 0;
    float fremTime = 0;
    [SerializeField] float maxfremtime = 3;
    [SerializeField] GameObject target = null;
    [SerializeField] Animator wizard_anim = null;
    void FixedUpdate()
    {

        //hierarchyからターゲットを取得
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        if (TargetDir(this.gameObject, target).magnitude > distance)
        {//ターゲットの距離によって移動。
            Move();
            status = Status.Walk;//歩き状態
        }
        else
        {
            status = Status.Idle;//止まる。//ターゲットに到達
            wizard_anim.SetBool("idle_combat", true);//攻撃待機中
            ////この辺、深夜脳死でかいたので、後で修正予定。
            if (!wizard_anim.GetCurrentAnimatorStateInfo(0).IsName("attack_short_001"))
            {//再生
                fremTime += Time.deltaTime; //3秒おきに攻撃
            }
            else
            {
            }


            if (fremTime > maxfremtime)
            {//フレーム（秒）攻撃する
                wizard_anim.SetBool("attack_short_001", true);//攻撃trigger
                transform.rotation = Quaternion.LookRotation(TargetDir(this.gameObject, target));//プレイヤーの方向を向く
                //再生中ならリセット 
                fremTime = 0;
            }
            else
            {
                wizard_anim.SetBool("attack_short_001", false);
            }
        }

        ////死亡フラグが立った時。
        if (Deathflg)
        {
            if (Init_animflg) { wizard_anim.SetBool("dead", true); Init_animflg = false; }; //一回のみ死亡アニメーション再生
        }

        //enemy_anim.AnimStatus(status);//アニメーション更新
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
            Debug.Log("HIt");
            GameObject.Find("Sphere").GetComponent<CoreLife>().CoreDamege();

        }

        // 魔法が当たるとダメージ
        if (other.gameObject.tag == "Magic")
        {
            Debug.Log("hit_tri");

            this.GetComponent<Wizard>().Damage(100.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 魔法が当たるとダメージ
        if (collision.gameObject.tag == "Magic")
        {
            Debug.Log(collision.gameObject);
            Debug.Log("hit_colli");
            GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(collision.gameObject);
            this.GetComponent<Wizard>().Damage(100.0f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 魔法が当たるとダメージ
        if (other.gameObject.tag == "Magic")
        {
            Debug.Log(other.gameObject);
            Debug.Log("hit_triStay");
            GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(other.gameObject);
            this.GetComponent<Wizard>().Damage(100.0f);
        }
    }
}
