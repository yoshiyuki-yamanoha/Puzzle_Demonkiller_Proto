using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemytest : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 OriPos;
    [SerializeField] float MoveSpeed = 1.0f;
    void Start()
    {
        OriPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(0, 0, -MoveSpeed * Time.deltaTime);
    }

    public void Damege()
    {
        transform.position = OriPos;
    }
}
