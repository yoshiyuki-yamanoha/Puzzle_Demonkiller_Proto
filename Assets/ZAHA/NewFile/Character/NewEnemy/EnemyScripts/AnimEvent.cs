using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    //[SerializeField] BoxCollider wepon_boxcollider = null;
    //[SerializeField] BoxCollider foot_boxcollider = null;

    private SEManager sePlay = null;

    [SerializeField] Transform destroy_effect = null;
    [SerializeField] ParticleSystem hit_effect = null;


    bool is_anim_attack = false;
    private void Start()
    {
        hit_effect.gameObject.SetActive(false);
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

    public void SetAnimAttack(bool flg)
    {
        is_anim_attack = flg;
    }

    public bool IsAnimAttack()
    {
        return is_anim_attack;
    }

    public void AnimAttackOn()
    {
        is_anim_attack = true;
    }

    public void AnimHitEffect()
    {
        hit_effect.gameObject.SetActive(true);
        hit_effect.Play();
    }


    public void DestroyEffect()
    {
        Transform chilled;
        List<ParticleSystem> destroy_effects = new List<ParticleSystem>();
        chilled = destroy_effect.GetComponentInChildren<Transform>();//Transformを持っている子供を獲得

        for (int number = 0; number < chilled.childCount; number++)
        {
            if (chilled.GetComponent<ParticleSystem>() != null)
            {
                destroy_effects.Add(chilled.GetComponent<ParticleSystem>());
            }
        }

        Instantiate(destroy_effects[0], transform.position, Quaternion.Euler(90, 0, 0));
        Destroy();
    }
}
