using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{

    //メニューの表示/非表示切り替え用
    [SerializeField] GameObject menu;

    //色替えのモードを変えるよう
    [SerializeField] PointControl pc;
    [SerializeField] Text colorChangeMode;
    string[] modeStr = new string[3] { "タイプA", "タイプB", "タイプC" };

    //敵の生成間隔と上限を設定する用
    [SerializeField] EnemySpawn es;
    [SerializeField] Text enemyNum;
    [SerializeField] Text enemySpawnInterval;

    //メニュー番号
    int menuNum;
    //[SerializeField] Transform menuLineTf;
    //[SerializeField] Transform menuLinePos;
    Vector3[] posABC;

    //アロー
    [SerializeField] Image[] arrows;

    //メニュー移動インターバル
    int intervalV;

    //メニュー変更インターバル
    int intervalH;

    void Start()
    {
        menu.SetActive(false);
        menuNum = 0;
        ///posABC = new Vector3[menuLinePos.childCount];
        //for (int i = 0; i < menuLinePos.childCount; i++)
        //    posABC[i] = menuLinePos.GetChild(i).position;

        enemyNum.text = es.GetMaxEnemyLimit().ToString("0") + "体";
        enemySpawnInterval.text = es.GetInterval().ToString("0.0") + "フレーム";
    }

    
    void Update()
    {
        //スタートボタンで表示/非表示切り替え
        if (Input.GetButtonDown("Start"))
        {
            menu.SetActive(!menu.activeInHierarchy);

            if (menu.activeInHierarchy) {
                MenuReset();
            }

            pc.isMenu = menu.activeInHierarchy;
        }

        //メニュー表示時の動作
        if (menu.activeInHierarchy) {

            //メニューのカーソル移動 (縦)
            MenuMoveCursor();

            //メニューの設定変更 (横)
            MenuOptionChange();
        }


    }

    //メニューを開くときに初期化
    void MenuReset() {
        menuNum = 0;
        arrows[menuNum].color = Color.yellow;
    }

    //メニューのカーソル移動
    void MenuMoveCursor() {

        int oldMenuNum = menuNum;

        if (intervalV == 0)
        {
            float ver = Input.GetAxis("Vertical");
            if (ver > 0) menuNum--;
            if (ver < 0) menuNum++;
            if (menuNum < 0) menuNum = 2;
            if (menuNum > 2) menuNum = 0;
        }
        else {
            intervalV--;
            if (intervalV <= 0) intervalV = 0;
        }

        //カーソルが移動されたら位置を変える
        if (menuNum != oldMenuNum)
        {
            //menuLineTf.position = posABC[menuNum];
            intervalV = 10;

            //矢印の色替え
            for(int i = 0; i < arrows.Length; i++) {
                arrows[i].color = Color.white;
                if (i == menuNum)
                    arrows[i].color = Color.yellow;
            }
                
        }

    }

    //メニューの設定変更
    void MenuOptionChange() {

        if (intervalH == 0)
        {
            float hori = Input.GetAxis("Horizontal");

            switch (menuNum)
            {
                case 0: //色替えモード
                    int currentCCMode = pc.GetChangeColorMode();
                    int buff3 = currentCCMode;
                    
                    if (hori > 0) buff3++;
                    if (hori < 0) buff3--;
                    if (buff3 < 0) buff3 = 0;
                    if (buff3 > 2) buff3 = 2;

                    if (currentCCMode != buff3) {
                        pc.SetChangeColorMode(buff3);
                        colorChangeMode.text = modeStr[buff3];
                        intervalH = 12;
                        
                    }
                    
                    break;
                case 1: //一度に湧く敵の数
                    int currentMaxNum = es.GetMaxEnemyLimit();
                    int buff = currentMaxNum;

                    if (hori > 0) buff += 1;
                    if (hori < 0) buff -= 1;
                    if (buff < 0) buff = 0;
                    if (buff > 10) buff = 10;

                    if (currentMaxNum != buff)
                    {
                        es.SetMaxEnemyLimit(buff);
                        enemyNum.text = buff.ToString("0") + "体";
                        intervalH = 12;
                    }

                    break;
                case 2: //敵の間隔
                    float currentInterval = es.GetInterval();
                    float buff2 = currentInterval;

                    if (hori > 0) buff2 += 1.0f;
                    if (hori < 0) buff2 -= 1.0f;
                    if (buff2 < 1) buff2 = 1;
                    if (buff2 > 180) buff2 = 180;

                    if (currentInterval != buff2)
                    {
                        es.SetInterval(buff2);
                        enemySpawnInterval.text = buff2.ToString("0") + "フレーム";
                        intervalH = 8;
                    }
                    break;
            }
        }
        else
        {
            intervalH--;
            if (intervalH <= 0) intervalH = 0;
        }

    }
}
