using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentaIceWall : MonoBehaviour
{
    bool Is_once, afterOneTurn,breakanim;
    private int LifeTrun;
    TrunManager tm;

    [SerializeField] private float breakSpeed = 0.5f;

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
            //Debug.Log("通ったよ");
            DestroyIce();
            afterOneTurn = false;
        }
        if (breakanim)
        {
            Material material = this.gameObject.GetComponent<Renderer>().material;
            if (material.HasProperty("_Destruction"))
            {
                float Des = material.GetFloat("_Destruction");
                Des += breakSpeed * Time.deltaTime;
                if (Des > 1.0f) { Des = 1.0f; }

                material.SetFloat("_Destruction", Des);
                Debug.Log(Des);
            }
        }
    }

    public void DestroyIce()
    {
        //Debug.Log("僕は消えました。");
        Debug.Log("START" + LifeTrun);
        LifeTrun--;
        if (LifeTrun <= 0)
        {
            breakanim = true;
            gameObject.transform.root.GetComponent<EnemyBase>().Ice_instance_flg = false;
            gameObject.transform.root.GetComponent<EnemyBase>().Ice_flg = false;
        }
    }
}
