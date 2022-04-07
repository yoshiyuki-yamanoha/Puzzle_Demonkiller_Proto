using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploDamage : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        //一定範囲内のエネミーを検索
        GameObject[] enes = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enes)
        {
            if (Vector3.Distance(transform.position, e.transform.position) <= transform.localScale.x * 2.0f)
                e.GetComponent<EnemyBase>().Damage(2);
        }

        
    }

}
