using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBas: MonoBehaviour
{



    public void InstantiateMagic(GameObject fx_obj, Vector3 instPos, Quaternion instRot)
    {
        Instantiate(fx_obj, instPos, instRot);
    }
}
