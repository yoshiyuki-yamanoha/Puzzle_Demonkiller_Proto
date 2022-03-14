using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************************************
 * 出題されるパズルをあらかじめこっちで設定する。
 * ①変数comboを参照し、その数に応じて出現させる魔法陣を変数cycle_Maxに格納 (コンボが０なら魔法陣の数は３つ。さらにコンボが増えると魔法陣の数もその数分増える。コンボが4になると強制的に魔法が発動)
 * ②時計回りに番号をふった魔法陣の配列にランダムでcycle_Max分魔法陣を割り振る
 * ③割り振ったデータ（答え）は答えの魔法陣に渡す。
 * ④割り振るデータ（答）にかぶらない魔法陣の形を組みプレイヤーの魔法陣に渡す。
 * **/
public class PuzzleMgr : MonoBehaviour
{

    const int CYCLE_MAX = 8; // 魔法陣を設置できる空のオブジェクトの数
    const int DEFAULT_CTCLE = 3;    // 0コンボ時の魔法陣の初期の数
    const int COMBO_MAX = 3;        // コンボの最大数

    public int clear_combo; // パズルをクリアした回数
    [System.NonSerialized] public Prepare_CyclesData[] cyclesData = new Prepare_CyclesData[8]; // 魔法陣を設置できる空のオブジェクトの情報
    public GameObject[] emptyPuzzleBase = new GameObject[8];  // パズルの親オブジェクト
    public string[] emptyPuzzleBaseChildName = new string[8];  // パズルの親オブジェクトの名前
    public bool[] ansPuzzle = new bool[8];

    [SerializeField] public int cycle_Max;    // 出現する魔法陣の数

    public GameObject r;
    public GameObject b;
    public GameObject g;


    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i< CYCLE_MAX;i++)
        {
            emptyPuzzleBaseChildName[i] = emptyPuzzleBase[i].transform.GetChild(0).gameObject.name;
            ansPuzzle[i] = false;
        }
        // 初期化
        clear_combo = 0;
        Prepare_CyclesData[] cyclesData = new Prepare_CyclesData[8];
        SetCycleMax();
        RandmCycleSet();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            CycleReset();
        }
    }

    public void SetCycleMax()
    {
        CycleActiveFalse();
        // コンボが最大数を超えていたら関数から出す
        if (clear_combo >= COMBO_MAX)
        {
            clear_combo = 0;
            return;
        }
        cycle_Max = DEFAULT_CTCLE + clear_combo; // デフォルトの魔法陣の数とクリア回数を足して魔法陣の数を増やす
    }

    public void RandmCycleSet()
    {
        int[] randNum = { 99, 99, 99, 99, 99, 99, 99, 99};
        List<int> numbers = new List<int>();
        
        // 使用する数字を確保
        for(int i=0;i< CYCLE_MAX;i++)
        {
            numbers.Add(i);
        }
        // 乱数でセットする場所を確保
        for (int i = 0; i < cycle_Max; i++)
        {
            int num = Random.Range(0, numbers.Count-1);
            
            randNum[i] = numbers[num];


            Debug.Log(randNum[i]);
            numbers.RemoveAt(num);
        }
        // 魔法陣を出現させる
        for(int i=0;i<CYCLE_MAX;i++)
        {
            if(randNum[i] != 99)
            {
                // 答えのデータ
                ansPuzzle[randNum[i]] = true;
                //emptyPuzzleBase[randNum[i]].SetActive(true);
            }
            
        }

        GameObject[] activePuzzle = new GameObject[CYCLE_MAX];
        // 答えのデータを元にプレイヤー側のパズルをシャッフル
        for(int i = 0, j=0; i < CYCLE_MAX; i++)
        {
            if(ansPuzzle[i])
            {
                emptyPuzzleBase[i].SetActive(true);
                activePuzzle[j++] = emptyPuzzleBase[i];
            }
        }

        int n = cycle_Max;

        while (n > 1)
        {

            n--;

            int k = UnityEngine.Random.Range(0, n);
            GameObject temp = activePuzzle[k].transform.GetChild(0).gameObject;
            activePuzzle[k].transform.GetChild(0).gameObject.transform.parent = activePuzzle[n].transform;
            activePuzzle[n].transform.GetChild(0).gameObject.transform.parent = activePuzzle[k].transform;

        }

    }

    public void CycleActiveFalse ()
    {
        for(int i=0; i< CYCLE_MAX; i++)
        {

            emptyPuzzleBase[i].SetActive(false);
        }
    }

    public void CycleReset()
    {
        for (int i = 0; i < CYCLE_MAX; i++)
        {
            Destroy(emptyPuzzleBase[i].transform.GetChild(0).gameObject);
            if(emptyPuzzleBaseChildName[i] == "R")
            {
                Instantiate(r, Vector3.zero, Quaternion.identity, emptyPuzzleBase[i].transform);
                emptyPuzzleBase[i].transform.GetChild(0).transform.position = Vector3.zero;
                emptyPuzzleBase[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            }
            if (emptyPuzzleBaseChildName[i] == "B")
            {
                Instantiate(b, Vector3.zero, Quaternion.identity, emptyPuzzleBase[i].transform);
                emptyPuzzleBase[i].transform.GetChild(0).transform.position = Vector3.zero;
                emptyPuzzleBase[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            }
            if (emptyPuzzleBaseChildName[i] == "G")
            {
                Instantiate(g, Vector3.zero, Quaternion.identity, emptyPuzzleBase[i].transform);
                emptyPuzzleBase[i].transform.GetChild(0).transform.position = Vector3.zero;
                emptyPuzzleBase[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            }
        }
    }

    // 正解のパズルが獲得できます。
    public List<bool> GetAnsPuzzle()
    {
        List<bool> ans = new List<bool>();

        for(int i=0; i<CYCLE_MAX;i++)
        {
            ans.Add(ansPuzzle[i]);
        }

        return ans;
    }
}
