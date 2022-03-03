using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Mgr : MonoBehaviour
{
    [SerializeField] int dieEnemyCount;  // 倒されたエネミーの数をカウント
    [SerializeField] int dieEnemy_Max; // 倒せる敵の上限 
    GameObject Clear_UI;
    private bool canClear;
    // Start is called before the first frame update
    void Start()
    {
        Clear_UI = GameObject.Find("Game_Clear");
        dieEnemyCount = 0;  // 倒されたエネミーの数を初期化
        canClear = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Clear_UI.SetActive(!canClear);

        if (dieEnemy_Max <= dieEnemyCount && canClear)   // ゲームクリア判定の確認
        {
            Invoke("Game_Clear", 3.0f);
            Clear_UI.SetActive(true);
            canClear = false;
        }

        if (Input.GetButtonDown("Fire4"))
        {
            GameMgr.Instance.GotoQuit();
        }

        if (Input.GetButtonDown("Fire5"))
        {
            //GameMgr.Instance.Restart();
        }
    }

    // 倒されたエネミーの数を1増やす
    public void DieEnemyCount()
    {
        dieEnemyCount++;
    }

    private void Game_Clear()
    {
        GameMgr.Instance.Restart();
        // GameClear
    }
}
