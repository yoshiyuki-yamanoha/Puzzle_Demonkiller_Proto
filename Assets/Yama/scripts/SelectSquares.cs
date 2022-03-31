using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSquares : MonoBehaviour
{
    /*[SerializeField]*/ GameObject selector = null;
    float selMovAmtH;
    float selMovAmtV;
    /*[SerializeField]*/ int coolTimeMax = 10;
    private int waitTime;

    // Start is called before the first frame update
    void Start()
    {
        selector = GameObject.Find("Selector").gameObject;
        Debug.Log(selector);
        selector.transform.position = Vector3.zero;

        selMovAmtH = 0f;
        selMovAmtV = 0f;
        waitTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 各スティックの値を取得
        (float hStick, float vStick) = GetStick();

        // 移動量の取得
        var(h, v) = ReturnSelectorMoveAmount(hStick, vStick);
        selMovAmtH = h;
        selMovAmtV = v;

        // セレクターの移動
        ChangePositionSelector();
    }

    // 各スティックの値を取得
    private (float, float) GetStick()
    {
        float hStick = Input.GetAxis("Horizontal");
        float vStick = Input.GetAxis("Vertical");
        return (hStick, vStick);
    }


    // 移動量の作成、返し
    private (float, float) ReturnSelectorMoveAmount(float hStick, float vStick)
    {
        float hMoveAmount = (float)Mathf.CeilToInt(hStick) * 5.0f;
        float vMoveAmount = (float)Mathf.CeilToInt(vStick) * 5.0f;

        return (hMoveAmount, vMoveAmount);
    }

    // セレクターの移動
    private void ChangePositionSelector()
    {
        if (CheckIfSelectorCanMove() == true)
        {
            waitTime = coolTimeMax;
            selector.transform.position += new Vector3(selMovAmtH, 0f, selMovAmtV);
        }
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

    /// <summary>
    /// 各状態のチェック
    /// </summary>
    /// <returns>すべての条件に引っかからなければ真を返す</returns>
    private bool CheckIfSelectorCanMove()
    {
        // クールタイムが残っているか
        bool check = ElapsedOfCoolingTimeOfMovement();
        if (check == false)
            return false;

        if (selMovAmtH == 0 && selMovAmtV == 0)
            return false;

        if (selector.transform.position.x + selMovAmtH < -5.0f)
        {
            Debug.Log("11");
            return false;
        }
        if (selector.transform.position.x + selMovAmtH > 5.0f)
        {
            Debug.Log("12");
            return false;
        }
        if (selector.transform.position.z + selMovAmtV < -5.0f)
        {
            Debug.Log("13");
            return false;
        }
        if (selector.transform.position.z + selMovAmtV > 25.0f)
        {
            Debug.Log("14");
            return false;
        }

        // 動かしてヨシ！
        return true;
    }

    // セレクターのポジションを返す
    public Vector3 GetSelectorPos()
    {
        return selector.transform.position;
    }
}
