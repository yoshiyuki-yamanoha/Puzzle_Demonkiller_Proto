using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrunManager : MonoBehaviour
{
    public enum TrunPhase
    {
        Player,//プレイヤー
        Puzzle,//パズル
        Enemy,//敵
        MagicAttack,//魔法
    }

    ////アクティブ系
    //bool is_playertrun = false;
    //bool is_enemytrun = false;
    //bool is_magicattacktrun = false;
    //bool is_puzzletrun = false;
    //int truncount = 0;//turnカウント
    //現在のターンを管理する変数

    public TrunPhase trunphase = TrunPhase.Player;

    //GETする関数


    //時間関係
    //bool initflg = true;//一回のみ実行Flg
    //[SerializeField] bool timer_start_flg = false;//時間計測
    //[SerializeField] const float trun_countdown_time = 7;//ターン時間
    //[SerializeField] const float puzzule_timer = 7;//パズル時間
    //float time = 0;//時間計測用

    ////プロパティ
    //public TrunPhase Trunphase { get => trunphase; set => trunphase = value; }
    //public int Truncount { get => truncount; set => truncount = value; }
    //public bool Is_playertrun { get => is_playertrun; set => is_playertrun = value; }
    //public bool Is_enemytrun { get => is_enemytrun; set => is_enemytrun = value; }
    //public bool Is_magicattacktrun { get => is_magicattacktrun; set => is_magicattacktrun = value; }
    //public bool Is_puzzletrun { get => is_puzzletrun; set => is_puzzletrun = value; }

    // Update is called once per frame
    void FixedUpdate()
    {
        //switch (trunphase)
        //{
        //    case TrunPhase.Player: //プレイヤーのターン
        //        PlayerTrun();
        //        break;
        //    case TrunPhase.Puzzle://パズルターン
        //        PuzzleTrun();
        //        break;
        //    case TrunPhase.Enemy: //敵のターン
        //        EnemyTrun();
        //        break;
        //    case TrunPhase.MagicAttack: //魔法攻撃のターン
        //        MagicAttackTrun();
        //        break;
        //}
    }

    public TrunPhase GetTrunPhase()
    {
        return trunphase;
    }

    public void SetTrunPhase(TrunPhase trunphase)
    {
        this.trunphase = trunphase;
    }

    //void PuzzleTrun() //パズルターン
    //{
    //    if (timer_start_flg) {//計測開始がOnなら
    //        if (initflg) { time = puzzule_timer; initflg = false; }//一回のみ実行Off
    //        time -= Time.deltaTime;//カウントダウン開始
    //        is_puzzletrun = true;//パズルターン開始
    //    }

    //    if(time < 0)
    //    {
    //        time = 0;
    //        is_puzzletrun = false;//パズルターンオフ
    //        timer_start_flg = false;//計測Off
    //        initflg = true;//一回のみ実行フラグON
    //        trunphase = TrunPhase.MagicAttack; //魔法フェーズに移動。
    //    }
    //}

    //void TrunChange()//ターン切り替え
    //{

    //}

    //void TrunStart()//ターンがstarする時
    //{

    //}

    //void TrunEnd()//ターン終了
    //{

    //}

    //public void PlayerTrun()
    //{
    //}

    //public void EnemyTrun()
    //{
    //    if (is_enemytrun)//移動したのか? //攻撃終わったのか
    //    {
    //        is_enemytrun = true;
    //    }
    //    else
    //    {
    //        is_enemytrun = false;
    //        trunphase = TrunPhase.Puzzle;
    //    }
    //    //敵のターン
    //    //移動したのか？
    //    //コアに近いのか
    //    //攻撃//攻撃が終わったのか
    //    //ターン終了
    //    //敵のターン終了したらパズルターン
    //}

    //public void MagicAttackTrun()
    //{
    //    //trunphase = TrunPhase.;//
    //}
}
