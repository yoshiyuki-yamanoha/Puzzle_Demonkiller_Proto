using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] BoxCollider wepon_boxcollider = null;
    [SerializeField] BoxCollider foot_boxcollider = null;

    private void Start()
    {
        if (wepon_boxcollider != null) {
            WeponHideAttack();
        }

        if (foot_boxcollider != null)
        {
            FootHideAttack();
        }
    }

    public void Destroy()
    {
        GameObject.Find("Stage1Mgr").GetComponent<Stage1Mgr>().DieEnemyCount();
        GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(this.gameObject);
        Destroy(transform.root.gameObject);//一番上の親オブジェクト削除
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
