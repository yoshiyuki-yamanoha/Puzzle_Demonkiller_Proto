using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] public GameObject fx_explosion = null;
    [SerializeField] public GameObject targetEnemy = null;
    [SerializeField] private Transform child = null;

    Vector3 targetPos;
    float velocity;


    void Start()
    {
        child = this.transform.GetChild(0).gameObject.transform;
        //StopAnimation();
        //Debug.Log(this.gameObject);
        //Debug.Log(Time.time);


        targetPos = new Vector3(targetEnemy.transform.position.x, 0f, targetEnemy.transform.position.z);
        velocity = 0.05f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FallMeteorMagic();
        //MeteorCollision();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Floor")
    //    {
    //        Debug.Log("collision");

    //        HitMagic();
    //    }
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            //Debug.Log("Fcolli");
            //Debug.Log(Time.time);

            HitMagic();
        }
    }

    private void FallMeteorMagic()
    {
        this.transform.rotation.SetLookRotation(targetPos, Vector3.up);
        //this.transform.position = Vector3.Lerp(this.transform.position, targetPos, velocity);
    }

    private void StopAnimation()
    {
        //foreach (Transform effect in child)
        //{
        //    //Debug.Log(effect);

        //    var tmp = effect.transform.GetComponent<ParticleSystem>().main;
        //    tmp.loop = false;
        //}
        //if (this.gameObject)
            Destroy(this.gameObject);
    }

    private void HitMagic()
    {
        Instantiate(fx_explosion, this.transform.position, Quaternion.identity);

        StopAnimation();
    }

    private void MeteorCollision()
    {


        if (this.transform.position.y <= 0)
            Destroy(this.gameObject);
    }
}
