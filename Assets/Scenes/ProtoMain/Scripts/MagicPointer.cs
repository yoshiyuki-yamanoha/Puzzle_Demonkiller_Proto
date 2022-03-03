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
    // Start is called before the first frame update
    void Start()
    {
        floorPos = floor.transform.position;
        floorNormal = floor.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        //meの位置
        Vector3 mePos = transform.position;

        //meのベクトル
        Vector3 meVec = transform.forward;

        float h = Vector3.Dot(floorNormal, floorPos);

        var point = mePos + ((h - Vector3.Dot(floorNormal,mePos)) / (Vector3.Dot(floorNormal, meVec))) * meVec;

        //pointer.transform.position = point;
        pointer.transform.position = EnemyMove.instance.enemyPosition;

        if(EnemyMove.instance.enemyPosition == null)
        {
            pointer.transform.position = EnemyMove.instance.enemyPosition;
        }
    }
}
