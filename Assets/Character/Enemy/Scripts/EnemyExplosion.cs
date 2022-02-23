using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MagicBas
{
    [SerializeField] public GameObject fx_explosion = null;

    // 後々消したいなぁ(そういう書き方しろ)
    void Start()
    {
        
    }

    // 後々消したいなぁ(そういう書き方しろ)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            InstantiateMagic(fx_explosion, this.transform.position, this.transform.rotation);
            EnemyDestroy();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.transform.tag == "Magic")
        //{
        Debug.Log("hit.e");

        EnemyDestroy();
        //}
    }

    private void EnemyDestroy()
    {
        //this.transform.GetComponent<MeshRenderer>().enabled = false;
        Destroy(this.gameObject);
    }
}
