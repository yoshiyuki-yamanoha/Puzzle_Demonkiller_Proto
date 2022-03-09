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
        SetAnsIni();        //シャッフル
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

    private string GetPlStr()
    {
        return GameObject.Find("Sphere").GetComponent<ShootMagic>().Get_Str();
    }

    public void SetAnsIni()
    {
        do
        {
            Shuffle();
            SetAnswer();
        } 
        while (answerStr == GetPlStr());
    }
}
