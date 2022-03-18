using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTowardsTheEnemy : MonoBehaviour
{
     public GameObject fx_explosion = null;
     public GameObject targetEnemy = null;
     private Transform child = null;


    Vector3 diff;
    Vector3 force;
    Rigidbody rb;
    float power;


    void Start()
    {
        child = this.transform.GetChild(0).gameObject.transform;
        //StopAnimation();
        Destroy(this.gameObject, 3.0f);
        power = 50.0f;

        rb = transform.GetComponent<Rigidbody>();
        //rb.AddForce(force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        diff = targetEnemy.transform.position - this.transform.position;
        force = Vector3.forward * power;
        rb.AddForce(force, ForceMode.Impulse);

        NullCheck_targetEnemy();
        HomingMagic();

    }

    void OnTriggerEnter(Collider other)
    {
        NullCheck_targetEnemy();


        if (other.tag == "Enemy")
        {
            Debug.Log("hit_tri");
                Destroy(other.gameObject);
            Destroy(this.gameObject);
                HitMagic();
            //if (other.gameObject == targetEnemy)
            {
            }
        }
    }

    private void HomingMagic()
    {
        //if (targetEnemy)
        //{
            this.transform.rotation.SetLookRotation(targetEnemy.transform.position, Vector3.up);
        //    this.transform.position = Vector3.Lerp(this.transform.position, targetEnemy.transform.position, force);
        //}
    }

    private void StopAnimation()
    {
        foreach (Transform effect in child)
        {

            var tmp = effect.transform.GetComponent<ParticleSystem>().main;
            tmp.loop = false;
        }

        Destroy(this.gameObject);
    }

    private void HitMagic()
    {
        //Instantiate(fx_explosion, this.transform.position, Quaternion.identity);

        StopAnimation();

    }

    // ターゲットがnullかチェックする
    // nullの場合、このスクリプトがアタッチされているオブジェクトを破壊する
    private void NullCheck_targetEnemy()
    {
        if (targetEnemy)
            return;
        else
            Destroy(this.gameObject);
    }

}
