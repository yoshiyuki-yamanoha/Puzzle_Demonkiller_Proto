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

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        oriName = gameObject.name;
    }

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
        if(s_circle)
            s_circle.transform.position = transform.position;
    }

    //選択サークル表示
    public void ShowSelectCircle(GameObject cir) {
        if(s_circle == null)
            s_circle = Instantiate(cir, transform.position, Quaternion.identity);
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
}
