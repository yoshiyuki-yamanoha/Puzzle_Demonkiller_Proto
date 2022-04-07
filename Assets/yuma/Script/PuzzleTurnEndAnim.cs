using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTurnEndAnim : TrunManager
{
    static private bool trunEndFlg; // Trueでアニメーション開始

    // アニメーション用の変数
    float startCoolTime;    // アニメーションに入る際少し硬直させる
    [SerializeField] float rotateSpeed; //魔法陣の回転速度
    float inAlpha;    // 小さい魔法陣の透明度
    float outAlpha;    // 大きい魔法陣の透明度


    TrunManager turnMgr;
    SelectUseOrb selUseOrb;

    private SEManager sePlay;

    // アニメーションで使用するGameObject
    [SerializeField] GameObject inCircle;
    [SerializeField] GameObject outCircle;
    [SerializeField] GameObject[] miniCircles = new GameObject[5];      // 小さい魔法陣の情報
    [SerializeField] MeshRenderer[] circleMesh = new MeshRenderer[5];   // 小さい魔法陣の透明度を変更する変数
    [SerializeField] MeshRenderer outCircleMesh;   // 大きい魔法陣の透明度を変更する変数

    // バックアップ用のGameObject
    Quaternion buf_inCircleRotate;
    Quaternion buf_outCircleRotate;

    void Init()
    {
        outAlpha = 0.5f;
        inAlpha = 1.0f;
        startCoolTime = 60.0f;
        rotateSpeed = 5.0f;
        inCircle.transform.rotation = buf_inCircleRotate;
        outCircle.transform.rotation = buf_outCircleRotate;

    }
    // Start is called before the first frame update
    void Start()
    {
        trunEndFlg = false;
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();//Se再生用
        turnMgr = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        selUseOrb = GameObject.Find("GameObject").GetComponent<SelectUseOrb>();
        buf_inCircleRotate = inCircle.transform.rotation;
        buf_outCircleRotate = outCircle.transform.rotation;
        Init();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(turnMgr.GetTrunPhase() == TrunPhase.Puzzle)
        {
            
            inCircle.SetActive(true);
            outCircle.SetActive(true);
            

        }
        if (GetPuzzleTurnEndFlg())
        {
            GameObject[] selecter = GameObject.FindGameObjectsWithTag("Selecter");
            foreach(GameObject o in selecter)
            {
                Destroy(o);
            }
            //inCircle.transform.Rotate(0.0f, 0.0f, rotateSpeed);
            //outCircle.transform.Rotate(-rotateSpeed, 0.0f, 0.0f);

            //Debug.Log("OK");
            for (int i = 0; i < 5; i++)
            {
                miniCircles[i].GetComponent<LineRenderer>().enabled = false;
            }
            if (startCoolTime-- < 0.0f)
            {
                
                startCoolTime = 0;
                inCircle.transform.Rotate(0.0f, 0.0f, rotateSpeed + 5.0f);
                outCircle.transform.Rotate(0.0f,rotateSpeed, 0.0f);

                if(inAlpha <= 0.0f)
                {
                    inAlpha = 0.0f;
                    turnMgr.SetTrunPhase(TrunPhase.MagicAttack);
                    selUseOrb.SetOrbType();
                    sePlay.Play("TurnChange"); //ターンチェンジの音を鳴らす
                    SetPuzzleTurnEndFlg(false);
                    Init();
                    inCircle.SetActive(false);
                    outCircle.SetActive(false);
                    for (int i = 0; i < 5; i++)
                    {
                        miniCircles[i].GetComponent<LineRenderer>().enabled = true;
                    }
                }
                else
                {
                    inAlpha -= 0.03f;
                    outAlpha -= 0.03f;
                }
                
                for(int i=0;i<5; i++)
                {
                    circleMesh[i].material.color = new Color(circleMesh[i].material.color.r, circleMesh[i].material.color.g, circleMesh[i].material.color.b, inAlpha);
                }

                outCircleMesh.material.color = new Color(outCircleMesh.material.color.r, outCircleMesh.material.color.g, outCircleMesh.material.color.b, outAlpha);
            }
        }
    }

    public bool GetPuzzleTurnEndFlg()
    {
        return trunEndFlg;
    }

    public void SetPuzzleTurnEndFlg(bool flg)
    {
        trunEndFlg = flg;
    }
}
