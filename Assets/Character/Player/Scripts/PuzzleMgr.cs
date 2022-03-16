using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] GameObject[] Ene_Magic_Puzzle = new GameObject[8];     //敵側の魔法陣

    [SerializeField] private string PuzzleBaseStr;
    [SerializeField] private string EnePuzzleStr;


    [SerializeField] public int cycle_Max;    // 出現する魔法陣の数

    [SerializeField] private GameObject[] Mag_Circle = new GameObject[4]; 

    [SerializeField] Material ans;
    [SerializeField] GameObject ene;

    private bool canPuzReset;
    private bool IsEstablished;
    [SerializeField] private bool IsComboReset;
    [SerializeField] private float Combo_GraceTime;
    [SerializeField,Range(0.5f,10.0f)]private float Combo_GraceMaxTime = 5.0f;

    [SerializeField] private Text combo_Text;
    [SerializeField] private GameObject Magic;
    [SerializeField] private GameObject Max_Magic;


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
        //Prepare_CyclesData[] cyclesData = new Prepare_CyclesData[8];
        PuzReset();
        IsEstablished = false;
        IsComboReset = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            CycleReset();
        }
        SetAnswer();
        if (Check_Puz() && canPuzReset)
        {
            Invoke("PuzReset", 0.4f);
            IsEstablished = true;
            canPuzReset = false;
        }
        Combo();
    }

    private void PuzReset()
    {
        SetCycleMax();
        RandmCycleSet();
        Set_Ene_Puz();
        canPuzReset = true;
    }

    public void SetCycleMax()
    {
        int comboCycle = clear_combo;     //コンボ分の魔法陣の量

        CycleActiveFalse();
        // コンボが最大数を超えていたら
        if (clear_combo >= COMBO_MAX)
        {
            //clear_combo = 0;
            //return;
            comboCycle = COMBO_MAX - 1;
        }
        if (IsComboReset)
        {
            comboCycle = 0;          //コンボ終了時に魔法陣を元の数に戻す
        }
        //cycle_Max = DEFAULT_CTCLE + clear_combo; // デフォルトの魔法陣の数とクリア回数を足して魔法陣の数を増やす
        cycle_Max = DEFAULT_CTCLE + comboCycle; // デフォルトの魔法陣の数とクリア回数を足して魔法陣の数を増やす
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

            //Debug.Log(randNum[i]);
            numbers.RemoveAt(num);
        }
        // 魔法陣を出現させる
        for (int i=0;i<CYCLE_MAX;i++)
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
            ansPuzzle[i] = false;
        }
    }

    public void CycleReset()
    {
        GameObject.Find("Pointer").GetComponent<Point_Con2>().SelClear();
        for (int i = 0; i < CYCLE_MAX; i++)
        {
            //emptyPuzzleBase[i].SetActive(true);
            emptyPuzzleBase[i].transform.GetChild(0).GetComponent<GoToParent>().FadeSelectCircle();
            Destroy(emptyPuzzleBase[i].transform.GetChild(0).gameObject);

            int num = i / 2;

            Instantiate(Mag_Circle[num], Vector3.zero, Quaternion.identity, emptyPuzzleBase[i].transform);
            emptyPuzzleBase[i].transform.GetChild(0).transform.position = Vector3.zero;
            emptyPuzzleBase[i].transform.GetChild(0).transform.rotation = Quaternion.identity;

            //if(emptyPuzzleBaseChildName[i][0] == 'R')
            //{
            //    Instantiate(r, Vector3.zero, Quaternion.identity, emptyPuzzleBase[i].transform);
            //    emptyPuzzleBase[i].transform.GetChild(0).transform.position = Vector3.zero;
            //    emptyPuzzleBase[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            //}
            //if (emptyPuzzleBaseChildName[i][0] == 'B')
            //{
            //    Instantiate(b, Vector3.zero, Quaternion.identity, emptyPuzzleBase[i].transform);
            //    emptyPuzzleBase[i].transform.GetChild(0).transform.position = Vector3.zero;
            //    emptyPuzzleBase[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            //}
            //if (emptyPuzzleBaseChildName[i][0] == 'G')
            //{
            //    Instantiate(g, Vector3.zero, Quaternion.identity, emptyPuzzleBase[i].transform);
            //    emptyPuzzleBase[i].transform.GetChild(0).transform.position = Vector3.zero;
            //    emptyPuzzleBase[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            //}
            //if (emptyPuzzleBaseChildName[i][0] == 'S')
            //{
            //    Instantiate(s, Vector3.zero, Quaternion.identity, emptyPuzzleBase[i].transform);
            //    emptyPuzzleBase[i].transform.GetChild(0).transform.position = Vector3.zero;
            //    emptyPuzzleBase[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            //}
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

    public void Set_Ene_Puz()
    {
        for (int i = 0; i < CYCLE_MAX; i++)
        {
            Ene_Magic_Puzzle[i].SetActive(ansPuzzle[i]);
        }

        ene.GetComponent<Renderer>().material = ans;

        //Debug.Log("0 : " + ansPuzzle[0] + " | 1 : " + ansPuzzle[1] + " | 2 : " + ansPuzzle[2] + " | 3 : " + ansPuzzle[3] +
        //            " | 4 : " + ansPuzzle[4] + " | 5 : " + ansPuzzle[5] + " | 6 : " + ansPuzzle[6] + " | 7 : " + ansPuzzle[7]);
    }

    string Set_Puz_Str(GameObject[] objects)
    {
        string answerStr = "";

        foreach (GameObject o in objects)
        {
            if (o.transform.childCount > 0 && o.gameObject.activeInHierarchy)
                answerStr = answerStr + o.transform.GetChild(0).gameObject.name[0];
        }
        return answerStr;
    }

    private void SetAnswer()
    {
        EnePuzzleStr = Set_Puz_Str(Ene_Magic_Puzzle);
        PuzzleBaseStr = Set_Puz_Str(emptyPuzzleBase);
    }

    private bool Check_Puz()
    {
        if(PuzzleBaseStr == EnePuzzleStr)
        {
            return true;
        }
        return false;
    }

    private void Combo()
    {
        if (IsEstablished)      //パズルが成立したら
        {
            if(clear_combo == 0)        //コンボが０なら
            {
                clear_combo++;
                Combo_GraceTime = Combo_GraceMaxTime;
                
                //コンボが成立しても時間を過ぎていたら０に戻す
                if (IsComboReset)
                {
                    clear_combo = 0;
                    IsComboReset = false;
                }
            }
            else
            {
                if(Combo_GraceTime >= 0f)
                {
                    clear_combo++;
                    Combo_GraceTime = Combo_GraceMaxTime;
                }
            }
            IsEstablished = false;

        }

        if(Combo_GraceTime >= 0f)
        {
            Combo_GraceTime -= Time.deltaTime;
        }
        else
        {
            if (clear_combo != 0)       //コンボ終了
            {
                int j = 0;
                shootMagic();

                clear_combo = 0;

                //パズルをリセット
                CycleReset();

                //PuzReset();

                IsComboReset = true;
            }
        }
        combo_Text.text = "COMBO : " + clear_combo;
    }

    void shootMagic()
    {
        if (clear_combo >= COMBO_MAX)
        {
            Max_Magic.SetActive(true);
        }
        else
        {
            Magic.SetActive(true);
        }
    }
}
