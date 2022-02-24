using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : EnemyBase
{
    
    [SerializeField] float distance = 0;
    float fremTime = 0;
    [SerializeField] float maxfremtime = 3;
    [SerializeField] EnemyAnimationBase enemy_anim;
    [SerializeField] GameObject target = null;


    void FixedUpdate()
    {
        //hierarchyからターゲットを取得
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        if (TargetDir(this.gameObject,target).magnitude > distance) {//ターゲットの距離によって移動。
            Move();
            status = Status.Walk;//歩き状態
        }
        else
        {
            status = Status.Idle;//止まる。//ターゲットに到達

            //この辺、深夜脳死でかいたので、後で修正予定。
            if (!enemy_anim.AnimPlayBack("EnemyAttack")) {//再生
                fremTime += Time.deltaTime; //3秒おきに攻撃
            }
            else
            {
            }

            if (fremTime > maxfremtime) {//フレーム（秒）攻撃する
                enemy_anim.TriggerAttack("Attack");//攻撃trigger
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

    //入力受付
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Damage(10);//damage
        }
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 魔法が当たるとダメージ
        if(collision.gameObject.tag == "Magic")
            this.GetComponent<Demon>().Damage(100.0f);
    }
}
