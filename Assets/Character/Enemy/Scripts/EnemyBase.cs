using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //変数
    [SerializeField] float hp = 0; //hp
    [SerializeField] float attack = 0; //攻撃
    [SerializeField] float speed = 0; //速度
    [SerializeField] bool deathflg = false; //死亡フラグ
    bool init_animflg = true; //アニメーションを一回だけ実行させたい時フラグ 
    public enum Status
    {
        Idle,
        Walk,
        Attack,
        Death,
    }

    public Status status;

    //参照用
    public float Hp { get => hp; }
    public float Attack { get => attack; }
    public float Speed { get => speed; }
    public bool Deathflg { get => deathflg; }
    public bool Init_animflg { get => init_animflg; set => init_animflg = value; }

    //ターゲットの方向に向き処理(移動に使用予定)
    public Vector3 TargetDir(GameObject Enemy , GameObject Target) 
    {
        Vector3 enemyPos = Enemy.transform.position;
        Vector3 targetPos = Target.transform.position;
        Vector3 def = targetPos - enemyPos;//向き生成。
        def.y = 0;//Y軸方向は見ない。
        return def;
    }

    //damage処理
    public float Damage(float damege)
    {
        hp -= damege;
        if (hp <= 0) { hp = 0; speed = 0; deathflg = true; /*死亡フラグ立てる 速度0 HP0*/ }

        return hp;
    }
}
