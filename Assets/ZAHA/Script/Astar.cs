using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    bool init_clear = true;
    MapMass map = null;
    EnemyBase enemy = null;

    //[SerializeField] bool init_turn = true;//1回のみ処理する変数

    List<Node> adjacent_node_list = new List<Node>();//隣接ノード
    List<Node> open_list = new List<Node>();//オープンリスト 経路候補
    List<Node> close_list = new List<Node>();//クローズリスト　計算終わったら入れるリスト

    Node start_node; //スタートノード
    Node goal_node; //ゴールノード
    Node current_node; //現在のノード

    Node current_root_node;

    List<Vector2Int> rootlist_node = new List<Vector2Int>();

    bool init_astar = true;

    //bool attackaria = false;
    //Vector2Int attackpos;

    int a_count = 0;//探索カウント

    internal Node Current_root_node { get => current_root_node; set => current_root_node = value; }
    public List<Vector2Int> Rootlist_node { get => rootlist_node; set => rootlist_node = value; }//root返すやつ。

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("MapInstance").GetComponent<MapMass>();
        enemy = transform.GetComponent<EnemyBase>();
    }

    //ゲットパス。
    void GetPath()
    {
        Current_root_node = current_node;//ゴールした時の位置代入
        while (Current_root_node != null)//現在のノードの親がnullだった時
        {
            Rootlist_node.Add(new Vector2Int(Current_root_node.Pos.x, Current_root_node.Pos.y));
            //Debug.Log("追加時" + "["+current_root_node.Pos.y + "]"+ "[" + current_root_node.Pos.x + "]");
            Current_root_node = Current_root_node.ParentNode;
        }
    } 

    public Vector2Int astar(Node start, Node goal, int num, bool init_goal)//Astarアルゴリズム
    {

        if (!init_goal && init_clear)
        {
            open_list.Clear();
            close_list.Clear();
            init_astar = true;
            init_clear = false;
        }

        //ゴール設定
        //Node goal_node = goal;
        //Node start_node = start;
        goal_node = goal;
        start_node = start;

        ////スタートノード指定
        if (init_astar)
        {
            open_list.Add(start_node);
            current_node = open_list[0];
            init_astar = false;
        }

        for (int i = 0; i < num; i++)
        {
            //オープンリストから現在のノードを取得。
            //current_node = open_list[0];
            a_count++;//今何回目のループか見ちゃうやつ

            ////オープンリストの中で一番コストが低いものを現在のノードにする。
            //foreach (var selectnode in open_list)
            //{
            //    //オープンリストの中で一番小さいノードを選ぶ
            //    if (selectnode.F < current_node.F)
            //    {
            //        current_node = selectnode;//現在のノードを小さいノードに代入
            //        //Debug.Log("選択された小さいノード + " + current_node.F + "[y] " + current_node.Pos.y + "[x] " + current_node.Pos.x);
            //    }
            //}
            open_list.Remove(current_node);//オープンリストの取得した削除

            bool close_flg = true; 

            foreach (var close in close_list)
            {
                if (close.Pos == current_node.Pos)//同じノード
                {
                    close_flg = false;
                    //Debug.Log("クローズリストに同じノードがあります。" + " クローズリスト[" + close.Pos.y + "," + close.Pos.x + "]" + "隣接ノード位置 [" + adjacent.Pos.y + "," + adjacent.Pos.x + "]");
                }
            }

            if (close_flg)
            {
                close_list.Add(current_node);//クローズリストに追加
            }
            //追加する前に追加されているものは追加しない。
            //foreach (var close in close_list)
            //{
            //    if (close.Pos != current_node.Pos)
            //    {
            //        open_list.Remove(current_node);//オープンリストの取得した削除
            //        close_list.Add(current_node);//クローズリストに追加
            //    }
            //}

            //Debug.Log((a_count + 1) + "回目" + "選択された小さいノード + " + current_node.F + "[y] " + current_node.Pos.y + "[x] " + current_node.Pos.x);
            //Debug.Log(a_count + "回目 " + "現在地" + "[" + current_node.Pos.y + "]" + "[" + current_node.Pos.x + "]");

            //クローズの中身をみるデバッグ

            //    ////ゴールチェック
            //if (GoolCheck(current_node, goal_node))
            //{
            //    Debug.Log("ゴールしました。");
            //    GetPath();
            //    Rootlist_node.Reverse();//反対にする。
            //    foreach (var rootlist in Rootlist_node)//ゴールの座標を表示させるやつ。
            //    {
            //        Debug.Log("[" + rootlist.y + "]" + "[" + rootlist.x + "]");
            //    }
            //    break;
            //}

            adjacent_node_list.Clear();//隣接ノードno一回中身をclear

            //鱗片をみる方向
            Vector2Int[] dir =
            {
                new Vector2Int(0, 1),//前
                new Vector2Int(0, -1),//後ろ
                new Vector2Int(-1, 0),//左
                new Vector2Int(1, 0),//右
            };

            for (int number = 0; number < 4; number++)//4方向分調べている。
            {
                Vector2Int adjacent_node_pos = new Vector2Int(current_node.Pos.x + dir[number].x, current_node.Pos.y + dir[number].y);//隣接ノードの位置生成

                //map範囲外はコンテニュー処理 //無限ループしてるかもー
                if (adjacent_node_pos.y > (map.Map.GetLength(0) - 1) || adjacent_node_pos.y < 0 || adjacent_node_pos.x > (map.Map.GetLength(1) - 1) || adjacent_node_pos.x < 0)
                {
                    //Debug.Log(this.gameObject + "範囲外デス。");
                    continue;
                }

                if (map.Map[adjacent_node_pos.y, adjacent_node_pos.x] != (int)MapMass.Mapinfo.NONE) //mapの移動出来る範囲をみる。//0は移動可能
                {
                    //Debug.Log(this.gameObject.name + "移動出来ない");
                    continue;
                }


                Node adjacent_node = new Node(current_node, adjacent_node_pos);//現在のオブジェクトを親ノードにする。
                adjacent_node_list.Add(adjacent_node); //隣接ノードを追加
            }

            //隣接の計算 
            foreach (var adjacent in adjacent_node_list)
            {
                //adjacent.G = StartMoveCost(current_node.G);
                adjacent.H = HeuristicCost(goal_node.Pos, adjacent.Pos);
                adjacent.F = /*adjacent.G + */adjacent.H;

                bool list_open_add = true;

                if (HeuristicCost(goal_node.Pos, current_node.Pos) < HeuristicCost(goal_node.Pos, adjacent.Pos))//隣接ノードのゴール距離が離れていれば追加しない。
                {
                    list_open_add = false;
                }

                if (map.Map[adjacent.Pos.y, adjacent.Pos.x] != (int)MapMass.Mapinfo.NONE)
                {
                    list_open_add = false;
                }

                //クローズリストに同じノードがあったら、オープンリストに追加しない。
                foreach (var close in close_list)
                {
                    if (close.Pos == adjacent.Pos)//同じノード
                    {
                        //Debug.Log("クローズリストに同じノードがあります。" + " クローズリスト[" + close.Pos.y + "," + close.Pos.x + "]" + "隣接ノード位置 [" + adjacent.Pos.y + "," + adjacent.Pos.x + "]");
                        list_open_add = false;
                    }
                }

                //オープンリストにすでに子がある時
                foreach (var open in open_list)
                {
                    if (open.Pos == adjacent.Pos/* && adjacent.G > open.G*/)//同じノード
                    {
                        //Debug.Log("Openlistに同じノードがあります。");
                        list_open_add = false;
                    }
                }

                if (list_open_add) open_list.Add(adjacent);//オープンリストに追加
            }

            //隣接ノード探索終了

            //for (int open = 0; open < open_list.Count; open++)
            //{
            //    Debug.Log(a_count + "回目Openの中身 " + "[" + open_list[open].Pos.y + "]" + "[" + open_list[open].Pos.x + "]" + "コスト" + open_list[open].F + "合計オープンリスト" + open_list.Count);
            //}

            foreach (var selectnode in open_list)
            {
                //オープンリストの中で一番小さいノードを選ぶ
                if (selectnode.F <= current_node.F /*&& enemy.Abnormal_condition != EnemyBase.AbnormalCondition.Ice*/)
                {
                    current_node = selectnode;//現在のノードを小さいノードに代入
                    //Debug.Log("小さかったノード" + a_count + "回目 " + "現在地" + "[" + current_node.Pos.y + "]" + "[" + current_node.Pos.x + "]" + "合計コスト" + current_node.F);
                }
            }


        }

        //Debug.Log("返す時の現在値" + a_count + "回目 " + "次の地点移動" + "[" + current_node.Pos.y + "]" + "[" + current_node.Pos.x + "]" + current_node.F);
        //DrawMap();
        return current_node.Pos;
        //クローズの中身をみるデバッグ
    }

    //ゴールをチェックしてくれる関数
    bool GoolCheck(Node current, Node goal)
    {
        if (current.Pos == goal.Pos)
        {
            return true;
        }
        return false;
    }


    ////スタートの移動コスト
    //int StartMoveCost(int g)
    //{
    //    return g + 1;
    //}


    //推定コスト
    int HeuristicCost(Vector2Int goal, Vector2Int current)//基準nodeとゴールの位置を渡す。
    {
        return ((goal.y - current.y) * (goal.y - current.y)) + ((goal.x - current.x) * (goal.x - current.x));
    }
}

[System.Serializable]
public class Node //ノードクラス
{

    //位置
    Vector2Int pos;//マスの要素数を入れる。X, Y

    //int g;//starとから移動コスト
    int h;//推定コスト
    int f;//スコア値

    Node parentNode;

    //コンストラクタ
    public Node(Node node, Vector2Int pos)
    {
        ParentNode = node;//親ノードを入れる変数
        Pos = pos;

        //g = 0;
        h = 0;
        f = 1000;
    }

    //プロパティ
    //public int G { get => g; set => g = value; }
    public int H { get => h; set => h = value; }
    public int F { get => f; set => f = value; }
    public Vector2Int Pos { get => pos; set => pos = value; }//ノードgrid
    internal Node ParentNode { get => parentNode; set => parentNode = value; }
}