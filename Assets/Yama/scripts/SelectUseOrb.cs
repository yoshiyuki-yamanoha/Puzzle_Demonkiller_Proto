using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUseOrb : TrunManager
{
    TrunManager s_turnMGR;
    OrbGage s_orbGage;
    int[] orbLevel = new int[6];
    int nowSelOrb;

    int coolTimeMax = 10;
    int waitTime;

    //List<>
    public GameObject selecter;

    // Start is called before the first frame update
    void Start()
    {
        SelectOrb_Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SelectOrb_Update();
    }

    /// <summary>
    /// オーブ選択の初期化
    /// </summary>
    private void SelectOrb_Init()
    {
        s_turnMGR = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        s_orbGage = GameObject.Find("GameObject").GetComponent<OrbGage>();
        // オーブの情報を取得
        orbLevel = s_orbGage.Get_Orb_Level();

        foreach (int lv in orbLevel)
        {
            if (lv > 0)
            {
                nowSelOrb = lv;
                break;
            }
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
                Debug.Log("LB");
                int moveL = -1;
                ChangeUseOrb(moveL);
            }

            // 後ろへ移動
            if (Input.GetButtonDown("Cont_R1"))
            {
                Debug.Log("RB");
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
    private void ChangeUseOrb(int num)
    {
        // 現在選択しているオーブから一周する
        for (int oi = nowSelOrb + num; oi == nowSelOrb; oi+=num)
        {
            //　オーブのレベルが１以上なら選択
            if (orbLevel[oi] >= 1)
            {
                nowSelOrb = oi;
                break;
            }
        }

        // クールタイムの追加
        waitTime = coolTimeMax;
    }

    /// <summary>
    /// オーブを選択しても良いか判断
    /// </summary>
    /// <returns>選択しても良い場合に真を返す</returns>
    private bool DecideCanChooseOrb()
    {
        // オーブを選択しても良いターンか判断
        TrunPhase currentPhase = s_turnMGR.GetTrunPhase();
        if (currentPhase != TrunPhase.MagicAttack)
            return false;

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
}
