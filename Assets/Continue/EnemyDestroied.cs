//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyDestroied : MonoBehaviour
//{
//    GenerationEnemy ge;
//    UltManager ultmanager;

//    private void Start()
//    {
//        ultmanager = GameObject.Find("GameObject").GetComponent<UltManager>();
//        ge = GameObject.Find("Sponer").GetComponent<GenerationEnemy>();
//    }

//    private void FixedUpdate()
//    {
//        Debug.Log("使われてます。");
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        //Enemyタグに当たったらコンボに応じて爆発のEffectを表示
//        if (other.gameObject.tag == "MarkedEnemy")
//        {
//            ultmanager = GameObject.Find("GameObject").GetComponent<UltManager>();
//            ultmanager.ultChage();
//            ge = GameObject.Find("Sponer").GetComponent<GenerationEnemy>();
//            Destroy(other.gameObject);
//            ge.initflg = true;
//        }
//    }
//}
