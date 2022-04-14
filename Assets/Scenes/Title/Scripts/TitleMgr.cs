using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMgr : MonoBehaviour
{

    enum Select
    {
        Start,
        Quit
    }
    [SerializeField] private Select selecter;
    float selectCoolTime;       // 選択カーソルが長押ししていても一瞬その場所で止まる時間
    const float SELECT_COOLTIME_MAX = 10.0f;// 選択カーソルが長押ししていても一瞬その場所で止まる最大時間
    Vector3[] selectMenuPos = new Vector3[2];
    [SerializeField] Transform cursor;
    // Start is called before the first frame update
    void Start()
    {
        selecter = Select.Start;
        selectCoolTime = 0.0f;
        selectMenuPos[0] = new Vector3(4.98f, -57.0f, 0.0f);
        selectMenuPos[1] = new Vector3(4.98f, -102.6f, 0.0f);
    }

    // Update is called once per frame
    private void Update() {

        selectCoolTime--;// クールタイムを起動
        if (Input.GetAxis("UPDOWN") <= 0.9f && Input.GetAxis("UPDOWN") >= 0.0f) {
            selectCoolTime = 0;
        }
        if (selectCoolTime <= 0.0) {
            float inputInfo = Input.GetAxis("UPDOWN");
            selectCoolTime = SELECT_COOLTIME_MAX;   // クールタイムの初期化
            if (Input.GetAxis("UPDOWN") > 0.9f || Input.GetAxis("UPDOWN") < 0.0f) {
                if(selecter == Select.Start) {
                    selecter = Select.Quit;
                }else {
                    selecter = Select.Start;
                }
            }
        }

        switch (selecter) {
            case Select.Start: cursor.localPosition = selectMenuPos[0]; break;
            case Select.Quit: cursor.localPosition = selectMenuPos[1]; break;
        }

        // 選択しているStageによって飛ばすシーンを変える
        if (Input.GetButtonDown("Fire1")) {
            switch (selecter) {
                case Select.Start: GameMgr.Instance.GotoSelectScene(); break;
                case Select.Quit: GameMgr.Instance.GotoQuit(); break;
            }
        }
    }
}
