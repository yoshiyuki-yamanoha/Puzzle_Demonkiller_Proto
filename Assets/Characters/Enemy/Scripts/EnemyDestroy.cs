using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {      
        if (other.CompareTag("Magic"))
        {
            Destroy(gameObject);
        }
    }
}
