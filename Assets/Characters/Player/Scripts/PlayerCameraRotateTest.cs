using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRotateTest : MonoBehaviour
{
    [SerializeField, Tooltip("ターゲットオブジェクト")]
    private GameObject TargetObject;

    [SerializeField, Tooltip("回転軸")]
    private Vector3 RotateAxis = Vector3.up;

    [SerializeField, Tooltip("速度係数")]
    private float SpeedFactor = 1f;

    float speed = 1;

    private bool Rflag = false;
    private bool Lflag = false;

    private float MoveSize;
    private float nowRotationY;
    // Start is called before the first frame update
    void Start()
    {
        MoveSize = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TargetObject == null) return;
        if ((Input.GetButtonDown("Cont_L1")&& Lflag == false ) || Lflag == true){
            Lflag = true;
            // 指定オブジェクトを中心に回転する時計回り
            this.transform.RotateAround(
                TargetObject.transform.position,
                RotateAxis,
                360.0f / (speed / SpeedFactor) * Time.deltaTime
                );
            MoveSize += 360.0f / (speed / SpeedFactor) * Time.deltaTime;
            if (MoveSize > 90)
            {
                MoveSize = 0;
                Lflag = false;
            }

        }
        if ((Input.GetButtonDown("Cont_R1")&& Rflag == false )|| Rflag == true) {
            Rflag = true;
            // 指定オブジェクトを中心に回転する反時計回り
            this.transform.RotateAround(
                TargetObject.transform.position,
                RotateAxis,
                360.0f / (-speed / SpeedFactor) * Time.deltaTime
                );
            MoveSize += 360.0f / (speed / SpeedFactor) * Time.deltaTime;
            if (MoveSize > 90)
            {
                MoveSize = 0;
                Rflag = false;
            }
        }
    }
}
