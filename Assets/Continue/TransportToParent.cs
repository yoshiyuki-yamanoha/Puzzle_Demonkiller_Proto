using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportToParent : MonoBehaviour
{
    [SerializeField] Transform goalObjTf;

    public Vector3 GetGoalPos() {

        return goalObjTf.position;
    }
}
