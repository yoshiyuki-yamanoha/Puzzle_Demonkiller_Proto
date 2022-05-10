﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentaIceWall : MonoBehaviour
{
    bool Is_once, afterOneTurn;
    int LifeTrun;
    TrunManager tm;
    private void Start()
    {
        Is_once = true;
        tm = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        LifeTrun = 3;
    }
    private void FixedUpdate()
    {
        if (tm.GetTrunPhase() == TrunManager.TrunPhase.Enemy && Is_once)
        {
            afterOneTurn = true;
            Is_once = false;
        }
        else if (tm.GetTrunPhase() == TrunManager.TrunPhase.MagicAttack)
        {
            if (!Is_once)
            {
                Is_once = true;
            }
        }

        if (afterOneTurn)
        {
            DestroyIce();
        }
    }

    public void DestroyIce()
    {
        Debug.Log("START" + LifeTrun);
        LifeTrun--;
        if (LifeTrun <= 0) Destroy(this.gameObject);
        afterOneTurn = false;
        gameObject.transform.root.GetComponent<EnemyBase>().Ice_instance_flg = false;
    }
}
