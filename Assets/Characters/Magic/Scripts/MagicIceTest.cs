using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicIceTest : MonoBehaviour
{
    //氷の魔法のテストスクリプト
    [SerializeField]
    GameObject IceObj;
    Vector3 pos = new Vector3(0,0,0);
    [SerializeField]
    bool icedoronflag = false;
    GameObject myself;
    public int magictype = 0;//０単体１壁
    //テスト
    public int magicPower;
    void Start()
    {
        pos = transform.position;
        myself = this.gameObject;
    }
    void FixedUpdate()
    {
        if (icedoronflag == false)
        {
            if (magictype == 0)
            {
                GameObject iceobj0 = Instantiate(IceObj, (pos + new Vector3(0, 1, 0)), Quaternion.Euler(0f, 0f, 0f));
                iceobj0.name = "ice";
                iceobj0.transform.parent = myself.transform;
                GameObject iceobj1 = Instantiate(IceObj, pos, Quaternion.Euler(45f, 45f, 0f));
                iceobj1.name = "ice";
                iceobj1.transform.parent = myself.transform;
                GameObject iceobj2 = Instantiate(IceObj, pos, Quaternion.Euler(0f, 0f, 45f));
                iceobj2.name = "ice";
                iceobj2.transform.parent = myself.transform;
                GameObject iceobj3 = Instantiate(IceObj, pos, Quaternion.Euler(70f, 100f, 0f));
                iceobj3.name = "ice";
                iceobj3.transform.parent = myself.transform;
                GameObject iceobj4 = Instantiate(IceObj, pos, Quaternion.Euler(-70f, 100f, 0f));
                iceobj4.name = "ice";
                iceobj4.transform.parent = myself.transform;
                GameObject iceobj5 = Instantiate(IceObj, pos, Quaternion.Euler(-45f, 0f, 45f));
                iceobj5.name = "ice";
                iceobj5.transform.parent = myself.transform;
                GameObject iceobj6 = Instantiate(IceObj, pos, Quaternion.Euler(-45f, -45f, 0f));
                iceobj6.name = "ice";
                iceobj6.transform.parent = myself.transform;
                icedoronflag = true;
            }
            if (magictype == 1)
            {
                if (magicPower > 0) {
                    GameObject iceobj1 = Instantiate(IceObj, (pos + new Vector3(5, 1, 0)), Quaternion.Euler(0f, 0f, 0f));
                    iceobj1.name = "ice";
                    iceobj1.transform.parent = myself.transform;
                    GameObject iceobj2 = Instantiate(IceObj, (pos + new Vector3(0, 1, 0)), Quaternion.Euler(0f, 0f, 0f));
                    iceobj2.name = "ice";
                    iceobj2.transform.parent = myself.transform;
                    GameObject iceobj3 = Instantiate(IceObj, (pos + new Vector3(-5, 1, 0)), Quaternion.Euler(0f, 0f, 0f));
                    iceobj3.name = "ice";
                    iceobj3.transform.parent = myself.transform;
                }
                if (magicPower > 1)
                {
                    GameObject iceobj0 = Instantiate(IceObj, (pos + new Vector3(10, 1, 0)), Quaternion.Euler(0f, 0f, 0f));
                    iceobj0.name = "ice";
                    iceobj0.transform.parent = myself.transform;
                    GameObject iceobj4 = Instantiate(IceObj, (pos + new Vector3(-10, 1, 0)), Quaternion.Euler(0f, 0f, 0f));
                    iceobj4.name = "ice";
                    iceobj4.transform.parent = myself.transform;
                }
                icedoronflag = true;
            }
        }
       // Destroy(magicIns,);
    }
}
