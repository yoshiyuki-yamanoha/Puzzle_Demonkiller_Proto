﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePointer : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 power;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(power);
    }
}
