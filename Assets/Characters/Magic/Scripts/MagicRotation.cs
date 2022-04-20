using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRotation : MonoBehaviour
{
    float X = 0, Y = 0;
    public bool tokei;
    // Start is called before the first frame update
    void Start()
    {

        Y = 90;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tokei == false)
        {
            X += 1;
        }
        else
        {
            X -= 1;
        }

        transform.rotation = Quaternion.Euler(X, Y, 90); // Z軸に10°回転
    }
}
