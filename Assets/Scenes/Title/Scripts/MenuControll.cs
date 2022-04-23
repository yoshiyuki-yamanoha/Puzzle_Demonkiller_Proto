using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControll : MonoBehaviour
{

    //class ControllInfo
    //{
    //    float inputY;

    //}
    public enum UpDown
    {
        UP,
        DOWN,
        NO = 99
    }

    float selectCoolTime;
    const float SELECT_COOLTIME_MAX = 10.0f;// 選択カーソルが長押ししていても一瞬その場所で止まる最大時間

    private void Start()
    {
        selectCoolTime = SELECT_COOLTIME_MAX;
    }

    public int GetUpDown()
    {
        selectCoolTime--;// クールタイムを起動
        float buttonInputInfo = Input.GetAxis("UPDOWN");
        float stickInputInfo = Input.GetAxis("Vertical");

        if (buttonInputInfo == 0.0f && stickInputInfo == 0.0f)
        {
            selectCoolTime = 0;
        }

        if(selectCoolTime <= 0.0f)
        {

            selectCoolTime = 0;   // クールタイムの固定
            if(buttonInputInfo >= 0.8f || stickInputInfo >= 0.8f)
            {
                selectCoolTime = SELECT_COOLTIME_MAX;
                return ((int)UpDown.UP);
            }else if (buttonInputInfo <= -0.8f || stickInputInfo <= -0.8f)
            {
                selectCoolTime = SELECT_COOLTIME_MAX;
                return ((int)UpDown.DOWN);
            }

        }

        return ((int)UpDown.NO);
    }
}
