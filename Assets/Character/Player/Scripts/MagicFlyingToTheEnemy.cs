using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlyingToTheEnemy : MagicBas
{

    public GameObject fx_flyingMagic = null;
    public GameObject spherePos = null;


    // 後々消したいなぁ(そういう書き方しろ)
    void Start()
    {
        
    }

    // 後々消したいなぁ(そういう書き方しろ)
    void FixedUpdate()
    {
        //if(Input.GetMouseButtonDown(0))
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    M_FireForward(this.gameObject);
        //}
    }

    private void MaterializeMagic()
    {

    }

    public void M_FireForward(GameObject enemy)
    {
        Vector3 heightToDropMagic = new Vector3(2.0f, 5.0f, 0.0f);
        Vector3 overTheplayer = spherePos.transform.position + heightToDropMagic;

        GameObject Obj = Instantiate(fx_flyingMagic, overTheplayer, Quaternion.identity);
        Obj.GetComponent<FlyTowardsTheEnemy>().targetEnemy = enemy;
    }
}
