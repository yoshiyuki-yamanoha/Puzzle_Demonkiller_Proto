using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magichoming : MonoBehaviour
{
    [SerializeField] GameObject Exp;
    [SerializeField] GameObject ExpMini;
    private GameObject TargetObject;
    private Vector3 TargetPos;

    public GameObject TargetObjTest;
    private Vector3 TargetPosTest;

    public ClearCheck ClCh;
    MagicPointer MagicP;

    int combo;
    int targetno;

    float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        //TargetPosTest = TargetObjTest.transform.position;
        TargetObject = GameObject.Find("MarkingPointer1");
        TargetPos = TargetObject.transform.position;
        ClCh = GameObject.Find("GameMana").gameObject.GetComponent<ClearCheck>();
        combo = ClCh.MaxCombo;
        //targetno = ClCh.enemyno;
        TargetPos = TargetObject.transform.position;
        MagicP = GameObject.Find("Main Camera").GetComponent<MagicPointer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 Target;
        //transform.position = Vector3.Lerp(transform.position, TargetPos, speed);

        //ターゲットの方向に向く
        if (combo > 5)
        {
            //動いている敵の座標を向く
            transform.LookAt(TargetObject.transform.position);
            Target = TargetObject.transform.position;
            //Target = TargetObjTest.transform.position;
        }
        else
        {
            //呼び出されたときの敵の座標を向く
            transform.LookAt(TargetPos);
            Target = TargetPos;
            //Target = TargetPosTest;
        }

        //Zに向かって移動
        transform.position += transform.rotation * new Vector3(0, 0, 1.0f) * speed;

        //Debug.Log(Vector3.Distance(transform.position, TargetPos));
        if (Vector3.Distance(transform.position, Target) < 0.5f)
        {
            if (combo > 10)
            {
                GameObject Explo = Instantiate(Exp, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(Explo, 1.0f);
            }
            else
            {
                GameObject Explo = Instantiate(ExpMini, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(Explo, 1.0f);
            }

        }
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    //Enemyタグに当たったらコンボに応じて爆発のEffectを表示
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        if (combo > 5)
    //        {
    //            GameObject Explo = Instantiate(Exp, transform.position, Quaternion.identity);
    //            Destroy(gameObject);
    //            Destroy(Explo, 1.0f);
    //        }
    //        else
    //        {
    //            GameObject Explo = Instantiate(ExpMini, transform.position, Quaternion.identity);
    //            Destroy(gameObject);
    //            Destroy(Explo, 1.0f);
    //        }
    //    }
    //}
}
