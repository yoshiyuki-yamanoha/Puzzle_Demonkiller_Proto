using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField,Tooltip("キャラの歩行速度")]
    private float walkSpeed;

    [SerializeField, Tooltip("キャラの走行速度")]
    private float runSpeed;

    [SerializeField, Tooltip("プレイヤーの視点感度")]
    private float camSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //視点
        float horiR = Input.GetAxis("RightStickHorizontal") * camSensitivity * Time.deltaTime;
        float vertR = Input.GetAxis("RightStickVertical") * camSensitivity * Time.deltaTime;

        transform.Rotate(new Vector3(0, horiR, 0));


        //移動
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 currentPos = transform.position;
        currentPos.x += hori * walkSpeed * Time.deltaTime;
        currentPos.z += vert * walkSpeed * Time.deltaTime;

        transform.position = currentPos;
        
    }
}
