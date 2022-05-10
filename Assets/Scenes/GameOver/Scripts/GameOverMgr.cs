using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMgr : MonoBehaviour
{
    float coolTime;
    // Start is called before the first frame update

    public BGMManager bgmPlay = null;

    SEManager sePlay = null;

    void Start()
    {
        coolTime = 0.5f;
        GenerationGameOver_BGM();
    }

    // Update is called once per frame
    void Update()
    {
        coolTime -= Time.deltaTime;
        if( Input.GetButtonDown("Fire1") && coolTime <= 0.0f ) {
            coolTime = 0.5f;
            GameMgr.Instance.GotoTitleScene();
        } else if(coolTime <= 0.0f) {
            coolTime = 0.0f;
        }
        this.GetComponent<MenuControll>().ResultMenuControll();
    }
    void GenerationGameOver_BGM()
    {
        if (sePlay != null) sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
        bgmPlay = GameObject.Find("BGMAudio").GetComponent<BGMManager>();//Se再生用  
        bgmPlay.Play("GAMEOVERBGM");
    }
}
