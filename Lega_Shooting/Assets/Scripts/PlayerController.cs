using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    public float speed, rotationSpeed, staAct, acc;
    Vector3 rbVel;
    public bool move, dash;
    public int ammo;
    public string nameWeapon;
    public GameObject[] weapons;
    public GameObject arma;
    private Vector3 moveDirection;
    public bool isPickableShotgun, isPickableGun, isPickableRifle;
    public Gun_Controller gun, shotgun, rifle;
    public GameObject explosion;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        rbVel = _rb.velocity;
        staAct = 20f;
    }


    private void Update()
    {
        PlayerRotation();
        if ((Input.GetAxisRaw("Vertical") != 0) || (Input.GetAxisRaw("Horizontal") != 0))
        {
            move = true;
        }
        else
        {
            move = false;
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift)) && (staAct < Stamina.instance.staminaActual))
        {
            dash = true;
        }
        if ((Input.GetKeyDown(KeyCode.E)) && (nameWeapon != null))
        {
            if (isPickableGun)
            {
                GetWeapon(0, nameWeapon);
                gun.ammo = 12;
                GameManager.Instance.ReloadAmmo(gun.ammo);
            }
            else if (isPickableShotgun)
            {
                GetWeapon(1, nameWeapon);
                shotgun.ammo = 8;
                GameManager.Instance.ReloadAmmo(shotgun.ammo);
            }
            else if (isPickableRifle)
            {
                rifle.ammo = 30;
                GetWeapon(2, nameWeapon);
                GameManager.Instance.ReloadAmmo(rifle.ammo);
            }
        }

    }

    private void FixedUpdate()
    {

        if (dash == true)
        {
            PlayerDash(staAct, moveDirection);
            Invoke("StopPlayer", 0.1f);
            dash = false;
        }

        PlayerMovement();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gun"))
        {
            GameManager.Instance.pickUpCanvas.SetActive(true); 
            isPickableGun = true;
            nameWeapon = other.gameObject.name;
            
        }
        if (other.CompareTag("Shotgun"))
        {
            GameManager.Instance.pickUpCanvas.SetActive(true); 
            isPickableShotgun = true;
            nameWeapon = other.gameObject.name;
            
        }
        if (other.CompareTag("Rifle"))
        {
            GameManager.Instance.pickUpCanvas.SetActive(true); 
            isPickableRifle = true;
            nameWeapon = other.gameObject.name;
            
        }
        if (other.CompareTag("Bullet"))
        {
            PlayerDeath();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gun"))
        {
            GameManager.Instance.pickUpCanvas.SetActive(false);
            isPickableGun = false;
            nameWeapon = null;
        }
        if (other.CompareTag("Shotgun"))
        {
            GameManager.Instance.pickUpCanvas.SetActive(false);
            isPickableShotgun = false;
            nameWeapon = null;
        }
        if (other.CompareTag("Rifle"))
        {
            GameManager.Instance.pickUpCanvas.SetActive(false);
            isPickableRifle = false;
            nameWeapon = null;
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

        _rb.AddForce(acc * moveDirection);

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, speed);

    }

    private void GetWeapon(int weapon, string name)
    {
        switch (weapon)
        {
            case 0:
                weapons[0].SetActive(true);
                weapons[1].SetActive(false);
                weapons[2].SetActive(false);
                break;
            case 1:
                weapons[0].SetActive(false);
                weapons[1].SetActive(true);
                weapons[2].SetActive(false);
                break;
            case 2:
                weapons[0].SetActive(false);
                weapons[1].SetActive(false);
                weapons[2].SetActive(true);
                break;
        }
        isPickableGun = false;
        isPickableShotgun = false;
        isPickableRifle = false;
        arma = GameObject.Find(name);
        Destroy(arma.gameObject);
        GameManager.Instance.pickUpCanvas.SetActive(false);
    }

    private void PlayerDash(float dash, Vector3 moveDash)
    {

        _rb.AddForce(moveDash * dash * 3, ForceMode.Impulse);
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

    private void PlayerDeath()
    {
        GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(newExplosion, 2);
        Destroy(this.gameObject);
        GameManager.Instance.GameLoss();
    }

}
