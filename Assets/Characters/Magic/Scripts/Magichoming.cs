using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magichoming : MonoBehaviour
{
    //爆破エフェクト
    [SerializeField] GameObject Exp;
    [SerializeField] GameObject ExpMini;

    //炎上エフェクト
    [SerializeField] GameObject fireEffe;

    //凍結エフェクト
    [SerializeField] GameObject iceEffe;

    //雷エフェクト
    [SerializeField] GameObject thunEffe;

    public GameObject TargetObject;
    private Vector3 TargetPos;

    public GameObject TargetObjTest;
    private Vector3 TargetPosTest;

    public ClearCheck ClCh;
    MagicPointer MagicP;
    PlayerController pc;

    int combo;
    public int targetno;

    float disPer = 0;
    Vector3 oriPos;

    float speed = 0.5f;

    //魔法のレベルと種類
    public int magicLevel = 1;
    public int magicType = 0;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("GameObject").GetComponent<PlayerController>();
        //TargetPosTest = TargetObjTest.transform.position;
        //TargetObject = GameObject.Find("MarkingPointer1");
        //TargetPos = TargetObject.transform.position;
        ClCh = GameObject.Find("GameMana").gameObject.GetComponent<ClearCheck>();
        combo = ClCh.MaxCombo;
        //targetno = pc.attackNum;
        //TargetPos = TargetObject.transform.position;

        oriPos = transform.position;

        //if (targetno == 0)
        //{
        //    TargetObject = GameObject.Find("MarkingPointer1");
        //    TargetObjTest = GameObject.Find("MarkingPointer1");
        //    TargetPos = TargetObject.transform.position;
        //}
        //if (targetno == 1)
        //{
        //    TargetObject = GameObject.Find("MarkingPointer2");
        //    TargetObjTest = GameObject.Find("MarkingPointer2");
        //    TargetPos = TargetObject.transform.position;
        //}
        //if (targetno == 2)
        //{
        //    TargetObject = GameObject.Find("MarkingPointer3");
        //    TargetObjTest = GameObject.Find("MarkingPointer3");
        //    TargetPos = TargetObject.transform.position;
        //}
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
        //transform.position += transform.rotation * new Vector3(0, 0, 1.0f) * speed;

        //距離を縮める
        disPer += 0.1f;
        if(TargetObject)
            transform.position = Vector3.Lerp(oriPos, TargetObject.transform.position, disPer);

        //魔法が目標地点に到達した瞬間
        if (disPer >= 1.0f)
        {
            if (combo > 10)
            {
                GameObject Explo = Instantiate(Exp, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(Explo, 1.0f);
            }
            else
            {

                //発生する追加効果の種類別の処理
                {
                    //炎上 レベルにより範囲が変わる
                    if (magicType == 0) {
                        //範囲により変わる爆発
                        GenerationMagic(ExpMini, transform.position, 1.0f);
                        //炎上するやつ
                        GenerationMagic(fireEffe, transform.position, 999.0f);
                    }

                    //低下|凍結 レベルにより、速度低下率が変わる
                    if (magicType == 1) {
                        //え＾～氷みたいなエフェクト
                        GenerationMagic(iceEffe, transform.position, 1.0f);
                    }

                    //ジャンプ&スタン レベルにより、ジャンプ回数が変わる (3～7)
                    if (magicType == 2) {
                        //雷エフェクト
                        GenerationMagic(thunEffe, transform.position, 1.0f);
                    }
                }

                
                Destroy(gameObject);
            }

        }
    }

    //魔法を生成する関数
    void GenerationMagic(GameObject mag, Vector3 pos,float breakTime = 99) {
        GameObject magicIns = Instantiate(mag, pos, Quaternion.identity);
        Destroy(magicIns, breakTime);
    }
}
