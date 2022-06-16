using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject[] bulletSpawnPoint;
    private Transform playerPosition;
    public float speed = 100f;
    public float shootWaitTime;

    private void Start()
    {
        playerPosition = FindObjectOfType<PlayerController>().transform;

        Invoke("ShootPlayer", 0);
    }

    private void Update()
    {
        
    }

    void ShootPlayer()
    {
        Vector3 playerDirection = playerPosition.position - transform.position;

        GameObject newBullet;
        for (int i = 0; i < bulletSpawnPoint.Length; i++)
        {
            newBullet = Instantiate(bulletPrefab, bulletSpawnPoint[i].transform.position, bulletSpawnPoint[i].transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(playerDirection * speed, ForceMode.Force);
        }

        Invoke("ShootPlayer", shootWaitTime);

    }
}
