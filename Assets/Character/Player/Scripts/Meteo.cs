using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : MonoBehaviour
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


        targetPos = new Vector3(targetEnemy.transform.position.x, 0f, targetEnemy.transform.position.z);
        velocity = 0.05f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MeteoMagic();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Player")
        {
            Debug.Log("hit");

            HitMagic();
        }
    }

    private void MeteoMagic()
    {
        this.transform.rotation.SetLookRotation(targetEnemy.transform.position, Vector3.up);
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, velocity);
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
        Instantiate(fx_explosion, this.transform.position, Quaternion.identity);

        StopAnimation();
    }
}
