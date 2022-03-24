using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class Magic : MonoBehaviour
{

    [SerializeField] ClearCheck cc;

    //魔法を生成する座標
    [SerializeField] private GameObject magicPosObj;
    Vector3 magicPos;

    //魔法s
    [SerializeField] private GameObject speedDownMagic;
    [SerializeField] private GameObject doubleComboMagic;
    //魔法の名前が未定↓
    [SerializeField] private GameObject yellowMagic;
    [SerializeField] private GameObject lightBlueMagic;
    [SerializeField] private GameObject greenMagic;

    int magicNum = 0;

    private void Start()
    {
        magicPos = magicPosObj.transform.position;
    }

    private void Update()
    {
        //デバッグ用

        //吹き飛ばし
        if (Input.GetKeyDown(KeyCode.F)) {
            BlowOffEnemies(5);
        }

        //遅くする
        if (Input.GetKeyDown(KeyCode.H)) {
            SpeedDown(5);
        }

        //コンボ2倍
        if (Input.GetKeyDown(KeyCode.N)) {
            StartCoroutine(DoubleCombo(5));
        }
    }

    //敵全員を吹き飛ばす
    public void BlowOffEnemies(int num) {
        GameObject[] enes = GameObject.FindGameObjectsWithTag("Enemy");

        Vector3 pow = new Vector3(0, 5000, 50);

        foreach (GameObject o in enes) {
            o.GetComponent<Rigidbody>().AddForce(pow*num);
        }

    }

    //敵全員を遅くする
    public void SpeedDown(float num) {
        GameObject[] enes = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject o in enes) {
            o.GetComponent<NavMeshAgent>().speed /= num;
        }

        GameObject ma = null;
        if(magicNum == 0)
            ma = Instantiate(doubleComboMagic, magicPos, Quaternion.identity);
        if (magicNum == 1)
            ma = Instantiate(speedDownMagic, magicPos, Quaternion.identity);
        //仮置き
        if (magicNum == 2)
            ma = Instantiate(yellowMagic, magicPos, Quaternion.identity);
        if (magicNum == 3)
            ma = Instantiate(lightBlueMagic, magicPos, Quaternion.identity);
        if (magicNum == 4)
            ma = Instantiate(greenMagic, magicPos, Quaternion.identity);
        Destroy(ma, 0.5f);
    }

    //一定時間コンボ2倍
    public IEnumerator DoubleCombo(int num) {

        int time = 600;

        cc.addComboNum += num;

        while (time > 0)
        {
            time--;
            yield return null;
        }

        cc.addComboNum = 1;

        Debug.Log(time);

    }

    //情報を渡す関数
    public void SetJouhou(int num) {


        magicNum = num;
        //if (magicNum > 0) magicNum = 1;
    }
}
