using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicActive : MonoBehaviour
{
    public GameObject particleObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown("a"))
        {
            Instantiate(particleObject, this.transform.position,transform.rotation);

        }
    }
}
