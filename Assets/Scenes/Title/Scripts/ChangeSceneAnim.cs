using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneAnim : MonoBehaviour
{
    TitleMgr tg;
    // Start is called before the first frame update
    void Start()
    {
        tg = GameObject.Find("TitleMgr").GetComponent<TitleMgr>();
    }

    public void GotoChangeScene()
    {
        tg.GotoNextScene();
    }

    public void FadeOut()
    {
        tg.OnGrayOutFlg();
    }
}

