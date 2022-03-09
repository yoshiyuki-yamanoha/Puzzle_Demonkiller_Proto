using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointControl : MonoBehaviour
{

    Transform tf;
    [SerializeField] private float power;

    [SerializeField] private GameObject selectCircle;
    [SerializeField] GameObject selectCircle2;

    //元の位置
    private Vector3 oriPos;

    //選択用
    [SerializeField] private Transform c_Select;
    [SerializeField] private GameObject selA;
    [SerializeField] private GameObject selB;
    private bool isSelect = false;

    //SE
    private SEPlayer sePlay;

    //変更の親保存用
    [SerializeField] private Transform selTf;

    //ぽいんたーが重なってるオブジェ
    //[SerializeField] private GameObject olObj;

    //角度
    [SerializeField] private float ang;

    //吸いつき範囲
    [SerializeField, Range(0, 100)] private float dist = 1.5f;

    //ゲームオブジェクト用
    [SerializeField]private GameObject[] circles;
    private int num;

    //前回選択してたオブジェクト(カーソル位置固定用)
    GameObject oldOverlapObject;

    // Start is called before the first frame update
    void Start()
    {
        //sePlay = GameObject.Find("SePlayer").GetComponent<SEPlayer>();

        tf = transform;

        oriPos = tf.position;

        RegisterCircles();

        oldOverlapObject = circles[0];
    }

    GameObject circleA = null;

    void Update()
    {

        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 ppos = oriPos + new Vector3(hori * power,vert * power,0);

        //tf.position = ppos;
        if(oldOverlapObject)
            tf.position = oldOverlapObject.transform.position;

        //デバッグ用
        Debug.DrawLine(tf.position, ppos);

        //スティックの角度を求める
        ang = Mathf.Atan2(vert, hori) * 180 / Mathf.PI;
        if (ang < 0) ang = 360.0f + ang;

        //吸いつき
        {

            //ポインターが一定以上の範囲に出た時
            //if (Vector3.Distance(Vector3.zero, tf.position) > 0.3f)
            {

                //当たり判定
                foreach (GameObject o in circles)
                {
                    GoToParent gp = o.GetComponent<GoToParent>();

                    float per = 0.1f;
                    Vector3 currentPerPos;
                    while (per < 1.0f)
                    {
                        currentPerPos = Vector3.Lerp(oriPos, ppos, per);

                        if (Vector3.Distance(currentPerPos, o.transform.position) < dist && oldOverlapObject != o)
                        {
                            //最近選択していたオブジェクト
                            oldOverlapObject = o;

                            //選択した親オブジェクトの位置にいく
                            oriPos = o.transform.position;

                            break;
                        }

                        per += 0.1f;
                    }

                    //魔法陣の中心からdist分の範囲内に入ったら
                    if (Vector3.Distance(tf.position, o.transform.position) < dist)
                    {

                        //選択サークルを出させる
                        gp.ShowSelectCircle(selectCircle);

                        //Lボタン選択
                        SelectCircle(o);
                        if (Input.GetButtonDown("Fire5"))
                        {
                            o.GetComponent<CycleRotate>().RotateCycleL();    // 選択されている魔法陣を回す
                        }else if(Input.GetButtonDown("Fire4"))
                        {
                            o.GetComponent<CycleRotate>().RotateCycleR();    // 選択されている魔法陣を回す
                        }

                    }
                    else
                    {  //入って無ければ
                        gp.FadeSelectCircle();
                    }

                }
            }

            
        }

    }


    public void RegisterCircles() {
        circles = GameObject.FindGameObjectsWithTag("My");
        num = circles.Length;
    }

    public void SelectCircle(GameObject obj) {
        //されてない状態
        if (!isSelect)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                //sePlay.Play("DECIDE");//SEを鳴らす（魔方陣を選択した音）
                selA = obj;                                   //選択したオブジェ保存
                selTf = selA.transform.parent;              //1個目の親オブジェ
                                                            //selA.transform.parent = c_Select;           //選択位置に移動

                isSelect = true;                            //選択フラグを立てる
                circleA = Instantiate(selectCircle2, selA.transform.position, Quaternion.identity);

            }
        }
        else //されてる状態
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                //sePlay.Play("SWITCH");//SEを鳴らす（魔方陣の位置が入れ替わる）
                selB = obj;
                selA.transform.parent = selB.transform.parent;
                selB.transform.parent = selTf;
                //selA.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                selA = null;
                selB = null;
                selTf = null;
                isSelect = false;
                Destroy(circleA);
            }
        }
    }

}
