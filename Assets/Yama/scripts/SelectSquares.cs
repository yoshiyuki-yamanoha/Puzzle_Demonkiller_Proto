using System.Collections.Generic;
using UnityEngine;

public class SelectSquares : TrunManager
{
    /*[SerializeField]*/
    GameObject selector = null;
    int selMovAmtH;
    int selMovAmtV;

    GameObject Pmass = null;
    List<GameObject> massList = new List<GameObject>();
    int cCount;
    int gcCount;
    int nowMassH;
    int nowMassV;

    /*[SerializeField]*/
    int coolTimeMax = 10;
    private int waitTime;
    private bool canSelect;

    //魔法を撃つ処理用のスクリプト
    [SerializeField] PlayerController s_PlayerController;

    TrunManager turnMGR;
    SelectUseOrb selUseOrb;
    OrbGage orbGage;

    // Start is called before the first frame update
    void Start()
    {
        SelectorInit();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TrunPhase currentPhase = turnMGR.GetTrunPhase();

        if (currentPhase == TrunPhase.MagicAttack)
        {
            FlowToMoveTheSelector();

            //ActivateMagic();    //魔法を撃つ処理
        }
    }

    public void SelectorInit()
    {
        selector = GameObject.Find("Selector").gameObject;
        selector.transform.position = Vector3.zero;

        Pmass = GameObject.Find("MassStage").gameObject;
        GetMassList();

        turnMGR = GameObject.Find("TrunManager").gameObject.GetComponent<TrunManager>();
        GameObject gameObj = GameObject.Find("GameObject").gameObject;
        selUseOrb = gameObj.GetComponent<SelectUseOrb>();
        orbGage = gameObj.GetComponent<OrbGage>();

        selMovAmtH = 0;
        selMovAmtV = 0;
        waitTime = 0;
    }

    // セレクターを動かす流れ
    public void FlowToMoveTheSelector()
    {
        selMovAmtH = 0;
        selMovAmtV = 0;

        // 各スティックの値を取得
        (float hStick, float vStick) = GetStick();

        // 移動量の取得
        var (h, v) = ReturnSticAsInt(hStick, vStick);
        selMovAmtH = h;
        selMovAmtV = v;

        var (type, lv) = selUseOrb.GetNowSelectOrb();
        int[] massRange = orbGage.GetMagicRanges();

        MoveToStage(type, massRange[type]);

        if (CheckIfSelectorCanMove(type, massRange[type]) == true)
        {
            // セレクターの移動
            ChangePositionSelector();
        }
    }

    // 各スティックの値を取得
    private (float, float) GetStick()
    {
        float hStick = Input.GetAxis("Horizontal");
        float vStick = Input.GetAxis("Vertical");
        //float hStick = Input.GetAxis("Vertical");//横表示テスト用
        //float vStick = Input.GetAxis("Horizontal");//横表示テスト用

        return (hStick, vStick);
    }


    // スティックをint型で返す
    private (int, int) ReturnSticAsInt(float hStick, float vStick)
    {
        int hMoveAmount = Mathf.CeilToInt(hStick);
        int vMoveAmount = Mathf.CeilToInt(vStick);

        return (hMoveAmount, vMoveAmount);
    }

    // セレクターの移動
    private void ChangePositionSelector()
    {

        waitTime = coolTimeMax;

        float vMoveAmount = (float)nowMassV * 5.0f;
        selector.transform.position = new Vector3(massList[nowMassH].transform.position.x,
                                                  massList[nowMassH].transform.position.y,
                                                  massList[nowMassH].transform.position.z + vMoveAmount);

    }

    /// <summary>
    /// 各状態のチェック
    /// </summary>
    /// <returns>すべての条件で真になれば真を返し、どれか一つでも偽であれば偽を返す</returns>
    private bool CheckIfSelectorCanMove(int type, int massRange)
    {
        // クールタイムが残っているか
        bool check = ElapsedOfCoolingTimeOfMovement();
        if (check == false)
            return false;

        if (selMovAmtH == 0 && selMovAmtV == 0)
            return false;

        // nowMassH + selMovAmtHが子要素の数を超えていなければヨシ！
        if (nowMassH + selMovAmtH + (massRange / 2) >= cCount)
            return false;
        if (nowMassH + selMovAmtH - (massRange / 2) < 0)
            return false;

        if (type > 0) massRange = 1;

        // nowMassVが子要素と親の数を超えていなければヨシ！
        if (nowMassV + selMovAmtV + (massRange / 2) > 0)
            return false;
        if (nowMassV + selMovAmtV - (massRange / 2) < -gcCount)
            return false;

        nowMassH += selMovAmtH;
        nowMassV += selMovAmtV;

        // 動かしてヨシ！
        return true;
    }

    // 移動のクールタイムの経過
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

    // セレクターのポジションを返す
    public Vector3 GetSelectorPos()
    {
        return selector.transform.position;
    }

    //魔法を撃つ処理
    void ActivateMagic()
    {

        //Aボタンで魔法を放つ
        if (Input.GetButtonDown("Fire1"))
        {
            var (type, lv) = selUseOrb.GetNowSelectOrb();
            s_PlayerController.ShotMagic(selector, type, lv);
        }
    }

    private void GetMassList()
    {
        const int P = 1;
        cCount = 0;
        gcCount = 0;

        for (int i = 0; i < Pmass.transform.childCount; i++)
        {
            if (Pmass.transform.GetChild(i).gameObject.activeSelf)
            {
                massList.Add(Pmass.transform.GetChild(i).gameObject);
                cCount++;
            }
        }

        for (int i = 0; i < massList[0].transform.childCount; i++)
        {
            if (massList[0].transform.GetChild(i).gameObject.activeSelf)
            {
                gcCount++;
            }
        }

        nowMassH = Pmass.transform.childCount / 2;
        nowMassV = -(gcCount + P) / 2;


        float vMoveAmount = (float)nowMassV * 5.0f;
        selector.transform.position = new Vector3(massList[nowMassH].transform.position.x,
                                                      massList[nowMassH].transform.position.y,
                                                      massList[nowMassH].transform.position.z + vMoveAmount);
    }

    private void MoveToStage(int type, int massRange)
    {

        // nowMassH + 横幅が子要素の数を超えていなければヨシ！
        while (nowMassH + (massRange / 2) >= cCount)
        {
            nowMassH--;
        }

        while (nowMassH - (massRange / 2) < 0)
        {
            nowMassH++;
        }

        if (type > 0) massRange = 1;

        // nowMassVが子要素と親の数を超えていなければヨシ！
        while (nowMassV + (massRange / 2) > 0)
        {
            nowMassV--;
        }

        while (nowMassV - (massRange / 2) < -gcCount)
        {
            nowMassV++;
        }
    }
}
