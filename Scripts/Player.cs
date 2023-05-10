using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [Header("Статы")]
    public int HP;
    public int Damage;

    [Header("Генерация")]
    [SerializeField] Transform currentRoom;
    [SerializeField] Transform[] rooms;
    [SerializeField] Transform startingRoom;

    [SerializeField] Transform roomSpawner;
    [SerializeField] Transform playerSpawner;

    private int RoomInList;
    private int roomCount;
    public float resetTimer = 3;
    [SerializeField] float invincibilityTime = 2;

    private bool RoomEnded;
    
    private Camera camera;
    private NavMeshAgent navMesh;

    void Start()
    {
        camera = Camera.main;
        navMesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (invincibilityTime > 0)
            invincibilityTime -= Time.deltaTime;
        if (Input.GetKey(KeyCode.R))
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                navMesh.SetDestination(hit.point);
            }
        }
        if (RoomEnded && roomCount < 19)
        {
            roomsSpawn();
            RoomEnded = false;
        }
    }

    void roomsSpawn()
    {
        RoomInList = Random.Range(1, rooms.Length);
        Instantiate(rooms[RoomInList], roomSpawner.transform.position += new Vector3(30, 4, 0), Quaternion.identity);

        currentRoom = rooms[RoomInList];
        roomSpawner.transform.position += new Vector3(0, -4, 0);
        roomCount++;
    }

    void takingDamage()
    {
        HP--;
        if (HP == 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("Bullet")) && invincibilityTime <= 0)
        {
            takingDamage();
            invincibilityTime = 2;
        }
        if (other.CompareTag("Door"))
        {
            RoomEnded = true;
            gameObject.transform.position = playerSpawner.transform.position;
        }
    }
}
