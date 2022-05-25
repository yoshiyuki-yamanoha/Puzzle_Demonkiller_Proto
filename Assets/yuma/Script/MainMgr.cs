
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

    // フェードマネージャー
    FadeOut fadeMGR;
    bool isVictoryAnim;
    bool finishWaitFlag;
    float waitTime;

    BGMManager bgmManager;

    private void Awake()
    {
        generation_enmy = GameObject.Find("Sponer").GetComponent<GenerationEnemy>();
        enemyDieNum = 0;
        //enemyDieMax = generation_enmy.GetMaxEnemy();

        fadeMGR = GameObject.Find("FadeCanvas/FadeImage").gameObject.GetComponent<FadeOut>();
        isVictoryAnim = false;
        finishWaitFlag = false;
        waitTime = 2.0f;

        bgmManager = GameObject.Find("BGMAudio").gameObject.GetComponent<BGMManager>();
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
                //GameMgr.Instance.GotoGameClearScene();
                isVictoryAnim = true;
            }
        }

        if (isVictoryAnim == true)
        {
            if (finishWaitFlag == false)
                StartVictoryAnimDirecting();
            else
            {

                if (FinishFadeOutDirecting() == true)
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

    private void StartVictoryAnimDirecting()
    {
        if (waitTime > 0)
            waitTime -= Time.deltaTime;
        else
        {
            finishWaitFlag = true;

            // 画面フェードアウトの開始
            fadeMGR.SetFadeMode(FadeOut.FadeMode.FADE_OUT);
            fadeMGR.SetFadeSpeed(0.4f);
            fadeMGR.SetColor_White();
            // 音量フェードアウトの開始
            var bgmVolume = bgmManager.GetBGMVolume();
            if (bgmVolume == 0) bgmVolume = 0.4f;
            float volumeAttenuation = bgmVolume * fadeMGR.soundFadeSpeed;
            bgmManager.StartSoundFadeOut(volumeAttenuation);
        }
    }

    /// <summary>
    /// フェードアウトが完了したら真を返す
    /// </summary>
    /// <returns></returns>
    private bool FinishFadeOutDirecting()
    {
        //// hpの表示が0出なければfalse
        //if (CheckDeath() == false) return false;
        // フェードアウトが完了してなければfalse
        if (fadeMGR.FinishFadeOut() == false) return false;
        // bgmのボリュームが0になってなければfalse
        if (bgmManager.LowerTheVolume() == true) return false;

        return true;
    }
}
