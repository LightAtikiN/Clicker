using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;

    [HideInInspector] public float Damage;
    void Start()
    {
        Destroy(gameObject, 3);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
}
