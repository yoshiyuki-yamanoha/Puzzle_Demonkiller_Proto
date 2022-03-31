﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Orb;
    [SerializeField] private Material[] _Colors = new Material[5];
    private Vector3 ORI_POS = new Vector3(11.0f,-3.69f,0f);
    private float Orb_dis = 1.2f;
    private int TotalNum;

    //[SerializeField] private float orbDis = 10.0f;
    void Start()
    {
        TotalNum = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    CreateOrb(_Colors[Random.Range(0, 5)], 1);
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    Orb_Clear();
        //}
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    for(int i = 0; i < TotalNum; i++)
        //    {
        //        Debug.Log("番号 : "+ i+ " |色 : " + Get_OrbColorInfos()[i] + "|型 : " + Get_OrbTypeInfos()[i]);
        //    }
        //}
    }

    public GameObject CreateOrb (Material _mat,int type)
    {
        GameObject obj = Instantiate(Orb);

        obj.GetComponent<Renderer>().material = _mat;
        obj.transform.parent = this.transform;
        obj.transform.localPosition = Cre_pos();
        obj.transform.GetChild(type).gameObject.SetActive(true);
        obj.name = _mat.name;
        TotalNum++;
        return obj;
    }

    Vector3 Cre_pos()
    {
        Vector3 pos = Vector3.zero;

        pos = new Vector3(ORI_POS.x + (TotalNum / 11) * -1.5f, ORI_POS.y + ((TotalNum % 11) * Orb_dis), ORI_POS.z);

        return pos;
    }

    public void Orb_Clear()
    {
        for(int i = 0; i < TotalNum; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        TotalNum = 0;
    }

    //List<int> OrbColorInfo = new List<int>();
    List<int> Get_OrbColorInfos()
    {
        List<int> OrbColorInfo = new List<int>();
        for (int i=0; i<this.transform.childCount; i++)
        {
            OrbColorInfo.Add(Get_OrbColorInfo(transform.GetChild(i).gameObject));
        }

        return OrbColorInfo;
    }
    
    List<int> Get_OrbTypeInfos()
    {
        List<int> OrbTypeInfo = new List<int>();
        for (int i=0; i<this.transform.childCount; i++)
        {
            OrbTypeInfo.Add(Get_OrbTypeInfo(transform.GetChild(i).gameObject));
        }

        return OrbTypeInfo;
    }


    /// <summary>
    /// 赤:0 青:1 黄:2 水:3 緑:4 -1:null
    /// </summary>
    /// <param name="orb"></param>
    /// <returns></returns>
    public int Get_OrbColorInfo(GameObject orb)
    {
        
        int colNum = -1;

        for(int i = 0; i<_Colors.Length; i++)
        {
            if (orb.name == _Colors[i].name)
            {
                colNum = i;
                return colNum;
            }
        }

        return colNum;
    }

    /// <summary>
    /// 0:STAR 1:PENTAGON
    /// </summary>
    /// <param name="orb"></param>
    /// <returns></returns>
    public int Get_OrbTypeInfo(GameObject orb)
    {
        int TypeNum = -1;

        if(orb.transform.GetChild(0).gameObject.activeSelf)
        {
            TypeNum = (int)PointControl.MAGIC_MODE.STAR;
        }
        else
        {
            TypeNum = (int)PointControl.MAGIC_MODE.PENTAGON;
        }

        return TypeNum;
    }
}
