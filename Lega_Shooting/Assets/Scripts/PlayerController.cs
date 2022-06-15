using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    public float speed, rotationSpeed, staAct;
    Vector3 rbVel;
    public Stamina stamina;
    public bool move;
    public GameObject[] weapons;
    private Vector3 moveDirection;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        rbVel = _rb.velocity;
        staAct = 20f;
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

        if ((Input.GetKeyDown(KeyCode.Space)) && (staAct < Stamina.instance.staminaActual))
        {
            PlayerDash(staAct, moveDirection);
        }

        PlayerMovement();


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Shotgun") && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(other.gameObject);
            weapons[0].SetActive(false);
            weapons[1].SetActive(true);
            weapons[2].SetActive(false);
        }
        if (other.CompareTag("Rifle") && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(other.gameObject);
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            weapons[2].SetActive(true);
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

        moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;

        moveDirection.Normalize();

        _rb.AddForce(speed * moveDirection);

    }


    private void PlayerDash(float dash, Vector3 moveDash)
    {

        _rb.AddForce(moveDash * dash * 10, ForceMode.Impulse);
        Stamina.instance.UseStamina(dash);

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
