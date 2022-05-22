using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotographEnemyCamera : MonoBehaviour
{
    Vector3 corePos = new Vector3(47.5f, 8.5f, -47.5f);//コアの上の座標
    Vector3 puzzlePos = new Vector3(47.5f, 43.8f, -132);//ステージの真ん中(パズルするときのいる位置)

    Camera photoCame;//優先度変更用
    [SerializeField, Tooltip("ターゲットオブジェクト")]
    private GameObject TargetObject;

    [SerializeField, Tooltip("回転軸")]
    private Vector3 RotateAxis = Vector3.up;

    [SerializeField, Tooltip("速度係数")]
    private float SpeedFactor = 1f;

    float tDistance;//ターゲットとの距離

    float speed = 50;

    bool cTarget = false;

    private bool Rflag = false;
    private bool Lflag = false;

    private float MoveSize;
    private float nowRotationY;
    private void Start()
    {
        photoCame = this.gameObject.GetComponent<Camera>();
        photoCame.depth = 1;
    }

    private void FixedUpdate()
    {
        tDistance = Vector3.Distance(transform.position, TargetObject.transform.position);
        MoveSetting();
        Move();
        Rotation();
    }
    void MoveSetting()
    {
        if (cTarget == true)
        {
            transform.LookAt(TargetObject.transform.position);
        }
    }
    void Move()
    {
        //左スティック
        float hori = Input.GetAxis("Horizontal");//スティックの入力を取っている
        float vert = Input.GetAxis("Vertical");//以下


        ////移動
        if (tDistance >= 1)
        {
            if (vert >= 0.5f) transform.position += transform.forward * speed * Time.deltaTime;//↑
        }
        if (vert <= -0.5f) transform.position -= transform.forward * speed * Time.deltaTime;//↓
        if (hori >= 0.5f) transform.position += transform.right * speed * Time.deltaTime;//→
        if (hori <= -0.5f) transform.position -= transform.right * speed * Time.deltaTime;//←

        //右スティック
        float horiR = Input.GetAxis("Horizontal2");
        float vertR = Input.GetAxis("Vertical2");

        //Debug.Log(transform.up + " UP");
        //Debug.Log(transform.forward + " FORWARD");
        //Debug.Log(transform.right + " RIGHT");

        //if (cForward == true)
        //{
        //    if (tDistance >= 1)
        //    {
        //        if (vertR >= 0.5f) transform.position += transform.forward * speed * Time.deltaTime;//↑
        //    }
        //    if (vertR <= -0.5f) transform.position -= transform.forward * speed * Time.deltaTime;//↓
        //}
        //else
        //{
        if (cTarget == true)
        {
            if (transform.up.z < 0.9f && transform.up.y > 0.4f)
            {
                if (vertR >= 0.5f) transform.position += transform.up * speed * Time.deltaTime;//↑
            }
            if (transform.position.y > 8.5f)
            {
                if (vertR <= -0.5f) transform.position -= transform.up * speed * Time.deltaTime;//↓
            }
        }
        else
        {
            //if (vertR >= 0.5f) transform.Rotate(new Vector3(-5, 0, 0));
            //if (vertR <= -0.5f) transform.Rotate(new Vector3(5, 0, 0));

            //if (horiR >= 0.5f) transform.Rotate(new Vector3(0, 5, 0));
            //if (horiR <= -0.5f) transform.Rotate(new Vector3(0, -5, 0));

            if (vertR >= 0.5f) transform.eulerAngles += new Vector3(-5, 0, 0);
            if (vertR <= -0.5f) transform.eulerAngles += new Vector3( 5, 0, 0);

            if (horiR >= 0.5f) transform.eulerAngles += new Vector3( 0, 5, 0);
            if (horiR <= -0.5f) transform.eulerAngles += new Vector3( 0, -5, 0);
        }

        //}
        //if (horiR >= 0.5f) transform.position += transform.right * speed * Time.deltaTime;//→
        //if (horiR <= -0.5f) transform.position -= transform.right * speed * Time.deltaTime;//←
    }
    void Rotation()
    {
        //if (TargetObject == null) return;
        if ((Input.GetButtonDown("Cont_L1")/* && Lflag == false) || Lflag == true*/))
        {
            cTarget = false;
            //Lflag = true;
            if (speed < 100)
            {
                speed += 1f;
            }
            else
            {
                speed = 100;
            }
            //// 指定オブジェクトを中心に回転する時計回り
            //this.transform.RotateAround(
            //    TargetObject.transform.position,
            //    RotateAxis,
            //    360.0f / (speed / SpeedFactor) * Time.deltaTime
            //    );
            //MoveSize += 360.0f / (speed / SpeedFactor) * Time.deltaTime;
            //if (MoveSize > 90)
            //{
            //    MoveSize = 0;
            //    Lflag = false;
            //}

        }
        if ((Input.GetButtonDown("Cont_R1")/* && Rflag == false) || Rflag == true*/))
        {
            cTarget = true;
            //Rflag = true;
            if (speed > 5)
            {
                speed -= 1f;
            }
            else
            {
                speed = 5;
            }
            //// 指定オブジェクトを中心に回転する反時計回り
            //this.transform.RotateAround(
            //    TargetObject.transform.position,
            //    RotateAxis,
            //    360.0f / (-speed / SpeedFactor) * Time.deltaTime
            //    );
            //MoveSize += 360.0f / (speed / SpeedFactor) * Time.deltaTime;
            //if (MoveSize > 90)
            //{
            //    MoveSize = 0;
            //    Rflag = false;
            //}
        }
    }
}
