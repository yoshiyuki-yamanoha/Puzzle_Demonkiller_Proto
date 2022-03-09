using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPointer : MonoBehaviour
{
    [SerializeField] GameObject me;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject pointer;

    Vector3 floorPos;
    Vector3 floorNormal;

    //MarkingPoint MarkP;

    GameObject Marks;

    public GameObject mostNearEnemy = null;


    int i = 1;
    public int Number = 0;
    // Start is called before the first frame update
    void Start()
    {
        //MarkP = GameObject.Find("MarkingPointer").GetComponent<MarkingPoint>();
        floorPos = floor.transform.position;
        floorNormal = floor.transform.up;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //視線と床の交点座標を求める (見ている地点にポインターを出す用)
        var point = GetPositionPointerOnFloor();

        //一番近い敵を探索
        SearchMostNearEnemy();

        //探索した一番近い敵を見つめる
        transform.LookAt(pointer.transform.position);
    }

    //一番近い敵にマークをつける
    public void Marking()
    {
        if (mostNearEnemy)
        {

            //Debug.Log("つけた");
            Marks = Instantiate(pointer, mostNearEnemy.transform.position, Quaternion.identity);
            Marks.name = "MarkingPointer";
            Marks.transform.parent = mostNearEnemy.transform;
            //mostNearEnemy.tag = "MarkedEnemy";
        }
    }

    //一番近い敵を探す関数
    void SearchMostNearEnemy() {
        //一番近い敵をリセット
        mostNearEnemy = null;

        //Enemyタグが付いたオブジェクトを探す
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //一番近い敵との距離を入れるよう
        float shotPos = 9999;

        foreach (GameObject o in enemies)
        {
            if (o.transform.childCount > 0)
                continue;

            //敵とカメラとの距離
            float dis = Vector3.Distance(transform.position, o.transform.position);


            if (dis < shotPos)//一番近い敵
            {
                shotPos = dis;//一番近い敵との距離
                mostNearEnemy = o;
            }

        }
    }

    //視線と床の交点座標を返す
    Vector3 GetPositionPointerOnFloor() {

        Vector3 ret_Pos;
        
        //meの位置
        Vector3 mePos = transform.position;

        //meのベクトル
        Vector3 meVec = transform.forward;

        //内積で高さを求める
        float h = Vector3.Dot(floorNormal, floorPos);

        //現在見ている敵にポインターを合わせる
        ret_Pos = mePos + ((h - Vector3.Dot(floorNormal, mePos)) / (Vector3.Dot(floorNormal, meVec))) * meVec;

        return ret_Pos;
    }
}
