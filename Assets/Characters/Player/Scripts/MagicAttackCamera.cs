using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackCamera : TrunManager
{
    //public
    private GameObject mainCamera;//パズルのカメラ
    private GameObject subCamera;//魔法攻撃のカメラ
    Camera subCame;//うつす優先度変更用
    private GameObject shakeCamera;//揺らすカメラ
    Camera shakeCame;//うつす優先度変更用
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
    public bool moveflag = false;//カメラが動くときのフラグ
    private int soeX, soeY; //添え字

    TrunManager trunMgr;

    float cameraX, cameraY, cameraZ;//カメラの座標いじるよう
    bool shakeCameraInit = true;

    bool attackMagickFlag = false;//魔法攻撃をしたか
    float shakeTime;//揺らす時間

    float magni = 0;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        subCamera = GameObject.Find("SubCamera");
        subCame = subCamera.GetComponent<Camera>();
        shakeCamera = GameObject.Find("ShakeCamera");
        shakeCame = shakeCamera.GetComponent<Camera>();

        MSCameraflag = false;
        trunMgr = GameObject.Find("TrunManager").GetComponent<TrunManager>();

        selector = GameObject.Find("MapInstance").GetComponent<MapMass>();
        subCamera.transform.localPosition = new Vector3(start.x + 0f, start.y + 85/*25f*/, start.z - 62f);
        speed = defSpeed;

        startTime = Time.time;

        subCame.depth = -2;
        shakeCame.depth = -2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (trunMgr.GetTrunPhase() == TrunManager.TrunPhase.MagicAttack)
        {
            if (attackMagickFlag == false)
            {
                subCame.depth = 0;
            }

            (soeX, soeY) = selector.GetMAgicMassSelector();
            selepos = selector.GetGameObjectOfSpecifiedMass(soeX, soeY);
            start = subCamera.transform.position;
            target = new Vector3(selepos.transform.position.x, selepos.transform.position.y + 50/*25*/, selepos.transform.position.z - 57/*27*/);
            MagicCameraOn();
            MagicCameraMove();
        }
        else
        {

            shakeCameraInit = true;
            //subCamera.SetActive(false);//魔法を撃つときのカメラをカメラを見えなくする
            subCame.depth = -2;
        }
        if(attackMagickFlag == true)
        {
            ShakeCamera();
        }
        else
        {
            shakeTimer = 0;
            shakeCamera.transform.position = subCamera.transform.position;
        }



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
            //MSCameraflag = true;
        }
        else if (/*(Input.GetButtonDown("Fire3") && MSCameraflag == true)||*/ trunMgr.GetTrunPhase() == TrunPhase.Enemy)
        {
            if (MSCameraflag == true)
            {
                MSCameraflag = false;
            }
            //mainCamera.SetActive(true);
            //subCamera.SetActive(false);//魔法を撃つときのカメラをカメラを見えなくする

            //speed = defSpeed;//魔法を撃つ時のカメラを止めるときにspeedをリセットする
            //MSCameraflag = false;
        }
    }
    public void MagicCameraMove()
    {
        if (MSCameraflag == true)
        {
            if ((soeY <= 20 && soeY >= 0) || (soeX <= 20 && soeX >= 0))
            {
                subCamera.transform.position = Vector3.Lerp(start, target, CalcMoveRatio());//



                if (subCamera.transform.position == target)
                {
                    moveflag = true;
                }
            }
        }
        //else
        //{
        //    subCamera.transform.position = mainCamera.transform.position;
        //    moveflag = true;
        //}

    }
    public float CalcMoveRatio()
    {
        if (moveflag == true)
        {
            startTime = Time.time;
            if (trunMgr.GetTrunPhase() == TrunPhase.Enemy)
            {
                speed = maAtakSelectSpeed + 1;
            }
            else if (trunMgr.GetTrunPhase() == TrunPhase.MagicAttack)
            {
                speed = maAtakSelectSpeed;//計算用のスピードに魔法を撃つときのカーソルを追いかけるスピードを入れる
            }
            moveflag = false;
        }
        float distCovered = 0;
        distCovered = (Time.time - startTime) * speed;
        return distCovered / JourneyLength;
    }

    Vector3 shakeCameraPos;//カメラを揺らす
    bool shakePlasflag = false;

    float shakeTimer = 0;
    public void ShakeCamera()
    {
        if (shakeCameraInit == true)
        {
            shakeCame.depth = 0;
            subCame.depth = -2;
            shakeTimer = 0;
            shakeCameraInit = false;
        }

        //if(subCamera.transform != shakeCamera.transform)
        //{
        //    cameraX = subCamera.transform.position.x;
        //    cameraY = subCamera.transform.position.y;
        //    cameraZ = subCamera.transform.position.z;
        //    shakeCameraPos = new Vector3(cameraX, cameraY, cameraZ);
        //}

        //if (shakePlasflag == false)
        //{
        //    if (cameraX <= (shakeCameraPos.x - 10))
        //    {
        //        subCamera.transform.position -= new Vector3(-3, 0, 0);
        //    }
        //    else
        //    {
        //        shakePlasflag = true;
        //    }
        //}
        //else if (shakePlasflag == true)
        //{ 

        //    if (cameraX >= (shakeCameraPos.x + 10))
        //    {
        //        subCamera.transform.position += new Vector3(3, 0, 0);
        //    }
        //    else
        //    {
        //        shakePlasflag = false;
        //    }
        //}


        if (attackMagickFlag == true)
        {
            Shake(0.25f, magni);
            shakeTimer += Time.deltaTime;
            if(shakeTimer > shakeTime)
            {
                subCame.depth = 0;
                shakeCame.depth = -2;
                shakeCameraInit = true;
                attackMagickFlag = false;
            }
        }




    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = shakeCamera.transform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y;
           // var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            shakeCamera.transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        shakeCamera.transform.localPosition = pos;
    }
    public void SetAttackMagicTrueFlag()
    {
        Debug.Log("はいった？");
        attackMagickFlag = true;
    }

    public void SetShakeTime(float shake_time)
    {
        shakeTime = shake_time;
    }
    public void SetMagni(float magni_Power)
    {
        magni = (magni_Power * 0.1f);
    }
}
