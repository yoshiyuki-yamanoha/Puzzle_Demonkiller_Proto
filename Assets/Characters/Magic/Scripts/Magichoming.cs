using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magichoming : MonoBehaviour
{
    [SerializeField] GameObject Exp;
    [SerializeField] GameObject ExpMini;
    private GameObject TargetObject;
    private Vector3 TargetPos;



    public ClearCheck comboNow;

    int combo;

    float distance_two;

    float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        TargetObject = GameObject.Find("Pointer");
        TargetPos = TargetObject.transform.position;
        comboNow = GameObject.Find("GameMana").gameObject.GetComponent<ClearCheck>();
        combo = comboNow.comboNum;
        distance_two = Vector3.Distance(transform.position, TargetPos);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float present_Location = (Time.time * speed) / distance_two;

        //transform.position = Vector3.Lerp(transform.position, TargetPos, speed);

        transform.LookAt(TargetObject.transform);

        transform.position += transform.rotation * new Vector3(0, 0, 1.0f) * speed;

        //if (Vector3.Distance(transform.position, TargetPos) < 10.0f)
        //{
        //    if (combo > 5)
        //    {
        //        GameObject Explo = Instantiate(Exp, transform.position, Quaternion.identity);
        //        Destroy(gameObject);
        //        Destroy(Explo, 1.0f);
        //    }
        //    else
        //    {
        //        GameObject Explo = Instantiate(ExpMini, transform.position, Quaternion.identity);
        //        Destroy(gameObject);
        //        Destroy(Explo, 1.0f);
        //    }

        //}
    }
    void OnTriggerEnter(Collider other)
    {
        //Enemyタグに当たったらコンボに応じて爆発のEffectを表示
        if (other.gameObject.tag == "Enemy")
        {
            if (combo > 5)
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
}
