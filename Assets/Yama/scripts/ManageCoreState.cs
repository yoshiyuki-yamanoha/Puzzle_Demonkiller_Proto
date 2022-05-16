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
    // フェイドマネージャー
    FadeOut fadeMGR;
    bool isCoreDeathAnim;

    SEManager sePlay;
    int CExpCount=0;

    void Start()
    {
        turnMGR = GameObject.Find("TrunManager").gameObject.GetComponent<TrunManager>();

        core.obj = this.gameObject;
        core.hp = core.max_hp;

        slider = GameObject.Find("CoreHPCanvas/Slider").gameObject.GetComponent<Slider>();
        slider.maxValue = core.max_hp;
        slider.value = slider.maxValue;

        sePlay = GameObject.Find("Audio").GetComponent<SEManager>(); //SE

        fadeMGR = GameObject.Find("FadeCanvas/FadeImage").gameObject.GetComponent<FadeOut>();
        isCoreDeathAnim = false;
    }


    void FixedUpdate()
    {

        TrunPhase currentPhase = turnMGR.GetTrunPhase();

        if (currentPhase == TrunPhase.Enemy)
        {
            CheckHP();
        }

        if(isCoreDeathAnim == true)
        {

            CheckFadeOut();
            
        }
    }

    public void ReceiveDamage(int dmgtype)// = EAP.knock)
    {
        core.hp -= dmgtype;
        if (core.hp < 0)
            core.hp = 0;

        CheckHP();
    }

    public void CheckHP()
    {
        // ｈｐが零になっていなるか確認
        if (core.hp < slider.value
            && slider.value > 0)
            slider.value--;

        if (slider.value <= 0)
        {
            CExpCount++;
            if (CExpCount==1)sePlay.Play("CoreExplosion");
            Debug.Log(CExpCount);

            isCoreDeathAnim = true;
            fadeMGR.SetFadeMode(FadeOut.FadeMode.FADE_OUT);
            fadeMGR.SetColor_Black();
        }
    }

    public void CheckFadeOut()
    {
        const float fadeOut = 1.0f;

        float fadeAlpha = fadeMGR.GetAlpha();
        if (fadeAlpha >= fadeOut)
        {
            // ゲームオーバーシーンへ遷移する関数
            GameMgr.Instance.GotoGameOverScene();

        }
    }
}
