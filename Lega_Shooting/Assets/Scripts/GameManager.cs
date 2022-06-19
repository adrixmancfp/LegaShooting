using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public GameObject crosshair;
    public GameObject enemiesContainer;
    [Header("Munición")]
    [SerializeField] private GameObject ammoCanvas;
    [SerializeField] private TMP_Text maxAmmoTMP;
    [SerializeField] private TMP_Text actAmmoTMP;
    [Header("Canvas Win Loss Pause")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject lossCanvas;
    [SerializeField] private GameObject pauseCanvasGlob;
    [Header("Opciones")]
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject opcionCanvas;
    [SerializeField] private Toggle fullScreen;
    [SerializeField] private TMP_Dropdown resolucionesDropDown;
    [SerializeField] private Resolution[] resoluciones;
    [Header("")]
    [SerializeField] private GameObject controlCanvas;    
    [SerializeField] private GameObject gameCanvas;    
    public GameObject pickUpCanvas;
    [SerializeField] private TMP_Text remainEnemiesTMP;
    [HideInInspector] public int enemyCount, enemyTotal, enemyRemain;
    [HideInInspector] public bool gameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
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

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            controlCanvas.SetActive(true);
            Time.timeScale = 0;
        }

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            
            if (Screen.fullScreen)
            {
                fullScreen.isOn = true;
            }
            else
            {
                fullScreen.isOn = false;
            }
            RevisarResolucion();
            ReloadAmmo(12);
            pauseCanvasGlob.SetActive(false);
            ammoCanvas.SetActive(false);

            enemyTotal = enemiesContainer.transform.childCount;
            enemyRemain = enemyTotal - enemyCount;
            UpdateEnemiesCount();
        }
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

    public void PauseGame()
    {
        if (Time.timeScale == 1 && !gameOver)
        {
            pauseCanvasGlob.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseCanvasGlob.SetActive(false);
            pauseCanvas.SetActive(true);
            opcionCanvas.SetActive(false);
            CambioCursorOff();
            Time.timeScale = 1;
        }
    }

    public void BeginGame()
    {
        controlCanvas.SetActive(false);
        CambioCursorOff();
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
        remainEnemiesTMP.text = "Enemigos restantes" + enemyRemain.ToString("\n0");
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

    public void LoadTutorial()
    {
        SceneManager.LoadScene(4);
    }

    public void SigTuto()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }

    public void ExitGame()
    {
        Application.Quit();
        
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void CambioCursorOn()
    {
        crosshair.SetActive(false);
        Cursor.visible = true;
    }

    public void CambioCursorOff()
    {
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    public void FullScreen()
    {
        if (fullScreen.isOn == true)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }
    
    public void ActivarOpciones()
    {
        opcionCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }
    
    public void DesactivarOpciones()
    {
        opcionCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }

    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;
        resolucionesDropDown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionAct = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + "x" + resoluciones[i].height /*+ " " + resoluciones[i].refreshRate + "fps"*/;

            opciones.Add(opcion);


            if ((Screen.fullScreen) && (resoluciones[i].width == Screen.currentResolution.width) && (resoluciones[i].height == Screen.currentResolution.height))
            {
                resolucionAct++;
            }
        }

        resolucionesDropDown.AddOptions(opciones);
        resolucionesDropDown.value = resolucionAct;
        resolucionesDropDown.RefreshShownValue();

        resolucionesDropDown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        PlayerPrefs.SetInt("numeroResolucion", resolucionesDropDown.value);

        Resolution resolucion = resoluciones[resolucionesDropDown.value];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }

}