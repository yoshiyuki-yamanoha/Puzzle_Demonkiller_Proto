using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkingPoint : MonoBehaviour
{
    public GameObject TargetObj;
    public int PointNumber;

    MagicPointer MagicP;
    // Start is called before the first frame update
    void Start()
    {
        MagicP = GameObject.Find("Main Camera").GetComponent<MagicPointer>();
        PointNumber = MagicP.Number;
    }

    // Update is called once per frame
    void Update()
    {
        //if (PointNumber == 1)
        //{
        //    TargetObj = MagicP.mostNearEnemy1;
        //    //Debug.Log(PointNumber);
        //    gameObject.transform.position = TargetObj.transform.position;
        //}
        //if (PointNumber == 2)
        //{
        //    TargetObj = MagicP.mostNearEnemy2;
        //    //Debug.Log(MagicP.mostNearEnemy);
        //    gameObject.transform.position = TargetObj.transform.position;
        //}
        //if (PointNumber == 3)
        //{
        //    TargetObj = MagicP.mostNearEnemy3;
        //    //Debug.Log(MagicP.mostNearEnemy);
        //    gameObject.transform.position = TargetObj.transform.position;
        //}
    }
}
