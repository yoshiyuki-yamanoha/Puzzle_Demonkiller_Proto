using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] BoxCollider wepon_boxcollider = null;

    private void Start()
    {
        HideAttack();
    }

    public void Destroy()
    {
        GameObject.Find("Stage1Mgr").GetComponent<Stage1Mgr>().DieEnemyCount();
        GameObject.Find("Sphere").GetComponent<ShootMagic>().Enelist_Delete(this.gameObject);
        Destroy(transform.root.gameObject);//一番上の親オブジェクト削除
    }

    public void ShowAttack()
    {
        wepon_boxcollider.enabled = true;
    }

    public void HideAttack()
    {
        wepon_boxcollider.enabled = false;
    }
}
