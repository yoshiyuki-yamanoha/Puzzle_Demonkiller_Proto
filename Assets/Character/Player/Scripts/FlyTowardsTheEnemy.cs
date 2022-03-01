using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTowardsTheEnemy : MonoBehaviour
{
    [SerializeField] public GameObject fx_explosion = null;
    [SerializeField] public GameObject targetEnemy = null;
    [SerializeField] private Transform child = null;

    Vector3 diff;
    float velocity; 


    void Start()
    {
        child = this.transform.GetChild(0).gameObject.transform;
        //StopAnimation();


        diff = targetEnemy.transform.position - this.transform.position;
        velocity = 0.05f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HomingMagic();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetEnemy)
        {
            HitMagic();
        }
    }

    private void HomingMagic()
    {
        this.transform.rotation.SetLookRotation(targetEnemy.transform.position, Vector3.up);
        this.transform.position = Vector3.Lerp(this.transform.position, targetEnemy.transform.position, velocity);
    }

    private void StopAnimation()
    {
        foreach (Transform effect in child)
        {
            //Debug.Log(effect);

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
}
