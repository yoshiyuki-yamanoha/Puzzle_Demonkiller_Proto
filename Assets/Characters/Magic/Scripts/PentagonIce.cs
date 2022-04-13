using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonIce : MonoBehaviour
{
    [SerializeField] GameObject IceWall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeIceWall(GameObject tage)
    {
        Instantiate(IceWall, tage.transform.position, Quaternion.identity);
    }
    public void EnemyFreeze()
    {

    }
}
