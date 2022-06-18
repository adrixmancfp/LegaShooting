using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public GameObject enemiesContainer;
    [Header("Munición")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject lossCanvas;
    [SerializeField] private GameObject ammoCanvas;
    [SerializeField] private GameObject pauseCanvas;    
    public GameObject pickUpCanvas;
    [SerializeField] private TMP_Text maxAmmoTMP;
    [SerializeField] private TMP_Text actAmmoTMP;
    [SerializeField] private TMP_Text remainEnemiesTMP;
    [HideInInspector] public int enemyCount, enemyTotal, enemyRemain;
    [HideInInspector] public bool gameOver;

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

    
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.visible = true;
        }
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }


        ReloadAmmo(12);
        pauseCanvas.SetActive(false);
        ammoCanvas.SetActive(false);

        enemyTotal = enemiesContainer.transform.childCount;
        enemyRemain = enemyTotal - enemyCount;
        UpdateEnemiesCount();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        //Debug.Log("Enemigos restantes: {0} " + enemyRemain);
    }

    public void ReloadAmmo(int maxAmmo)
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

    public void ShootAmmo(int actAmmo)
    {
        actAmmoTMP.text = actAmmo.ToString();
    }

    public void PauseGame()
    {
        if (Time.timeScale == 1 && !gameOver)
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Iniciar()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void CountEnemy()
    {
        enemyCount++;
        UpdateEnemiesCount();
        CheckWin();
    }
    public void UpdateEnemiesCount()
    {
        enemyRemain = enemyTotal - enemyCount;
        remainEnemiesTMP.text = "Enemigos restantes  " + enemyRemain.ToString("\t00");
    }
    public void CheckWin()
    {
        if (enemyCount >= enemyTotal)
        {
            GameWon();
        }
    }
    public void GameWon()
    {
        GameOver();

        winCanvas.SetActive(true);
    }



    public void GameLoss()
    {
        GameOver();
        lossCanvas.SetActive(true);
    }

    public void GameOver()
    {
        gameOver = true;

    }
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Has salido del juego.");
    }

    public void ExitToMenu()
    {
         SceneManager.LoadScene(0);
    }


}