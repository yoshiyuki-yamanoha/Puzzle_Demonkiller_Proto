using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_sel : MonoBehaviour
{
    [SerializeField, Range(1, 3)] int test_Level = 1;
    [SerializeField, Range(0, 5)] int test_type = 1;
    [SerializeField, Range(0, 10)] int cre_X = 0;
    [SerializeField, Range(0, 14)] int cre_Y = 0;

    [SerializeField] private GameObject Magic;
    [SerializeField]private GameObject StartPos;

    [SerializeField] private GameObject Stage_mass;
    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Vector3 pos = StartPos.transform.position;
        //    GameObject mag = Instantiate(Magic, pos,Quaternion.identity);

        //    int targetNum = ((cre_X * 15) + (cre_Y % 15));

        //    GameObject tage = Stage_mass.transform.GetChild(targetNum).gameObject;
        //    mag.GetComponent<Shoot_The_at_Enemy_Magic>().Set_Ini(tage, test_Level, test_type);
        //}
    }
}
