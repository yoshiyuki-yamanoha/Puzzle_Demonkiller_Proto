using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCenter : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        //ワールド座標0

        Vector3 oldAngle = transform.eulerAngles;
        transform.LookAt(Vector3.zero);
        transform.rotation = new Quaternion(oldAngle.x, oldAngle.y, transform.rotation.z, 1);
    }
}
