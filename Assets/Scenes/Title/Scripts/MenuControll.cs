using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    enum Select
    {
        Start,
        Quit
    }

    float selectCoolTime;
    const float SELECT_COOLTIME_MAX = 10.0f;// 選択カーソルが長押ししていても一瞬その場所で止まる最大時間

    Vector3[] selectMenuPos = new Vector3[2];
    [SerializeField] Transform cursor;
    [SerializeField] Sprite[] startImg = new Sprite[2];    //　スタート文字の画像
    [SerializeField] Sprite[] quitImg = new Sprite[2];     //　Quit文字の画像

    [SerializeField] Image ui_Start;    // Start表示画像の情報
    [SerializeField] Image ui_Quit;     // Quit表示画像の情報
    [SerializeField] Image ui_Line;     // Line表示画像の情報
    [SerializeField] private Select selecter;
    private void Start()
    {
        selectCoolTime = SELECT_COOLTIME_MAX;
        selecter = Select.Start;
        selectCoolTime = 0.0f;
        selectMenuPos[0] = new Vector3(370.55f, -87.0f, 0.0f);
        selectMenuPos[1] = new Vector3(370.55f, -225.0f, 0.0f);
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

    void ResultMenuControll()
    {
        int inputnum = GetUpDown();
        if (inputnum == ((int)MenuControll.UpDown.DOWN))
        {
            selecter = Select.Quit;
            ui_Start.sprite = startImg[0];
            ui_Quit.sprite = quitImg[1];
        }
        if (inputnum == ((int)MenuControll.UpDown.UP))
        {
            selecter = Select.Start;
            ui_Start.sprite = startImg[1];
            ui_Quit.sprite = quitImg[0];

        }

        switch (selecter)
        {
            case Select.Start: cursor.localPosition = selectMenuPos[0]; break;
            case Select.Quit: cursor.localPosition = selectMenuPos[1]; break;
        }

        // 選択しているStageによって飛ばすシーンを変える
        if (Input.GetButtonDown("Fire1"))
        {


            switch (selecter)
            {
                case Select.Start: GameMgr.Instance.GotoBuildScene(); break;
                case Select.Quit: GameMgr.Instance.GotoTitleScene(); break;
            }
        }
    }
}
