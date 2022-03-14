using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_MagicCircle : MonoBehaviour
{
    [SerializeField] private Transform[] ans;

    [SerializeField] string answerStr;       //敵の魔法陣の正解


    [SerializeField] GameObject clearEffe;

    PuzzleMgr puMgr;

    private void Start()
    {
        puMgr = GameObject.Find("PuzzleMgr").GetComponent<PuzzleMgr>();
        SetAnsIni();        //シャッフル
    }
    void SetAnswer()
    {
        answerStr = "";

        foreach (Transform o in ans)
        {
            if (o.childCount > 0 && o.gameObject.activeInHierarchy)
                answerStr = answerStr + o.GetChild(0).gameObject.name;
        }
    }

    void Shuffle()
    {

        List<bool>activePuzzle =  puMgr.GetAnsPuzzle();

        for(int i=0;i< 8; i++)
        {
            ans[i].gameObject.SetActive(false);
            if (activePuzzle[i] == true)
            {
                ans[i].gameObject.SetActive(true);
            }
        }
        //int n = puMgr.cycle_Max;

        //while (n > 1)
        //{
        //    n--;

        //    int k = UnityEngine.Random.Range(0, n + 1);
        //    GameObject temp = ans[k].GetChild(0).gameObject;
        //    ans[k].GetChild(0).gameObject.transform.parent = ans[n];
        //    ans[n].GetChild(0).gameObject.transform.parent = ans[k];

        //}
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
        puMgr.SetCycleMax();
        puMgr.RandmCycleSet();
        do
        {
            Shuffle();
            SetAnswer();
        } 
        while (answerStr == GetPlStr());
    }
}
