using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("子のscale" + transform.localScale);
        //Debug.Log("親のscale" + transform.root.localScale);
        //Vector3 scalesize = new Vector3(
        //        transform.localScale.x / transform.root.localScale.x,
        //        transform.localScale.y / transform.root.localScale.y,
        //        transform.localScale.z / transform.root.localScale.z
        //);

        //transform.localScale = scalesize;

        //Debug.Log("scaleサイズ " + scalesize);
    }

    private void FixedUpdate()
    {
        gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }
}
