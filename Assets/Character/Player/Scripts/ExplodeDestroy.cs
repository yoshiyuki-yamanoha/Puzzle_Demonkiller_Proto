using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeDestroy : MonoBehaviour
{


    
    void Update()
    {
        int livingChild = this.gameObject.transform.childCount;
        if (livingChild == 0)
            Destroy(this.gameObject);
        
    }
}
