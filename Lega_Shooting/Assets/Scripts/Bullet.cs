using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody _rb;    
    public float maxDistance;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * speed;


        maxDistance += 1 * Time.deltaTime;

        if (maxDistance > 3)
        {
            Destroy(this.gameObject);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
         {
             Destroy(other.gameObject);
             Destroy(this.gameObject);
         }

        if (other.CompareTag("Obstacle"))
        {
            Destroy(this.gameObject);
        }
    }


}
