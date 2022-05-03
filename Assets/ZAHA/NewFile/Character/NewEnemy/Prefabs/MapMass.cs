using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMass : MonoBehaviour
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
    GameObject[,] core_bari_Data = new GameObject[20, 20];//コアとバリケードの位置保存

    int i = 0;
    int j = 0;

    GameObject rootobj_;
    [SerializeField] GameObject tilemas_prefab = null;
    [SerializeField] GameObject icemas_prefab = null;
    [SerializeField] GameObject core_prefab = null;
    [SerializeField] GameObject tree_prefab = null;
    [SerializeField] GameObject bari_prefab = null;

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

    public struct Core
    {
        public List<Vector2Int> pos;
        public List<GameObject> obj;
    }

    Core core_info = new Core();

    int[,] map = new int[20, 20]{
        {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,4,4,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,5,5,5,4,4,5,5,5,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,0,0,0,0,0,4,4,0,0,0,4},
        {4,0,0,0,4,4,0,0,0,0,0,0,0,0,4,4,0,0,0,4},
        {4,5,5,5,4,4,0,0,0,0,0,0,0,0,4,4,5,5,5,4},
        {4,0,0,0,0,0,0,0,0,3,3,0,0,0,0,0,0,0,0,4},
        {4,0,0,0,0,0,0,0,0,3,3,0,0,0,0,0,0,0,0,4},
        {4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
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

    int[,] spawn = new int[20, 20]
    {
        {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
        {4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
        {4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
        {4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
        {4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
        {4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
        {4,1,1,1,4,4,1,1,1,4,4,1,1,1,4,4,1,1,1,4},
        {4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
        {4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
        {4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
        {4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
        {4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
        {4,2,2,2,4,4,2,2,2,4,4,2,2,2,4,4,2,2,2,4},
        {4,3,3,3,4,4,5,5,5,4,4,5,5,5,4,4,3,3,3,4},
        {4,3,3,3,4,4,0,0,0,0,0,0,0,0,4,4,3,3,3,4},
        {4,3,3,3,4,4,0,0,0,0,0,0,0,0,4,4,3,3,3,4},
        {4,5,5,5,4,4,0,0,0,0,0,0,0,0,4,4,5,5,5,4},
        {4,0,0,0,0,0,0,0,0,6,6,0,0,0,0,0,0,0,0,4},
        {4,0,0,0,0,0,0,0,0,6,6,0,0,0,0,0,0,0,0,4},
        {4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
    };


    public int[,] Map { get => map; set => map = value; }
    public GameObject Tilemas_prefab { get => tilemas_prefab; }
    public GameObject[,] Core_bari_Data { get => core_bari_Data; set => core_bari_Data = value; }
    public int[,] Spawn { get => spawn; set => spawn = value; }

    //魔法セレクターの座標 (添え字)
    int selX, selY;

    private void Start()
    {
        rootobj_ = new GameObject("MassRoot");//空のオブジェクトを作成-!
        InstanceMap();//インスタントmapを
        core_info.pos = new List<Vector2Int>();
        core_info.obj = new List<GameObject>();
        InitCore();//コア配列作成
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
                if (Map[y, x] == (int)MapMass.Mapinfo.core)
                {
                    if (i > core_obj.Length)
                        continue;
                    Core_bari_Data[y, x] = core_obj[i];
                    SetCore(new Vector2Int(x, y) , core_obj[i]);//コアの場所を追加
                    i++;
                }
                else if (Map[y, x] == (int)MapMass.Mapinfo.bari)
                {
                    if (j > bari_obj.Length)
                        continue;
                    Core_bari_Data[y, x] = bari_obj[j];
                    j++;
                }
                else
                {
                    Core_bari_Data[y, x] = null;
                }
            }
        }
    }

    void SetCore(Vector2Int pos , GameObject obj)
    {
        core_info.pos.Add(pos);
        core_info.obj.Add(obj); 
    }

    public Core GetCore()
    {
        return core_info;
    }

    void InstanceMap()
    {
        GameObject obj = null;
        float fence_CorrectionX = 2.85f;
        var treeParent = new GameObject("Trees");
        var barriParent = new GameObject("Barricades");

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
                    case (int)Mapinfo.core:
                        obj = Instantiate(core_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, Tilemas_prefab.transform.localScale.y * 2, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        //obj.transform.parent = rootobj_.transform;
                        obj.gameObject.name = "Core";
                        obj.gameObject.tag = "Core";
                        obj = Instantiate(Tilemas_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, 0, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        obj.transform.parent = rootobj_.transform;//親にしたいオブジェクトを設定。
                        break;
                    case (int)Mapinfo.tree:
                        obj = Instantiate(tree_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, /*Tilemas_prefab.transform.localScale.y*/0, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        obj.transform.parent = treeParent.transform;
                        obj.gameObject.name = "tree";
                        //obj.gameObject.tag = "Core";
                        obj = Instantiate(Tilemas_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x, 0, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
                        obj.transform.parent = rootobj_.transform;//親にしたいオブジェクトを設定。
                        break;
                    case (int)Mapinfo.bari:
                        obj = Instantiate(bari_prefab, new Vector3(x * Tilemas_prefab.transform.localScale.x + fence_CorrectionX, Tilemas_prefab.transform.localScale.y, y * -Tilemas_prefab.transform.localScale.z), Quaternion.identity);
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
