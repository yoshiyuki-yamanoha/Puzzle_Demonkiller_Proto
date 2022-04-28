using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackCamera : TrunManager
{
    //public
    private GameObject mainCamera;
    private GameObject subCamera;
    private bool MSCameraflag;
    private GameObject selepos;//セレクターの位置情報
    private MapMass selector;
    //private Vector3 subCamePos;

    private Vector3 start;
    private Vector3 target;

    public float startTime;//開始時間
    private float distance = 15;//範囲

    private float defSpeed = 1f;//Defaultのスピード
    private float maAtakSelectSpeed = 5f;//魔法を撃つときのカメラのスピード
    private float speed;//計算用のスピード

    public float JourneyLength = 10f;
    bool moveflag = false;
    private int soeX, soeY; //添え字

    TrunManager trunMgr;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        subCamera = GameObject.Find("SubCamera");

        MSCameraflag = false;
        trunMgr = GameObject.Find("TrunManager").GetComponent<TrunManager>();

        selector = GameObject.Find("MapInstance").GetComponent<MapMass>();
        subCamera.transform.localPosition = new Vector3(start.x + 0f, start.y + 85/*25f*/, start.z - 62f);
        speed = defSpeed;

        startTime = Time.time;

        subCamera.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        (soeX, soeY) = selector.GetMAgicMassSelector();
        selepos = selector.GetGameObjectOfSpecifiedMass(soeX, soeY);
        start = subCamera.transform.position;
        target = new Vector3(selepos.transform.position.x, selepos.transform.position.y + 25, selepos.transform.position.z - 27);
        MagicCameraOn();
        MagicCameraMove();



    }

    public void MagicCameraOn()//魔法を撃つときにカメラを魔法を撃つときのカメラを起動
    {
        if (/*(Input.GetButtonDown("Fire3") && MSCameraflag == false)||*/ trunMgr.GetTrunPhase() == TrunPhase.MagicAttack)
        {
            if (MSCameraflag == false)
            {
                //subCamera.transform.localPosition = new Vector3(selector.transform.position.x, selector.transform.position.y + 25, selector.transform.position.z - 27);

                MSCameraflag = true;
            }
            //if (MSCameraflag == true)
            //{
            //    mainCamera.transform.position = Vector3.Lerp(start, target, CalcMoveRatio());
            //}
            //mainCamera.SetActive(false);
            subCamera.SetActive(true);
            //MSCameraflag = true;
        }
        else if (/*(Input.GetButtonDown("Fire3") && MSCameraflag == true)||*/ trunMgr.GetTrunPhase() == TrunPhase.Enemy)
        {
            if (MSCameraflag == true)
            {
                MSCameraflag = false;
            }
            //mainCamera.SetActive(true);
            subCamera.SetActive(false);//魔法を撃つときのカメラをカメラを見えなくする

            speed = defSpeed;//魔法を撃つ時のカメラを止めるときにspeedをリセットする
            //MSCameraflag = false;
        }
    }
    public void MagicCameraMove()
    {
        if (MSCameraflag == true)
        {
            if ((soeY <= 20 && soeY >= 0) || (soeX <= 20 && soeX >= 0))
            {
                if (moveflag == true)
                {
                    startTime = Time.time;
                    moveflag = false;
                    speed = maAtakSelectSpeed;//計算用のスピードに魔法を撃つときのカーソルを追いかけるスピードを入れる
                }
                subCamera.transform.position = Vector3.Lerp(start, target, CalcMoveRatio());
                if (subCamera.transform.position == target)
                {
                    moveflag = true;
                }
            }
        }
        else
        {
            subCamera.transform.position = mainCamera.transform.position;
            moveflag = true;
        }

    }
    public float CalcMoveRatio()
    {
        float distCovered = 0;
        distCovered = (Time.time - startTime) * speed;
        return distCovered / JourneyLength;
    }
}
