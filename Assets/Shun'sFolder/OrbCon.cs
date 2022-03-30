using System.Collections;
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

    struct OrbInfo
    {
        public int type;
        public int colorNum;

        public void Set_type(int num)
        {
            type = num;
        }
        public void Set_ColNum(int num)
        {
            colorNum = num;
        }
    }
    List<OrbInfo> orbSum = new List<OrbInfo>();
    //[SerializeField] private float orbDis = 10.0f;
    void Start()
    {
        TotalNum = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CreateOrb(_Colors[Random.Range(0, 5)], Random.Range(0, 1));
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Orb_Clear();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Get_OrbInfos();
        }
    }

    public GameObject CreateOrb (Material _mat,int type)
    {
        GameObject obj = Instantiate(Orb);

        obj.GetComponent<Renderer>().material = _mat;
        obj.transform.parent = this.transform;
        obj.transform.localPosition = Cre_pos();
        obj.transform.GetChild(type).gameObject.SetActive(true);
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

    List<OrbInfo> Get_OrbInfos()
    {
        orbSum.Clear();
        for (int i=0; i<this.transform.childCount; i++)
        {
            orbSum.Add(new OrbInfo());
            orbSum[i].Set_type(Get_OrbTypeInfo(transform.GetChild(i).gameObject));
            orbSum[i].Set_ColNum(Get_OrbColorInfo(transform.GetChild(i).gameObject));
        }

        //foreach(OrbInfo info in orbSum)
        //{
        //    Debug.Log("番号 : " + orbSum.IndexOf(info) + " 型 : " + info.type + " 色 + " + info.colorNum);
        //}

        return orbSum;
    }


    /// <summary>
    /// 赤:0 青:1 黄:2 水:3 緑:4 -1:null
    /// </summary>
    /// <param name="orb"></param>
    /// <returns></returns>
    public int Get_OrbColorInfo(GameObject orb)
    {
        
        int colNum = -1;
        Renderer _orbMat = orb.GetComponent<Renderer>();

        for(int i = 0; i<_Colors.Length; i++)
        {
            if (_orbMat.material.name == _Colors[i].name)
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
