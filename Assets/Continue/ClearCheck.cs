using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ClearCheck : TrunManager
{
    [SerializeField] private Transform[] play;  //親オブジェクト


    [SerializeField] GameObject clearEffe;
    [SerializeField] Transform effePos;

    [SerializeField] private AudioClip se;
    [SerializeField] private AudioSource ass;

    [SerializeField] GameObject[] playObjs;     //子オブジェクト

    [SerializeField] int[] soe = new int[5] { 2, 3, 0, 4, 1 };

    //魔力 ←あとで増やす
    public float magicPoint;
    [SerializeField] Text mpText;

    //クリア判定フラグ
    bool cleared;

    bool attack;
    public int MaxCombo;
    //public int enemyno;
    //public GameObject MarkPoint1 = GameObject.Find("MarkingPointer1");
    //public GameObject MarkPoint2 = GameObject.Find("MarkingPointer2");
    //public GameObject MarkPoint3 = GameObject.Find("MarkingPointer3");
    //public GameObject[] enemy;
    //シャッフルカウント
    float shuffleInterval = 30;
    float shuffleCount;

    //プレイヤーコントローラー
    [SerializeField] PlayerController pc;
    Magichoming Mh;

    //ばりえーしょん
    //Attackvariation AttackV;
    //ポイントコントロール
    PointControl ppp;

    //敵にマークつけるよう
    MagicPointer mp;


    //ゲージ
    [SerializeField] Slider sld;
    [SerializeField] Text comboTex;

    //コンボタイム用へ3ん数
    float comboTime = 600;
    float nowComboTime = 0;
    float testVarTime = 0;
    public int comboNum = 0;
    [NonSerialized] public int addComboNum = 1;

    //見えなくする用
    [SerializeField] GameObject puzzle;
    [SerializeField] GameObject gauge;
    [SerializeField] GameObject bgCircle;
    [SerializeField] GameObject pointer;
    int fadeTime = 180;
    [SerializeField] int fadeNowTime = 0;
    [SerializeField] PointControl s_PointControl;

    //オーブぐるぐるアニメーション用
    [SerializeField] private Animator orbAnimator;
    [SerializeField] private Animator puzzleAnimator;

    bool magicCircleAnimOn = false;

    // Puzzleターンが終了する時に使用する変数
    PuzzleTurnEndAnim puzzleTurnEndAnim;

    [SerializeField] TrunManager trunMgr;

    [SerializeField] private SEManager sePlay;

    //既にレベルMaxになっているオーブか検証用
    [SerializeField] OrbGage s_OrbGage;
    public bool changeColorLine = false;

    //パズルふぇいずに戻ってきたときに一回だけシャッフルする
    bool isShuffle = true;

    //パズル
    [SerializeField] bool puzzleOnlyMode;


    //OrbGage oGage;//オーブのゲージ

    private void Start()
    {
        attack = false;
        DrawLine();
        if (!puzzleOnlyMode)
        {
            pc = GameObject.Find("GameObject").GetComponent<PlayerController>();
            mp = GameObject.Find("Main Camera").GetComponent<MagicPointer>();
            trunMgr = GameObject.Find("TrunManager").GetComponent<TrunManager>();
            puzzleTurnEndAnim = this.GetComponent<PuzzleTurnEndAnim>();
            sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
            gauge.SetActive(false);
        }
        ppp = GameObject.Find("Pointer").GetComponent<PointControl>();




        //線の色を付ける
        foreach (GameObject o in playObjs)
        {
            o.GetComponent<GoToParent>().LineSetColor();
            o.GetComponent<GoToParent>().LineSetWidth();
        }

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (/*puzzleOnlyMode || */trunMgr.trunphase == TrunPhase.Puzzle)
        {
            //1回だけシャッフル
            if (isShuffle) { Shuffle(); isShuffle = false;  }
            if (magicCircleAnimOn == true) { puzzleAnimator.SetTrigger("Go1"); magicCircleAnimOn = false; }
            else { puzzleAnimator.SetTrigger(""); }
            if (!cleared)
            {
                //線が被らなければクリア (グルっと一周)↓２つのif文でチェック
                if (CheckClear(1))
                {
                    ClearReward((int)PointControl.MAGIC_MODE.PENTAGON);
                }
                else if (CheckClear(-1))
                {
                    ClearReward((int)PointControl.MAGIC_MODE.PENTAGON);
                }
                //星の形になってればクリア(五芒星)↓２つのif文でチェック
                else if (CheckClear(2))
                {
                    ClearReward((int)PointControl.MAGIC_MODE.STAR);
                }
                else if (CheckClear(-2))
                {
                    ClearReward((int)PointControl.MAGIC_MODE.STAR);
                }

                if (changeColorLine == true)
                {
                    //線の色を付ける
                    foreach (GameObject o in playObjs)
                        o.GetComponent<GoToParent>().LineSetColor();
                    changeColorLine = false;

                }

                //全ての線が後ろの線に重なってればクリア(理想かも)
            }

            //シャッフル
            if (cleared)
            {
                if (shuffleCount > 1) shuffleCount--;
                if (shuffleCount == 1)
                {

                    shuffleCount = 0;
                    Shuffle();

                    //線の色を戻す
                }
            }

            

            if (!puzzleOnlyMode)
            {
                //コンボタイムを減らしていく
                if (nowComboTime != 0)
                {
                    nowComboTime--;

                    //コンボタイムが0になったらコンボ数を0に
                    if (nowComboTime <= 0)
                    {
                        puzzleAnimator.SetTrigger("Go2");
                        //AttackV.attackvar();
                        //bgCircle.SetActive(false);
                        MaxCombo = comboNum;
                        comboNum = 0;
                        nowComboTime = 0;
                        //オーブリセット
                        ppp.ResetOrbs();

                        //AttackV.attackvar_erase();
                        nowComboTime = 0;
                    }
                }

                //コンボタイムが切れた時
                if (attack == true && nowComboTime == 0)
                {
                    //初期化
                    ppp.SelectInit();

                    //パズルを消す
                    gauge.SetActive(false);
                    pointer.SetActive(false);

                    attack = false;
                    pc.attackNum = 0;
                    puzzleTurnEndAnim.SetPuzzleTurnEndFlg(true);
                }

                if (trunMgr.GetTrunPhase() == TrunPhase.Puzzle && !puzzleTurnEndAnim.GetPuzzleTurnEndFlg())
                {
                    pointer.SetActive(true);
                    //puzzle.SetActive(true);
                    gauge.SetActive(true);
                    //bgCircle.SetActive(true);
                    //s_PointControl.enabled = true;
                }

                //ゲージに反映
                float per = nowComboTime / comboTime;
                sld.value = per;
                comboTex.text = "コンボ：" + comboNum.ToString();
            }


            

            //魔法打ってる間魔法陣が消える処理
            if (fadeNowTime != 0)
            {
                fadeNowTime--;
                if (fadeNowTime <= 0)
                {
                    fadeNowTime = 0;
                }
            }

        }
        else {
            if(!puzzleOnlyMode) isShuffle = true;
            puzzleAnimator.SetTrigger("");
            magicCircleAnimOn = true;
        }
    }

    public void SkipComboTime() {
        if(trunMgr.GetTrunPhase() == TrunPhase.Puzzle)
            nowComboTime = 1;
    }


    public void Shuffle() {

        //チェック通過フラグ
        bool isPass = true;

        do
        {

            //魔法陣の数
            int n = playObjs.Length;

            //フラグを倒す
            isPass = true;

            while (n > 1)
            {

                n--;

                int k = UnityEngine.Random.Range(0, n + 1);

                int te = soe[k];
                soe[k] = soe[n];
                soe[n] = te;
            }

            //線を更新
            DrawLine();

            //最初から揃ってたらやり直し
            for (int i = -2; i < 3; i++)
            {
                if (i != 0)
                {
                    if (CheckClear(i))
                    {
                        isPass = false;
                        break;
                    }
                }
            }


        } while (!isPass);

        //クリアフラグを倒す
        cleared = false;

        //線の色を白にする
        foreach (GameObject o in playObjs)
            o.GetComponent<GoToParent>().LineColorWhite();

        //魔方陣の色をシャッフル
        //ppp.RandomColorSet();

    }

    //線を引かせる
    //今の処理だとシャッフルして自分と同じ添え字が来たら消える。
    void DrawLine() {
        for (int i = 0; i < playObjs.Length; i++)
        {
            //int here = soe[i];
            int next = 0;
            if (i != playObjs.Length - 1) next = i + 1;

            playObjs[soe[i]].GetComponent<GoToParent>().SetLine(playObjs[soe[next]]);
        }
    }

    bool CheckClear(int nextNum)
    {
        if (!puzzleOnlyMode)
        {

            //既に揃っているオーブかどうか確認
            var levels = s_OrbGage.Get_Orb_Level();

            //現在の魔法陣の色
            string na = GameObject.FindGameObjectWithTag("My").name;

            //色判別
            int cn = 0;
            if (na == "S") cn = 1;
            if (na == "Y") cn = 2;


            //五角形
            if (Math.Abs(nextNum) == 1)
            {
                if (levels[3 + cn] == 30) return false;
            }
            //五芒星
            if (Math.Abs(nextNum) == 2)
            {
                if (levels[cn] == 30) return false;
            }

        }
        
        for (int i = 0; i < play.Length; i++)
        {
            int maxNum = play.Length - 1;
            int next = i + nextNum;
            if (next > maxNum) next -= play.Length;
            if (next < 0) next += play.Length;

            GameObject a = null, b = null;
            for (int c = 0; c < play[i].childCount; c++)
            {
                a = play[i].GetChild(c).gameObject;
                GoToParent gp = a.GetComponent<GoToParent>();
                if (gp)
                {
                    a = gp.GetLineEnd();
                    break;
                }
            }
            for (int c = 0; c < play[next].childCount; c++)
            {
                b = play[next].GetChild(c).gameObject;
                if (b.tag == "My") break;
            }

            if (a != b) return false;

        }

        return true;

    }

    void ClearReward(int type) {
        //AddMagicPoint(point);
        ShowEffeLingSound();

        //今bのタイム更新
        if(nowComboTime == 0)
            nowComboTime = comboTime;

        //コンボタイムが残ってたらコンボを増やす
        if (nowComboTime > 0)
        {
            //comboNum+=addComboNum;
        }

        shuffleCount = shuffleInterval;

        //pc.PlayerAttack();

        //mp.Marking();

        cleared = true;
        attack = true;

        //線の色を付ける
        foreach (GameObject o in playObjs)
            o.GetComponent<GoToParent>().LineSetColor();

        //色統一ならオーブを光らせる
        ppp.CheckOneColor(type);

        //魔法陣の色を減らす
        ppp.BreakColor();

        ppp.MagicText();

    }

    //魔力を増やす
    void AddMagicPoint(int num) {
        magicPoint += num;
        mpText.text = "魔力: "+ magicPoint.ToString("0");
    }

    void ShowEffeLingSound() {
        s_PointControl.enabled = false;
        Destroy(Instantiate(clearEffe, effePos),1.0f);
        Invoke("ChangePointerMode", 0.5f);

        if(!puzzleOnlyMode) sePlay.Play("matchSE");//SEを鳴らす（魔方陣の位置が入れ替わる）
        //ass.PlayOneShot(se);
    }
    
    //ポイントコントロールのアクティブ/非アクティブ化
    void ChangePointerMode() {

        s_PointControl.enabled = true;
    }

    public void SubMP() {

        magicPoint -= 3;
        if(magicPoint < 0)
        {
            magicPoint = 0;
        }
        mpText.text = "魔力: " + magicPoint.ToString("0");
    }

}
