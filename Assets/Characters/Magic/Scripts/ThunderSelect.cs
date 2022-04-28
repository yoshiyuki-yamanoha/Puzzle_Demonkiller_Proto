using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class ThunderSelect : MonoBehaviour
{
    List<GameObject> KillEnemyList_Thunder = new List<GameObject>();
    LightningBoltScript lb;

    [SerializeField] GameObject ElectricCurrent;
    public GameObject Electricobj;
    public GameObject[] test = new GameObject[5];
    public int now = 0;
    public int next = 1;
    // Start is called before the first frame update
    void Awake()
    {
        now = 0;
        next = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ThunderSelecter()
    {
        //Electricobj = Instantiate(ElectricCurrent, transform.position, transform.rotation);
        lb = Electricobj.GetComponent<LightningBoltScript>();
        lb.StartObject = KillEnemyList_Thunder[now];
        lb.EndObject = KillEnemyList_Thunder[next];
        //lb.StartObject = test[now];
        //lb.EndObject = test[next];
        if(next <= KillEnemyList_Thunder.Count)
        {
            Invoke(nameof(ThunderSelecter_Add), 1.0f);
        }
        else
        {
            Thunder_EnemyDamage();

        }
        //ThunderSelecter_Add();
    }

    public void ThunderSelecter_Add()
    {
        //Destroy(Electricobj);
        now++;
        next++;
        ThunderSelecter();
    }

    public void Set_List(GameObject[] objs)
    {
        foreach(GameObject o in objs)
        {
            KillEnemyList_Thunder.Add(o);
        }
        ThunderSelecter();
    }

    public void Thunder_EnemyDamage()
    {

    }
}
