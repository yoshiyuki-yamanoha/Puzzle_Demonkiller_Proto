using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakIce : MonoBehaviour
{
    Vector3 rotate = new Vector3(0,0,0);
    //どこの氷か
    public bool left;
    public bool right;
    public bool front;
    public bool back;
    // Start is called before the first frame update
    void Start()
    {
        if(left == true)
        {
            rotate = new Vector3(0, 0, 12);
        }
        if (right == true)
        {
            rotate = new Vector3(0, 0, -12);
        }
        if (front == true)
        {
            rotate = new Vector3(12, 0, 0);
        }
        if(back == true)
        {
            rotate = new Vector3(-12, 0, 0);
        }
        //rotate += new Vector3(0,0,30);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition += new Vector3(0.0f, 0.01f, 0.0f);
        //this.transform.Rotate(rotate);
        transform.rotation = Quaternion.Euler(rotate);
    }
    
}