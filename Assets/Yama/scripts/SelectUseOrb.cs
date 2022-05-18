using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectUseOrb : TrunManager
{
    TrunManager s_turnMGR;
    OrbGage s_orbGage;
    [NonSerialized] public int[] orbLevel = new int[6];
    int nowSelOrb;

    int coolTimeMax = 10;
    int waitTime;

    //List<>
    public GameObject selecter;

    //オーブUIの情報 
    private GameObject pofm;
    private GameObject poim;
    private GameObject potm;
    private GameObject sofm;
    private GameObject soim;
    private GameObject sotm;
    private float defoY;//オーブUIの最初の高さ
    private float moveY;//オーブUIの選択時の高さ

    [SerializeField] MagicRangeDetector s_MagicRangeDetector;
    [SerializeField] MagicMassSelecter s_MagicMassSelecter;
    [SerializeField] MagicRangeDetector2 s_MagicRangeDetector2;
    [SerializeField] MagicMassSelecter2 s_MagicMassSelecter2;

    //オーブの切り替えを出来ないようにする
    bool notSwitchOrb = false;
    public bool osf { get => notSwitchOrb; set => notSwitchOrb = value; }


    int orbtype = 0, orblevel = 0;

    //魔法専用シーン
    [SerializeField] bool magicMagicMode;
    
    [SerializeField] MapMass s_MapMass;
    int defSelX = 9, defSelY = 10;//マスの座標用
    // Start is called before the first frame update
    void Start()
    {
        SelectOrb_Init();
        setOrbUI();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!notSwitchOrb || magicMagicMode) && s_orbGage.orbChangeflag == true)
        {
            SelectOrb_Update();
        }

        if (magicMagicMode || s_turnMGR.GetTrunPhase() == TrunManager.TrunPhase.MagicAttack && s_orbGage.OrbCheckExsistens())//Player攻撃ターンか？
        {
            //選択しているオーブの表示
            selectOrb();

            //魔法の範囲を変える
            s_MagicRangeDetector.ChangeMagicRange();
            

            //選択しているオーブのレベルが０ならずらす
            //selectmoveOrb();
        }
        else
        {//攻撃ターンじゃない時にオーブの位置を元に戻す
            selectResetOrb();
        }

        //敵が0体だった場合、設置系以外を使え無くする
        var enes = GameObject.FindGameObjectsWithTag("Enemy");
        if (s_turnMGR.GetTrunPhase() == TrunPhase.MagicAttack && enes.Length <= 0) {
            orbLevel[0] = 0;
            orbLevel[1] = 0;
            orbLevel[2] = 0;
            orbLevel[4] = 0;
            s_orbGage.OrbLevelZero(0);
            s_orbGage.OrbLevelZero(1);
            s_orbGage.OrbLevelZero(2);
            s_orbGage.OrbLevelZero(4);
            ChangeUseOrb(1);
        }
    }

    /// <summary>
    /// オーブ選択の初期化
    /// </summary>
    private void SelectOrb_Init()
    {
        s_turnMGR = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        s_orbGage = GameObject.Find("GameObject").GetComponent<OrbGage>();

        SetOrbType();

        //魔法専用シーンだけの初期化
        if (magicMagicMode) {
            for (int i = 0; i < orbLevel.Length; i++)
                orbLevel[i] = 1;
        }

    }

    /// <summary>
    /// オーブ選択の更新
    /// </summary>
    private void SelectOrb_Update()
    {
        //　オーブを選択可能か判断
        if(DecideCanChooseOrb() == true)
        {
            // 前へ移動
            if (Input.GetButtonDown("Cont_L1"))
            {
                int moveL = -1;
                ChangeUseOrb(moveL);
            }

            // 後ろへ移動
            if (Input.GetButtonDown("Cont_R1"))
            {
                int moveR = 1;
                ChangeUseOrb(moveR);
            }
        }
    }

    /// <summary>
    /// オーブ選択のクールタイムの経過
    /// </summary>
    /// <returns>クールタイムが残っていれば真を、残っていれば偽を返す</returns>
    private bool ElapsedOfCoolingTimeOfMovement()
    {
        // クールタイム経過
        if (waitTime > 0)
        {
            waitTime--;

            return false;
        }

        return true;
    }

    /// <summary>
    /// レベルが１以上のオーブを探す
    /// </summary>
    /// <param name="num">プラスの場合前に、マイナスの場合後ろに進む</param>
    public void ChangeUseOrb(int num)
    {

        int numLim = 0;

        // 現在選択しているオーブから一周する
        for (int oi = nowSelOrb + num; oi != nowSelOrb; oi+=num)
        {
            if (oi > 5) oi -= 6;
            if (oi < 0) oi += 6;

            //　オーブのレベルが１以上なら選択
            if (orbLevel[oi] >= 1)
            {
                nowSelOrb = oi;
                break;
            }

            if (++numLim > 6) break;
        }

        

        if (nowSelOrb != 2)
            s_MapMass.SetMagicMassSelector(defSelX, defSelY);//オーブが切り替わるときにカーソルを最初の位置へ移動

        // クールタイムの追加
        waitTime = coolTimeMax;

        //五芒星雷のときのみ敵選択モードに切り替え
        if (!magicMagicMode)
        {
            if (nowSelOrb == 2) s_MagicMassSelecter.SwitchSelectType(1);
            else s_MagicMassSelecter.SwitchSelectType(0);
        }
        else {
            if (nowSelOrb == 2) s_MagicMassSelecter.SwitchSelectType(1);
            else s_MagicMassSelecter.SwitchSelectType(0);
        }

        //魔法の範囲を変える
        //s_MagicRangeDetector.ChangeMagicRange();
    }

    /// <summary>
    /// オーブを選択しても良いか判断
    /// </summary>
    /// <returns>選択しても良い場合に真を返す</returns>
    private bool DecideCanChooseOrb()
    {
        // オーブを選択しても良いターンか判断
        if (!magicMagicMode)
        {
            TrunPhase currentPhase = s_turnMGR.GetTrunPhase();
            if (currentPhase != TrunPhase.MagicAttack)
                return false;
        }

        // クールタイムが残っているか判断
        if (ElapsedOfCoolingTimeOfMovement() == false)
            return false;

        //　オーブを選択してもヨシ！
        return true;
    }

    public (int, int) GetNowSelectOrb()
    {
        return (nowSelOrb, orbLevel[nowSelOrb]);
    }

    public void SetOrbType()
    {
        // オーブの情報を取得
        orbLevel = s_orbGage.Get_Orb_Level();

        for (int ty = 0; ty < orbLevel.Length;ty++)
        {
            if (orbLevel[ty] > 0)
            {
                nowSelOrb = ty;
                break;
            }
        }
    }
    private void setOrbUI()
    {
        pofm = GameObject.Find("PentagonOrbFireMask");
        poim = GameObject.Find("PentagonOrbIceMask");
        potm = GameObject.Find("PentagonOrbThunderMask");
        sofm = GameObject.Find("StarOrbFireMask");
        soim = GameObject.Find("StarOrbIceMask");
        sotm = GameObject.Find("StarOrbThunderMask");
        defoY = pofm.transform.position.y;
        moveY = defoY + 20f;
    }
    private void selectOrb()
    {
        if (nowSelOrb == 0)
        {
            sofm.transform.position = new Vector3(sofm.transform.position.x, moveY, sofm.transform.position.z);
        }
        else
        {
            sofm.transform.position = new Vector3(sofm.transform.position.x, defoY, sofm.transform.position.z);
        }
        if (nowSelOrb == 1)
        {
            soim.transform.position = new Vector3(soim.transform.position.x, moveY, soim.transform.position.z);
        }
        else
        {
            soim.transform.position = new Vector3(soim.transform.position.x, defoY, soim.transform.position.z);
        }
        if (nowSelOrb == 2)
        {
            sotm.transform.position = new Vector3(sotm.transform.position.x, moveY, sotm.transform.position.z);
        }
        else
        {
            sotm.transform.position = new Vector3(sotm.transform.position.x, defoY, sotm.transform.position.z);
        }
        if (nowSelOrb == 3)
        {
            pofm.transform.position = new Vector3(pofm.transform.position.x, moveY, pofm.transform.position.z);
        }
        else
        {
            pofm.transform.position = new Vector3(pofm.transform.position.x, defoY, pofm.transform.position.z);
        }
        if (nowSelOrb == 4)
        {
            poim.transform.position = new Vector3(poim.transform.position.x, moveY, poim.transform.position.z);
        }
        else
        {
            poim.transform.position = new Vector3(poim.transform.position.x, defoY, poim.transform.position.z);
        }
        if (nowSelOrb == 5)
        {
            potm.transform.position = new Vector3(potm.transform.position.x, moveY, potm.transform.position.z);
        }
        else
        {
            potm.transform.position = new Vector3(potm.transform.position.x, defoY, potm.transform.position.z);
        }
    }

    private void selectResetOrb()
    {
        sofm.transform.position = new Vector3(sofm.transform.position.x, defoY, sofm.transform.position.z);
        soim.transform.position = new Vector3(soim.transform.position.x, defoY, soim.transform.position.z);
        sotm.transform.position = new Vector3(sotm.transform.position.x, defoY, sotm.transform.position.z);
        pofm.transform.position = new Vector3(pofm.transform.position.x, defoY, pofm.transform.position.z);
        poim.transform.position = new Vector3(poim.transform.position.x, defoY, poim.transform.position.z);
        potm.transform.position = new Vector3(potm.transform.position.x, defoY, potm.transform.position.z);
    }
    void selectmoveOrb()//選択しているオーブがからの場合横に動かし
    {
        (orbtype, orblevel) = GetNowSelectOrb();
        if(orblevel == 0)
        {
            int moveR = 1;
            ChangeUseOrb(moveR);
        }

    }
}
