using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMagic : MonoBehaviour
{

    [SerializeField] private GameObject Magic;

    List<GameObject> Enemy_List = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateMagic()
    {
        Instantiate(Magic, transform.position, Quaternion.identity);
    }


    public void Enelist_Add(GameObject obj)
    {
        Enemy_List.Add(obj);
    }
    
    public void Enelist_Delete(GameObject obj)
    {
        //Enemy_List.(obj);
    }
}
