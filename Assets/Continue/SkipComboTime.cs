using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipComboTime : MonoBehaviour
{
    [SerializeField] ClearCheck s_ClearCheck;

    void Update() {

        if(s_ClearCheck != null)
            if (Input.GetButtonDown("Fire3"))
                s_ClearCheck.SkipComboTime();

        if (Input.GetButtonDown("Start"))
        {
            GameMgr.Instance.Restart();
        }

    }
}
