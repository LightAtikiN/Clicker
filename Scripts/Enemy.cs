using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    [Header("Характеристики")]
    [SerializeField] int HP;
    [SerializeField] bool melee;

    [Header("Позиции")]
    [SerializeField] Transform bulletPrefab;
    [SerializeField] GameObject target;
    [SerializeField] Transform bullet;
    [SerializeField] GameObject playerPos;
    [SerializeField] Transform currentPos;

    [Header("Таймеры")]
    [SerializeField] float timeToMove;
    [SerializeField] float reloadTime;
    [SerializeField] float timeToFire;

    [SerializeField] bool isPlayerFounded;
    [SerializeField] LayerMask playerLayer;

    private Door door;
    private Player player;
    private NavMeshAgent navMesh;
    void Start()
    {
        if (melee)
            target = GameObject.FindGameObjectWithTag("Player");
        playerPos = GameObject.FindGameObjectWithTag("Player");

        door = FindObjectOfType<Door>();
        player = FindObjectOfType<Player>();
        navMesh = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        movement();
    }

    void movement()
    {
        if (melee)
            navMesh.destination = target.transform.position;
        else
        {
            if (timeToMove <= 0)
            {
                timeToMove = 4;
                target.transform.position += new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            }
            else
                timeToMove -= Time.deltaTime;
            navMesh.destination = target.transform.position;
            if (isPlayerFounded)
            {
                if (timeToFire <= 0)
                {
                    Transform bullet = Instantiate(bulletPrefab, currentPos.position, Quaternion.identity);
                    bullet.LookAt(playerPos.transform.position);
                    timeToFire = reloadTime;
                }
                else
                    timeToFire -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Physics.Linecast(transform.position, playerPos.transform.position, playerLayer))
        {
            isPlayerFounded = true;
        }
    }
    private void OnMouseDown()
    {
        HP -= player.Damage;
        if (HP <= 0)
        {
            door.KillCount++;
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, playerPos.transform.position);
    }
}
