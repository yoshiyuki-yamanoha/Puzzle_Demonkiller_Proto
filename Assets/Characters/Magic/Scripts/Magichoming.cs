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

    //炎のかべ　エフェクト
    [SerializeField] GameObject firetrapEffe;

    //凍結エフェクト
    [SerializeField] GameObject iceEffe;

    //雷エフェクト
    [SerializeField] GameObject Pentagon_thunEffe;
    [SerializeField] GameObject StarthunEffe;

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

    Transform acmg;

    GameObject Enemies;

    public GameObject[] targets;

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

        acmg = GameObject.Find("ActivateMagics").transform;
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
                    if (magicType == 0)
                    {
                        Debug.Log("炎の五芒星");
                        GameObject fire;
                        ////範囲により変わる爆発
                        fire = GenerationMagic(Exp, transform.position);
                        //// レベルに応じてスケールの変更
                        //float scalX = 1f;
                        //if (magicLevel > 0)
                        //    scalX = magicLevel * 2f - 1f;

                        //fire.transform.localScale = new Vector3(scalX, scalX, scalX);
                        //炎上するやつ
                        //GameObject fire = GenerationMagic(fireEffe, transform.position);
                        //fire.GetComponent<FireMagic>().SetMagicRange(magicLevel);
                    }

                    if (magicType == 3) { 
                        GameObject magBuf = GenerationMagic(firetrapEffe, transform.position);

                        Vector3 magBufSca = magBuf.transform.localScale;

                        magBuf.transform.Rotate(new Vector3(0, 90, 0));

                        magBuf.transform.position += new Vector3(0,2.5f,0);

                        float scalZ = magicLevel * 2f - 1f;

                        magBuf.transform.localScale = new Vector3(magBufSca.x, magBufSca.y, magBufSca.z * scalZ);
                    }

                    //低下|凍結 レベルにより、速度低下率が変わる
                    if (magicType == 1)
                    {
                        //え＾～氷みたいなエフェクト
                        GameObject _magic = GenerationMagic(iceEffe, transform.position);
                        _magic.GetComponent<Star_Ice>().Create_IceBergs(TargetObject,magicLevel);
                        Destroy(gameObject);
                    }

                    //ジャンプ&スタン レベルにより、ジャンプ回数が変わる (3～7)
                    if (magicType == 2)
                    {
                        //雷エフェクト
                        GameObject _magic = GenerationMagic(StarthunEffe, transform.position);

                        ThunderSelect ts = _magic.GetComponent<ThunderSelect>();
                        ts.Set_List(targets);
                        ts.ThunderSelecter();
                    }

                    //雷のタレット
                    if (magicType == 5)
                    {
                        //雷エフェクト
                        GameObject _magic = GenerationMagic(Pentagon_thunEffe, transform.position);

                        _magic.GetComponent<Ele_tur_Attack>().Set_Init(magicLevel, TargetObject);
                    }
                }

                
                Destroy(gameObject);
            }

        }
    }

    //魔法を生成する関数
    GameObject GenerationMagic(GameObject mag, Vector3 pos,float breakTime = 99) {
        GameObject magicIns = Instantiate(mag, pos, Quaternion.identity);
        magicIns.transform.parent = acmg;

        if(breakTime != 99.0f)
            Destroy(magicIns, breakTime);

        return magicIns;
    }
}
