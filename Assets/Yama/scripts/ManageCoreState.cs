using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoreBase;

public class ManageCoreState : TrunManager
{
    TrunManager turnMGR;

    // コア
    public Core core = new Core();

    // スライダー
    [SerializeField] Slider slider = null;
    HPBarSystemsSc hpGage = new HPBarSystemsSc();


    // フェイドマネージャー
    FadeOut fadeMGR;
    bool isCoreDeathAnim;
    float waitTime;

    SEManager sePlay;
    int CExpCount = 0;

    BGMManager bgmManager;


    void Start()
    {
        turnMGR = GameObject.Find("TrunManager").gameObject.GetComponent<TrunManager>();

        core.obj = this.gameObject;
        core.hp = core.max_hp;

        slider = GameObject.Find("CoreHPCanvas/Slider").gameObject.GetComponent<Slider>();
        slider.maxValue = core.max_hp;
        slider.value = slider.maxValue;
        hpGage = GameObject.Find("HpGaugeCanvas 1").gameObject.GetComponent<HPBarSystemsSc>();

        sePlay = GameObject.Find("Audio").GetComponent<SEManager>(); //SE

        fadeMGR = GameObject.Find("FadeCanvas/FadeImage").gameObject.GetComponent<FadeOut>();
        isCoreDeathAnim = false;
        waitTime = 3.0f;

        bgmManager = GameObject.Find("BGMAudio").gameObject.GetComponent<BGMManager>();
    }


    void FixedUpdate()
    {

        TrunPhase currentPhase = turnMGR.GetTrunPhase();
        //if (currentPhase == TrunPhase.Enemy)
        //if (core.hp <= 0)
        if (CheckDeath() == true && isCoreDeathAnim == false)
            StartDeathAnimDirecting();

        if (isCoreDeathAnim == true)
        {
            if (FinishFadeOutDirecting() == true)
            {
                // ゲームオーバーシーンへ遷移する関数
                GameMgr.Instance.GotoGameOverScene();
            }
        }
    }

    public void ReceiveDamage(int dmgtype)// = EAP.knock)
    {
        core.hp -= dmgtype;
        if (core.hp <= 0)
        {
            core.hp = 0;
            //StartDeathAnimDirecting();
        }
    }

    /// <summary>
    /// 表示中の残ｈｐを確認
    /// </summary>
    /// <returns>hpの表示が０なら真を返す</returns>
    public bool CheckDeath()
    {
        int nowHp = hpGage.GetHPOnDisplay();
        // ｈｐが零になっていなるか確認
        if (nowHp <= 0 && core.hp <= 0)
            return true;

        return false;
    }

    private void StartDeathAnimDirecting()
    {
        if (waitTime > 0)
            waitTime -= Time.deltaTime;
        else
        {
            CExpCount++;
            if (CExpCount == 1) sePlay.Play("CoreExplosion");

            isCoreDeathAnim = true;

            // 画面フェードアウトの開始
            fadeMGR.SetFadeMode(FadeOut.FadeMode.FADE_OUT);
            fadeMGR.SetFadeSpeed(0.4f);
            fadeMGR.SetColor_Black();
            // 音量フェードアウトの開始
            float bgmVolume = bgmManager.GetBGMVolume();
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
        // bgmのボリュームが0になってなければtrue
        if (bgmManager.LowerTheVolume() == true) return false;

        return true;
    }


}