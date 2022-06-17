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
    [SerializeField] private TMP_Text maxAmmoTMP;
    [SerializeField] private TMP_Text actAmmoTMP;
    [HideInInspector] public int enemyCount, enemyTotal;
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
        ReloadAmmo(12);
        pauseCanvas.SetActive(false);
        ammoCanvas.SetActive(false);

        enemyTotal = enemiesContainer.transform.childCount;

        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
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

    private void PauseGame()
    {
        if (Time.timeScale == 1)
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
        CheckWin();
    }

    public void CheckWin()
    {
        if (enemyCount >= enemyTotal)
        {
            GameOver();
            winCanvas.SetActive(true);
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