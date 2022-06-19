using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject[] bulletSpawnPoint;
    public AudioSource audioSource;
    public AudioClip clip;
    private Transform playerPosition;
    public float speed = 100f;
    public float shootWaitTime, distanceToPlayer;
    public bool isShoted = true, avoidShoot = true;

    public float distanceToFollowPlayer = 10;

    private void Start()
    {
        playerPosition = FindObjectOfType<PlayerController>().transform;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
        Invoke("ShootPlayer", 0);
        Invoke("EvitarDisparoInicial", 1.8f);
    }

    private void Update()
    {
        CheckObstacles();
        distanceToPlayer = Vector3.Distance(transform.position, playerPosition.transform.position);

        if (CheckObstacles() && (isShoted) && distanceToPlayer <= distanceToFollowPlayer)
        {
            Invoke("ShootPlayer", shootWaitTime);
            audioSource.PlayOneShot(clip);
            isShoted = false;
        }
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
        isShoted = true;
    }

    public bool CheckObstacles()
    {
        bool res = false;
        Vector3 rayOrigin = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
        Vector3 rayDirection = new Vector3(playerPosition.transform.position.x, playerPosition.transform.position.y + 1, playerPosition.transform.position.z);
        rayDirection -= rayOrigin;
        Ray ray = new Ray(rayOrigin, rayDirection);
        Debug.DrawRay(rayOrigin, rayDirection);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                res = true;
            }
        }
        return res;
    }

    private void EvitarDisparoInicial()
    {
        audioSource.volume = .1f;
    }


}
