using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//サンプルコード下の方に移しました。 2022/05/13 ZAHA

public class MapMass : MonoBehaviour
{
    GameObject[,] core_bari_Data = new GameObject[20, 20];//コアとバリケードの位置保存

    int j = 0;

    GameObject rootobj_;
    [SerializeField] GameObject tilemas_prefab = null;
    [SerializeField] GameObject icemas_prefab = null;
    [SerializeField] GameObject core_prefab = null;
    [SerializeField] GameObject tree_prefab = null;
    [SerializeField] GameObject bari_prefab = null;
    [SerializeField] GameObject log_prefab = null;
    [SerializeField] GameObject emp_prefab = null;

    struct MassState
    {
        public int state;
        public GameObject massObj;
    }

    public enum Mapinfo
    {
        NONE,
        Enemy,
        Ice,
        core,
        tree,
        bari,
    }

    MassState[,] masses = new MassState[20, 20];

    public struct MapObj
    {
        public List<Vector2Int> pos;
        public List<GameObject> obj;
    }

    MapObj core_info = new MapObj();
    MapObj bari_info = new MapObj();

    int[,] map = new int[20, 20]{
        {0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0},
        {4,4,4,4,4,4,4,4,4,5,5,4,4,4,0,0,0,0,0,0},
        {4,4,4,4,4,4,4,4,4,0,0,4,4,4,0,0,0,0,0,0},
        {4,4,4,4,4,4,4,4,4,0,0,4,4,4,5,5,5,5,5,5},
        {0,0,0,0,0,0,0,0,0,3,3,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,3,3,0,0,0,0,0,0,0,0,0},
        {5,5,5,5,5,5,4,4,4,0,0,4,4,4,4,4,4,4,4,4},
        {0,0,0,0,0,0,4,4,4,0,0,4,4,4,4,4,4,4,4,4},
        {0,0,0,0,0,0,4,4,4,0,0,4,4,4,4,4,4,4,4,4},
        {0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
    };


    public enum spawninfo
    {
        NONE,
        Spawn_S,
        Spawn_M,
        Spawn_L,
        Wall,
        Bari,
        Core,
    }

    //int[,] spawn = new int[20, 20]
    //{
    //    //{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
    //    //{4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
    //    //{4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
    //    //{4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
    //    //{4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
    //    //{4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
    //    //{4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
    //    //{4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
    //    //{4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
    //    //{4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
    //    //{4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
    //    //{4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
    //    //{4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
    //    //{4,3,3,3,4,4,5,5,5,4,4,5,5,5,4,4,3,3,3,4},
    //    //{4,3,3,3,4,4,0,0,0,0,0,0,0,0,4,4,3,3,3,4},
    //    //{4,3,3,3,4,4,0,0,0,0,0,0,0,0,4,4,3,3,3,4},
    //    //{4,5,5,5,4,4,0,0,0,0,0,0,0,0,4,4,5,5,5,4},
    //    //{4,0,0,0,0,0,0,0,0,6,6,0,0,0,0,0,0,0,0,4},
    //    //{4,0,0,0,0,0,0,0,0,6,6,0,0,0,0,0,0,0,0,4},
    //    //{4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},

    //    //使用している数字
    //    //0
    //    //1
    //    //2
    //    //3
    //    //4
    //    //5
    //    //6

    //    //{0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0},//
    //    //{0,6,6,6,0,0,0,0,0,0,0,4,4,4,0,7,7,7,0,0},
    //    //{0,6,6,6,0,0,0,0,0,0,0,4,4,4,0,7,7,7,0,0},
    //    //{0,6,6,6,0,0,0,0,0,0,0,4,4,4,0,7,7,7,0,0},
    //    //{0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0},
    //    //{0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,2,2,2,0,0},//
    //    //{4,4,4,4,4,4,4,4,4,5,5,4,4,4,0,2,2,2,0,0},
    //    //{4,4,4,4,4,4,4,4,4,0,0,4,4,4,0,0,0,0,0,0},
    //    //{4,4,4,4,4,4,4,4,4,0,0,4,4,4,5,5,5,5,5,5},
    //    //{0,0,0,0,0,0,0,0,0,3,3,0,0,0,0,0,0,0,0,0},
    //    //{0,0,0,0,0,0,0,0,0,3,3,0,0,0,0,0,0,0,0,0},//
    //    //{5,5,5,5,5,5,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
    //    //{0,0,2,2,2,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
    //    //{0,0,2,2,2,0,4,4,4,5,5,4,4,4,4,4,4,4,4,4},
    //    //{0,0,0,0,0,0,4,4,4,0,0,4,4,4,4,4,4,4,4,4},
    //    //{0,0,8,8,8,0,4,4,4,0,0,4,4,4,4,4,4,4,4,4},//
    //    //{0,0,8,8,8,0,4,4,4,0,2,2,2,0,9,9,9,0,0,0},
    //    //{0,0,8,8,8,0,4,4,4,0,2,2,2,0,9,9,9,0,0,0},
    //    //{0,0,0,0,0,0,4,4,4,0,2,2,2,0,9,9,9,0,0,0},
    //    //{0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
    //};

    int barricadeNumber;    // バリケードの番号

    public int[,] Map { get => map; set => map = value; }
    public GameObject Tilemas_prefab { get => tilemas_prefab; }
    public GameObject[,] Core_bari_Data { get => core_bari_Data; set => core_bari_Data = value; }
    //public int[,] Spawn { get => spawn; set => spawn = value; }

    //魔法セレクターの座標 (添え字)
    int selX, selY;

    private void Start()
    {
        rootobj_ = new GameObject("MassRoot");//空のオブジェクトを作成-!
        InstanceMap();//インスタントmapを
        core_info.pos = new List<Vector2Int>();
        core_info.obj = new List<GameObject>();
        bari_info.pos = new List<Vector2Int>();
        bari_info.obj = new List<GameObject>();
        InitCore();//コア配列作成
        barricadeNumber = 1;
    }

    private void FixedUpdate()
    {
        //for (int i = 0; i < bari_info.pos.Count; i++)
        //{
        //    Debug.Log("バリケード name " + bari_info.obj[i].name + " Y 座標 " + bari_info.pos[i].y + "X 座標" + bari_info.pos[i].x);
        //}
    }

    void InitCore()
    {
        GameObject[] bari_obj = GameObject.FindGameObjectsWithTag("Bari");
        GameObject[] core_obj = GameObject.FindGameObjectsWithTag("Core");

        //for (int core = 0; core < core_obj.Length; core++ )
        //{
        //    Debug.Log(core_obj[core].name = "core" + core);
        //}

        //for (int bari = 0; bari < bari_obj.Length; bari++)
        //{
        //    Debug.Log(bari_obj[bari].name = "core" + bari);
        //}

        for (int y = 0; y < Map.GetLength(0); y++)
        {
            for (int x = 0; x < Map.GetLength(1); x++)
            {
                if (Map[y, x] == (int)MapMass.Mapinfo.core)//コア
                {
                    Core_bari_Data[y, x] = core_obj[0];
                    SetCore(new Vector2Int(x, y), core_obj[0]);//コアのmap座標とコアオブジェクトを追加
                }
                else if (Map[y, x] == (int)MapMass.Mapinfo.bari)//バリケード
                {
                    if (j > bari_obj.Length)
                        continue;
                    Core_bari_Data[y, x] = bari_obj[j];
                    bari_obj[j].gameObject.GetComponent<ManageBarricade>().SetMapPos(new Vector2Int(x, y));
                    SetBari(new Vector2Int(x, y) , bari_obj[j]);//バリケードのmap座標をSet
                    j++;
                }
                else
                {
                    Core_bari_Data[y, x] = emp_prefab.gameObject;
                }
            }
        }
    }

    void SetBari(Vector2Int pos, GameObject obj)//セットバリケード (位置のみ)
    {
        bari_info.pos.Add(pos);
        bari_info.obj.Add(obj);
    }

    public MapObj GetBari()//ゲットバリケ―ド
    {
        return bari_info;
    }

    void SetCore(Vector2Int pos, GameObject obj)//コアは位置とオブジェクトのみ
    {
        core_info.pos.Add(pos);
        core_info.obj.Add(obj);
    }

    public MapObj GetCore()//ゲットコア
    {
        return core_info;
    }

    void InstanceMap()
    {
        GameObject obj = null;
        GameObject core = null;
        float fence_CorrectionX = 2.85f;
        //GameObject treeParent = new GameObject("Trees");
        GameObject barriParent = new GameObject("Barricades");
        GameObject logParent = new GameObject("Log");
        int[,] logPosArray = new int[4, 2] { { 12, 4 }, { 4, 7 }, { 15, 12 }, { 7, 15 } };

        for (int y = 0, logNum = 0; y < Map.GetLength(0); y++)
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
                    case (int)Mapinfo.core:
                        if (core == null)
                        {
                            core = Instantiate(core_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x + Tilemas_prefab.transform.localScale.x / 2, Tilemas_prefab.transform.localScale.y / 2, y * -Tilemas_prefab.transform.localScale.z - Tilemas_prefab.transform.localScale.z / 2), Quaternion.identity);
                            //obj.transform.parent = rootobj_.transform;
                            core.gameObject.name = "Core";
                            core.gameObject.tag = "Core";
                        }
                        obj = Instantiate(Tilemas_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, 0, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        obj.transform.parent = rootobj_.transform;//親にしたいオブジェクトを設定。
                        break;
                    case (int)Mapinfo.tree:
                        {
                            if (y == logPosArray[logNum, 1])
                            {
                                if (x == logPosArray[logNum, 0])
                                {
                                    obj = Instantiate(log_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, Tilemas_prefab.transform.localScale.x / 2, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity/*AngleAxis(-90.0f, Vector3.right)*/);
                                    obj.transform.SetParent(logParent.transform, false);
                                    //obj.transform.localScale = new Vector3(10.0f, 20.0f, 10.0f);
                                    obj.gameObject.name = "logs";
                                    if ((x + y) % 2 != 0)
                                    {
                                        obj.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up);
                                    }
                                    if (logNum < logPosArray.GetLength(0) - 1)
                                        logNum++;
                                }
                            }
                        }

                        //obj = Instantiate(tree_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, /*Tilemas_prefab.transform.localScale.y*/0, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        //obj.transform.parent = treeParent.transform;
                        //obj.gameObject.name = "tree";
                        //obj.gameObject.tag = "Core";
                        obj = Instantiate(Tilemas_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, 0, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        obj.transform.parent = rootobj_.transform;//親にしたいオブジェクトを設定。
                        break;
                    case (int)Mapinfo.bari:
                        obj = Instantiate(bari_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x + fence_CorrectionX, Tilemas_prefab.transform.localScale.y / 2, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);

                        // バリケードに独自の番号を付与
                        obj.GetComponent<ManageBarricade>().SetMyNumber(barricadeNumber);
                        if (x < Map.GetLength(1) - 1)
                        {
                            if (Map[y, x + 1] != (int)Mapinfo.bari)//隣をみている。
                            {
                                barricadeNumber++;
                            }
                        }
                        else if (x == Map.GetLength(1) - 1)
                        {
                            barricadeNumber++;
                        }

                        obj.transform.parent = barriParent.transform;
                        obj.gameObject.name = "Bari";
                        obj.gameObject.tag = "Bari";
                        obj = Instantiate(Tilemas_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, 0, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        obj.transform.parent = rootobj_.transform;//親にしたいオブジェクトを設定。
                        break;
                }

                masses[y, x].state = 0;
                masses[y, x].massObj = obj;
            }
        }
        rootobj_.AddComponent<ChildName>();
    }
    //指定した添え字のGameObjectを返してくれる関数
    public GameObject GetGameObjectOfSpecifiedMass(int x, int y)
    {

        if (y > Map.GetLength(0) - 1 || y < 0) return null;
        if (x > Map.GetLength(1) - 1 || x < 0) return null;

        return masses[y, x].massObj;
    }

    //全てのマスGameObjectをゲットする
    public GameObject[] GetAllMassObjects()
    {

        var m = new GameObject[masses.Length];
        int n = 0;

        foreach (var o in masses)
        {
            m[n] = o.massObj;
            n++;
        }

        return m;
    }

    //魔法セレクターの位置を格納
    public void SetMagicMassSelector(int x, int y)
    {
        selX = x;
        selY = y;
    }

    public (int, int) GetMAgicMassSelector()
    {

        return (selX, selY);
    }
}
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
