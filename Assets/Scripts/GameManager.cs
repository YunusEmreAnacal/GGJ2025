using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseMenuUI;

    private bool isGameOver = false;
    private bool isPaused = false;

    private void Awake()
    {
        // GameManager'in tekil olmasını sağla
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverUI.SetActive(false);
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        // Escape tuşu ile Pause menüsünü aç/kapat
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }

    // Oyunu yeniden başlat
    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false; 
        gameOverUI.SetActive(false); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    // Ana menüye dön
    public void ReturnToMainMenu()
    {
         
        SceneManager.LoadScene("MainMenu"); 
    }

    // Oyundan çık
    public void ExitGame()
    {
        Application.Quit(); 
        UnityEditor.EditorApplication.isPlaying = false; 
    }

    // Oyunu duraklat ve Pause menüsünü aç
    public void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true); // Pause menüsünü aç
        Time.timeScale = 0f; // Oyun zamanını duraklat
    }

    // Oyunu devam ettir ve Pause menüsünü kapat
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false); // Pause menüsünü kapat
        Time.timeScale = 1f; // Oyun zamanını devam ettir
    }
}

