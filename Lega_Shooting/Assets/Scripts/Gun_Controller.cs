using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Controller : MonoBehaviour
{
    public Bullet bulletPrefab;
    public GameObject[] bulletSpawnPoint;
    public GameObject bullet;
    private Transform bulletSpawned;
    public bool isShot = false;
    public int ammo;
    public float waitTime;
    public PlayerController pController;
    public AudioClip clip;


    private void Update()
    {

        float shoot = Input.GetAxis("Fire1");
        if ((shoot > 0.1f) && (isShot == false) && (ammo > 0))
        {
            Shoot();
            Invoke("isShoted", waitTime);
            GetComponent<AudioSource>().PlayOneShot(clip);
            ammo--;
            GameManager.Instance.shootAmmo(ammo);
            isShot = true;
        }
    }


    private void Shoot()
    {
       
        for (int i = 0; i < bulletSpawnPoint.Length; i++)
        {
            //Para que la bala avance en dirección al cursor
            bulletSpawned = Instantiate(bulletPrefab.transform, bulletSpawnPoint[i].transform.position, bulletSpawnPoint[i].transform.rotation);
        }
    }

    private void isShoted()
    {
        isShot = false;
    }


}
