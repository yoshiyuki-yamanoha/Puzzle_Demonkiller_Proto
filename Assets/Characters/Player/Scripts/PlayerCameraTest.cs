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
        subCamera.transform.localPosition = new Vector3(0f, 25f, -60f);
    }

    // Update is called once per frame
    void Update()
    {
        subCamera.transform.localPosition = new Vector3(0f, 25f, -60f);
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
        //if (/*Input.GetButtonDown("Horizontal2") ||*/ Input.GetButtonDown("Fire1"))
        //{
        //    subCamera.transform.localPosition += new Vector3(1,0,0);
        //}
        //subCamera.transform.localPosition += new Vector3(0,0,1);

    }
}
