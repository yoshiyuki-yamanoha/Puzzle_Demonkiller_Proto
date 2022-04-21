using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class ManageBarricade : TrunManager
{
    GameObject barriObj;
    int barriHp;

    TrunManager turnMGR;

    void Start()
    {
        barriObj = this.gameObject;
        barriHp = core.max_hp;

        turnMGR = GameObject.Find("TrunManager").gameObject.GetComponent<TrunManager>();
    }


    void FixedUpdate()
    {

        TrunPhase currentPhase = turnMGR.GetTrunPhase();

        if (currentPhase == TrunPhase.Enemy)
        {
            CheckHP();
        }
    }

    public void ReceiveDamage()
    {
        barriHp -= EAP.knock;

        Debug.Log("bHp" + barriHp);

        CheckHP();

    }

    public void CheckHP()
    {
        // ｈｐが零になっていなるか確認
        if (barriHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
