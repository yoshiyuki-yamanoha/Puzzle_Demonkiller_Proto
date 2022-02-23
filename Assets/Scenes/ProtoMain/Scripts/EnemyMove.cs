using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    public GameObject target;private bool inArea = false;
    public float chasespeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();

        }

        if (target.activeInHierarchy==false)
        {
            //GetComponent<Renderer>().material.color = origColor;
        }

        if (inArea==true&&target.activeInHierarchy==true)
        {
            agent.destination = target.transform.position;
            EneChasing();
        }
    }

    void GotoNextPoint()
    {
        if (points.Length == 0) return;

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            inArea = true;
            target = other.gameObject;
            EneChasing();
            Destroy(gameObject);
        }   
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inArea = false;
            GotoNextPoint();
        }
    }

    public void EneChasing()
    {
        transform.position += transform.forward * chasespeed;
    }

}
