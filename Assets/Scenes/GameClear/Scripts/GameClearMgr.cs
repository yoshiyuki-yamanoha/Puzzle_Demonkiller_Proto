using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearMgr : MonoBehaviour
{
    float coolTime;
    // Start is called before the first frame update

    public BGMManager bgmPlay = null;

    SEManager sePlay = null;
    FadeOut fadeout;

    void Start() {
        fadeout = GameObject.Find("FadeImage").GetComponent<FadeOut>();

        fadeout.SetFadeSpeed(1.0f);
        fadeout.SetFadeMode(FadeOut.FadeMode.FADE_IN);
        coolTime = 0.5f;
        GenerationClear_Jingle();
    }

    // Update is called once per frame
    void Update() {
        coolTime -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && coolTime <= 0.0f) {
            coolTime = 0.5f;
            //GameMgr.Instance.GotoTitleScene();
        }
        else if (coolTime <= 0.0f) {
            coolTime = 0.0f;
        }

        this.GetComponent<MenuControll>().ResultMenuControll();

    }
    void GenerationClear_Jingle()
    {
        if (sePlay != null) sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
        bgmPlay = GameObject.Find("BGMAudio").GetComponent<BGMManager>();//Se再生用  
        bgmPlay.Play("CLEARBGM");
    }
}
