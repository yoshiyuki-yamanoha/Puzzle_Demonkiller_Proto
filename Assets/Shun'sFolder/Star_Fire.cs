using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star_Fire : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject Magic_Obj;
    private int Level = 0;
    private GameObject tage;
    private float damege_num = 2.0f;

    private bool TrunInit;      //ターンが切り替わったタイミング用


    void Start()
    {
        TrunInit = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void set_Init(int _Level, GameObject _tage)
    {
        Level = _Level;
        tage = _tage;
    }

    public void Magic_Create(int _Level, GameObject[] _tage)
    {
        Level = _Level;
        GameObject[] tages = _tage;

        foreach(GameObject tage in tages)
        {
            GameObject Magic = Instantiate(Magic_Obj, tage.transform.position, transform.rotation,transform);
        }
    }

    private void ene_Damege(GameObject ene)
    {
        EnemyBase e = ene.GetComponent<EnemyBase>();
        if (e != null)
        {
            e.Damage(damege_num);
        }

    }
}
