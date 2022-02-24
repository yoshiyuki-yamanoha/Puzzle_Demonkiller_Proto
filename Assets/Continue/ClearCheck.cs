using UnityEngine;
using UnityEngine.UI;

public class ClearCheck : MonoBehaviour
{
    //[SerializeField] private Transform[] ans;
    [SerializeField] private Transform[] play;  //親オブジェクト
    //[SerializeField] string answerStr;       //答えオブジェクトの名前を並べたもの
    //[SerializeField] string playerStr;       //プレイヤーオブジェクトの名前を並べたもの

    [SerializeField] private Text stageNum;
    private int sNum = 1;

    [SerializeField] GameObject clearEffe;
    [SerializeField] Transform effePos;

    [SerializeField] private AudioClip se;
    [SerializeField] private AudioSource ass;

    [SerializeField] GameObject[] playObjs;     //子オブジェクト

    [SerializeField] int[] soe = new int[5] { 2, 3, 0, 4, 1 };

    //魔力 ←あとで増やす
    float magicPoint;
    [SerializeField] Text mpText;

    private void Start()
    {
        DrawLine();
        SetAnswer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        {
            //それぞれの名前を繋げる処理 あとで関数にまとめまーす💛
            //{
            //    playerStr = "";

            //    foreach (Transform o in play) {
            //        if (o.childCount > 0)
            //            playerStr = playerStr + o.GetChild(0).gameObject.name;
            //    }
            //}

            //名前完全一致型クリアチェック
            //if (answerStr == playerStr)
            //{
            //    //クリア
            //    Shuffle();
            //    Instantiate(clearEffe, effePos);
            //    ass.PlayOneShot(se);
            //}
        }

        //線が被らなければクリア (グルっと一周)
        CheckRingLine();
        CheckRingLineR();

        //星の形になってればクリア(五芒星)
        CheckStarLine();
        CheckStarLineR();


        //全ての線が後ろの線に重なってればクリア(理想かも)

        //if (Input.GetButtonDown("Fire3"))
         //   Shuffle();

    }

    void SetAnswer() {
        //answerStr = "";

        //foreach (Transform o in ans)
        //{
        //    if (o.childCount > 0)
        //        answerStr = answerStr + o.GetChild(0).gameObject.name;
        //}
    }

    void SetRandomSide() {
        //foreach (Transform t in ans) {

        //    GameObject g = t.GetChild(0).gameObject;

        //    int num = Random.Range(0, 2);
        //    g.GetComponent<GoToParent>().ChangeMat(num);

        //}
    }

    void Shuffle() {
        int n = playObjs.Length;

        //Transform[] ansTemp = ans;

        while (n > 1) {

            n--;

            int k = UnityEngine.Random.Range(0, n + 1);
            //GameObject temp = ans[k].GetChild(0).gameObject;
            //ans[k].GetChild(0).gameObject.transform.parent = ans[n];
            //ans[n].GetChild(0).gameObject.transform.parent = ans[k];

            int te = soe[k];
            soe[k] = soe[n];
            soe[n] = te;
        }

        //表か裏か決めるやつ
        //SetRandomSide();

        //線を更新
        DrawLine();

        //最初から揃ってたらやり直し
        CheckRingLine();
        CheckRingLineR();
        CheckStarLine();
        CheckStarLineR();

        //答えオブジェクトをセットし直す
        //SetAnswer();


        //ステージ数を進める
        //sNum++;
        stageNum.text = sNum.ToString("00");
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

    //グルっと一周してるか判断
    void CheckRingLine() {
        for (int i = 0; i < play.Length; i++)
        {

            int next = 0;
            if (i < play.Length - 1) next = i + 1;
            GameObject a = play[i].GetChild(0).gameObject.GetComponent<GoToParent>().GetLineEnd();
            GameObject b = play[next].GetChild(0).gameObject;
            if (a != b) return;

        }

        //Debug.Log("とけい");
        Shuffle();
        AddMagicPoint(1);
        ShowEffeLingSound();
    }

    void CheckRingLineR() {
        for (int i = 4; i >= 0; i--)
        {

            int next = 4;
            if (i > 0) next = i - 1;
            GameObject a = play[i].GetChild(0).gameObject.GetComponent<GoToParent>().GetLineEnd();
            GameObject b = play[next].GetChild(0).gameObject;
            if (a != b) return;

        }

        //Debug.Log("はんとけい");
        Shuffle();
        AddMagicPoint(1);
        ShowEffeLingSound();
    }

    void CheckStarLine() {
        for (int i = 0; i < play.Length; i++)
        {

            int next = i + 2;
            if (next >= play.Length) next -= play.Length;
            GameObject a = play[i].GetChild(0).gameObject.GetComponent<GoToParent>().GetLineEnd();
            GameObject b = play[next].GetChild(0).gameObject;
            if (a != b) return;

        }

        //Debug.Log("ほし");
        Shuffle();
        AddMagicPoint(2);
        ShowEffeLingSound();
    }

    void CheckStarLineR()
    {
        for (int i = 0; i < play.Length; i++)
        {

            int next = i - 2;
            if (next < 0) next += play.Length;
            GameObject a = play[i].GetChild(0).gameObject.GetComponent<GoToParent>().GetLineEnd();
            GameObject b = play[next].GetChild(0).gameObject;
            if (a != b) return;

        }

        //Debug.Log("逆ほし");
        Shuffle();
        AddMagicPoint(2);
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
}
