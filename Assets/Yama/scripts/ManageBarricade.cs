using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreBase;

public class ManageBarricade : TrunManager
{
    TrunManager turnMGR;

    Barricade_Class barri = new Barricade_Class();

    void Start()
    {
        barri.obj= this.gameObject;
        barri.hp = barri.max_hp;

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
        barri.hp -= EAP.knock;

        CheckHP();

    }

    public void CheckHP()
    {
        // ｈｐが零になっていなるか確認
        if (barri.hp<= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
