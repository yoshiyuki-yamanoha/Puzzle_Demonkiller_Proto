using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRotation : MonoBehaviour
{
    float X = 0, Y = 0, Z = 0;
    public bool tokei;
    float Timer = 0f;
    // Start is called before the first frame update
    void Start()
    {

       // Y = 90;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Timer += Time.deltaTime;

        //if(Timer > 2)
        //{
        //    if (tokei == false)
        //    {
        //        tokei = true;
        //        Timer = 0;
        //    }
        //    else
        //    {
        //        tokei = false;
        //        Timer = 0;
        //    }
        //}

        if (tokei == false)
        {
            Z += 1;
        }
        else
        {
            Z -= 1;
        }

        transform.rotation = Quaternion.Euler(X, Y, Z); // Z軸に10°回転
    }
}
