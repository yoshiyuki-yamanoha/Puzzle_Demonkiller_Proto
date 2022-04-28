using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoreBase;

public class ManageCoreState : TrunManager
{
    TrunManager turnMGR;

    // コア
    Core core = new Core();


    // スライダー
    [SerializeField] Slider slider = null;

    void Start()
    {
        turnMGR = GameObject.Find("TrunManager").gameObject.GetComponent<TrunManager>();

        core.obj = this.gameObject;
        core.hp = core.max_hp;

        slider = GameObject.Find("CoreHPCanvas/Slider").gameObject.GetComponent<Slider>();
        slider.maxValue = core.max_hp;
        slider.value = slider.maxValue;
    }


    void FixedUpdate()
    {

        TrunPhase currentPhase = turnMGR.GetTrunPhase();

        if (currentPhase == TrunPhase.Enemy)
        {
            CheckHP();
        }
    }

    public void ReceiveDamage(int dmgtype = EAP.knock)
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
            // ゲームオーバーシーンへ遷移する関数
            GameMgr.Instance.GotoGameOverScene();
        }
    }
}
