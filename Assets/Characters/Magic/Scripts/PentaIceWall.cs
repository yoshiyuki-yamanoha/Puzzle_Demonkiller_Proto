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
        Is_once = true;//1フラグ
        tm = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        LifeTrun = 3; //体力
    }

    private void FixedUpdate()
    {
        if (tm.GetTrunPhase() == TrunManager.TrunPhase.Enemy && Is_once)//敵のターン　1回フラグ
        {
            afterOneTurn = true;
            Is_once = false;
        }
        else if (tm.GetTrunPhase() == TrunManager.TrunPhase.MagicAttack)//魔法攻撃のターン
        {
            if (!Is_once)//1ターンがfalse
            {
                Is_once = true;//1ターンがtrue
            }
        }

        if (afterOneTurn)
        {
            //Debug.Log("通ったよ");
            //Debug.Log("僕は消えました。");
            Debug.Log("START" + LifeTrun);
            LifeTrun--;//ライフを減らす
            if (LifeTrun <= 0)
            {
                DestroyIce();
            }

            afterOneTurn = false;
        }

        if (breakanim) //消えるアニメーション
        {
            //Debug.Log("氷を消すゾーイ");
            //Material material = this.gameObject.GetComponent<Renderer>().material;
            //if (material.HasProperty("_Destruction"))
            //{
            //    float Des = material.GetFloat("_Destruction");
            //    Des += breakSpeed * Time.deltaTime;
            //    if (Des > 1.0f) { Des = 1.0f; }

            //    material.SetFloat("_Destruction", Des);
            //    Debug.Log(Des);
            //}
            Destroy(transform.parent.gameObject);
        }
    }

    public void DestroyIce()
    {
        breakanim = true;
        gameObject.transform.root.GetComponent<EnemyBase>().Ice_instance_flg = false;//アイスが解け終わったら　生成できるようにする
        gameObject.transform.root.GetComponent<EnemyBase>().Ice_del_flg = false;
    }
}
