using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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
    //private SEPlayer sePlay;

    //変更の親保存用
    [SerializeField] private Transform selTf;

    //ぽいんたーが重なってるオブジェ
    //[SerializeField] private GameObject olObj;

    //角度
    [SerializeField] private float ang;

    //吸いつき範囲
    [SerializeField, Range(0, 100)] private float dist = 1.5f;
    private float portDist = 0.6f;

    //ゲームオブジェクト用
    private GameObject[] circles;

    //誘導用透明オブジェクトたち
    private GameObject[] porters;

    //前回選択してたオブジェクト(カーソル位置固定用)
    [NonSerialized] public GameObject oldOverlapObject=null;

    //ポジション移動インターバル
    [SerializeField, Range(0, 60)]
    float interval;
    float interCount;

    //魔法陣の色変更用のマテリアル
    [Serializable]
    public struct mats
    {
        public Material mat;
        public string name;
    }
    [SerializeField] private mats[] circleMats;

    //色の数
    private int changeCircleNum = 3;

    //使った魔法陣の色を使えないようにする用の配列
    bool[] usedMatNum = new bool[3];

    //オーブオブジェクトたち
    [SerializeField] GameObject[] orbs;
    int orbNum = 0;
    [SerializeField] Material resetMat;

    Text MagicColor;//魔法のいろ
    Text MagicEffect;//魔法の効果
    Text MagicType;//魔法の種類
    Text MagicPower;//魔法の威力
    Text MagicColorNum;//魔方陣の色の数
    string[] magiccolor;//色のいろ
    string[] magiceffect;
    string[] magictype;
    string[] magicpower;
    string magiccolornow;
    string magiceffectnow;//今の色の効果
    string magictypenow;//今の魔法の種類
    string magicpowernow;//今の魔法の威力

    string colorcom;
    int orbpower;
    //色の数
    public int red = 0;
    public int blue = 0;
    public int yellow = 0;
    public int light_blue = 0;
    public int green = 0;

    bool speed_downflag = false;
    int combo_plus = 0;

    OrbGage oGage;//オーブのゲージ

    ClearCheck CC;

    [SerializeField] Transform rootPuzzlesObj;
    [SerializeField] Transform[] oyas;

    public int ccMode;
    public bool isMenu = false;

    private SEManager sePlay;

    //オーブの処理
    OrbCon oc;

    //魔法の五角形五芒星
    public enum MAGIC_MODE
    {
        STAR = 0,
        PENTAGON = 1,
    }

    // Start is called before the first frame update
    void Start()
    {
        //魔方陣の色
        magiccolor = new string[] { "赤", "水", "黄", "青", "緑", "なし" };
        MagicColor = GameObject.Find("MagicColor").GetComponent<Text>();
        //魔法の効果
        magiceffect = new string[] { "コンボｎ倍", "減速", "攻撃", "攻撃", "攻撃", "なし" };
        MagicEffect = GameObject.Find("MagicEffect").GetComponent<Text>();
        //魔法の威力
        magicpower = new string[] { "小", "中", "大", "特大", "極大", "なし" };//特大・極大はポコダンを参考
        MagicPower = GameObject.Find("MagicPower").GetComponent<Text>();
        //魔法の種類
        magictype = new string[] { "炎", "水", "雷", "氷", "風", "なし" };
        MagicType = GameObject.Find("MagicType").GetComponent<Text>();
        //魔方陣の色の数
        MagicColorNum = GameObject.Find("MagicColorNum").GetComponent<Text>();

        CC = GameObject.Find("GameMana").GetComponent<ClearCheck>();

        //sePlay = GameObject.Find("SePlayer").GetComponent<SEPlayer>();

        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();

        oc = GameObject.Find("ColorOrbs").GetComponent<OrbCon>();

        oGage = GameObject.Find("GameObject").GetComponent<OrbGage>();

        tf = transform;

        oriPos = tf.position;

        RegisterCircles();

        oldOverlapObject = circles[0];

        ResetMaterialBool();

        int num = rootPuzzlesObj.transform.childCount - 1;
        oyas = new Transform[num];
        for (int i = 0; i < num; i++)
            oyas[i] = rootPuzzlesObj.GetChild(i);

        ccMode = 0;
        ResetOrbs();
    }

    GameObject circleA = null;

    void FixedUpdate()
    {

        if (!isMenu)
        {


            //Aボタン選択
            SelectCircle(oldOverlapObject);

            //L1Riで色の数を決めれる
            ChangeColorNum();

        }

        // 色を変える
        ColorChange();
    }


    public void RegisterCircles()
    {
        circles = GameObject.FindGameObjectsWithTag("My");

        porters = GameObject.FindGameObjectsWithTag("Porter");
    }

    public void SelectCircle(GameObject obj)
    {
        //されてない状態
        if (!isSelect)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                sePlay.Play("Select");//SEを鳴らす（魔方陣を選択した音）
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
                sePlay.Play("MagicChange");//SEを鳴らす（魔方陣の位置が入れ替わる）
                selB = obj;
                selA.transform.parent = selB.transform.parent;
                selB.transform.parent = selTf;
                //selA.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
                selA = null;
                selB = null;
                selTf = null;
                isSelect = false;
                Destroy(circleA);
            }
        }
    }

    public Material old;

    public int len;

    void ChangeColorMat(GameObject obj)
    {

        Renderer buf = obj.GetComponent<Renderer>();

        len = circleMats.Length;

        //選択中のオブジェクトのマテリアルゲットォオ
        old = buf.material;

        //Bボタンで色入れ替え
        if (Input.GetButtonDown("Fire2"))
        {

            int next = 0;
            for (int i = 0; i < changeCircleNum; i++)
            {
                if (old.color == circleMats[i].mat.color)
                {

                    //使える色がくるまで
                    for (int j = 1; j < changeCircleNum; j++)
                    {
                        next = i + j;
                        if (next > changeCircleNum - 1) next -= changeCircleNum;
                        if (usedMatNum[next] == true) break;
                    }

                    break;
                }
            }




            buf.material = circleMats[next].mat;
            obj.name = circleMats[next].name;

        }

    }
    public void ColorChange()
    {
        Renderer buf = circles[0].GetComponent<Renderer>();

        len = circleMats.Length;

        //選択中のオブジェクトのマテリアルゲットォオ
        old = buf.material;

        //Bボタンで色入れ替え
        if (Input.GetButtonDown("Cont_L1"))
        {
            //魔方陣の入れ替えの音を入れる

            int next = 0;
            for (int i = 0; i < changeCircleNum; i++)
            {
                if (old.color == circleMats[i].mat.color)
                {

                    //使える色がくるまで
                    for (int j = 1; j < changeCircleNum; j++)
                    {
                        next = i + j;
                        if (next > changeCircleNum) next -= changeCircleNum;
                        if (next >= 3) next = 0;
                        if (usedMatNum[next] == true) break;
                    }

                    break;
                }
            }



            foreach(GameObject o in circles)
            {
                o.GetComponent<Renderer>().material = circleMats[next].mat;
                o.name = circleMats[next].name;
            }

        }else if(Input.GetButtonDown("Cont_R1"))
        {
            int next = 0;
            for (int i = 0; i < changeCircleNum; i++)
            {
                if (old.color == circleMats[i].mat.color)
                {

                    //使える色がくるまで
                    for (int j = 1; j < changeCircleNum; j++)
                    {
                        next = i - j;
                        if (next > changeCircleNum) next -= changeCircleNum;
                        if (next <= -1) next = 2;
                        if (usedMatNum[next] == true) break;
                    }

                    break;
                }
            }



            foreach (GameObject o in circles)
            {
                o.GetComponent<Renderer>().material = circleMats[next].mat;
                o.name = circleMats[next].name;
            }
        }
    }
    public void RandomColorSet()
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("My");

        foreach (GameObject o in circles)
        {
            int ran = UnityEngine.Random.Range(0, changeCircleNum);

            //for (int i = 1; i < 5; i++)
            //{
            //    int num = ran + i;
            //    if (num > 4) num -= 5;

            //    if (usedMatNum[num])
            //    {
            //        o.GetComponent<Renderer>().material = circleMats[num].mat;
            //        o.name = circleMats[num].name;
            //    }
            //}

            o.GetComponent<Renderer>().material = circleMats[ran].mat;
            o.name = circleMats[ran].name;
        }
    }

    //使った色を使えなくする
    public void BreakColor()
    {
        //GameObject[] circles = GameObject.FindGameObjectsWithTag("My");

        //foreach (GameObject o in circles) {
        //    for(int i = 0;i<circleMats.Length;i++) {
        //        if (o.GetComponent<Renderer>().material.color == circleMats[i].mat.color) {
        //            usedMatNum[i] = false;
        //        }
        //    }
        //}
    }

    //マテリアルブールをリセット
    public void ResetMaterialBool()
    {
        for (int i = 0; i < 3; i++)
            usedMatNum[i] = true;
    }

    //1色で統一してマッチさせた時に右のオーブに色をやどらせる
    public void CheckOneColor(int type)
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("My");

        Color firstColor = circles[0].GetComponent<Renderer>().material.color;

        //全部同じ色か判定
        foreach (GameObject o in circles)
        {
            if (firstColor != o.GetComponent<Renderer>().material.color) return;
        }

        for (int i = 0; i < circleMats.Length; i++)
        {

            if (firstColor == circleMats[i].mat.color)
            {
                //orbs[orbNum].GetComponent<Renderer>().material = circleMats[i].mat;

                //orbs[orbNum].transform.GetChild(type).gameObject.SetActive(true);
                //oc.CreateOrb(circleMats[i].mat, type);//OrbConのcreateオーブ
                oGage.ChargeOrb(type);

                orbNum++;
                if (orbpower < 5) orbpower++;
                //if (orbNum > orbs.Length - 1) orbNum = 0;
                break;
            }
        }

        //colorcombinow[0] = colorcombi[1];


    }

    //オーブの色をリセット
    public void ResetOrbs()
    {

        orbNum = 0;
        orbpower = 0;
        colorcom = null;

        magicpowernow = null;
        magiccolornow = null;
        magictypenow = null;
        magiceffectnow = null;
        red = 0;
        blue = 0;
        yellow = 0;
        light_blue = 0;
        green = 0; 
        speed_downflag = false;
        MagicColor.text = "魔法のいろ：\n　赤：０\n　青：０\n　黄：０\n　水：０\n　緑：０";
        MagicEffect.text = "魔法の効果：\n"
            + "　コンボ加算：オフ\n"
            + "　減速：オフ";
        MagicType.text = "魔法の種類：\nなし";
        MagicPower.text = "魔法の威力：\nなし";
        //for (int i = 0; i < orbs.Length; i++)
        //{
        //    orbs[i].GetComponent<Renderer>().material = resetMat;
        //    orbs[i].transform.GetChild(0).gameObject.SetActive(false);
        //    orbs[i].transform.GetChild(1).gameObject.SetActive(false);
        //}
        oc.Orb_Clear();

    }

    //魔法のテキスト化
    public void MagicText()
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("My");

        Color firstColor = circles[0].GetComponent<Renderer>().material.color;
        string clearColor = null;
        //int mc = 0;
        bool colorflag = true;
        speed_downflag = false;
        foreach (GameObject o in circles)
        {
            if (firstColor != o.GetComponent<Renderer>().material.color) colorflag = false;
        }
        if (colorflag == true)
        {
            //オーブの色がどれだけあるか
            for (int i = 0; i < circleMats.Length; i++)
            {

                if (firstColor == circleMats[i].mat.color)
                {
                    colorcom += magiccolor[i];
                    magiccolornow += magiccolor[i];
                    magictypenow += magictype[i];
                    magiceffectnow += magiceffect[i];
                    clearColor = magiccolor[i];
                }

            }
            if (clearColor == "赤")
            {
                oGage.colorflag = 1;
            }
            if(clearColor == "水")
            {
                oGage.colorflag = 2;
            }
            if(clearColor == "黄")
            {
                oGage.colorflag = 3;
            }
            int colorcomnum = magiccolornow.Length;
            if (colorcomnum > 5)
            {
                magiccolornow = magiccolornow.Remove(0, 1);
            }
            red = CountChar(magiccolornow, '赤');

            blue = CountChar(magiccolornow, '青');
            yellow = CountChar(magiccolornow, '黄');
            light_blue = CountChar(magiccolornow, '水');
            green = CountChar(magiccolornow, '緑');
            //red = CountChar(colorcom, '赤');
            //blue = CountChar(colorcom, '青');
            //yellow = CountChar(colorcom, '黄');
            //light_blue = CountChar(colorcom, '水');
            //green = CountChar(colorcom, '緑');

            int magiceffectnum = magiceffectnow.Length;
            int acount = CountChar(magiceffectnow, '攻');
            int ccount = CountChar(magiceffectnow, 'コ');
            int hcount = CountChar(magiceffectnow, '回');
            int dcount = CountChar(magiceffectnow, '減');
            int flame_count = CountChar(magictypenow, '炎');
            int water_count = CountChar(magictypenow, '水');
            int thunder_count = CountChar(magictypenow, '雷');
            int ice_count = CountChar(magictypenow, '氷');
            int wind_count = CountChar(magictypenow, '風');
            //int[] colorCount = null;


            if (ccount > 1 /*&& magiceffectnum < 7*/)
            {
                magiceffectnow = magiceffectnow.Replace("コンボｎ倍", "");
                magiceffectnow += "コンボｎ倍";
            }
            if (acount > 1 && magiceffectnum < 7)
            {
                magiceffectnow = magiceffectnow.Replace("攻撃", "");
                magiceffectnow += "攻撃";
            }
            if (hcount > 1 && magiceffectnum < 7)
            {
                magiceffectnow = magiceffectnow.Replace("回復", "");
                magiceffectnow += "回復";
            }
            if (dcount > 1 && magiceffectnum < 7)
            {
                magiceffectnow = magiceffectnow.Replace("減速", "");
                magiceffectnow += "減速";
            }
            if ((acount > 1 || hcount > 1 || dcount > 1) && magiceffectnum > 6)
            {
                magiceffectnow = magiceffectnow.Remove(6, 2);
            }
            if (flame_count > 1)
            {
                magictypenow = magictypenow.Replace("炎", "");
                magictypenow += "炎";
            }
            if (water_count > 1)
            {
                magictypenow = magictypenow.Replace("水", "");
                magictypenow += "水";
            }
            if (thunder_count > 1)
            {
                magictypenow = magictypenow.Replace("雷", "");
                magictypenow += "雷";
            }
            if (ice_count > 1)
            {
                magictypenow = magictypenow.Replace("氷", "");
                magictypenow += "氷";
            }
            if (wind_count > 1)
            {
                magictypenow = magictypenow.Replace("風", "");
                magictypenow += "風";
            }
            //mc++;
        }
        //オーブ溜まり具合かまたはコンボによって魔法の威力を上げる

        if (orbpower > 4 && CC.comboNum > 14)
        {
            magicpowernow = magicpower[4];
        }
        else if (orbpower > 3 && CC.comboNum > 9)
        {
            magicpowernow = magicpower[3];
        }
        else if (orbpower > 4 || CC.comboNum > 6)
        {
            magicpowernow = magicpower[2];
        }
        else if (orbpower > 2 || CC.comboNum > 4)
        {
            magicpowernow = magicpower[1];
        }
        else if (orbpower > 0 || CC.comboNum > 0)
        {
            magicpowernow = magicpower[0];
        }
        if(blue > 0)
        {
            speed_downflag = true;
        }
        //オーブが溜まった後
        MagicColor.text = "魔法のいろ：\n"
            + "　赤：" + red.ToString() + "\n"
            + "　青：" + blue.ToString() + "\n"
            + "　黄：" + yellow.ToString() + "\n"
            + "　水：" + light_blue.ToString() + "\n"
            + "　緑：" + green.ToString();
        MagicEffect.text = "魔法の効果：\n"
            + "　コンボ加算：";
        if (red > 0)
        {
            MagicEffect.text += red.ToString() + "倍\n　減速：";
        }
        else
        {
            MagicEffect.text += "オフ\n　減速：";
        }
        if (speed_downflag == true)
        {
            MagicEffect.text += "オン" + "\n";
        }
        else
        {
            MagicEffect.text += "オフ" + "\n";
        }
        MagicType.text = "魔法の種類：\n　" + magictypenow;
        MagicPower.text = "魔法の威力：\n　" + magicpowernow;

    }
    // 文字の出現回数をカウント
    public static int CountChar(string s, char c)
    {
        return s.Length - s.Replace(c.ToString(), "").Length;
    }

    //L1R1ボタンで色の数を変更する
    void ChangeColorNum() {

        int oldNum = changeCircleNum;

        if (Input.GetButtonDown("Cont_L1")) {
            sePlay.Play("MagicAreaSelect"); //色を入れ替える時の音
            changeCircleNum--;
            if (changeCircleNum < 2) changeCircleNum = 2;

        }

        if (Input.GetButtonDown("Cont_R1")) {
            sePlay.Play("MagicAreaSelect");//色を入れ替える時の音
            changeCircleNum++;
            if (changeCircleNum > 5) changeCircleNum = 5;
        }
        CC.changeColorLine = true;
        //if (oldNum != changeCircleNum)
        //    RandomColorSet();
    }

    public int GetChangeColorMode() {
        return ccMode;
    }

    public void SetChangeColorMode(int num)
    {
        ccMode = num;
    }

    public float Get_Interval()
    {
        return interval;
    }

    public void Set_Interval(float num)
    {
        interval = num;
    }
}
