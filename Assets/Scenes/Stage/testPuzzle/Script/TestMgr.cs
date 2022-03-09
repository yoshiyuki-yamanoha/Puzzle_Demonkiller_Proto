using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMgr : MonoBehaviour
{
    bool isClear;
    [SerializeField] GameObject t_gameClear;
    [SerializeField] GameObject t_pushToB;
    float buttonCoolTimeCunt;
    // Start is called before the first frame update
    void Start()
    {
        isClear = false;
        t_gameClear.SetActive(false);
        t_pushToB.SetActive(false);
        buttonCoolTimeCunt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isClear)
        {
            t_gameClear.SetActive(true);
            t_pushToB.SetActive(true);
            if (Input.GetButtonDown("Fire2"))
            {
                GameMgr.Instance.Restart(); // ゲームリスタート
                if(GameMgr.Instance.GetNowSceneName() == "testPuzzle")
                {
                    GameMgr.Instance.GotoStage2();
                }else
                {
                    GameMgr.Instance.GotoStage1();
                }
            }
        }

        // デバッグ用のゲーム終了処理
        if (Input.GetButtonDown("Start"))
        {
            //Debug.Log("Out");
            GameMgr.Instance.GotoQuit();
        }
    }

    public void GameClearOn()
    {
        isClear = true;
    }
}
