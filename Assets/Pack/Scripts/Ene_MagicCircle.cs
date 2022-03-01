using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_MagicCircle : MonoBehaviour
{
    [SerializeField] private Transform[] ans;

    [SerializeField] string answerStr;       //敵の魔法陣の正解


    [SerializeField] GameObject clearEffe;

    private void Start()
    {
        Shuffle();
        SetAnswer();
        //string pstr = GameObject.Find("Sphere").GetComponent<ShootMagic>().Get_Str();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnswer();
        //それぞれの名前を繋げる処理 あとで関数にまとめまーす💛
        //{
        //    playerStr = "";

        //    foreach (Transform o in play)
        //    {
        //        if (o.childCount > 0)
        //            playerStr = playerStr + o.GetChild(0).gameObject.name;
        //    }
        //}

        //名前完全一致型クリアチェック
        //if (answerStr == playerStr)
        //{
        //    Instantiate(clearEffe, gameObject.transform.position,Quaternion.identity);

        //    GameObject.Find("Sphere").GetComponent<ShootMagic>().CreateMagic();
        //}

    }

    void SetAnswer()
    {
        answerStr = "";

        foreach (Transform o in ans)
        {
            if (o.childCount > 0)
                answerStr = answerStr + o.GetChild(0).gameObject.name;
        }
    }

    void SetRandomSide()
    {
        foreach (Transform t in ans)
        {

            GameObject g = t.GetChild(0).gameObject;

            int num = Random.Range(0, 2);
            g.GetComponent<GoToParent>().ChangeMat(num);

        }
    }

    void Shuffle()
    {
        int n = ans.Length;

        while (n > 1)
        {
            n--;

            int k = UnityEngine.Random.Range(0, n + 1);
            GameObject temp = ans[k].GetChild(0).gameObject;
            ans[k].GetChild(0).gameObject.transform.parent = ans[n];
            ans[n].GetChild(0).gameObject.transform.parent = ans[k];

        }
    }

    public string GetEneStr()
    {
        return answerStr;
    }
}
