using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class PhaseManager : MonoBehaviour
{
    /*イベントトリガー*/
    [SerializeField,Tooltip("ウェーブ開始時に呼ばれる関数たち")]
    private StageGame startFunctions = new StageGame();
    [SerializeField,Tooltip("ウェーブ終了時に呼ばれる関数たち")]
    private EndGame finichFunctions = new EndGame();

    /*ウェーブ数関連*/
    int currentWaveNum = 0;     //現在のステージの現在のウェーブ
    int maxWaveNum = 99;         //現在のステージのウェーブ数
    [SerializeField] Text waveNumText;           //ウェーブのカウントを表示するUI

    /*タイムリミット関連 (パズル用)*/
    float nowTime = 0;          //現在のパズルの経過時間
    [SerializeField] float maxTime = 60;         //現在のパズルの時間制限
    [SerializeField] Text timeText;              //時間を表示するUI

    /*敵の数関連 (ディフェンス用)*/
    int killedEnemyNum = 0;     //倒した敵の数
    int maxEnemyNum = 0;        //現ウェーブの敵の数
    [SerializeField] Text enemyText;             //敵の数を表示するUI

    /*フェイズ管理関連*/
    enum Phase {
        Puzzle,
        Defence }
    Phase phase;
    [SerializeField] Text phaseNameText;        //現在のフェーズを見るよう
    [SerializeField] GameObject puzzles;        //パズル用

    /*ゲームフラグ関連*/
    bool isGame = false;        //ウェーブ開始フラグ
    bool isPause = false;       //一時停止フラグ

    //操作出来ないようにするためのやつ
    [SerializeField] PlayerController pc;

    //時間が立ったら自動で切り替わるかのフラグ
    [SerializeField, Tooltip("時間が立ったら自動でフェイズが切り替わるか")]
    private bool changePhaseAuto = true;

    void Start()
    {
        GameInit();             //ゲーム初期化
        WaveInit();             //ウェーブ情報初期化
    }

    void FixedUpdate()
    {
        if (changePhaseAuto) {

            switch (phase)
            {
                case Phase.Puzzle:

                    //時間を減らす
                    nowTime -= Time.deltaTime;

                    //現ウェーブの時間が0になったら
                    if (nowTime < 0)
                    {
                        startFunctions.Invoke();
                        nowTime = 0;                //0で初期化
                        ChangePhase(1);             //パズルフェイズに移行
                    }

                    //テキストUIに表示させる
                    timeText.text = "残り" + nowTime.ToString("00") + "秒";

                    break;

                case Phase.Defence:
                    //スタートボタンでウェーブ開始
                    if (!isGame && Input.GetButtonDown("Start")) isGame = true;

                    //ウェーブを開始しているかつポーズ中でない
                    if (isGame && !isPause)
                    {
                        int currentEnemyNum = maxEnemyNum - killedEnemyNum;

                        //現ウェーブの敵を全員倒したら
                        if (currentEnemyNum <= 0)
                        {
                            finichFunctions.Invoke();   //イベントを呼び出す
                                                        //WaveInit();                 //ウェーブ初期化
                            ChangePhase(0);             //パズルフェイズに移行
                        }

                        //テキストUIに表示させる
                        enemyText.text = killedEnemyNum.ToString() + "/" + maxEnemyNum.ToString();
                    }
                    break;

            }
        }

        ////Bボタンでフェーズ切替え
        //if (Input.GetButtonDown("Fire2")) {
        //    phase = 1 - phase;
        //    if (phase == Phase.Puzzle)
        //    {
        //        puzzles.SetActive(true);
        //        pc.enabled = false;
        //    }
        //    else
        //    {
        //        puzzles.SetActive(false);
        //        pc.enabled = true;
        //    }
        //}

        //現在のフェーズを出す
        phaseNameText.text = phase.ToString();
    }

    //ゲーム初期化関数
    void GameInit() {
        currentWaveNum = 0;     //ウェーブ数の初期化
        phase = Phase.Defence;  //最初のフェーズをパズルに
        puzzles.SetActive(false);
    }

    //ウェーブ初期化関数
    void WaveInit() {
        nowTime = maxTime;  //時間制限の初期化
        currentWaveNum++;   //ウェーブ数を1進める
        isGame = false;     //ウェーブ開始フラグ
        waveNumText.text = "Wave:" + currentWaveNum.ToString() + "/" + maxWaveNum.ToString();
    }

    //現在のフェイズを取得
    public int GetCurrentPhase() {

        return (int)phase;
    }

    //フェイズを変更できる関数 (0：パズルふぇいず 1:魔法フェイズ other：自動切換え)
    public void ChangePhase(int num = 99) {
        if (num == 0) phase = Phase.Puzzle;
        else if (num == 1) phase = Phase.Defence;
        else phase = 1 - phase;
    }

    //ウェーブ開始時に呼び出す関数用イベント
    [Serializable]
    public class StageGame : UnityEvent
    {
    }

    //ウェーブ終了時に呼び出す関数用イベント
    [Serializable]
    public class EndGame : UnityEvent
    {
    }
}
