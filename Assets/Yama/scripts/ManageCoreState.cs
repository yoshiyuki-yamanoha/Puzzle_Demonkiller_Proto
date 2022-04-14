using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class ManageCoreState : TrunManager
{
    GameObject coreObj;
    int coreHp;

    TrunManager turnMGR;

    void Start()
    {
        coreObj = this.gameObject;
        coreHp = core.max_hp;

        turnMGR = GameObject.Find("TrunManager").gameObject.GetComponent<TrunManager>();
    }

    
    //void Update()
    //{

    //    TrunPhase currentPhase = turnMGR.GetTrunPhase();
    //    TrunPhase currentPhase = turnMGR.GetTrunPhase();

    //    if (currentPhase == TrunPhase.Enemy)
    //    {

    //    }
    //}

    public void ReceiveDamage()
    {
        coreHp -= EAP.zako;

        Debug.Log("cHp"+coreHp);

        CheckHP();

    }

    public void CheckHP()
    {
        // ｈｐが零になっていなるか確認
        if (coreHp <= 0)
        {
            // ゲームオーバーシーンへ遷移する関数
            GameMgr.Instance.GotoGameOverScene();
        }
    }
}
