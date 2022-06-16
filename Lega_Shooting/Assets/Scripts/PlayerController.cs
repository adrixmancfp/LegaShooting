using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    public float speed, rotationSpeed;
    Vector3 rbVel;
    //public Opciones opciones;
    public bool move;
    public int ammo;
    public GameObject[] weapons;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        rbVel = _rb.velocity;
    }


    private void Update()
    {
        PlayerRotation();
    }

    private void FixedUpdate()
    {
        if ((Input.GetKeyDown(KeyCode.W)) && (Input.GetKeyDown(KeyCode.A)) && (Input.GetKeyDown(KeyCode.S)) && (Input.GetKeyDown(KeyCode.D)))
        {
            move = true;
        }
        else
        {
            move = false;
        }
        PlayerMovement();


    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shotgun"))
        {
            Destroy(other.gameObject);
            weapons[0].SetActive(false);
            weapons[1].SetActive(true);
            weapons[2].SetActive(false);
        }
        if (other.CompareTag("Rifle"))
        {
            Destroy(other.gameObject);
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            weapons[2].SetActive(true);
        }
        if (other.CompareTag("Gun"))
        {
            Destroy(other.gameObject);
            weapons[0].SetActive(true);
            weapons[1].SetActive(false);
            weapons[2].SetActive(false);
        }
    }

    private void PlayerMovement()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");


        if (move == false)
        {
            StopPlayer();
        }

        Vector3 moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;

        moveDirection.Normalize();

        _rb.AddForce(speed * moveDirection);

    }

    private void PlayerRotation()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float hitDist;
        if (playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);   
        }
    }

    private void StopPlayer()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }


}
