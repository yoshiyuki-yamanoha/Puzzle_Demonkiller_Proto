using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum Mode
    {
        Y,
        X,
    }

    //変数
    [SerializeField] float hp = 0; //hp
    [SerializeField] float attack = 0; //攻撃
    [SerializeField] float speed = 0; //速度
    float init_speed = 0;
    [SerializeField] bool deathflg = false; //死亡フラグ
    bool init_animflg = true; //アニメーションを一回だけ実行させたい時フラグ

    [SerializeField] GenerationEnemy generation_enemy = null;//生成スクリプト
    [SerializeField] EnemyAnimationBase enemy_anim = null;

    [SerializeField] int y = 0;//次の位置の要素数
    [SerializeField] int x = 0;//スタート位置の要素数

    [SerializeField] TrunManager trun_manager = null;
    Transform core = null;

    bool targetchangeflg = false;//次の目的値を切り替えるフラグ
    float targetdistance = 0.1f;
    float targetchangetime = 0;//ターゲット切り替え時間

    bool rangechek = true;
    bool attackflg = false;
    float attacktime = 0;
    [SerializeField] public EnemyKinds enemy_kinds;

    bool istrun = false;//ターン中か
    bool is_action = false;
    bool ismove = true;

    //氷があるのか
    bool isbarrier = true;

    public enum EnemyKinds //敵種類
    {
        Demon,
        Demon1,
        Boss,
    }

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
    public float Speed { get => speed; set => speed = value; }
    public bool Deathflg { get => deathflg; }
    public bool Init_animflg { get => init_animflg; set => init_animflg = value; }
    public GenerationEnemy Generation_enemy { get => generation_enemy; set => generation_enemy = value; }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public bool Targetchangeflg { get => targetchangeflg; set => targetchangeflg = value; }
    public EnemyAnimationBase Enemy_anim { get => enemy_anim; set => enemy_anim = value; }
    public float Targetdistance { get => targetdistance; }
    public float Targetchangetime { get => targetchangetime; set => targetchangetime = value; }
    public Transform Core { get => core; set => core = value; }
    public bool Rangechek { get => rangechek; set => rangechek = value; }
    public float Attacktime { get => attacktime; set => attacktime = value; }
    public bool Attackflg { get => attackflg; set => attackflg = value; }
    public TrunManager Trun_manager { get => trun_manager; set => trun_manager = value; }
    public bool Istrun { get => istrun; set => istrun = value; }
    public bool Is_action { get => is_action; set => is_action = value; }
    public bool Ismove { get => ismove; set => ismove = value; }
    public bool Isbarrier { get => isbarrier; set => isbarrier = value; }
    public float Init_speed { get => init_speed; set => init_speed = value; }

    public Vector3 TargetDir(GameObject Enemy, GameObject Target)//ターゲットの方向に向き処理(移動に使用予定) 
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

    public void Move(int startpos, int nextpos)//Move処理 //親(初期位置) //目的値
    {
        Vector3 target = TargetDir(this.gameObject, Generation_enemy.rootpos[startpos].transform.GetChild(nextpos).gameObject).normalized;
        transform.position += target * Speed * Time.deltaTime;//移動

        //Debug.DrawRay(this.transform.position, target * 2, Color.red);
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
            target = TargetDir(this.gameObject, Generation_enemy.rootpos[x].transform.GetChild(y).gameObject);
        }

        transform.rotation = Quaternion.LookRotation(target);
    }

    public bool IndexCheck(int index, int mode)
    {
        rangechek = false;

        if (mode == (int)Mode.X)
        {
            if (index < 0) { x = 0; rangechek = true; }
            if (index >= Generation_enemy.max_x) { x = Generation_enemy.max_x; rangechek = true; }
        }
        if (mode == (int)Mode.Y)
        {
            if (index < 0) { y = 0; rangechek = true; }
            if (index >= Generation_enemy.max_y) { y = Generation_enemy.max_y; rangechek = true; Debug.Log("チェック中" + index); }

        }
        return rangechek;
    }

    public void SpeedDown()
    {
        Speed = Init_speed / 2;
    }

    public void SpeedStop()
    {
        Speed = 0;
    }

    public void SpeedReset()
    {
        Speed = Init_speed;
    }
}
