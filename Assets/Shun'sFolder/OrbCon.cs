using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Orb;
    [SerializeField] private Material[] _Colors = new Material[5];
    private Vector3 ORI_POS = new Vector3(11.0f, -3.69f, 0f);
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
        if (Input.GetKeyDown(KeyCode.B))
        {
            CreateOrb(_Colors[Random.Range(0, 5)], 0);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            del_FirstOrb();
        }
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    for(int i = 0; i < TotalNum; i++)
        //    {
        //        Debug.Log("番号 : "+ i+ " |色 : " + Get_OrbColorInfos()[i] + "|型 : " + Get_OrbTypeInfos()[i]);
        //    }
        //}
        MoveChild();
    }

    public void MoveChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 pos = new Vector3(ORI_POS.x + (i / 11) * -1.5f, ORI_POS.y + ((i % 11) * Orb_dis), ORI_POS.z);
            if (pos != transform.GetChild(i).localPosition)
            {
                transform.GetChild(i).localPosition = Vector3.Lerp(transform.GetChild(i).localPosition, pos, 0.5f);
            }
        }
    }

    public GameObject CreateOrb(Material _mat, int type)
    {
        if (type == (int)PointControl.MAGIC_MODE.STAR) {      //星型なら　
            for (int i = 0; i < transform.childCount; i++)
            {
                if (_mat.name == transform.GetChild(i).name 
                    && Get_OrbTypeInfo(transform.GetChild(i).gameObject) == (int)PointControl.MAGIC_MODE.STAR)
                {
                    transform.GetChild(i).GetComponent<RotateOrb>().rotateSpeedMultiply++;

                    return null;
                }
            }
        }

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
        int num = transform.childCount - 1;

        pos = new Vector3(ORI_POS.x + (num / 11) * -1.5f, ORI_POS.y + ((num % 11) * Orb_dis), ORI_POS.z);

        return pos;
    }

    public void Orb_Clear()
    {
        TotalNum = 0;
    }

    //List<int> OrbColorInfo = new List<int>();
    public List<int> Get_OrbColorInfos()
    {
        List<int> OrbColorInfo = new List<int>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            OrbColorInfo.Add(Get_OrbColorInfo(transform.GetChild(i).gameObject));
        }

        return OrbColorInfo;
    }

    public List<int> Get_OrbLevelInfos()
    {
        List<int> Get_OrbLevelInfo = new List<int>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<RotateOrb>() != null)
            {
                Get_OrbLevelInfo.Add((int)transform.GetChild(i).gameObject.GetComponent<RotateOrb>().rotateSpeedMultiply);
            }
            else
            {
                Get_OrbLevelInfo.Add(0);
            }
        }

        return Get_OrbLevelInfo;
    }

    public List<int> Get_OrbTypeInfos()
    {
        List<int> OrbTypeInfo = new List<int>();
        for (int i = 0; i < this.transform.childCount; i++)
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

        for (int i = 0; i < _Colors.Length; i++)
        {
            if (orb.name == _Colors[i].name)
            {
                colNum = i;
                return colNum;
            }
        }

        return colNum;
    }

    public int Get_OCInfo(int num)
    {
        GameObject orb = transform.GetChild(num).gameObject;
        int colNum = -1;

        for (int i = 0; i < _Colors.Length; i++)
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

        if (orb.transform.GetChild(0).gameObject.activeSelf)
        {
            TypeNum = (int)PointControl.MAGIC_MODE.STAR;
        }
        else
        {
            TypeNum = (int)PointControl.MAGIC_MODE.PENTAGON;
        }

        return TypeNum;
    }

    public void del_FirstOrb()
    {
        Destroy(transform.GetChild(0).gameObject);

        TrunManager TM = GameObject.Find("TrunManager").GetComponent<TrunManager>();
        if(transform.childCount == 0)
        {
            TM.SetTrunPhase(TrunManager.TrunPhase.Enemy);
        }
    }

    public int Get_FOC_info()
    {

        if(transform.childCount == 0) return -1;

        GameObject orb = transform.GetChild(0).gameObject;
        int colNum = -1;

        for (int i = 0; i < _Colors.Length; i++)
        {
            if (orb.name == _Colors[i].name)
            {
                colNum = i;
                return colNum;
            }
        }
        return colNum;
    }

    public int Get_FOT_Info()
    {
        int TypeNum = -1;
        if (transform.childCount == 0) return -1;
        GameObject orb = transform.GetChild(0).gameObject;

        if (orb.transform.GetChild(0).gameObject.activeSelf)
        {
            TypeNum = (int)PointControl.MAGIC_MODE.STAR;
        }
        else
        {
            TypeNum = (int)PointControl.MAGIC_MODE.PENTAGON;
        }

        return TypeNum;
    }

    public int Get_FOL_Info()
    {
        int LevlNum = -1;
        if (transform.childCount == 0) return -1;
        GameObject orb = transform.GetChild(0).gameObject;

        if (transform.GetChild(0).gameObject.GetComponent<RotateOrb>() != null)
        {
            LevlNum = ((int)transform.GetChild(0).gameObject.GetComponent<RotateOrb>().rotateSpeedMultiply);
        }
        else
        {
            LevlNum = 0;
        }

        return LevlNum;
    }

}
