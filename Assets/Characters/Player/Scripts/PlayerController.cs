﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject[] particleObject;

    private Vector3 cameraDefaultPosition = new Vector3(0,15,-12);
    private Vector3 lastMousePosition;
    private Vector3 newAngle = new Vector3(0, 0, 0);

    public float y_rotate, x_rotate, y_reverce, x_reverce;

    public ClearCheck cc;

    public Magichoming Mh;
    public int attackNum = 0;

    //おーぶしゅとく
    [SerializeField] OrbCon s_OrbCon;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera.transform.position = cameraDefaultPosition;
        newAngle = this.transform.localEulerAngles;
        lastMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        newAngle.y += Input.GetAxis("Horizontal") * y_rotate * x_reverce;
        //newAngle.y += (Input.mousePosition.x - lastMousePosition.x) * y_rotate * x_reverce;
        newAngle.x -= Input.GetAxis("Vertical") * x_rotate * y_reverce;
        //newAngle.x -= (Input.mousePosition.y - lastMousePosition.y) * x_rotate * y_reverce;
        //mainCamera.gameObject.transform.localEulerAngles = newAngle;
        lastMousePosition = Input.mousePosition;
        //if (cc.magicPoint > 0)
        //{
        //    if (Input.GetButtonDown("Fire1"))
        //    {
        //        PlayerAttack();
        //        cc.SubMP();
        //    }
        ////}
    }

    public void PlayerAttack()
    {
        attackNum += 1;
        GameObject Magic = Instantiate(particleObject[0], mainCamera.transform.position, transform.rotation);
        Magic.name = "FireMagic"+attackNum;

        //Mh = GameObject.Find("FireMagic").GetComponent<Magichoming>(); 
        //Mh.targetno = attackNum;
        //if (attackNum > 2)
        //{
        //    attackNum = 0;
        //}

    }

    //魔法を生成、番号指定で撃つ魔法を変える。
    public void ShotMagic(GameObject tage) {
        //一番先頭のオーブを取得 0:r 1:b 2:y
        List<int> unko = s_OrbCon.Get_OrbColorInfos();

        //先頭のオーブ
        GameObject Magic = Instantiate(particleObject[unko[0]], mainCamera.transform.position, transform.rotation);

        //魔法とんでいくターゲットを変える。
        Magic.GetComponent<Magichoming>().TargetObject = tage;

    }
}
