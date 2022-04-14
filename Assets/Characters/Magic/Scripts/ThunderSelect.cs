using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class ThunderSelect : MonoBehaviour
{
    List<GameObject> KillEnemyList_Thunder;
    LightningBoltScript lb;

    [SerializeField] GameObject ElectricCurrent;
    GameObject Electricobj;
    public GameObject[] test = new GameObject[5];
    public int now, next;
    // Start is called before the first frame update
    void Start()
    {
        now = 0;
        next = now + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ThunderSelecter();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ThunderSelecter_Add();
        }

    }
    public void ThunderSelecter()
    {
        //Electricobj = Instantiate(ElectricCurrent, transform.position, transform.rotation);
        lb = Electricobj.GetComponent<LightningBoltScript>();
        lb.StartObject = KillEnemyList_Thunder[now];
        lb.EndObject = KillEnemyList_Thunder[next];
        //lb.StartObject = test[now];
        //lb.EndObject = test[next];
    }

    public void ThunderSelecter_Add()
    {
        Destroy(Electricobj);
        now++;
        next++;
        ThunderSelecter();
    }

    public void Set_List(GameObject[] objs)
    {
        foreach(GameObject obj in objs)
        {
            KillEnemyList_Thunder.Add(obj);
        }
    }
}
