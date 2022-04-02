using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTransform : MonoBehaviour
{
    Vector3 nowscale = new Vector3(0, 0, 0);
    Vector3 limitscale = new Vector3(3, 5, 3);
    MagicIceTest MIceT;
    public bool desflag = false;

    //テスト用変数
    float interval = 2;
    [SerializeField]
    float timer = 0;

    Vector3 pos = new Vector3(0, 0, 0);
    [SerializeField]
    GameObject miniIce;//氷が壊れたときのオブジェクトを入れる
    GameObject originObj;//親オブジェクトを入れる


    //氷の大きさを変える
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        originObj = transform.root.gameObject;
        MIceT = GameObject.Find("IceEfect").gameObject.GetComponent<MagicIceTest>();
        this.transform.localScale = new Vector3(0, 0, 0);
        this.transform.position += new Vector3(0,-1,0);
    }
   

    // Update is called once per frame
    void FixedUpdate()
    {
        if (limitscale.y >= nowscale.y)
        {
            if (MIceT.magictype == 1)
            {
                IceScaleWall();
            }
            if(MIceT.magictype == 0)
            {
                IceScale();
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
        //消えるテスト用
        if(timer > interval)
        {
            desflag = true;
        }
        if(desflag == true)
        {
            GameObject breakobj0 = Instantiate(miniIce, (pos + new Vector3(0, 1, 0)), Quaternion.Euler(0f, 0f, 0f));
            breakobj0.name = "miniIce";
            breakobj0.transform.parent = originObj.transform;
            Destroy(gameObject);
        }

    }

    //オブジェクトの大きさを変える
    void IceScaleWall()
    {
        this.transform.localScale += new Vector3(0.1f, 0.16f, 0.1f);
        nowscale = transform.localScale;
    }
    void IceScale()
    {
        this.transform.localScale += new Vector3(0.03f, 0.06f, 0.03f);
        nowscale = transform.localScale;
    }
}
