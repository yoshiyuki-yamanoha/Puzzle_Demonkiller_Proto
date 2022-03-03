using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_Con2 : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]private GameObject FirstSelect;
    float hori, vert;
    [SerializeField]private float ang;
    [SerializeField] private GameObject Select_Circle;
    [SerializeField]private GameObject SELECTER_OBJ;
    [SerializeField] private GameObject Selecter;


    //SE
    private SEPlayer sePlay;
    void Start()
    {
        Selecter_Initialeze();

        sePlay = GameObject.Find("SePlayer").GetComponent<SEPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        Get_Stick();
        PointMove();
        PointerSelect();
        Cange_Select();

        SeleCter_Visible();
    }

    void Get_Stick()
    {
        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");


        if(Mathf.Abs(hori) <= 0.2f && Mathf.Abs(vert) <= 0.2f)
        {
            ang = -1.0f;
        }
        else
        {
            //スティックの角度を求める
            ang = (Mathf.Atan2(vert, hori) * 180 / Mathf.PI) - 90;
            if (ang < 0) ang = 360.0f + ang;
        }
    }
    void PointMove()
    {

        this.transform.localPosition = new Vector3((hori * 2f), (vert * 2f) + 0.4f, 0);
    }

    void SeleCter_Visible()
    {
        if(Select_Circle == null)
        {
            Selecter.SetActive(false);
        }
        else
        {
            Selecter.SetActive(true);
            Selecter.transform.position = Select_Circle.transform.position;
        }
    }

    void PointerSelect()
    {
        if(ang == -1.0f)       //スティック入力されていなかったら
        {
            return;
        }

        if(ang == 180.0f)      //スティックが真下の時
        {
            //右上か左上が選択されていたらその下を選択するように
            if(Select_Circle.name == "Four")
            {
                Select_Circle = GameObject.Find("Six");
                Debug.Log("補正");

                return;
            }
            else if(Select_Circle.name == "Six" || Select_Circle.name == "Seven")
            {
                //下の魔法陣をカーソル中は真下の反応を消す
                return;
            }
        }

        if (ang >= 350.0f || ang <= 10)      //上の魔法陣を選択
        {
            Select_Circle = GameObject.Find("Two");
        }
        else if (ang >= 265.0f)              //右上の魔法陣を選択
        {
            Select_Circle = GameObject.Find("Four");
        }
        else if (ang > 180.0f)               //右下の魔法陣を選択
        {
            Select_Circle = GameObject.Find("Six");
        }
        else if (ang >= 95.0f)               //左下の魔法陣を選択
        {
            Select_Circle = GameObject.Find("Seven");
        }
        else                                 //左上の魔法陣を選択
        {
            Select_Circle = GameObject.Find("Eight");
        }

    }

    void Cange_Select()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
        {
            if(FirstSelect == null)           //初めに選択されたら
            {
                sePlay.Play("DECIDE");//SEを鳴らす（魔方陣を選択した音）
                FirstSelect = Select_Circle;
            }
            else           //2回目の選択なら
            {
                if(FirstSelect == Select_Circle)    //２回目の選択と同じ魔法陣なら
                {
                    //何もしない
                    FirstSelect = null;
                }
                else                            //２回目の選択と違う魔法陣なら
                {
                    //魔法陣を入れ替える
                    FirstSelect.transform.GetChild(0).parent = Select_Circle.transform;
                    Select_Circle.transform.GetChild(0).parent = FirstSelect.transform;

                    sePlay.Play("SWITCH");//SEを鳴らす（魔方陣の位置が入れ替わる）

                    //入れ替えた後に初期化
                    FirstSelect = null;
                }
            }
        }
    }

    void Selecter_Initialeze()
    {
        Selecter = Instantiate(SELECTER_OBJ);
        Selecter.transform.parent = GameObject.Find("Puzzles").transform;
        Selecter.transform.localPosition = Vector3.zero;
        Selecter.SetActive(false);
    }
}
