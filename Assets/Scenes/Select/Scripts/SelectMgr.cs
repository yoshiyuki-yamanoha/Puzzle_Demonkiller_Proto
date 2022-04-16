using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMgr : MonoBehaviour
{
    enum Select
    {
        Stage1,
        Stage2,
        Stage3
    }

    [SerializeField] private Select selecter;    // 現在なにを選択しているか
    float selectCoolTime;       // 選択カーソルが長押ししていても一瞬その場所で止まる時間
    const float SELECT_COOLTIME_MAX = 10.0f;// 選択カーソルが長押ししていても一瞬その場所で止まる最大時間
    Vector3[] selectMenuPos = new Vector3[3];
    [SerializeField] Transform cursor;

    MenuControll menuControll;
    // Start is called before the first frame update
    void Start()
    {
        selecter = Select.Stage1;
        selectCoolTime = 0.0f;
        selectMenuPos[0] = new Vector3(139.0f, 39.19f, 0.0f);
        selectMenuPos[1] = new Vector3(139.0f, -5.76f, 0.0f);
        selectMenuPos[2] = new Vector3(139.0f, -54.0f, 0.0f);
        this.gameObject.AddComponent<MenuControll>();
        menuControll = this.GetComponent<MenuControll>();
    }

    // Update is called once per frame
    private void Update() {

        int inputInfo = menuControll.GetUpDown();
        if(inputInfo == ((int)MenuControll.UpDown.UP))
        {
            if (selecter != Select.Stage1)
            {
                selecter--;
            }
            else
            {
                selecter = Select.Stage3;
            }
        }
        else if(inputInfo == ((int)MenuControll.UpDown.DOWN))
        {
            if (selecter != Select.Stage3)
            {
                selecter++;
            }
            else
            {
                selecter = Select.Stage1;
            }
        }
        //selectCoolTime--;// クールタイムを起動
        //if(Input.GetAxis("UPDOWN") <= 0.9f && Input.GetAxis("UPDOWN") >= 0.0f) {
        //    selectCoolTime = 0;
        //}
        //if(selectCoolTime <= 0.0) {
        //    float inputInfo = Input.GetAxis("UPDOWN");
        //    selectCoolTime = SELECT_COOLTIME_MAX;   // クールタイムの初期化
        //    if(inputInfo > 0.0f ) {
        //        if (selecter != Select.Stage1) {
        //            selecter--;
        //        }else {
        //            selecter = Select.Stage3;
        //        }
        //    }else if(inputInfo < 0.0f){
        //        if(selecter != Select.Stage3) {
        //            selecter++;
        //        }else {
        //            selecter = Select.Stage1;
        //        }
        //    } 
        //}

        switch (selecter) {
            case Select.Stage1: cursor.localPosition = selectMenuPos[0]; break;
            case Select.Stage2: cursor.localPosition = selectMenuPos[1]; break;
            case Select.Stage3: cursor.localPosition = selectMenuPos[2]; break;
        }

        // 選択しているStageによって飛ばすシーンを変える
        if (Input.GetButtonDown("Fire1")) {
            switch (selecter) {
                case Select.Stage1: GameMgr.Instance.GotoBuildScene(); break;
                case Select.Stage2: GameMgr.Instance.GotoStage2Scene(); break;
                case Select.Stage3: GameMgr.Instance.GotoStage3Scene(); break;
            }
        }

        // タイトルへ戻る
        if(Input.GetButtonDown("Cont_L1")) {
            GameMgr.Instance.GotoTitleScene();
        }
    }
}
