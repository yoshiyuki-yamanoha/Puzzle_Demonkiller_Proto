using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon1 : EnemyBase
{
    float dis = 0;

    [SerializeField] float Distance = 0;
    float FremTime = 0;
    [SerializeField] float Maxfremtime = 3;
    [SerializeField] GameObject Target = null;
    [SerializeField] EnemyAnimationBase Enemy_anim = null;
    void FixedUpdate()
    {
        //hierarchyからターゲットを取得
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Barricade");
        }

        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player");
        }


        dis = TargetDir(this.gameObject, Target).magnitude;
        if (TargetDir(this.gameObject,Target).magnitude > Distance) {//ターゲットの距離によって移動。
            Move();
            status = Status.Walk;//歩き状態
        }
        else
        {
            status = Status.Idle;//止まる。//ターゲットに到達
            Debug.Log("止まる");
            //この辺、深夜脳死でかいたので、後で修正予定。
            if (!Enemy_anim.AnimPlayBack("EnemyAttack")) {//再生
                FremTime += Time.deltaTime; //3秒おきに攻撃
            }

            if (FremTime > Maxfremtime){ //フレーム（秒）攻撃する
                Enemy_anim.TriggerAttack("Attack");//攻撃trigger
                transform.rotation = Quaternion.LookRotation(TargetDir(this.gameObject, Target));//プレイヤーの方向を向く
                //再生中ならリセット 
                FremTime = 0;
            }
        }

        //死亡フラグが立った時
        if (Deathflg)
        {
            if (Init_animflg) { Enemy_anim.TriggerDeath("Death"); Init_animflg = false;}; //一回のみ死亡アニメーション再生
        }

        Enemy_anim.AnimStatus(status);//アニメーション更新

    }

    private void Move()
    {
        transform.position += TargetDir(this.gameObject, Target).normalized * Speed * Time.deltaTime;//移動
        transform.rotation = Quaternion.LookRotation(TargetDir(this.gameObject, Target));//ターゲットの方向を向く
    }

    private void OnTriggerEnter(Collider other)
    {
        //ここにコアのdamage処理を追加する。
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("HIt");
            GameObject.Find("Sphere").GetComponent<CoreLife>().CoreDamege();
        }

        if (other.gameObject.tag == "Barricade")
        {
            Debug.Log("HIt");
            other.GetComponent<Barricade>().Damage(Attack);
            other.GetComponent<Barricade>().ColorChange();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 魔法が当たるとダメージ
        if (other.gameObject.tag == "Meteor")
        {
            Debug.Log(other.gameObject);
            Debug.Log("hit_triStay");
            GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(other.gameObject);
            this.GetComponent<Demon1>().Damage(100.0f);
        }
    }

}

