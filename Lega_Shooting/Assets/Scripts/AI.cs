using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public Transform[] destinations;

    public float distanceToFollowPath = 2;

    private int i = 0;



    public bool followPlayer;

    private GameObject player;
    private float distancetoPlayer;

    public float distanceToFollow = 10;

    void Start()
    {
        navMeshAgent.destination = destinations[i].transform.position;
        player = FindObjectOfType<PlayerController>().gameObject;
    }


    void Update()
    {
        distancetoPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distancetoPlayer <= distanceToFollow && followPlayer)
        {
            FollowPlayer();
        }
        else
        {
            EnemyPath();
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

}
