using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Header("Municion")]
    [SerializeField] private GameObject ammoCanvas;
    [SerializeField] private GameObject initialMenu;
    [SerializeField] private TMP_Text maxAmmoTMP;
    [SerializeField] private TMP_Text actAmmoTMP;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        reloadAmmo(12);
        initialMenu.SetActive(false);
        ammoCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                initialMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                initialMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void reloadAmmo(int maxAmmo)
    {
        ammoCanvas.SetActive(true);
        if (maxAmmo > 10)
        {
            maxAmmoTMP.text = "/ " + maxAmmo.ToString();
        }
        else
        {
            maxAmmoTMP.text = "/  " + maxAmmo.ToString();
        }
        actAmmoTMP.text = maxAmmo.ToString();
    }

    public void shootAmmo(int actAmmo)
    {
        actAmmoTMP.text = actAmmo.ToString();
    }

    public void Iniciar()
    {
        initialMenu.SetActive(false);
        Time.timeScale = 1;
    }

}