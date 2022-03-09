using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPuzzleClearCheck : MonoBehaviour
{
    [SerializeField] GameObject[] puzzleOya = new GameObject[3]; // 魔法陣の親オブジェクト
    string[] pea = new string[3];
    string[] child = new string[3];
    TestMgr tmgr;
    // Start is called before the first frame update
    void Start()
    {
        pea[0] = "1_1";
        pea[1] = "2_2";
        pea[2] = "3_3";

        child[0] = "1";
        child[1] = "2";
        child[2] = "3";
        tmgr = this.GetComponent<TestMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        int cunt=0;
        for (int i = 0; i < 3; i++)
        {
            if (puzzleOya[i].transform.name == pea[i] && puzzleOya[i].transform.GetChild(0).name == child[i]
            && puzzleOya[i].transform.GetChild(0).rotation.z == 0f)
            {
                cunt++; // クリアしているパズルの数
            }
        }
        
        // パズルをクリアしていたらゲームクリアフラグをオンにする
        if(cunt >= 3)
        {
            tmgr.GameClearOn();
        }
    }
}
