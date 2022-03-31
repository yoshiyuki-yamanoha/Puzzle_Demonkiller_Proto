using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] float maxhp;

    public float Maxhp { get => maxhp; }
    public float Hp { get => hp; set => hp = value; }


    // Start is called before the first frame update
    void Start()
    {
        Hp = Maxhp;
    }

    void FixedUpdate()
    {

        if (HPCheck(Hp))
        {
            Debug.Log("消えろー");
            Destroy(this.gameObject);
        }
    }

    bool HPCheck(float hp)
    {
        if(Hp <= 0)
        {
            Hp = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wepon"))
        {
            Debug.Log("敵の攻撃あたったー");
            hp -= 10;
        }
    }
}
