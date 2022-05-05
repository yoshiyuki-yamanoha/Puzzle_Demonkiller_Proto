
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMgr : MonoBehaviour
{

    EnemyBase enemy;
    // 敵が倒された数
    static private int enemyDieNum;
    const int enemyDieMax = 48;
    // クリアしたフラグを取得
    EnemyCamera eCamera;
    // Start is called before the first frame update

    //残り体数テキスト
    [SerializeField] Text enemyNum;

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

        //残り敵数の表示
        enemyNum.text = "あと"+(enemyDieMax - enemyDieNum).ToString("00")+"体";
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
