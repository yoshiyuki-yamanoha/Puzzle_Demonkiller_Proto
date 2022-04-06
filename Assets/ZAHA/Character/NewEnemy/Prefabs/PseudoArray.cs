using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoArray : MonoBehaviour
{
    [SerializeField] bool whoisflg = false;

    public bool Whoisflg { get => whoisflg; set => whoisflg = value; }
    public MassStatus Mass_status { get => mass_status; set => mass_status = value; }

    MassStatus mass_status = MassStatus.NONE;//マスの状態

    public enum MassStatus
    {
        NONE,//誰もいない
        ENEMY,//敵がいるよん
        DEBUFFARIA,//デバフエリア
    };
}
