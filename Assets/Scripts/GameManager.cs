using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameCanvas, menuCanvas, deathCanvas;
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager found!");
            return;
        }
        Instance = this;
    }
    
    void Start()
    {
        gameCanvas.SetActive(false);
        deathCanvas.SetActive(false);
    }

    public void StartGame()
    {
        gameCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void DeathScreen()
    {
        Invoke(nameof(PauseGame), 2f);
        deathCanvas.SetActive(true);
        gameCanvas.SetActive(false);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1f;
    }
}
