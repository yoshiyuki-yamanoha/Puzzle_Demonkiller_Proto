using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCamera : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Lock_Enemy() != null)
        {
            GameObject target = Lock_Enemy();
            MagicCircle_Visible(target);
            this.transform.LookAt(target.transform);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            string name = "";
            foreach(GameObject obj in Get_EneList())
            {
                name += obj.name + "**";
            }
            Debug.Log(Get_EneList().Count + "NAME" + name);
        }
    }

    GameObject Lock_Enemy()
    {
        if (Get_EneList().Count == 0) return null;
        int Ene_sum = Get_EneList().Count;

        float Ene_Dis = Mathf.Infinity;
        int Ene_Num = -1;

        foreach (GameObject ene in Get_EneList())
        {
            if(Ene_sum != Get_EneList().Count)
            {
                return null;
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
}
