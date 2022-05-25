
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMgr : MonoBehaviour
{

    EnemyBase enemy;
    // 敵が倒された数
    static private int enemyDieNum;
    const int enemyDieMax = 30;//48;
    // クリアしたフラグを取得
    EnemyCamera eCamera;
    // Start is called before the first frame update

    //残り体数テキスト
    [SerializeField] Text enemyNum;
    GenerationEnemy generation_enmy; //敵の最大数を取得するため

    private void Awake()
    {
        generation_enmy = GameObject.Find("Sponer").GetComponent<GenerationEnemy>();
        enemyDieNum = 0;
        //enemyDieMax = generation_enmy.GetMaxEnemy();
    }

    void Start()
    {
        
        eCamera = GameObject.Find("EnemyCamera").GetComponent<EnemyCamera>();
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


        //残り敵数の表示
        enemyNum.text = "×"+(enemyDieMax - enemyDieNum).ToString("00");
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
