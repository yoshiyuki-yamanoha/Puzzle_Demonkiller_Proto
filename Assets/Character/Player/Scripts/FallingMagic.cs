using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingMagic : MagicBas
{

    [SerializeField] public GameObject fx_fallingMagic = null;
    [SerializeField] public GameObject enemy = null;

    // 後々消したいなぁ(そういう書き方しろ)
    void Start()
    {

    }

    // 後々消したいなぁ(そういう書き方しろ)
    void FixedUpdate()
    {

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    M_FireFall(this.gameObject);
        //}
    }

    public void M_FireFall(GameObject ene)
    {
        Vector3 heightToDropMagic = new Vector3(0.0f, 20.0f, 0.0f);
        //Vector3 overTheEnemy = ene.transform.position + heightToDropMagic;
        Vector3 overTheEnemy = new Vector3(0, 0, 50.0f) + heightToDropMagic;
        //Debug.Log(ene.transform.position);

        GameObject obj = Instantiate(fx_fallingMagic, overTheEnemy, this.transform.rotation);
        obj.GetComponent<Meteor>().targetEnemy = ene;
    }
}
