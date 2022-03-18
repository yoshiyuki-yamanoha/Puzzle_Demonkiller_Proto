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
    private Slider comboTimerSlider = null;     // コンボ受付時間のスライダー：スライダー


    [SerializeField] private Text combo_Text;
    [SerializeField] private GameObject Magic;
    [SerializeField] private GameObject Max_Magic;

    public Canvas meteorPoint = null;
    private Image[] mPImage = new Image[3];

    public AudioClip SE_meteorPointCharge;
    AudioSource audioSource;


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
        Init_Slider();
        Init_MagicShootCnt_Image();
        Init_audio();
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
        Set_SliderValue(Combo_GraceTime);
    }

    private void PuzReset()
    {
        SetCycleMax();
        RandmCycleSet();
        Set_Ene_Puz();
        ChargeMeteorPoint();
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

        //Debug.Log("0 : " + randNum[0] + " | 1 : " + randNum[1] + " | 2 : " + randNum[2] + " | 3 : " + randNum[3] +
        //            " | 4 : " + randNum[4] + " | 5 : " + randNum[5] + " | 6 : " + randNum[6] + " | 7 : " + randNum[7]);

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
        // 答えのデータを元にプレイヤー側のパズルをシャッフル答えのデータを元にプレイヤー側のパズルをシャッフル
        for(int i = 0, j=0; i < CYCLE_MAX; i++)
        {
            if(ansPuzzle[i])
            {
                emptyPuzzleBase[i].SetActive(true);
                activePuzzle[j++] = emptyPuzzleBase[i];
            }
        }

        int n = cycle_Max;
        if (Input.GetKey(KeyCode.Escape))return ;

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
            emptyPuzzleBase[i].transform.GetChild(0).GetComponent<GoToParent>().FadeSelectCircle();
            //emptyPuzzleBase[i].SetActive(true);
            //Ene_Magic_Puzzle[i].transform.GetChild(0).GetComponent<GoToParent>().FadeSelectCircle();
            Destroy(Ene_Magic_Puzzle[i].transform.GetChild(0).gameObject);

            //int num = i / 2;

            //Instantiate(Mag_Circle[num], Vector3.zero, Quaternion.identity, Ene_Magic_Puzzle[i].transform);
            //Ene_Magic_Puzzle[i].transform.GetChild(0).transform.position = Vector3.zero;
            //Ene_Magic_Puzzle[i].transform.GetChild(0).transform.rotation = Quaternion.identity;

            if (emptyPuzzleBase[i].transform.GetChild(0).transform.name[0] == 'R')
            {
                Instantiate(Mag_Circle[0], Vector3.zero, Quaternion.identity, Ene_Magic_Puzzle[i].transform);
                Ene_Magic_Puzzle[i].transform.GetChild(0).transform.position = Vector3.zero;
                Ene_Magic_Puzzle[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            }
            if (emptyPuzzleBase[i].transform.GetChild(0).transform.name[0] == 'B')
            {
                Instantiate(Mag_Circle[1], Vector3.zero, Quaternion.identity, Ene_Magic_Puzzle[i].transform);
                Ene_Magic_Puzzle[i].transform.GetChild(0).transform.position = Vector3.zero;
                Ene_Magic_Puzzle[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            }
            if (emptyPuzzleBase[i].transform.GetChild(0).transform.name[0] == 'G')
            {
                Instantiate(Mag_Circle[2], Vector3.zero, Quaternion.identity, Ene_Magic_Puzzle[i].transform);
                Ene_Magic_Puzzle[i].transform.GetChild(0).transform.position = Vector3.zero;
                Ene_Magic_Puzzle[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
            }
            if (emptyPuzzleBase[i].transform.GetChild(0).transform.name[0] == 'S')
            {
                Instantiate(Mag_Circle[3], Vector3.zero, Quaternion.identity, Ene_Magic_Puzzle[i].transform);
                Ene_Magic_Puzzle[i].transform.GetChild(0).transform.position = Vector3.zero;
                Ene_Magic_Puzzle[i].transform.GetChild(0).transform.rotation = Quaternion.identity;
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
                shootMagic();

                clear_combo = 0;

                //パズルをリセット
                CycleReset();

                PuzReset();

                Init_SetColor();

            }
        }
        combo_Text.text = "COMBO : " + clear_combo;
    }

    void shootMagic()
    {
        if (clear_combo >= COMBO_MAX)
        {
            Max_Magic.SetActive(true);
            // メテオ
            this.gameObject.GetComponent<FallingMagic>().M_FireFall(ene);

            audioSource.PlayOneShot(SE_meteorPointCharge);
        }
        else
        {
            Magic.SetActive(true);
            // フライ
            this.gameObject.GetComponent<MagicFlyingToTheEnemy>().M_FireForward(ene);

            ChargeMeteorPoint();
            audioSource.PlayOneShot(SE_meteorPointCharge);
        }
    }

    private void Init_Slider()
    {
        GameObject slider = GameObject.Find("Canvas/Slider").gameObject;
        comboTimerSlider = slider.GetComponent<Slider>();
        //comboTimerSlider.value = 0;
    }

    private void Set_SliderValue(float combo_remainingTime)
    {
        float time = combo_remainingTime / Combo_GraceMaxTime;

        comboTimerSlider.value = time;
    }

    private void Init_MagicShootCnt_Image()
    {
        int cc = meteorPoint.transform.childCount;

        for (int i = 0; i < cc; i++)
        {
            mPImage[i] = meteorPoint.transform.GetChild(i).gameObject.transform.GetComponent<Image>();

        }

        Init_SetColor();
    }

    private void Init_SetColor()
    {
        Color white = new Color(255, 255, 255, 255);

        for (int i = 0; i < 3; i++)
        {
            if (mPImage[i] == null)
                return;
            else
                mPImage[i].color = white;
        }
    }

    private void ChargeMeteorPoint()
    {
        if (clear_combo == 0) return;
        int image_num;
        if (clear_combo < COMBO_MAX)
            image_num = clear_combo - 1;
        else
            image_num = COMBO_MAX - 1;
        if (mPImage[image_num] == null) return;

        Color red = new Color(255, 0, 0, 255);

        mPImage[image_num].color = red;
    }

    private void Init_audio()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = SE_meteorPointCharge;
    }
}