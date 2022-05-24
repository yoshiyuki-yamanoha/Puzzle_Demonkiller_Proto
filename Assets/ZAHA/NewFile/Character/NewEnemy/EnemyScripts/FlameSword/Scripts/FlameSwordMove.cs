using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlameSwordMove : EnemyBase
{
    [SerializeField] Image Noimage = null; 
    [SerializeField] ParticleSystem[] effect_motion = null;
    [SerializeField] MaterialChange material_change;
    float time = 0;

    bool ice_status = false;

    bool icebreak_flg = false;

    public bool Ice_status { get => ice_status; set => ice_status = value; }

    // Start is called before the first frame update
    void Start()
    {
        InitFunction();
        Noimage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //自分(敵)のターンだったら
        if (Trun_manager.trunphase == TrunManager.TrunPhase.Enemy)
        {
            if (!AbnormalStatus())
            {//ステータスダメージが喰らったらエネミーターンにする。
                time += Time.deltaTime;
                if (time > 3)
                {
                    EnemyTurnStart();
                    time = 0;
                }

                if (Targetchangeflg)
                {
                    Noimage.gameObject.SetActive(false);
                }
            }
        }
        else //ターンを終了する時
        {
            EnemyTurnEnd();
            time = 0;
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
                    EnemyMovement(4);//動けるマス範囲
                    break;
            }
        }

        // 氷の状態異常にはならない処理
        //IceBreak();

        EnemyDeath();//敵が死んだときの処理
        Enemy_anim.AnimStatus(status);//アニメーション更新
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magic"))//当たった相手が魔法だったら
        {
        }

        if (other.CompareTag("Fire"))//燃焼のタグ
        {
            FireEffectPlay();
            Fire_Abnormal_UI();
            Abnormal_condition = AbnormalCondition.Fire;
            Fire_abnormality_turncount = 0;//持続リセット
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Ice"))
        {
            material_change.SetMaterial(material_change.change_material);
            Enemy_anim.SetFloat(0);
            Enemy_anim.TriggerAttack("NoIceAttack");
            Ice_status = true;
            //Noimage.gameObject.SetActive(true);
            //ここでエフェクトを出す。
            //foreach(var effect in effect_motion)
            //{
            //    Debug.Log("氷きかないeffect");
            //    effect.Play();
            //}
            //Abnormal_condition = AbnormalCondition.Ice;
            //other.GetComponent<PentagonIce>().Tin(transform.position);
            //pentaIceEff = GameObject.Find("BreakIce_honmono");
            //Instantiate(pentaIceEff, transform.position, Quaternion.identity);
            //Ice_abnormality_turncount = 0;
            Destroy(other.gameObject);
        }
    }


    /// <summary>
    /// 氷の状態異常になったとき、即状態異常を解除して２ダメージ受ける
    /// </summary>
    void IceBreak() {
        if (Abnormal_condition == AbnormalCondition.Ice) {
            Abnormal_condition = AbnormalCondition.NONE;
            Damage(2);  // 2ダメージ
        }
    }
}
