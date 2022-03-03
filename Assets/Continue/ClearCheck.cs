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

    private void Start()
    {
        DrawLine();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //線が被らなければクリア (グルっと一周)
        CheckRingLine();
        CheckRingLineR();

        //星の形になってればクリア(五芒星)
        CheckStarLine();
        CheckStarLineR();


        //全ての線が後ろの線に重なってればクリア(理想かも)

    }


    void Shuffle() {
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
        CheckRingLine();
        CheckRingLineR();
        CheckStarLine();
        CheckStarLineR();

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
            int maxNum = play.Length-1;
            int next = i + 1;
            int back = i - 1;
            if (next > maxNum) next -= maxNum;
            if (back < 0) back += play.Length;
            GameObject a = play[i].GetChild(0).gameObject.GetComponent<GoToParent>().GetLineEnd();
            GameObject b = play[next].GetChild(0).gameObject;
            GameObject c = play[back].GetChild(0).gameObject;
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

    public void SubMP() {

        magicPoint -= 3;
        if(magicPoint < 0)
        {
            magicPoint = 0;
        }
        mpText.text = "魔力: " + magicPoint.ToString("0");
    }
}
