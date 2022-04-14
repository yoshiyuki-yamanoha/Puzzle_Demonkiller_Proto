using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star_Ice : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject Ice;
    [SerializeField]private GameObject Ice_efe;
    private GameObject Stage_mass;

    private float upSpeed = 10.0f;
    private float Max_y = 0.8f;

    private List<GameObject> Ice_objs = new List<GameObject>();
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        child_Move();
    }

    public void Create_IceBergs(GameObject taget)
    {
        int num = (int.Parse(taget.name) / 15);

        Stage_mass = GameObject.Find("MassRoot");
        Ice_objs.Clear();
        for (int i = 0; i < 15; i++)
        {
            StartCoroutine(cre_ice(num ,i));
        }
    }

    IEnumerator cre_ice(int num,int child)
    {
        float delay = (child) * 0.1f;
        yield return new WaitForSeconds(delay);


        int ObjNum = (num * 15) + (14 - child);
        if (0 <= ObjNum || ObjNum > Stage_mass.transform.childCount - 1)
        {
            Vector3 crePos = new Vector3(Stage_mass.transform.GetChild(ObjNum).position.x,
                                           Stage_mass.transform.GetChild(ObjNum).position.y - 2.0f,
                                              Stage_mass.transform.GetChild(ObjNum).position.z);


            GameObject IceChild = Instantiate(Ice, crePos, Quaternion.identity, this.transform);
            GameObject Iceefe = Instantiate(Ice_efe, Stage_mass.transform.GetChild(ObjNum).position, Quaternion.identity);

            Ice_objs.Add(IceChild);

            IceChild.transform.localScale = new Vector3(2.5f, 1.0f, 2.5f);
            Iceefe.transform.localScale = new Vector3(1f, 1.5f, 1f);

            Destroy(IceChild, 3.0f);
            Destroy(Iceefe, 1.5f);
        }
    }

    private void child_Move()
    {
        for(int i=0; i < Ice_objs.Count-1; i++)
        {
            Vector3 pos = Ice_objs[i].transform.localPosition;
            if (Max_y > pos.y)
            {
                pos.y += upSpeed * Time.deltaTime;

                Ice_objs[i].transform.localPosition = pos;
            }
            else
            {
                Ice_objs.Remove(Ice_objs[i]);
            }
        }
    }
}
