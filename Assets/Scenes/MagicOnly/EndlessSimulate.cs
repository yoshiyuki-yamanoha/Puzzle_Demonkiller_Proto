using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSimulate : MonoBehaviour
{
    TrunManager s_TrunManager;

    float time = 0.5f;

    private void Start()
    {
        s_TrunManager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
    }

    void Update()
    {
        //パズルのターンになったら即魔法ターンに行く
        if (s_TrunManager.GetTrunPhase() == TrunManager.TrunPhase.Puzzle)
            s_TrunManager.SetTrunPhase(TrunManager.TrunPhase.MagicAttack);

        //0.5秒ごとに敵のHPを回復しつづける
        if (time > 0) {
            time -= Time.deltaTime;

            if (time <= 0f) {
                var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            }
        }
    }
}
