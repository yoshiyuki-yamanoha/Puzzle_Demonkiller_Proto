using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    //[SerializeField] BoxCollider wepon_boxcollider = null;
    //[SerializeField] BoxCollider foot_boxcollider = null;

    private SEManager sePlay = null;
    [SerializeField] ParticleSystem[] destroy = null;
    [SerializeField] ParticleSystem[] hit_effect = null;
    float time;
    float particle_time = 1.5f;
    bool is_particle_play = false;


    bool is_anim_attack = false;
    private void Start()
    {
        //sePlay = GameObject.Find("Audio").GetComponent<SEManager>();//Se再生用
        if (sePlay != null) sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
    }

    public void Destroy()
    {
        //GameObject.Find("Stage1Mgr").GetComponent<Stage1Mgr>().DieEnemyCount();
        //GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(this.gameObject);
        //敵が消える音を入れる//
        if (sePlay != null) sePlay.Play("EnemyDead");
        Destroy(transform.root.gameObject);//一番上の親オブジェクト削除
    }

    public void IsParticlePlayOn()
    {
        is_particle_play = true;
    }

    bool IsParticlePlay()
    {
        return is_particle_play;
    }

    public bool IsAnimAttack()
    {
        return is_anim_attack;
    }

    public void SetAnimAttack()
    {
        is_anim_attack = true;
        Debug.Log("攻撃呼ばれました。");
    }

    private void FixedUpdate()
    {
        if (IsParticlePlay()) {
            time += Time.deltaTime;
            if (time > particle_time)
            {
                time = 0;
                Destroy();
            }
        }
    }

    public void HitEffect()
    {
        for (int number = 0; number < hit_effect.Length; number++)
        {
            hit_effect[number].gameObject.SetActive(true);
            hit_effect[number].Play();
        }
    }

    public void DestroyEffect()
    {
        IsParticlePlayOn();
        for (int number = 0; number < destroy.Length; number++)
        {
            destroy[number].gameObject.SetActive(true);
            destroy[number].Play();
        }
    }
}
