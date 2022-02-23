using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magichoming : MonoBehaviour
{
    [SerializeField] GameObject Exp;

    private GameObject TargetObject;
    private Vector3 TargetPos;
    // Start is called before the first frame update
    void Start()
    {
        TargetObject = GameObject.Find("Pointer");
        TargetPos = TargetObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPos, 0.01f);

        transform.LookAt(TargetObject.transform);

        if(Vector3.Distance(transform.position,TargetPos) < 10.0f)
        {
            GameObject Explo = Instantiate(Exp, transform.position,Quaternion.identity);
            Destroy(gameObject);
            Destroy(Explo, 1.0f);
        }
    }
}
