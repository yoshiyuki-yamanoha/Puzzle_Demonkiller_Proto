using UnityEngine;
using UnityEngine.UI;

public class ClearCheck : MonoBehaviour
{
    [SerializeField] private Transform[] play;  //親オブジェクト


    [SerializeField] GameObject clearEffe;
    [SerializeField] Transform effePos;

    [SerializeField] private AudioClip se;
    [SerializeField] private AudioSource ass;

    [SerializeField] GameObject[] playObjs;     //子オブジェクト

    [SerializeField] int[] soe = new int[5] { 2, 3, 0, 4, 1 };

    //魔力 ←あとで増やす
    public float magicPoint;
    [SerializeField] Text mpText;

    //クリア判定フラグ
    bool cleared;

    bool attack;
    public int MaxCombo;
    //public int enemyno;
    //public GameObject MarkPoint1 = GameObject.Find("MarkingPointer1");
    //public GameObject MarkPoint2 = GameObject.Find("MarkingPointer2");
    //public GameObject MarkPoint3 = GameObject.Find("MarkingPointer3");
    //public GameObject[] enemy;
    //シャッフルカウント
    float shuffleInterval = 30;
    float shuffleCount;

    //プレイヤーコントローラー
    PlayerController pc;
    Magichoming Mh;

    [SerializeField] Slider sld;
    [SerializeField] Text comboTex;

    //コンボタイム用へ3ん数
    float comboTime = 90;
    float nowComboTime = 90;
    public int comboNum = 0;
    
    private void Start()
    {
        attack = false;
        DrawLine();
        pc = GameObject.Find("GameObject").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!cleared)
        {
            //線が被らなければクリア (グルっと一周)
            if (CheckClear(1)) ClearReward(1);
            else if (CheckClear(-1)) ClearReward(1);

            //星の形になってればクリア(五芒星)
            if (CheckClear(2)) ClearReward(99);
            else if (CheckClear(-2)) ClearReward(2);



            //全ての線が後ろの線に重なってればクリア(理想かも)
        }

        if (shuffleCount > 1) shuffleCount--;
        if (shuffleCount == 1)
        {

            shuffleCount = 0;
            Shuffle();

            //線の色を戻す
        }

        //コンボタイムを減らしていく
        if (nowComboTime > 0) {
            nowComboTime--;
        }

        //コンボタイムが0になったらコンボ数を0に
        if (nowComboTime == 0) {
            MaxCombo = comboNum;
            comboNum = 0;
            nowComboTime = 0;
        }

        if (attack == true && nowComboTime == 0)
        {
            //int c= 0;
            //{
            //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //    foreach (GameObject o in enemies)
            //    {
            //        if (c == 0)
            //        {
            //            pc.PlayerAttack();
            //            enemyno = 1;
            //            c = 1;
            //        }
            //        else if (c == 1)
            //        {
            //            pc.PlayerAttack();
            //            enemyno = 2;
            //            c = 2;
            //        }
            //        else if (c == 2)
            //        {
            //            pc.PlayerAttack();
            //            enemyno = 3;
            //        }
            //    }
            //}
            pc.PlayerAttack();
            attack = false;
        }

        //ゲージに反映
        sld.value = nowComboTime / comboTime;
        comboTex.text = "コンボ：" + comboNum.ToString();

    }


    public void Shuffle() {
        int n = playObjs.Length;

        //Transform[] ansTemp = ans;

        while (n > 1) {

            n--;

            int k = UnityEngine.Random.Range(0, n + 1);

            int te = soe[k];
            soe[k] = soe[n];
            soe[n] = te;
        }

        //線を更新
        DrawLine();

        //最初から揃ってたらやり直し
        for (int i = -2; i < 3; i++)
            if (i != 0) CheckClear(i);

        //クリアフラグを倒す
        cleared = false;

    }

    //線を引かせる
    //今の処理だとシャッフルして自分と同じ添え字が来たら消える。
    void DrawLine() {
        for (int i = 0; i < playObjs.Length; i++)
        {
            //int here = soe[i];
            int next = 0;
            if (i != playObjs.Length - 1) next = i + 1;

            playObjs[soe[i]].GetComponent<GoToParent>().SetLine(playObjs[soe[next]]);
        }
    }

    bool CheckClear(int nextNum)
    {
        for (int i = 0; i < play.Length; i++)
        {
            int maxNum = play.Length - 1;
            int next = i + nextNum;
            if (next > maxNum) next -= play.Length;
            if (next < 0) next += play.Length;

            GameObject a = null, b = null;
            for (int c = 0; c < play[i].childCount; c++)
            {
                a = play[i].GetChild(c).gameObject;
                GoToParent gp = a.GetComponent<GoToParent>();
                if (gp)
                {
                    a = gp.GetLineEnd();
                    break;
                }
            }
            for (int c = 0; c < play[next].childCount; c++)
            {
                b = play[next].GetChild(c).gameObject;
                if (b.tag == "My") break;
            }

            if (a != b) return false;

        }

        //今bのタイム更新
        nowComboTime = comboTime;

        //コンボタイムが残ってたらコンボを増やす
        if (nowComboTime > 0)
        {
            comboNum++;
        }

        shuffleCount = shuffleInterval;


        //pc.PlayerAttack();

        cleared = true;
        attack = true;
        return true;

    }

    void ClearReward(int point) {
        //Shuffle();
        AddMagicPoint(point);
        ShowEffeLingSound();
    }

    //魔力を増やす
    void AddMagicPoint(int num) {
        magicPoint += num;
        mpText.text = "魔力: "+ magicPoint.ToString("0");
    }

    void ShowEffeLingSound() {
        Instantiate(clearEffe, effePos);
        ass.PlayOneShot(se);
    }

    public void SubMP() {

        magicPoint -= 3;
        if(magicPoint < 0)
        {
            magicPoint = 0;
        }
        mpText.text = "魔力: " + magicPoint.ToString("0");
    }
}
