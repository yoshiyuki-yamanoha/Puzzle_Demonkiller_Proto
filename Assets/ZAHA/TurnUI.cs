using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnUI : MonoBehaviour
{
    [SerializeField] string[] trun_str;
    string now_str;
    Text truntext = null;
    TrunManager trunmager = null;

    // Start is called before the first frame update
    void Start()
    {
        trunmager = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        truntext = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChangeTurn(trunmager.trunphase);
    }

    void ChangeTurn(TrunManager.TrunPhase trunphase)
    {
        switch (trunphase)
        {
            case TrunManager.TrunPhase.Player:
                now_str = trun_str[(int)TrunManager.TrunPhase.Player];
                break;
            case TrunManager.TrunPhase.Puzzle:
                now_str = trun_str[(int)TrunManager.TrunPhase.Puzzle];
                break;
            case TrunManager.TrunPhase.Enemy:
                now_str = trun_str[(int)TrunManager.TrunPhase.Enemy];
                break;
            case TrunManager.TrunPhase.MagicAttack:
                now_str = trun_str[(int)TrunManager.TrunPhase.MagicAttack];
                break;
        }

        truntext.text = now_str;
    }
}
