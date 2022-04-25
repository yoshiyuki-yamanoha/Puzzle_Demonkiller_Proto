
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMgr : MonoBehaviour
{

    // 敵が倒された数
    static private int enemyDieNum;
    const int enemyDieMax = 10;
    // Start is called before the first frame update
    void Start()
    {
        enemyDieNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyDieNum >= enemyDieMax) {
            GameMgr.Instance.GotoGameClearScene();
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
}
