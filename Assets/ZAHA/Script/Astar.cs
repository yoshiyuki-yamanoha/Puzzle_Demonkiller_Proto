using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    bool one_clear = true;
    MapMass map = null;

    List<Node> adjacent_node_list = new List<Node>();//隣接ノード
    List<Node> open_list = new List<Node>();//オープンリスト 経路候補
    List<Node> close_list = new List<Node>();//クローズリスト　計算終わったら入れるリスト

    Node start_node; //スタートノード
    Node goal_node; //ゴールノード
    Node current_node; //現在のノード

    bool init_astar = true;

    int a_count = 0;//探索カウント

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("MapInstance").GetComponent<MapMass>();
    }

    public Vector2Int astar(Node start, Node goal, int num, bool init_goal)//Astarアルゴリズム
    {

        ////再初期化用
        if (!init_goal && one_clear)
        {
            open_list.Clear();//オープンリストclear
            close_list.Clear();//クローズリストclear
            init_astar = true;//Astar初期化
            one_clear = false;
        }

        //ゴール設定
        goal_node = goal;
        start_node = start;

        ////スタートノード指定
        if (init_astar)
        {
            open_list.Add(start_node);//オープンリストに現在値追加
            current_node = open_list[0];//現在のノードをオープンリスト追加
            init_astar = false;
        }

        for (int i = 0; i < num; i++)
        {
            a_count++;//今何回目のループか見ちゃうやつ

            open_list.Remove(current_node);//オープンリストから現在のノードを削除

            bool close_flg = true;

            foreach (var close in close_list)
            {
                if (close.Pos == current_node.Pos)//同じノード
                {
                    close_flg = false;
                }
            }

            if (close_flg)
            {
                close_list.Add(current_node);//クローズリストに追加
            }

            adjacent_node_list.Clear();//隣接ノードをclear

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
                    continue;
                }

                if (map.Map[adjacent_node_pos.y, adjacent_node_pos.x] != (int)MapMass.Mapinfo.NONE) //mapの移動出来る範囲をみる。//0は移動可能
                {
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
                        list_open_add = false;
                    }
                }

                //オープンリストにすでに子がある時
                foreach (var open in open_list)
                {
                    if (open.Pos == adjacent.Pos/* && adjacent.G > open.G*/)//同じノード
                    {
                        list_open_add = false;
                    }
                }

                if (list_open_add) open_list.Add(adjacent);//オープンリストに追加
            }

            foreach (var selectnode in open_list)
            {
                //オープンリストの中で一番小さいノードを選ぶ
                if (selectnode.F <= current_node.F)
                {
                    current_node = selectnode;//現在のノードを小さいノードに代入
                }
            }
        }

        return current_node.Pos;
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