using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnGate : MonoBehaviour
{
    [SerializeField] int enemy_Max;
    [SerializeField] private Transform[] enePos = null;

    public void Init(int enemyMax) {
        enePos = new Transform[enemyMax];
    }

    
}
