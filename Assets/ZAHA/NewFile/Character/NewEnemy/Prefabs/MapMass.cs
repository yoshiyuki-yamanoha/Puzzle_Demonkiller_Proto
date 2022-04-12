using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMass:MonoBehaviour
{
    //サンプルコード
    //int width;//幅
    //int height;//高さ
    //int out_of_range = -1;
    //int[] values = null;//mapデーター

    //private void Start()
    //{
    //    //MapMass mapmass = new MapMass();
    //    //mapmass.Create(8, 8);
    //    //mapmass.Set(1, 2, 5);
    //    //mapmass.Set(3, 7, 3);
    //    //mapmass.Dump();
    //}

    //public int Width
    //{
    //    get{ return width; }
    //}

    //public int Height
    //{
    //    get { return height; }
    //}

    //public void Create(int width , int height)
    //{
    //    this.width = width;
    //    this.height = height;

    //    values = new int[Width * Height];
    //    Debug.Log("配列値" + values);
    //}

    //public int ToIdx(int x, int y)
    //{
    //    return x + (y * Width);
    //}

    ////範囲内かチェック
    //public bool IsOutOfRange(int x, int y)
    //{
    //    if (x < 0 || x >= Width) { return true; }
    //    if(y < 0 || y >= Height) { return true; }

    //    return false;
    //}

    //public int Get(int x , int y)
    //{
    //    if (IsOutOfRange(x, y))
    //    {
    //        return out_of_range;
    //    }

    //    return values[y * Width + x];
    //}

    //public void Set(int x, int y, int v)
    //{
    //    if (IsOutOfRange(x, y))
    //    {
    //        return;
    //    }

    //    values[y * Width + x] = v;
    //}

    //public void Dump()
    //{
    //    Debug.Log("[Array2D] (w,h)=(" + Width + " , " + Height + " ) ");
    //    for (int y = 0; y < Height; y++)
    //    {
    //        string s = "";
    //        for (int x = 0; x < Width; x++)
    //        {
    //            s += Get(x, y) + ",";
    //        }
    //        Debug.Log(s);
    //    }
    //}

    GameObject rootobj_;
    [SerializeField] GameObject tilemas_prefab = null;
    [SerializeField] GameObject icemas_prefab = null;

    public enum Mapinfo
    {
        NONE,
        Enemy,
        Ice,
    }

    int[,] map = new int[15, 11]{
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
    };

    public int[,] Map { get => map; set => map = value; }
    public GameObject Tilemas_prefab { get => tilemas_prefab;}

    private void Start()
    {
        rootobj_ = new GameObject("MassRoot");//空のオブジェクトを作成-!
        InstanceMap();//インスタントmapを
    }

    void InstanceMap()
    {
        GameObject obj;
        for (int y = 0; y < Map.GetLength(0); y++)
        {
            for (int x = 0; x < Map.GetLength(1); x++)
            {
                switch (Map[y, x])
                {
                    case (int)Mapinfo.NONE:
                        obj = Instantiate(Tilemas_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, 0, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        obj.transform.parent = rootobj_.transform;//親にしたいオブジェクトを設定。
                        break;
                    case (int)Mapinfo.Ice:
                        obj = Instantiate(icemas_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, 0, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        obj.transform.parent = rootobj_.transform;
                        break;
                }
            }
        }
    }
}
