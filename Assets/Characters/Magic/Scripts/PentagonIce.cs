using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonIce : MonoBehaviour
{
    [SerializeField] GameObject IceWall;

    Enemy enemy;
    public GameObject testIce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(testIce != null)
        {
            Debug.Log("お前を殺す");
            //EnemyFreeze();
        }
    }

    public void MakeIceWall(GameObject tage)
    {
        Instantiate(IceWall, tage.transform.position, Quaternion.identity);
    }
    //public void EnemyFreeze(GameObject e)
    //{
    //    Debug.Log("this is " + e);
    //    EnemyBase eneb = e.transform.GetComponent<EnemyBase>();
    //    eneb.Abnormal_condition = EnemyBase.AbnormalCondition.Ice;
    //}

    ////enemy = GameObject.Find("Demon0").GetComponent<Enemy>();
    //List<GameObject> el = new List<GameObject>();
    ////el = りょうがから敵をもらう♥
    //foreach(GameObject ene in el)
    //{
    //    if (ene.transform.GetComponent<Enemy>() == null)
    //        continue;

    //    ene.transform.GetComponent<Enemy>().enabled = false;
    //}
}
