using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject target;
    [SerializeField] public bool canTargetChange;
    [SerializeField,Range(1.0f,10.0f)] private float transTime = 2.0f;
    private float rotTime = 0f;
    private float oldYpos;

    void Start()
    {
        canTargetChange = true;
        rotTime = 0f;
        OldRotPosSet();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Lock_Enemy() != null && canTargetChange)
        {
            target = Lock_Enemy();
            MagicCircle_Visible(target);
            canTargetChange = false;
            rotTime = 0f;
            OldRotPosSet();
        }


        if(target != null)
        {
            float RotY = (oldYpos * (1.0f - rotTime) + (mkCamRot() * rotTime)) / 2;

            transform.eulerAngles =new Vector3((transform.rotation.eulerAngles.x),
                                                    RotY,
                                               (transform.rotation.eulerAngles.z));
        }
        else
        {
            canTargetChange = true;
        }
        transitionRot();
    }

    GameObject Lock_Enemy()
    {
        if (Get_EneList().Count == 0) return null;

        float Ene_Dis = Mathf.Infinity;
        int Ene_Num = -1;

        foreach (GameObject ene in Get_EneList())
        {

            if (ene == null)
            {
                GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(ene);
                continue;
            }
            Vector3 Pl_Pos = GameObject.Find("Sphere").transform.position;

            float Dis = Vector3.Distance(ene.transform.position, Pl_Pos);
            if(Ene_Dis > Dis)
            {
                Ene_Dis = Dis;
                Ene_Num = Get_EneList().IndexOf(ene);
            }
        }
        return Get_EneList()[Ene_Num];
    }

    List<GameObject> Get_EneList()
    {
        return GameObject.Find("Sphere").GetComponent<ShootMagic>().get_EneList();
    }

    void MagicCircle_Visible(GameObject target)
    {
        foreach (GameObject ene in Get_EneList())
        {
            if (ene == null) return;
            if (target == ene)   //ターゲットの魔法陣を可視化
            {
                ene.transform.GetChild(1).gameObject.SetActive(true);
            }
            else                //ターゲット以外の魔法陣を不可視化 
            {
                ene.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    float mkCamRot()
    {
        Vector2 start2D = new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 target2D = new Vector2(target.transform.position.x, target.transform.position.z);

        Vector2 dt = target2D - start2D;
        float rad = Mathf.Atan2(dt.y, dt.x);
        float degree = rad * Mathf.Rad2Deg;
        //if (degree < 0) degree = 360.0f + degree;
        degree = (degree - 90.0f) * -1;

        //float ang = Mathf.Atan2(this.transform.position.x, target.transform.position.z);
        //Debug.Log("角度" + degree);

        return degree;
    }

    void transitionRot()
    {
        if(rotTime < 1.0f)
        {
            rotTime += Time.deltaTime * transTime;
        }else if(rotTime >= 1.0f)
        {
            rotTime = 1.0f;
        }
    }

    void OldRotPosSet()
    {

        oldYpos = transform.rotation.eulerAngles.y;
        if(oldYpos > 270)
        {
            oldYpos -= 360.0f;
        }
        Debug.Log("++" + oldYpos);
    }
}
