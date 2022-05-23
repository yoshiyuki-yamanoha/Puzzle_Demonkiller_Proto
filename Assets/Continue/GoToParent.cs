using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToParent : MonoBehaviour
{
    public bool c_ColorFlg = false;
    [SerializeField] float curAngleY;

    //まてりあらああああ
    [SerializeField] Material matR;

    //初期マテリアル
    Material mat;

    //初期ネーム
    string oriName;

    //裏の名前
    [SerializeField] private string OppoName;

    //選択サークルのインスタンス化保存用
    private GameObject s_circle = null;

    //線用
    [SerializeField] LineRenderer lr;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        oriName = gameObject.name;

        //線
        lr.startWidth = 0.3f;                   // 開始点の太さを0.1にする
        lr.endWidth = 0.3f;                     // 終了点の太さを0.1にする
    }

    GameObject endLinePos;

    private void Update()
    {
        if (transform.localPosition != Vector3.zero)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 0.5f);
        }

        //色替え
        if (c_ColorFlg)
        {
            curAngleY = 15f;
            if (transform.eulerAngles.y > 165)
                curAngleY = 0;

            transform.Rotate(new Vector3(0, curAngleY, 0));
        }
        else {
            curAngleY = -15f;
            if (transform.eulerAngles.y <= 1.5f)
                curAngleY = 0;

            transform.Rotate(new Vector3(0, curAngleY, 0));
        }


        //選択サークルの位置を魔法陣に合わせる
        if (s_circle)
        {
            s_circle.transform.position = transform.parent.position;
            s_circle.transform.parent = transform.parent;
            s_circle.GetComponent<Renderer>().sharedMaterial.color = endLinePos.GetComponent<Renderer>().material.color;
        }

        

        if(endLinePos){
            var positions = new Vector3[]{
            transform.position,
            endLinePos.transform.position
        };

            // 線を引く場所を指定する
            if (lr)
            {
                lr.SetPositions(positions);
            }
        }

        LineMoveWidth();
    }

    //選択サークル表示
    public void ShowSelectCircle(GameObject cir)
    {
        if (s_circle == null)
        {
            s_circle = Instantiate(cir, transform.position, Quaternion.identity);
            s_circle.name = "Selecter";
        }
    }

    public void FadeSelectCircle() {
        Destroy(s_circle);
    }

    ///ガシャっと180度回して色替えるフラグオン
    public void ChangeColor() {
        c_ColorFlg = !c_ColorFlg;

        if (c_ColorFlg)
            ChangeMat(1);
        else
            ChangeMat(0);
            
    }

    //マテリアルを指定の物に変更する処理(外から呼び出す用で分けた)
    public void ChangeMat(int num) {
        switch (num) {
            case 1:
                gameObject.GetComponent<Renderer>().material = matR;
                gameObject.name = OppoName;
                break;
            case 0:
                gameObject.GetComponent<Renderer>().material = mat;
                gameObject.name = oriName;
                break;
        }
    }

    //線を結ぶ用の関数
    public void SetLine(GameObject obj) {

        //線の先のオブジェクトを変える
        endLinePos = obj;
    }

    //線の先のオブジェクトをゲット
    public GameObject GetLineEnd() {
        return endLinePos;
    }

    //着色
    public void LineSetColor() {
        //自分のマテリアルの色と先のマテリアルの色を獲る
        Color a = GetComponent<Renderer>().material.color;
        Color b = endLinePos.GetComponent<Renderer>().material.color;

        a.a = 1f;
        b.a = 1f;

        //線の色をオブジェクトに合わせる
        lr.startColor = a;
        lr.endColor = b;
    }
    public void LineSetWidth()
    {
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        
    }

    bool sWidthSmallFlag = false;
    bool eWidthSmallFlag = false;
    float widthMax = 0.3f;
    float widthMin = 0.1f;
    float widthS = 0.2f;
    float widthE = 0.2f;
    public void LineMoveWidth()
    {
        if ((widthMax > widthS)&&sWidthSmallFlag == false)
        {
            widthS += 0.01f;
            widthE -= 0.01f;
            if (widthMax < widthS)
            {
                sWidthSmallFlag = true;
            }
        }else if ((widthMin < widthS)&&sWidthSmallFlag == true )
        {
            widthS -= 0.01f;
            widthE += 0.01f;
            if (widthMin > widthS)
            {
                sWidthSmallFlag = false;
            }
        }
        lr.startWidth = widthS;
        lr.endWidth = widthE;

    }

    public void LineColorWhite() {
        lr.startColor = Color.white;
        lr.endColor = Color.white;
    }
}
