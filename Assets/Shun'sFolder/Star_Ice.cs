using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star_Ice : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject Ice;
    [SerializeField]private GameObject Ice_efe;
    [SerializeField]private GameObject Stage_mass;

    private float upSpeed = 10.0f;
    private float Max_y = 0.8f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Create_IceBergs(4);
        }
        child_Move();
    }

    public void Create_IceBergs(int num)
    {
        Transform parent_obj = Stage_mass.transform.GetChild(num);

        for(int i = 0; i < parent_obj.childCount; i++)
        {
            StartCoroutine(cre_ice(num, i));
        }
    }

    IEnumerator cre_ice(int child, int g_child)
    {
        float delay = g_child * 0.1f;
        yield return new WaitForSeconds(delay);
        int num = Stage_mass.transform.GetChild(child).childCount - g_child;
        Vector3 crePos = new Vector3(Stage_mass.transform.GetChild(child).GetChild(num).position.x,
                                        Stage_mass.transform.GetChild(child).GetChild(num).position.y - 2.0f,
                                           Stage_mass.transform.GetChild(child).GetChild(num).position.z);


        GameObject IceChild = Instantiate(Ice, crePos, Quaternion.identity, this.transform);
        GameObject Iceefe = Instantiate(Ice_efe, Stage_mass.transform.GetChild(child).GetChild(num).position, Quaternion.identity);
        IceChild.transform.localScale = new Vector3(2.5f, 1.0f, 2.5f);
        Iceefe.transform.localScale = new Vector3(1f, 1.5f, 1f);
        Destroy(IceChild, 3.0f);
        Destroy(Iceefe, 1.5f);
    }

    private void child_Move()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Vector3 pos = transform.GetChild(i).transform.localPosition;
            if (Max_y > pos.y)
            {
                pos.y += upSpeed * Time.deltaTime;

                transform.GetChild(i).transform.localPosition = pos;
            }
        }
    }
}
