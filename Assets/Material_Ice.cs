using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_Ice : MonoBehaviour
{
    Material ice_material;
    float des;
    // Start is called before the first frame update
    void Start()
    {
        ice_material = transform.gameObject.GetComponent<Renderer>().material;
        des = ice_material.GetFloat("_Destruction");
        Debug.Log("アイスmaterialの数値" + des);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }
}
