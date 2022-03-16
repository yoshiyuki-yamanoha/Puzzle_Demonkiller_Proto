using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeFalse : MonoBehaviour
{
    // Start is called before the first frame update
    bool IsOnce;
    void Start()
    {
        IsOnce = true;
    }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf && IsOnce)
        {
            IsOnce = false;
            Invoke("SetFalse", 2.0f);
        }
    }

    // Update is called once per frame
    void SetFalse()
    {
        IsOnce = true;
        this.gameObject.SetActive(false);
    }
}
