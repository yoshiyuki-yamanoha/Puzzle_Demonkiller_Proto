using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Efe_Play : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Efe;
    void Start()
    {
        Destroy(gameObject, 1.5f);
    }
}
