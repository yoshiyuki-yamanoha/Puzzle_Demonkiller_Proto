using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TitleMgr : MonoBehaviour
{

    enum Select
    {
        Start,
        Quit
    }
    [SerializeField] private Select selecter;
    float selectCoolTime;       // 選択カーソルが長押ししていても一瞬その場所で止まる時間
    const float SELECT_COOLTIME_MAX = 10.0f;// 選択カーソルが長押ししていても一瞬その場所で止まる最大時間
    Vector3[] selectMenuPos = new Vector3[2];
    [SerializeField] Transform cursor;

    MenuControll menuControll;

   public BGMManager bgmPlay = null;

    SEManager  sePlay = null;

    public AudioSource audioSource;

    [SerializeField] Sprite[] startImg = new Sprite[2];    //　スタート文字の画像
    [SerializeField] Sprite[] quitImg = new Sprite[2];     //　Quit文字の画像

    [SerializeField] Image ui_Start;    // Start表示画像の情報
    [SerializeField] Image ui_Quit;     // Quit表示画像の情報
    [SerializeField] Image ui_Line;     // Line表示画像の情報

    [SerializeField] Animator cycleAnim;
    bool nextSceneFlg;
    bool buttonInputFlg;    // 一回しか押せないフラグ
    float line_width;
    
    // Start is called before the first frame update
    void Start()
    {
        buttonInputFlg = false;
        nextSceneFlg = false;
        selecter = Select.Start;
        selectCoolTime = 0.0f;
        selectMenuPos[0] = new Vector3(868.0f, 122.0f, 0.0f);
        selectMenuPos[1] = new Vector3(868.0f, -19.0f, 0.0f);
        this.gameObject.AddComponent<MenuControll>();
        menuControll = this.GetComponent<MenuControll>();
        GenerationTitle_BGM();
    }

    // Update is called once per frame
    private void Update()
    {

        //if(menuControll.GetUpDown() != 99)
        //{
        //    if (selecter == Select.Start)
        //    {
        //        selecter = Select.Quit;
        //        ui_Start.sprite = startImg[0];
        //        ui_Quit.sprite = quitImg[1];
        //    }
        //    else
        //    {
        //        selecter = Select.Start;
        //        ui_Start.sprite = startImg[1];
        //        ui_Quit.sprite = quitImg[0];


        //    }
        //}
        int inputnum = menuControll.GetUpDown();
        if (inputnum == ((int)MenuControll.UpDown.DOWN))
        {
            if (selecter == Select.Start) sePlay.Play("MagicCursorSelect");
            selecter = Select.Quit;
            ui_Start.sprite = startImg[0];
            ui_Quit.sprite = quitImg[1];
            
        }
        if(inputnum == ((int)MenuControll.UpDown.UP))
        {
            if (selecter == Select.Quit) sePlay.Play("MagicCursorSelect");
            selecter = Select.Start;
            ui_Start.sprite = startImg[1];
            ui_Quit.sprite = quitImg[0];
            ;

        }
        
        switch (selecter)
        {
            case Select.Start: cursor.localPosition = selectMenuPos[0]; break;
            case Select.Quit: cursor.localPosition = selectMenuPos[1]; break;
        }

        // 選択しているStageによって飛ばすシーンを変える
        if (nextSceneFlg)
        {
            switch (selecter)
            {
                case Select.Start: GameMgr.Instance.GotoBuildScene(); break;
                case Select.Quit: GameMgr.Instance.GotoQuit(); break;
            }
        }

        if(Input.GetButtonDown("Fire1") && !buttonInputFlg)
        {
            
            // 魔法陣のアニメーションの開始
            cycleAnim.SetTrigger("Go2");
            buttonInputFlg = true;
            sePlay.Play("TitleDecision");
        }
        //selectCoolTime--;// クールタイムを起動
        //if (Input.GetAxis("UPDOWN") <= 0.9f && Input.GetAxis("UPDOWN") >= 0.0f) {
        //    selectCoolTime = 0;
        //}
        //if (selectCoolTime <= 0.0) {
        //    float inputInfo = Input.GetAxis("UPDOWN");
        //    selectCoolTime = SELECT_COOLTIME_MAX;   // クールタイムの初期化
        //    if (Input.GetAxis("UPDOWN") > 0.9f || Input.GetAxis("UPDOWN") < 0.0f) {
        //        if(selecter == Select.Start) {
        //            selecter = Select.Quit;
        //        }else {
        //            selecter = Select.Start;
        //        }
        //    }
        //}


    }

    public void GotoNextScene()
    {
        nextSceneFlg = true;
    }
    void GenerationTitle_BGM()
    {
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
        bgmPlay = GameObject.Find("BGMAudio").GetComponent<BGMManager>();//Se再生用  
        bgmPlay.Play("TITLEBGM");
        
    }
}
