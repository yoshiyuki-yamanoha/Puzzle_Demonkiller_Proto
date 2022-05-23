using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    //[SerializeField] BoxCollider wepon_boxcollider = null;
    //[SerializeField] BoxCollider foot_boxcollider = null;

    private SEManager sePlay = null;

    [SerializeField] ParticleSystem[] fire = null;
    [SerializeField] ParticleSystem[] ice_star = null;
    [SerializeField] Transform destroy_effect = null;
    [SerializeField] ParticleSystem hit_effect = null;
    [SerializeField] TrailRenderer trail_renderer = null;

    bool is_anim_attack = false;
    bool is_flame_ice_break;
    private void Start()
    {
        //Nullじゃない
        if (hit_effect != null)
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

    public void EmittingOn()
    {
        trail_renderer.gameObject.SetActive(true);
        trail_renderer.emitting = true;
    }

    public void EmittingOff()
    {
        trail_renderer.gameObject.SetActive(false);
        trail_renderer.emitting = false;
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

    public void Fire()
    {
        fire[0].gameObject.SetActive(true); //アクティブオン

        if (fire != null)
        {
            for (int i = 0; i < fire.Length; i++)
            {
                fire[i].Play();
            }
        }
    }

    public void Ice_star()
    {
        ice_star[0].gameObject.SetActive(true); //アクティブオン

        if (ice_star != null)
        {
            for (int i = 0; i< ice_star.Length; i++)
            {
                ice_star[i].Play();
            }
        }
    }

    public bool GetFlameIcebreak()
    {
        return is_flame_ice_break;
    }

    public void FlameBreakIceOn()
    {
        is_flame_ice_break = true;
    }

    public void FlameBreakIceOff()
    {
        is_flame_ice_break = false;
    }
}
