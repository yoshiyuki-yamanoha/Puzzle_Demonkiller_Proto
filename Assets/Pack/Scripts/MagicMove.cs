using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float MoveSpeed = 2.0f;
    void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(0, 0, MoveSpeed * Time.deltaTime);
    }


}
