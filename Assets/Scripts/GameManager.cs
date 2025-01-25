using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject controllerUI;

    private bool isGameOver = false;

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
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        controllerUI.SetActive(false);
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
}

