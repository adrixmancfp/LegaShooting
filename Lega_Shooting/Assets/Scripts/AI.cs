using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] destinations;
    public GameObject explosion;
    public GameObject droppedWeapon;
    public float distanceToFollowPath = 2;
    public bool dropWeapon = false, enemyDeath = false;

    private int i = 0;


    [Header("--Follow Player--")]
    public bool followPlayer;
    private GameObject player;
    private float distanceToPlayer;

    public float distanceToFollowPlayer = 10;

    void Start()
    {
        navMeshAgent.destination = destinations[i].transform.position;
        player = FindObjectOfType<PlayerController>().gameObject;
    }


    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= distanceToFollowPlayer && followPlayer)
        {
            FollowPlayer();
            transform.LookAt(player.transform.position);
        }
        else
        {
            EnemyPath();
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            EnemyDeath();
        }
    }


    public void EnemyPath()
    {
        navMeshAgent.destination = destinations[i].position;
        if (Vector3.Distance(transform.position, destinations[i].position) <= distanceToFollowPath)
        {
            if (destinations[i] != destinations[destinations.Length - 1])
            {
                i++;
            }
            else
            {
                i = 0;
            }
        }
    }

    public void FollowPlayer()
    {
        navMeshAgent.destination = player.transform.position;
    }


    private void EnemyDeath()
    {
        if (!enemyDeath)
        {
            GameManager.Instance.CountEnemy();
            enemyDeath = true;
        }
        if ((!dropWeapon) && (droppedWeapon != null))
        {
            Instantiate(droppedWeapon, new Vector3(transform.position.x, 1.44f, transform.position.z), Quaternion.identity);
            dropWeapon = true;
        }
        GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(newExplosion, 2);
  
    }


}
