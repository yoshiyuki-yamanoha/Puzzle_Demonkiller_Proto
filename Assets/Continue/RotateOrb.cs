using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOrb : MonoBehaviour
{
    //回る速度倍率　レベルによって変わる
    public float rotateSpeedMultiply = 1;
    float rotateSpeed = 1f;

    Vector3 rotateVec;

    private void Start()
    {
        rotateVec = Vector3.zero;
    }

    private void FixedUpdate()
    {
        float multi = (rotateSpeedMultiply - 1) * (rotateSpeedMultiply - 1);
        rotateVec.y = rotateSpeed * multi;

        transform.Rotate(rotateVec);
    }
}
