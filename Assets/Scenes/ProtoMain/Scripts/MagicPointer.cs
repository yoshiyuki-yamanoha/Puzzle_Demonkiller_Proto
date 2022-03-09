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


    bool flag1;
    bool flag2;
    bool flag3;

    public GameObject mostNearEnemy1 = null;
    public GameObject mostNearEnemy2 = null;
    public GameObject mostNearEnemy3 = null;


    int i = 1;
    public int Number = 0;
    // Start is called before the first frame update
    void Start()
    {
        //MarkP = GameObject.Find("MarkingPointer").GetComponent<MarkingPoint>();
        floorPos = floor.transform.position;
        floorNormal = floor.transform.up;

        flag1 = false;
        flag2 = false;
        flag3 = false;
    }

    // Update is called once per frame
    void Update()
    {
        //meの位置
        Vector3 mePos = transform.position;

        //meのベクトル
        Vector3 meVec = transform.forward;

        float h = Vector3.Dot(floorNormal, floorPos);

        var point = mePos + ((h - Vector3.Dot(floorNormal, mePos)) / (Vector3.Dot(floorNormal, meVec))) * meVec;


        //GameObject mostNearEnemy1 = null;
        //敵のなんたら
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");


            //float uenoPos = 9999;
            //float nextPos = 9999;
            float shotPos = 9999;
            float longPos = 0;

            foreach (GameObject o in enemies)
            {
                float dis = Vector3.Distance(transform.position, o.transform.position);
                //Debug.Log(new Vector3(shotPos, dis, longPos));

                if (dis < shotPos)//一番近い敵
                {
                    shotPos = dis;//一番近い敵との距離
                    mostNearEnemy1 = o;
                    if (flag1 == false)
                    {
                        flag1 = true;
                        Marking();
                        Number = 1;
                        i++;
                    }
                }
                else
                if (dis > shotPos && longPos == 0)//二番目に近い敵
                {
                    longPos = dis;//二番目に近い敵との距離
                    mostNearEnemy2 = o;
                    if (flag2 == false)
                    {
                        flag2 = true;
                        Marking();
                        Number = 2;
                        i++;
                    }
                }
                else
                if (longPos < dis)//一番遠い敵
                {

                    //longPos = dis;
                    mostNearEnemy3 = o;
                    if (flag3 == false)
                    {
                        flag3 = true;
                        Marking();
                        Number = 3;
                        i++;
                    }
                }

            }


            //pointer.transform.position = point;
            //pointer.transform.position = EnemyMove.instance.enemyPosition;

            //pointer.transform.position = mostNearEnemy.transform.position;

            transform.LookAt(pointer.transform);
        }
    }

    void Marking()
    {
            Marks = Instantiate(pointer, gameObject.transform.position, Quaternion.identity);
            Marks.name = "MarkingPointer"+i;
    }
}
