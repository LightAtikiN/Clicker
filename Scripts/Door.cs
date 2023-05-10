using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int KillCount;
    [SerializeField] int enemyCount;

    void Update()
    {
        if (KillCount >= enemyCount)
        {
            Destroy(gameObject);
            KillCount = 0;
        }
    }
}
