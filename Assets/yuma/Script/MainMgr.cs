
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMgr : MonoBehaviour
{

    // 敵が倒された数
    static private int enemyDieNum;
    const int enemyDieMax = 10;
    // クリアしたフラグを取得
    EnemyCamera eCamera;
    // Start is called before the first frame update
    void Start()
    {
        eCamera = GameObject.Find("EnemyCamera").GetComponent<EnemyCamera>();
        enemyDieNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyDieNum >= enemyDieMax) {
            if (eCamera.endFlag)
            {
                GameMgr.Instance.GotoGameClearScene();
            }
        }
        if(Input.GetButtonDown("Start"))
        {
            GameMgr.Instance.Restart();
        }
    }

    public void EnemiDieCount() {
        enemyDieNum++;
    }
    public int GetEnemyDieCount() {
        return enemyDieNum;
    }
    public int GetEnemyDieCountMax()
    {
        return enemyDieMax;
    }
}
