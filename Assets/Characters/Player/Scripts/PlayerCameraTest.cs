using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCameraTest : TrunManager
{
    //public
    private GameObject mainCamera;
    private GameObject subCamera;
    private bool MSCameraflag;
    private GameObject selector;
    private Vector3 subCamePos;

    private Vector3 start;
    private Vector3 target;

    private float startTime;
    private float distance;//範囲


    public float Speed = 0.2f;
    public float JourneyLength = 10f;

    TrunManager trunMgr;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        subCamera = GameObject.Find("SubCamera");

        subCamera.SetActive(false);
        MSCameraflag = false;
        trunMgr = GameObject.Find("TrunManager").GetComponent<TrunManager>();

        selector = GameObject.Find("Selector");
        start = selector.transform.position;
        subCamera.transform.localPosition = new Vector3(start.x + 0f, start.y + 25f,start.z - 60f);


        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //subCamePos = subCamera.transform.position;
        //target = new Vector3(selector.transform.position.x, selector.transform.position.y + 25, selector.transform.position.z - 27);
        //MagicCameraOn();
        ////subCamera.transform.position = new Vector3(selector.transform.position.x,( selector.transform.position.y + 25), (selector.transform.position.z -27));
        //if ((target.x > subCamePos.x + distance && target.x < subCamePos.x - distance) && (target.z > subCamePos.z + distance && target.z < subCamePos.z - distance))
        //{
        //    subCamera.transform.position = Vector3.Lerp(start, target, CalcMoveRatio());
        //    start = subCamera.transform.position;
        //}
    }

    public void MagicCameraOn()//魔法を撃つときにカメラを魔法を撃つときのカメラを起動
    {
        //subCamera.transform.localPosition = new Vector3(0f, 25f, -60f);
        if (/*(Input.GetButtonDown("Fire3") && MSCameraflag == false)||*/ trunMgr.GetTrunPhase() == TrunPhase.MagicAttack)
        {
            //mainCamera.SetActive(false);
            subCamera.SetActive(true);
            //MSCameraflag = true;
        }
        else if (/*(Input.GetButtonDown("Fire3") && MSCameraflag == true)||*/ trunMgr.GetTrunPhase() == TrunPhase.Enemy)
        {
            //mainCamera.SetActive(true);
            subCamera.SetActive(false);
            //MSCameraflag = false;
        }
    }
    public void MagicCameraMove()
    {

    }
    float CalcMoveRatio()
    {
        var distCovered = (Time.time - startTime) * Speed;
        return distCovered / JourneyLength;
    }

}
