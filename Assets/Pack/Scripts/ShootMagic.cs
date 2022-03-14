using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootMagic : MonoBehaviour
{

    [SerializeField] private GameObject Magic;

    [SerializeField] private Transform[] Player;

    [SerializeField] string PlayerStr;       //敵の魔法陣の正解

    PuzzleMgr puMgr;


    private int magicShootCnt;               // 魔法を撃った数
    private float magicComboTimer;            // 魔法のコンボ受付時間
    private GameObject ComboTimerSlider_obj; // コンボ受付時間のスライダー：オブジェクト
    private Slider ComboTimerSlider_com;     // コンボ受付時間のスライダー：スライダー
    private Text ComboTimerText;
    float timetext = 0;

    [SerializeField]List<GameObject> Enemy_List = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        puMgr = GameObject.Find("PuzzleMgr").GetComponent<PuzzleMgr>();
        setPlayerMagicStr();

        magicShootCnt = 0;
        Init_magicShootCnt();

        Init_Slider();
    }

    // Update is called once per frame
    void Update()
    {
        setPlayerMagicStr();
        Ene_MC_Check();
        Check_MagicTimer(ComboTimerSlider_com.value);
    }

    void Ene_MC_Check()
    {
        if (Enemy_List.Count == 0) return;

        foreach (GameObject ene in Enemy_List)
        {
            if(ene != null)
            {
                string eneStr = ene.transform.GetChild(1).GetComponent<Ene_MagicCircle>().GetEneStr();
                if (eneStr == PlayerStr)
                {
                    //魔法を撃つ
                    //魔法を５回撃つごとに落ちてくる魔法に変える
                    if (magicShootCnt < 3)
                    {
                        this.gameObject.GetComponent<MagicFlyingToTheEnemy>().M_FireForward(ene);
                        magicShootCnt++;

                        //if(magicComboTimer == 0)
                        if (ComboTimerSlider_com.value == 0)
                        {
                            int magicComboTimeLimit = 1;
                            //magicComboTimer = magicComboTimeLimit;
                            ComboTimerSlider_com.value = magicComboTimeLimit;
                        }
                    }
                    else
                    {
                        this.gameObject.GetComponent<FallingMagic>().M_FireFall(ene);
                        magicShootCnt = 0;
                        //magicComboTimer = 0;
                        ComboTimerSlider_com.value = 0;
                    }

                    //敵が倒れる処理
                    Enemy_List.Remove(ene);
                    Destroy(ene.transform.GetChild(1).gameObject);
                    //del.GetComponent<Demon>().Damage(100.0f);
                    break;
                }
            }
        }
    }

    public void CreateMagic()
    {
        Instantiate(Magic, transform.position, Quaternion.identity);
    }

    public void Enelist_Add(GameObject obj)
    {
        Enemy_List.Add(obj);
    }
    
    public void Enelist_Delete(GameObject obj)
    {
        Enemy_List.Remove(obj);
    }

    void setPlayerMagicStr()
    {
        PlayerStr = "";

        foreach (Transform o in Player)
        {
            if (o.childCount > 0 && o.gameObject.activeSelf)
                PlayerStr = PlayerStr + o.GetChild(0).gameObject.name;
        }
    }

    public string Get_Str()
    {
        return PlayerStr;
    }

    public List<GameObject> get_EneList()
    {
        return Enemy_List;
    }


    private void Init_magicShootCnt()
    {
        magicShootCnt = 0;
        magicComboTimer = 0;
    }

    // コンボ受付時間が残っているかチェック
    private void Check_MagicTimer(float timeBuf)
    {
        //int decreaseTime = 6000;
        float decreaseTime = Application.targetFrameRate * 30.0f;
        float amountOfDecrease = 1 / (float)decreaseTime;

        if (timeBuf > 0)
        {
            //magicComboTimer--;
            ComboTimerSlider_com.value -= amountOfDecrease;
            //ComboTimerText.text = (ComboTimerSlider_com.value - amountOfDecrease).ToString();
            ComboTimerText.text = (timetext += Time.deltaTime).ToString();
        }
        else if (magicComboTimer > 0)
        {
            //magicComboTimer--;
            ComboTimerSlider_com.value -= amountOfDecrease;
            //ComboTimerText.text = (ComboTimerSlider_com.value - amountOfDecrease).ToString();
            ComboTimerText.text = (timetext += Time.deltaTime).ToString();
        }
        else
        {
            Init_magicShootCnt();
        }
    }

    private void Init_Slider()
    {
        ComboTimerSlider_obj = GameObject.Find("UIs/TimerUI/Slider").gameObject;
        ComboTimerSlider_com = ComboTimerSlider_obj.GetComponent<Slider>();
        ComboTimerSlider_com.value = 0;

        ComboTimerText = GameObject.Find("UIs/TimerUI/timer").GetComponent<Text>();
    }
}
