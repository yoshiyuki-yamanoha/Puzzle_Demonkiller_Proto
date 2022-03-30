﻿using System.Collections;
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

    [SerializeField] GeneralEnemy generalenemy = null;//生成スクリプト
    [SerializeField] EnemyAnimationBase enemy_anim = null;

    [SerializeField] int nextpos = 0;//次の位置の要素数
    [SerializeField] int startpos = 0;//スタート位置の要素数
    
    Transform core = null;

    bool targetchangeflg = false;//次の目的値を切り替えるフラグ
    float targetdistance = 0.01f;
    float targetchangetime = 0;//ターゲット切り替え時間

    bool rangechek = true;
    bool attackflg = false;
    float attacktime = 0;
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
    public GeneralEnemy Generalenemy { get => generalenemy; set => generalenemy = value; }
    public int Nextpos { get => nextpos; set => nextpos = value; }
    public int Startpos { get => startpos; set => startpos = value; }
    public bool Targetchangeflg { get => targetchangeflg; set => targetchangeflg = value; }
    public EnemyAnimationBase Enemy_anim { get => enemy_anim; set => enemy_anim = value; }
    public float Targetdistance { get => targetdistance;}
    public float Targetchangetime { get => targetchangetime; set => targetchangetime = value; }
    public Transform Core { get => core; set => core = value; }
    public bool Rangechek { get => rangechek; set => rangechek = value; }
    public float Attacktime { get => attacktime; set => attacktime = value; }
    public bool Attackflg { get => attackflg; set => attackflg = value; }

    public Vector3 TargetDir(GameObject Enemy , GameObject Target)//ターゲットの方向に向き処理(移動に使用予定) 
    {
        Vector3 enemyPos = Enemy.transform.position;
        Vector3 targetPos = Target.transform.position;
        Vector3 def = targetPos - enemyPos;//向き生成。
        def.y = 0;//Y軸方向は見ない。
        return def;
    }

    public float Damage(float damege)//damage処理
    {
        hp -= damege;
        if (hp <= 0) { hp = 0; speed = 0; deathflg = true;/*死亡フラグ立てる 速度0 HP0*/ }

        return hp;
    }

    public void CoreCheck()
    {
        if (Core == null)
        {
            Core = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
    }

    public void Move(int startpos , int nextpos)//Move処理 //親(初期位置) //目的値
    {
        Vector3 target = TargetDir(this.gameObject, Generalenemy.rootpos[startpos].transform.GetChild(nextpos).gameObject).normalized;
        transform.position += target * Speed * Time.deltaTime;//移動
        
        Debug.DrawRay(this.transform.position, target * 2, Color.red);
    }

    public void LookTarget(bool ismove)
    {
        Vector3 target;
        if (ismove)
        {
            target = Vector3.back - this.gameObject.transform.position;
        }
        else
        {
            target = TargetDir(this.gameObject, Generalenemy.rootpos[startpos].transform.GetChild(nextpos).gameObject);
        }

        transform.rotation = Quaternion.LookRotation(target);
    }

    public bool IndexCheck(int index, int mode)
    {
        rangechek = true;
        if (mode == 0) {
            if (index < 0) { nextpos = 0; rangechek = false; }
            if (index >= generalenemy.max_next - 2) { nextpos = generalenemy.max_next - 2; rangechek = false; }
            
        }
        else if(mode == 1){
            if (index < 0) { startpos = 0; rangechek = false; }
            if (index >= generalenemy.max_startpos - 2) {startpos = generalenemy.max_startpos - 2; rangechek = false; }
        }
        return rangechek;
    }
}
