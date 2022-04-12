using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] BoxCollider wepon_boxcollider = null;
    [SerializeField] BoxCollider foot_boxcollider = null;

    private SEManager sePlay = null;

    private void Start()
    {
        //sePlay = GameObject.Find("Audio").GetComponent<SEManager>();//Se再生用
        if (wepon_boxcollider != null) {
            WeponHideAttack();
        }

        if (foot_boxcollider != null)
        {
            FootHideAttack();
        }
        if (sePlay != null) sePlay = GameObject.Find("Audio").GetComponent<SEManager>();
    }

    public void Destroy()
    {
        //GameObject.Find("Stage1Mgr").GetComponent<Stage1Mgr>().DieEnemyCount();
        //GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(this.gameObject);
        Destroy(transform.root.gameObject);//一番上の親オブジェクト削除
        //敵が消える音を入れる//
        if (sePlay != null) sePlay.Play("EnemyDead");

    }

    public void WeponShowAttack()
    {
        wepon_boxcollider.enabled = true;
    }

    public void WeponHideAttack()
    {
        wepon_boxcollider.enabled = false;
    }

    public void FootShowAttack()
    {
        foot_boxcollider.enabled = true;
    }

    public void FootHideAttack()
    {
        foot_boxcollider.enabled = false;
    }
}
