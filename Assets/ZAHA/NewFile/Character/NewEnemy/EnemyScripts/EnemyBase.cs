using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyBase : MonoBehaviour
{
    bool pos_register = false;

    [SerializeField] AnimEvent anim_event = null;
    bool init_death_flg = true;
    bool ice_flg = false;
    bool ice_instance_flg = false;//アイスオブジェクトを一回のみ生成

    [SerializeField] bool camera_target_core_flg;
    [SerializeField] bool camera_target_bari_flg;

    bool move_wait_flg = false;
    [SerializeField] float move_wait_time = 0;
    float movetime = 0;
    //particleデス!
    [SerializeField] GameObject ice_obj = null;
    [SerializeField] ParticleSystem[] fire_effect = null;//ファイヤーエフェクト
    [SerializeField] Image fire_image = null;
    //ParticleSystem chilled_fire_effect = null;

    SEManager sePlay = null;  //SE
    FootSE footSEPlay;

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

    [SerializeField] Slider hpbar_green = null;
    [SerializeField] Slider hpbar_red = null;

    AbnormalCondition abnormal_condition = AbnormalCondition.NONE; //初期状態は状態異常なし。
    public Status status;
    public EnemyKinds enemy_kinds;
    EnemyAction enemy_action;

    //状態異常UIで使用
    //[SerializeField] Image fire = null;
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
        Goblin,
        Demon,
        Bom,
        Flame,
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

    public void SetPos_Register()
    {
        pos_register = true;
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
    public bool Init_abnormal_ui { get => init_abnormal_ui; set => init_abnormal_ui = value; }
    public bool Init_abnormal { get => init_abnormal; set => init_abnormal = value; }
    public bool Init_anim_flg { get => init_anim_flg; set => init_anim_flg = value; }
    public MapMass Map { get => map; set => map = value; }
    public bool Target_distance { get => target_distance; set => target_distance = value; }
    public bool Attackaria { get => attackaria; set => attackaria = value; }
    public Vector2Int Attackpos { get => attackpos; set => attackpos = value; }
    public bool Init_attack_search { get => init_attack_search; set => init_attack_search = value; }
    public ParticleSystem[] Fire_effect { get => fire_effect; set => fire_effect = value; }
    public Image Fire_image { get => fire_image; set => fire_image = value; }
    public bool Ice_instance_flg { get => ice_instance_flg; set => ice_instance_flg = value; }
    public bool Ice_flg { get => ice_flg; set => ice_flg = value; }
    public Slider Hpbar_red { get => hpbar_red; set => hpbar_red = value; }
    public Slider Hpbar_green { get => hpbar_green; set => hpbar_green = value; }

    public void InitFunction()
    {
        if (enemy_kinds != EnemyKinds.Flame)
        {
            IceObjSetActivOff();//アイスオブジェクトオフ
        }
        Camera_TargetInit();//カメラのターゲット格納初期化
        move_wait_time = Random.Range(0.0f, 1.0f);
        GetObject();
        Init_speed = Speed;//初期のスピード保存
        HPBarInit();
        fire_image.gameObject.SetActive(false);
        enemys_.Add(this.gameObject);//自分を追加
        mynumber = enemys_.Count;
    }

    public void HPBarInit()
    {
        Hp = Max_hp;//MaxHP
        HPBar_Off();
        Hpbar_red.maxValue = Max_hp;//HPゲージに反映
        Hpbar_green.maxValue = Max_hp;
    }

    public void EnemyTurnStart()
    {
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>(); //SE
        footSEPlay = GameObject.Find("WalkSE").GetComponent<FootSE>();
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
        move_wait_flg = false;
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
        Hpbar_green.value = hp;
        Hpbar_red.value = Mathf.Lerp(Hpbar_red.value, hp, Time.deltaTime);//HPbar更新
    }

    public float Damage(float damage)//damage処理
    {

        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            speed = 0;

            //if (enemy_kinds == EnemyKinds.Goblin)//1:ゴブリンの死亡SE
            //{
            //    Debug.Log("ゴブリンSEの中身" + sePlay);
            //    sePlay.Play("GoblinDeath");

            //}
            //else if (enemy_kinds == EnemyKinds.Demon)//2:デモンの死亡SE
            //{
            //    Debug.Log("デーモンSEの中身" + sePlay);
            //    sePlay.Play("DemonDeath");
            //}
            //else if (enemy_kinds == EnemyKinds.Bom)//3:ボム兵の死亡SE
            //{
            //    Debug.Log("ボムSEの中身" + sePlay);
            //    sePlay.Play("BombDeath");
            //}
            //else if (enemy_kinds == EnemyKinds.Flame) //炎の剣の死亡SE
            //{
            //    Debug.Log("炎の剣SEの中身" + sePlay);
            //    sePlay.Play("FlameDeath");
            //}
            /*死亡フラグ立てる 速度0 HP0*/
            if (this.gameObject == null)
            {
                enemys_.Remove(this.gameObject);
            }
            deathflg = true;
        }
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

        Enemy_anim.SetFloat(1);//アニメーションスピードを0にするー

        if (Fire_abnormality_turncount <= 3)
        {
            //Debug.Log("ダメージアニメーション");
            //2ダメージ減らす。
            FireEffectPlay();
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
            Fire_image.gameObject.SetActive(true);
        }
        else
        {
            Fire_image.gameObject.SetActive(false);
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

        Enemy_anim.SetFloat(0);//アニメーションスピードを0にするー

        //Debug.Log(ice_abnormality_turncount);
        if (Ice_abnormality_turncount >= 3)//2ターン経過したら
        {
            if (enemy_kinds != EnemyKinds.Flame)
            {
                IceObjSetActivOff();//オフ
            }

            Enemy_anim.SetFloat(1);//アニメーションスピードを1に戻す―
            Abnormal_condition = AbnormalCondition.NONE;
            ice_abnormality_turncount = 0;
            Damage(1);
            Enemy enemy = transform.GetComponent<Enemy>();
            // Destroy(enemy.pentaIceEff);
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

        //for (int list_number = enemys_.Count - 1; list_number >= 0; list_number--)
        //{
        //    if (enemys_[list_number] == null)//リストに入っていた死亡して敵が消えてた場合はリストから削除
        //    {
        //        Debug.Log("敵死にましたー");
        //        enemys_.Remove(enemys_[list_number]);//敵をリストから消す
        //    }
        //}
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
                    if (Fire_image.gameObject.activeInHierarchy)
                    {
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
            if (init_death_flg)
            {
                MainMgr mg = new MainMgr();
                mg.EnemiDieCount();
                init_death_flg = false;
            }
            map.Map[y, x] = (int)MapMass.Mapinfo.NONE;

            if (Init_animflg) { Enemy_anim.TriggerDeath("Death"); Init_animflg = false; }; //一回のみ死亡アニメーション再生
        }
    }

    void TargetChangeUpdate(Vector2Int pos)
    {
        //ターゲット座標からフェンスの存在を見る----
        if (map.Map[pos.y, pos.x] != (int)MapMass.Mapinfo.bari)
        {
            goal = new Node(null, map.GetCore().pos[Random.Range(0, 4)]);//コアの座標を渡す
            init_goal = false;
        }
    }

    public void SetTargetPos(Vector2Int target)
    {
        goal = new Node(null, target);
    }

    public void EnemyMovement(int massnum) //移動か攻撃か判定管理
    {
        if (Init_attack_search) //毎ターン一回のみ判定
        {
            if (!Deathflg)//死亡したのなら見なーい
            {
                AttackSearchMovement();//攻撃出来る場所か判定
                Init_attack_search = false;
            }
            else
            {
                return;
            }
        }

        ////自分がボムだったら
        if (enemy_kinds == EnemyKinds.Bom)
        {
            if (Attackaria) deathflg = true;//攻撃する時自滅するため、死亡フラグを立てる。
        }

        if (Attackaria && Abnormal_condition != AbnormalCondition.Ice) //攻撃処理
        {
            if (Init_anim_flg)
            {
                Enemy_anim.TriggerAttack("Attack");
                Init_anim_flg = false;
            }

            if (anim_event != null)
            {
                if (anim_event.IsAnimAttack())
                {
                    EnemyAttack();
                }
            }
            else
            {
                Debug.Log("eventanimがenemyBaseに入っていません!!" + gameObject.name);
            }
        }
        else //移動処理
        {
            if (Targetchangeflg)//一回のみ処理 行ける座標を取得
            {
                move_pos = astar.astar(new Node(null, new Vector2Int(X, Y)), goal, massnum, init_goal);//移動処理 取得は移動できる座標


                if (map.Map[move_pos.y, move_pos.x] != (int)MapMass.Mapinfo.NONE)
                {
                    Debug.Log("移動出来ない" + gameObject.name);
                    Ismove = false;
                    Target_distance = true;
                }
                else
                {
                    footSEPlay.Play("FootSE");
                    NextposX = move_pos.x;//移動できる座標を設定
                    NextposY = move_pos.y;//移動できる座標を設定
                    Ismove = true;//移動フラグをオン
                    Target_distance = false;//ターゲット距離オフ
                    Targetchangeflg = false;//ターゲットチェンジオフ
                }
            }

            Oldx = X;//今の位置を前回の位置として保存
            Oldy = Y;//今の位置を前回の位置として保存

            //移動フラグがオンになった時
            if (Ismove && Abnormal_condition != AbnormalCondition.Ice)
            {
                Map.Map[IndexCheckY(NextposY), IndexCheckX(NextposX)] = (int)MapMass.Mapinfo.Enemy;//移動先に敵情報を渡す。

                if (MoveTime(move_wait_time))//移動するまで待機する時間
                {
                    MassMove(IndexCheckY(NextposY), IndexCheckX(NextposX));//実移動
                    status = Status.Walk;//歩くアニメーション
                }

                if (Abnormal_condition == AbnormalCondition.Fire)//炎の状態異常が掛かっていたら炎の状態異常をUIをオフ
                {
                    fire_image.gameObject.SetActive(false);
                }

                if (Status.Walk == status)//歩いていたら
                {
                    Map.Map[IndexCheckY(Oldy), IndexCheckX(Oldx)] = (int)MapMass.Mapinfo.NONE;//前回の位置をMap情報をなし。
                }
            }

            //目的値についているかフラグがオンなら
            if (Target_distance || Abnormal_condition == AbnormalCondition.Ice)
            {
                if (Ismove) {
                    Y = IndexCheckY(NextposY);//次の位置を現在位置にする
                    X = IndexCheckX(NextposX);//次の位置を現在位置にする
                }

                status = Status.Idle;//立ち止まっている状態         
                Ismove = false;//動きを止める。

                //HPバー再表示

                if (Abnormal_condition == AbnormalCondition.Fire)//炎の状態異常をアクティブ状態オン
                {
                    fire_image.gameObject.SetActive(true);
                }

                Targetchangeflg = true;//ターゲットチェンジをオン
                Is_action = true;//行動したフラグをオン
            }
        }

        if (init_goal)
        {
            //ターゲットチェンジ更新
            TargetChangeUpdate(goal.Pos);//ターゲットチェンジ
        }

        if (!deathflg)//死亡したフラグがオフの時
        {
            UIFacing();//キャラが他の方向を見た時、UIを前に向ける処理
        }
    }

    public void HPBar_On()
    {
        Hpbar_green.gameObject.SetActive(true);//HPバー再表示
        Hpbar_red.gameObject.SetActive(true);//HPバー再表示
    }

    public void HPBar_Off()
    {
        Hpbar_green.gameObject.SetActive(false);//HPバー再表示
        Hpbar_red.gameObject.SetActive(false);//HPバー再表示
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
        Vector2Int attackpos = Attackpos;

        Vector3 attack_pos_dir = new Vector3(attackpos.x * map.Tilemas_prefab.transform.localScale.x, 0, attackpos.y * -map.Tilemas_prefab.transform.localScale.z) - transform.position;


        if (map.Core_bari_Data[attackpos.y, attackpos.x].gameObject != null) //エラーアクセス使用としている。
        {
            if (map.Map[attackpos.y, attackpos.x] == (int)MapMass.Mapinfo.bari) //バリケードだったら
            {//バリケード
                LookTarget(attack_pos_dir);//向き移動
                if (enemy_kinds != EnemyKinds.Flame) {//フレームじゃなければ
                    map.Core_bari_Data[attackpos.y, attackpos.x].GetComponent<ManageBarricade>().ReceiveDamage(Attack);//バリケードにダメージよーん
                }
                else
                {
                    map.Core_bari_Data[attackpos.y, attackpos.x].GetComponent<ManageBarricade>().ReceiveDamage(10);//バリケードにダメージよーん
                }
            }
            else if (map.Map[attackpos.y, attackpos.x] == (int)MapMass.Mapinfo.core)
            {
                LookTarget(attack_pos_dir);//向き移動
                if (enemy_kinds != EnemyKinds.Flame)
                {
                    map.Core_bari_Data[attackpos.y, attackpos.x].GetComponent<ManageCoreState>().ReceiveDamage(Attack);//コアにダメージよーん
                }
                else
                {
                    map.Core_bari_Data[attackpos.y, attackpos.x].GetComponent<ManageCoreState>().ReceiveDamage(5);//コアにダメージよーん
                }
            }
        }

        if (enemy_kinds == EnemyKinds.Bom)  ////敵の種類のに応じて攻撃のSEを変える　　//爆弾敵/
        {
            sePlay.Play("BombEnemExplosion");
        }
        else if (enemy_kinds == EnemyKinds.Demon || enemy_kinds == EnemyKinds.Goblin) //ゴブリン、デーモン
        {
            sePlay.Play("EnemyAtack");
        }
        else if (enemy_kinds == EnemyKinds.Flame)//炎の剣
        {
            sePlay.Play("FlameAttack");
        }

        Is_action = true;
        anim_event.SetAnimAttack(false);
    }

    public void AttackSearchMovement()
    {
        if (Map.Map[IndexCheckY(Y + 1), IndexCheckX(X)] == (int)MapMass.Mapinfo.core || Map.Map[IndexCheckY(Y + 1), IndexCheckX(X)] == (int)MapMass.Mapinfo.bari)//下方向
        {
            if (map.Core_bari_Data[IndexCheckY(Y + 1), IndexCheckX(X)].gameObject != null)
            {
                //攻撃対象判別。
                if (map.Core_bari_Data[IndexCheckY(Y + 1), IndexCheckX(X)].gameObject.CompareTag("Core"))
                {
                    camera_target_core_flg = true;
                }
                else if (map.Core_bari_Data[IndexCheckY(Y + 1), IndexCheckX(X)].gameObject.CompareTag("Bari"))
                {
                    camera_target_bari_flg = true;
                }

                Attackpos = new Vector2Int(IndexCheckX(X), IndexCheckY(Y + 1));
                Attackaria = true;
            }
            else
            {
                Attackaria = false;
                Camera_TargetInit();
            }
        }
        else if (Map.Map[IndexCheckY(Y), IndexCheckX(X + 1)] == (int)MapMass.Mapinfo.core || Map.Map[IndexCheckY(Y), IndexCheckX(X + 1)] == (int)MapMass.Mapinfo.bari)//右
        {
            if ((map.Core_bari_Data[IndexCheckY(Y), IndexCheckX(X + 1)].gameObject != null))
            {

                if (map.Core_bari_Data[IndexCheckY(Y), IndexCheckX(X + 1)].gameObject.CompareTag("Core"))
                {
                    camera_target_core_flg = true;
                }
                else if (map.Core_bari_Data[IndexCheckY(Y), IndexCheckX(X + 1)].gameObject.CompareTag("Bari"))
                {
                    camera_target_bari_flg = true;
                }

                Attackpos = new Vector2Int(IndexCheckX(X + 1), IndexCheckY(Y));
                Attackaria = true;
            }
            else
            {
                Camera_TargetInit();
                Attackaria = false;
            }
        }
        else if (Map.Map[IndexCheckY(Y), IndexCheckX(X - 1)] == (int)MapMass.Mapinfo.core || Map.Map[IndexCheckY(Y), IndexCheckX(X - 1)] == (int)MapMass.Mapinfo.bari)//左
        {
            if ((map.Core_bari_Data[IndexCheckY(Y), IndexCheckX(X - 1)].gameObject != null))
            {
                if (map.Core_bari_Data[IndexCheckY(Y), IndexCheckX(X - 1)].gameObject.CompareTag("Core"))
                {
                    camera_target_core_flg = true;
                }
                else if (map.Core_bari_Data[IndexCheckY(Y), IndexCheckX(X - 1)].gameObject.CompareTag("Bari"))
                {
                    camera_target_bari_flg = true;
                }

                Attackpos = new Vector2Int(IndexCheckX(X - 1), IndexCheckY(Y));
                Attackaria = true;
            }
            else
            {
                Camera_TargetInit();
                Attackaria = false;
            }
        }
        else if (Map.Map[IndexCheckY(Y - 1), IndexCheckX(X)] == (int)MapMass.Mapinfo.core || Map.Map[IndexCheckY(Y - 1), IndexCheckX(X)] == (int)MapMass.Mapinfo.bari)//上方向
        {
            if ((map.Core_bari_Data[IndexCheckY(Y - 1), IndexCheckX(X)].gameObject != null))
            {
                if (map.Core_bari_Data[IndexCheckY(Y - 1), IndexCheckX(X)].gameObject.CompareTag("Core"))
                {
                    camera_target_core_flg = true;
                }
                else if (map.Core_bari_Data[IndexCheckY(Y - 1), IndexCheckX(X)].gameObject.CompareTag("Bari"))
                {
                    camera_target_bari_flg = true;
                }

                Attackpos = new Vector2Int(IndexCheckX(X), IndexCheckY(Y - 1));
                Attackaria = true;
            }
            else
            {
                Camera_TargetInit();
                Attackaria = false;
            }
        }
        else
        {
            Debug.Log("攻撃エリアなし!");
            Attackaria = false;
        }
    }

    public void MassMove(int next_y, int next_x)
    {
        Vector3 next_pos = new Vector3(next_x * map.Tilemas_prefab.transform.localScale.x, 0, next_y * -map.Tilemas_prefab.transform.localScale.z);
        Debug.DrawRay(transform.position, next_pos - transform.position);

        Vector3 def = next_pos - transform.position;

        LookTarget(def);//向き移動
        transform.position += def.normalized * Speed * Time.deltaTime;//移動

        if (def.sqrMagnitude < 1f)
        {
            Vector3 v3_target_pos = new Vector3(goal.Pos.x * map.Tilemas_prefab.transform.localScale.x, 0, goal.Pos.y * -map.Tilemas_prefab.transform.localScale.z);
            Vector3 def_dir = v3_target_pos - this.transform.position;
            def_dir.y = 0;
            Quaternion quaternion = Quaternion.LookRotation(def_dir);
            transform.rotation = quaternion;

            Target_distance = true;
            transform.position = next_pos;
        }
    }

    public void IceObjSetActivOn()
    {
        ice_obj.SetActive(true);
    }

    public void IceObjSetActivOff()
    {
        ice_obj.SetActive(false);
    }

    public void UIFacing()
    {
        Hpbar_green.transform.forward = Vector3.back;
        Hpbar_red.transform.forward = Vector3.back;
        fire_image.transform.forward = Vector3.back;
    }

    public void FireEffectPlay()
    {
        foreach (var fire_effect in Fire_effect)
        {
            fire_effect.gameObject.SetActive(true);
            fire_effect.Play();
        }
    }

    bool MoveTime(float time)
    {
        if (!move_wait_flg)
        {
            movetime += Time.deltaTime;
        }

        if (movetime >= time)
        {
            move_wait_flg = true;
            movetime = 0;
        }

        return move_wait_flg;
    }

    public bool Camera_Target_Core_flg()
    {
        return camera_target_core_flg;
    }

    public bool Camera_Target_Bari_flg()
    {
        return camera_target_bari_flg;
    }

    void Camera_TargetInit()
    {
        camera_target_bari_flg = false;
        camera_target_core_flg = false;
    }
}
