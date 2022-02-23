using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Mgr : MonoBehaviour
{
    int dieEnemyCount;  // 倒されたエネミーの数をカウント
    [SerializeField]  int　dieEnemy_Max; // 倒せる敵の上限 
    // Start is called before the first frame update
    void Start()
    {
        dieEnemyCount = 0;  // 倒されたエネミーの数を初期化
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(dieEnemy_Max <= dieEnemyCount)   // ゲームクリア判定の確認
        {
            // GameClear
            Debug.Log("GameClear");
        }
    }

    // 倒されたエネミーの数を1増やす
    public void DieEnemyCount()
    {
        dieEnemyCount++;
    }
}
