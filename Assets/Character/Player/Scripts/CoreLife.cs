using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreLife : MonoBehaviour
{

    //private
    int coreLife;    // コアのライフ

    [SerializeField] int coreLife_Max; // コアの最大のライフ
    
    // Start is called before the first frame update
    void Start()
    {
        coreLife = coreLife_Max;    // ライフの初期化
    }

    void FixedUpdate()
    {
       
        if(CheckGameOver())
        {
            // ゲームオーバー
            Debug.Log("GameOver");
        }    
    }

    // コアがダメージを受ける際に使用する関数
    public void CoreDamege()
    {
        int damge = 1;  // 与えるダメージ
        coreLife -= damge;  // ダメージを与える
    }

    // 現在のライフを取得する
    public int GetCoreLife()
    {
        return coreLife;
    }

    // ライフが0になっているかどうかの確認関数
    public bool CheckGameOver()
    {
        if(GetCoreLife() <= 0) {
            return true;
        }
        return false;
    }

}
