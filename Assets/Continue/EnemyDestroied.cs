using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroied : MonoBehaviour
{
    GeneralEnemy ge;

    private void Start()
    {
        ge = GameObject.Find("Sponer").GetComponent<GeneralEnemy>();
    }

    void OnTriggerEnter(Collider other)
    {
        //Enemyタグに当たったらコンボに応じて爆発のEffectを表示
        if (other.gameObject.tag == "MarkedEnemy")
        {
            ge = GameObject.Find("Sponer").GetComponent<GeneralEnemy>();
            Destroy(other.gameObject);
            ge.initflg = true;
        }
    }
}
