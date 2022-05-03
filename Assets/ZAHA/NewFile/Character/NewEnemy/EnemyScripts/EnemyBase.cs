﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    //攻撃エリア
    bool init_goal = true;
    Node goal;
    bool attackaria;
    Vector2Int attackpos;
    bool init_attack_search = true;

    Astar astar;//インスタンス化
    Vector2Int move_pos;
    static List<GameObject> enemys_ = new List<GameObject>();//敵のリスト化
    [SerializeField] int mynumber;

    [SerializeField] MapMass map = null;
    bool endflg = false;
    bool init_search_flg = true;

    //前回の位置
    int old_x = 0;
    int old_y = 0;

    int next_x = 0;//目的値設定 //次の移動先を見る。
    int next_y = 0;

    bool target_distance = false;
    //変数
    float hp = 0; //hp
    [SerializeField] float max_hp = 0; //hp
    [SerializeField] int attack = 0; //攻撃
    [SerializeField] float speed = 0; //速度
    float init_speed = 0;
    [SerializeField] bool deathflg = false; //死亡フラグ
    bool init_animflg = true; //アニメーションを一回だけ実行させたい時フラグ

    [SerializeField] GenerationEnemy generation_enemy = null;//生成スクリプト
    [SerializeField] EnemyAnimationBase enemy_anim = null;

    [SerializeField] int x = 0;//スタート位置の要素数
    [SerializeField] int y = 0;//次の位置の要素数

    [SerializeField] TrunManager trun_manager = null;
    [SerializeField] ManageCoreState core = null;

    bool targetchangeflg = true;//次の目的値を切り替えるフラグ
    float targetdistance = 0.1f;
    float targetchangetime = 0;//ターゲット切り替え時間

    bool rangechek = true;
    bool attackflg = false;
    float attacktime = 0;


    bool istrun = false;//ターン中か
    bool is_action = false;
    bool ismove = false;

    [SerializeField] Slider hpber = null;

    AbnormalCondition abnormal_condition = AbnormalCondition.NONE; //初期状態は状態異常なし。
    public Status status;
    public EnemyKinds enemy_kinds;
    EnemyAction enemy_action;

    //状態異常UIで使用
    [SerializeField] Image fire = null;
    int fire_abnormality_turncount = 0;//火異常カウント
    int ice_abnormality_turncount = 0;

    bool init_abnormal_ui = true;
    bool init_abnormal = true;
    bool init_anim_flg = true;
    //列挙体
    public enum AbnormalCondition //状態異常
    {
        NONE,//無し
        Fire,//燃えてる
        Ice,//凍結
    };

    public enum EnemyKinds //敵種類
    {
        Demon,
        Demon1,
        Boss,
        Bom,
    }

    public enum Status
    {
        Idle,
        Walk,
        Attack,
        Death,
    }

    public enum Mode
    {
        Y,
        X,
    }

    public enum EnemyAction
    {//エネミーアクション
        Generation,
        Movement,
        Attack,
    }


    //参照用
    public float Hp { get => hp; set => hp = value; }
    public int Attack { get => attack; }
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
    public ManageCoreState Core { get => core; set => core = value; }
    public bool Rangechek { get => rangechek; set => rangechek = value; }
    public float Attacktime { get => attacktime; set => attacktime = value; }
    public bool Attackflg { get => attackflg; set => attackflg = value; }
    public TrunManager Trun_manager { get => trun_manager; set => trun_manager = value; }
    public bool Istrun { get => istrun; set => istrun = value; }
    public bool Is_action { get => is_action; set => is_action = value; }
    public bool Ismove { get => ismove; set => ismove = value; }
    public float Init_speed { get => init_speed; set => init_speed = value; }
    public float Max_hp { get => max_hp; set => max_hp = value; }
    public Slider Hpber { get => hpber; set => hpber = value; }
    public EnemyAction Enemy_action { get => enemy_action; set => enemy_action = value; }
    public int Oldx { get => old_x; set => old_x = value; }
    public int Oldy { get => old_y; set => old_y = value; }
    public AbnormalCondition Abnormal_condition { get => abnormal_condition; set => abnormal_condition = value; }
    public int Fire_abnormality_turncount { get => fire_abnormality_turncount; set => fire_abnormality_turncount = value; }
    public int Ice_abnormality_turncount { get => ice_abnormality_turncount; set => ice_abnormality_turncount = value; }
    public int NextposX { get => next_x; set => next_x = value; }
    public int NextposY { get => next_y; set => next_y = value; }
    public bool Init_search_flg { get => init_search_flg; set => init_search_flg = value; }
    public bool Endflg { get => endflg; set => endflg = value; }
    public Image Fire { get => fire; set => fire = value; }
    public bool Init_abnormal_ui { get => init_abnormal_ui; set => init_abnormal_ui = value; }
    public bool Init_abnormal { get => init_abnormal; set => init_abnormal = value; }
    public bool Init_anim_flg { get => init_anim_flg; set => init_anim_flg = value; }
    public MapMass Map { get => map; set => map = value; }
    public bool Target_distance { get => target_distance; set => target_distance = value; }
    public bool Attackaria { get => attackaria; set => attackaria = value; }
    public Vector2Int Attackpos { get => attackpos; set => attackpos = value; }
    public bool Init_attack_search { get => init_attack_search; set => init_attack_search = value; }

    public void InitFunction()
    {
        GetObject();
        Init_speed = Speed;//初期のスピード保存
        Hp = Max_hp;//MaxHP
        Hpber.maxValue = Max_hp;//HPゲージに反映
        Fire.gameObject.SetActive(false);
        enemys_.Add(this.gameObject);//自分を追加
        mynumber = enemys_.Count;
    }

    public void EnemyTurnStart()
    {
        Istrun = true;//自分のターン(敵)開始
    }

    public void EnemyTurnEnd()
    {
        Init_abnormal = true;//状態異常に1回のみ入るフラグ
        Init_abnormal_ui = true;//1ターンに1回のみ処理する用フラグ
        Init_attack_search = true;//攻撃検索オン

        Enemy_action = EnemyAction.Movement;//ターンを動きにする
        Istrun = false;//ターン終了
        Init_anim_flg = true;
        Is_action = false;//アクションをオフにする
    }

    public Vector3 TargetDir(GameObject Enemy, GameObject Target)//ターゲットの方向に向き処理(移動に使用予定) 
    {
        Vector3 enemyPos = Enemy.transform.position;
        Vector3 targetPos = Target.transform.position;
        Vector3 def = targetPos - enemyPos;//向き生成。
        def.y = 0;//Y軸方向は見ない。
        return def;
    }
    public List<GameObject> GetEnemyList()
    {
        return enemys_;
    }

    public void HPber()
    {
        Hpber.value = hp;
    }

    public float Damage(float damage)//damage処理
    {

        hp -= damage;
        if (hp <= 0) { hp = 0; speed = 0; deathflg = true; /*死亡フラグ立てる 速度0 HP0*/  enemys_.Remove(this.gameObject); }
        else { Enemy_anim.TriggerAttack("HitDamage"); }

        return hp;
    }

    public void CoreCheck()
    {
        if (Core == null)
        {
            Core = GameObject.FindWithTag("Core").GetComponent<ManageCoreState>();
        }
    }

    //public void Move(int startpos, int nextpos)//Move処理 //親(初期位置) //目的値
    //{
    //    Vector3 target = TargetDir(this.gameObject, Generation_enemy.rootpos[startpos].transform.GetChild(nextpos).gameObject).normalized;
    //    transform.position += target * Speed * Time.deltaTime;//移動
    //}

    public void LookTarget(Vector3 dif)//プレイヤーの位置と目的値を渡す
    {
        Vector3 dir = new Vector3(dif.x, 0, dif.z);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public int IndexCheckX(int index)
    {

        if (index < 0) { index = 0; }
        if (index > Generation_enemy.max_x - 1) { index = Generation_enemy.max_x - 1; }
        return index;
    }

    public int IndexCheckY(int index)
    {
        if (index < 0) { index = 0; }
        if (index > Generation_enemy.max_y - 1) { index = Generation_enemy.max_y - 1; }
        return index;
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

    //自分に一番近いかつ、自分より後ろに居る敵を返してくれる優しい関数
    public (GameObject, int) GetNextEnemy(int currentJumpNum)
    {

        GameObject nearestEnemy = null;

        //一番近いかつ自分より後ろに居る敵を検索
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float dist = 999.0f;
        foreach (var e in enemies)
        {
            float distt = Vector3.Distance(transform.position, e.transform.position);

            //暫定一番近いやつより近い奴が居たら
            if (distt < dist)
            {

                dist = distt;
                nearestEnemy = e;
            }
        }

        return (nearestEnemy, currentJumpNum);
    }

    //火の魔法状態。
    public void Fire_Abnormal_Condition()
    {
        //Debug.Log("Im On fire");
        Fire_abnormality_turncount++;

        if (Fire_abnormality_turncount <= 3)
        {
            //Debug.Log("ダメージアニメーション");
            //2ダメージ減らす。
            Damage(2);
            enemy_anim.TriggerAttack("HitDamage");
        }
        else
        {
            Abnormal_condition = AbnormalCondition.NONE;//状態異常解除
            Fire_abnormality_turncount = 0;//ターンリセット
        }
    }

    public void Fire_Abnormal_UI()
    {

        if (Fire_abnormality_turncount < 3)
        {
            Fire.gameObject.SetActive(true);
        }
        else
        {
            Fire.gameObject.SetActive(false);
        }
    }

    //凍結処理
    public void Ice_Abnormal_Condition()
    {
        //Debug.Log("凍結魔法だわよん");
        if (!Is_action)
        {
            Ice_abnormality_turncount++;//呼ばれたらカウント
            //Debug.Log("凍結経過ターン" + Ice_abnormality_turncount);

        }

        //Debug.Log(ice_abnormality_turncount);
        if (Ice_abnormality_turncount >= 3)//2ターン経過したら
        {
            Abnormal_condition = AbnormalCondition.NONE;
            ice_abnormality_turncount = 0;
            Damage(1);
            Enemy enemy = transform.GetComponent<Enemy>();
            Destroy(enemy.pentaIceEff);
        }
    }


    public void DeleteListEnemy()
    {
        List<GameObject> g = new List<GameObject>();
        foreach (var enemy in enemys_)
        {
            if (enemy == null)
            {
                g.Add(enemy);
            }
        }

        for (int i = 0; i < g.Count; i++)
        {
            enemys_.Remove(g[i]);
        }
    }

    public bool AbnormalStatus()
    {
        //Debug.Log(Abnormal_condition);
        if (Init_abnormal)//1回のみ入るフラグ
        {
            //Debug.Log("状態異常確認");
            switch (Abnormal_condition)//状態異常の中身見る
            {
                case AbnormalCondition.NONE:
                    break;
                case AbnormalCondition.Fire:
                    if (Fire.gameObject.activeInHierarchy)
                    {//
                        Fire_Abnormal_Condition();
                    }
                    break;
                case AbnormalCondition.Ice:
                    Ice_Abnormal_Condition();
                    break;
            }

            //Debug.Log("ターン終了");
            Init_abnormal = false;
        }

        return Init_abnormal;
    }

    public void EnemyDeath()
    {
        //死亡フラグが立った時。
        if (Deathflg)
        {
            map.Map[y, x] = (int)MapMass.Mapinfo.NONE; //死亡した位置敵情報削除
            if (Init_animflg) { Enemy_anim.TriggerDeath("Death"); Init_animflg = false; }; //一回のみ死亡アニメーション再生
        }
    }

    public void EnemyMovement(int massnum)
    {
        if (Init_attack_search)
        {
            AttackSearchMovement(massnum);
            Init_attack_search = false;
        }

        if (Attackaria && Abnormal_condition != AbnormalCondition.Ice)
        {
            Attacktime += Time.deltaTime;
            if (Attacktime > 2)
            {
                Is_action = true;
                Attacktime = 0;
            }
            EnemyAttack();
        }
        else
        {
            if (Targetchangeflg)//一回のみ処理 行ける座標を取得
            {
                if (init_goal)
                {
                    goal = new Node(null, map.GetCore().pos[1/*Random.Range(0, 4)*/]);//ランダムをコメントアウト
                    init_goal = false;
                }

                //SearchMovement(massnum); //2マス。
                move_pos = astar.astar(new Node(null, new Vector2Int(X, Y)), goal);
                NextposX = move_pos.x;
                NextposY = move_pos.y;
                Ismove = true;
                Target_distance = false;
                Targetchangeflg = false;
            }

            //移動している時
            if (Ismove && Abnormal_condition != AbnormalCondition.Ice)
            {
                Map.Map[IndexCheckY(NextposY), IndexCheckX(NextposX)] = (int)MapMass.Mapinfo.Enemy;
            }

            Oldx = X;//位置を保存
            Oldy = Y;//位置を保存

            if (Ismove && Abnormal_condition != AbnormalCondition.Ice)
            {
                
                MassMove(IndexCheckY(NextposY), IndexCheckX(NextposX));
                status = Status.Walk;//移動処理

                Hpber.gameObject.SetActive(false);
            }


            //移動したらオン

            //目的値についているか?
            if (Target_distance || Abnormal_condition == AbnormalCondition.Ice)//target.magnitude < Targetdistance
            {
                Map.Map[IndexCheckY(Oldy), IndexCheckX(Oldx)] = (int)MapMass.Mapinfo.NONE;
                status = Status.Idle;//アイドル状態         
                Ismove = false;//動きを止める。
                Hpber.gameObject.SetActive(true);

                Y = IndexCheckY(NextposY);
                X = IndexCheckX(NextposX);

                //ここで前方が攻撃エリアか見る

                //自分の回りの位置が攻撃エリアかをみて
                //攻撃エリアなら攻撃するフラグをオン


                //ここで状態異常確認
                if (Init_abnormal_ui)//1回のみ入るフラグ
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
                    Init_abnormal_ui = false;
                }

                Targetchangeflg = true;
                Is_action = true;//行動した。

            }
        }

        UIFacing();
        //}
        //攻撃
        //EnemyAttack();
    }

    //取得
    public void GetObject()
    {
        //if (Enemy_anim == null) return; //敵のアニメーション取得
        Generation_enemy = GameObject.Find("Sponer").GetComponent<GenerationEnemy>();
        Trun_manager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        Map = GameObject.Find("MapInstance").GetComponent<MapMass>();
        astar = this.gameObject.GetComponent<Astar>();
    }

    //敵攻撃
    public void EnemyAttack()
    {
        //if (!Enemy_anim.AnimPlayBack("EnemyAttack"))
        //{//再生

        //    Attacktime += Time.deltaTime; //3秒おきに攻撃
        //}
        Vector2Int attackpos = Attackpos;

        Vector3 attack_pos_dir = new Vector3(attackpos.x * map.Tilemas_prefab.transform.localScale.x, 0, attackpos.y * -map.Tilemas_prefab.transform.localScale.z) - transform.position;

        if (Init_anim_flg)
        {
            if (map.Core_bari_Data[attackpos.y, attackpos.x].gameObject != null)
            {
                if (map.Map[attackpos.y, attackpos.x] == (int)MapMass.Mapinfo.bari) //バリケードだったら
                {//バリケード
                    LookTarget(attack_pos_dir);//向き移動
                    map.Core_bari_Data[attackpos.y, attackpos.x].GetComponent<ManageBarricade>().ReceiveDamage(Attack);//バリケードにダメージよーん
                }
                else if (map.Map[attackpos.y, attackpos.x] == (int)MapMass.Mapinfo.core)
                {
                    LookTarget(attack_pos_dir);//向き移動
                    map.Core_bari_Data[attackpos.y, attackpos.x].GetComponent<ManageCoreState>().ReceiveDamage(Attack);//コアにダメージよーん
                }
            }

            Enemy_anim.TriggerAttack("Attack");
            Init_anim_flg = false;
        }

        //if (Attacktime > 1.5f)
        //{
        //    Attacktime = 0;
        //}
    }

    public void AttackSearchMovement(int massnum)
    {
        if (Map.Map[IndexCheckY(Y + massnum), IndexCheckX(X)] == (int)MapMass.Mapinfo.core || Map.Map[IndexCheckY(Y + massnum), IndexCheckX(X)] == (int)MapMass.Mapinfo.bari)//下方向
        {
            if (map.Core_bari_Data[IndexCheckY(Y + massnum), IndexCheckX(X)].gameObject != null)
            {
                Attackpos = new Vector2Int(IndexCheckX(X), IndexCheckY(Y + massnum));
                Attackaria = true;
            }
            else
            {
                Map.Map[IndexCheckY(Y + massnum), IndexCheckX(X)] = (int)MapMass.Mapinfo.NONE;
                Attackaria = false;
            }
        }
        else if (Map.Map[IndexCheckY(Y), IndexCheckX(X + massnum)] == (int)MapMass.Mapinfo.core)//右
        {
            if ((map.Core_bari_Data[IndexCheckY(Y), IndexCheckX(X + massnum)].gameObject != null))
            {
                Attackpos = new Vector2Int(IndexCheckX(X + massnum), IndexCheckY(Y));
                Attackaria = true;
            }
            else
            {
                Attackaria = false;
                Map.Map[IndexCheckY(Y), IndexCheckX(X + massnum)] = (int)MapMass.Mapinfo.NONE;
            }
        }
        else if (Map.Map[IndexCheckY(Y), IndexCheckX(X - massnum)] == (int)MapMass.Mapinfo.core)//左
        {
            if ((map.Core_bari_Data[IndexCheckY(Y), IndexCheckX(X - massnum)].gameObject != null))
            {
                Attackpos = new Vector2Int(IndexCheckX(X - massnum), IndexCheckY(Y));
                Attackaria = true;
            }
            else
            {
                Attackaria = false;
                Map.Map[IndexCheckY(Y - massnum), IndexCheckX(X)] = (int)MapMass.Mapinfo.NONE;
            }
        }
        else if (Map.Map[IndexCheckY(Y - massnum), IndexCheckX(X)] == (int)MapMass.Mapinfo.core)//左
        {
            if ((map.Core_bari_Data[IndexCheckY(Y - massnum), IndexCheckX(X)].gameObject != null))
            {
                Attackpos = new Vector2Int(IndexCheckX(X), IndexCheckY(Y - massnum));
                Attackaria = true;
            }
            else
            {
                Attackaria = false;
                Map.Map[IndexCheckY(Y - massnum), IndexCheckX(X)] = (int)MapMass.Mapinfo.NONE;
            }
        }
        //else
        //{
        //    Debug.Log("攻撃エリアなし!");
        //    Attackaria = false;
        //}
    }

    public void MassMove(int next_y, int next_x)
    {
        Vector3 next_pos = new Vector3(next_x * map.Tilemas_prefab.transform.localScale.x, 0, next_y * -map.Tilemas_prefab.transform.localScale.z);
        Debug.DrawRay(transform.position, next_pos - transform.position);

        Vector3 def = next_pos - transform.position;
        /*Vector3.MoveTowards(transform.position, next_pos, Speed * Time.deltaTime);*/
        //Debug.Log("目的値距離" + def.sqrMagnitude);

        LookTarget(def);//向き移動
        transform.position += def.normalized * Speed * Time.deltaTime;//移動

        if (def.sqrMagnitude < 1f)
        {
            LookTarget(Vector3.back);
            Target_distance = true;
            transform.position = next_pos;
        }

        //目的値に着いたらflgを変える。
        // if(){

        //while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
        //}

        //transform.position = targetPos;
    }

    private void UIFacing()
    {
        hpber.transform.forward = Vector3.back;
        Fire.transform.forward = Vector3.back;
    }
}
