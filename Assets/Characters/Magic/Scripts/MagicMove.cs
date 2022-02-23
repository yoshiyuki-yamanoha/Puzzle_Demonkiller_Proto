using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMove : MonoBehaviour
{

    public GameObject particleObject;

    [Header("移動速度")]
    public float MoveSpeed;

    [Header("移動の向き")]
    public bool X;
    public bool Y;
    public bool Z;
    [Header("向きのマイナス化")]
    public bool Reverse;

    [Header("発動フラグ")]
    public bool MagicActivateFlag;

    //[Header("Rotationてすと")]
    //public Quaternion Rote;
    // Start is called before the first frame update
    void Start()
    {
        if (Reverse == true)
        {
            MoveSpeed *= -1;
            if (X == true)
            {//-X方向
                transform.rotation = new Quaternion(0, 0, -1, 1);
            }
            if (Y == true)
            {//-Y方向
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            if (Z == true)
            {//-Z方向
                transform.rotation = new Quaternion(0, 1, 1, 0);
            }
        }
        else
        {
            if (X == true)
            {//+X方向
                transform.rotation = new Quaternion(0, 0, 180, 180);
            }
            if (Y == true)
            {//＋Y方向
                transform.rotation = new Quaternion(0, 0, 1, 0);
            }
            if (Z == true)
            {//+Z方向
                transform.rotation = new Quaternion(0, -1, 1,0);
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (X == true)
        {
            transform.position += new Vector3(MoveSpeed, 0, 0);
        }
        if(Y == true){
            transform.position += new Vector3(0, MoveSpeed, 0);
        }
        if(Z == true){
            transform.position += new Vector3(0, 0, MoveSpeed);
        }
        //if(Input.GetKey(KeyCode.A))
        //{
        //    MagicActivateFlag = true;
        //}
        if(MagicActivateFlag == true)
        {
            transform.position = new Vector3(-7, 0, -3);
            MagicActivateFlag = false;
        }
        if (Reverse == true)
        {
            MoveSpeed *= -1;
            if (X == true)
            {//-X方向
                transform.rotation = new Quaternion(0, 0, -1, 1);
            }
            if (Y == true)
            {//-Y方向
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            if (Z == true)
            {//-Z方向
                transform.rotation = new Quaternion(0, 1, 1, 0);
            }
        }
        else
        {
            if (X == true)
            {//+X方向
                transform.rotation = new Quaternion(0, 0, 180, 180);
            }
            if (Y == true)
            {//＋Y方向
                transform.rotation = new Quaternion(0, 0, 1, 0);
            }
            if (Z == true)
            {//+Z方向
                transform.rotation = new Quaternion(0, -1, 1, 0);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //タグで探す
        //if (other.CompareTag("Player"))
        //{
        //    Destroy(this.gameObject);
        //}
        //名前で探す
        if (GameObject.Find("Enemy(Kari)"))
        {

            Instantiate(particleObject, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
