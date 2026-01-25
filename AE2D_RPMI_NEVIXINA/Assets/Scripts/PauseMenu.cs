using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Canvas con botones de pausa
    public Button settingsButton;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        
        resumeButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitToMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !pausePanel.activeSelf;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        
        // Si GameManager tiene la propiedad IsPaused, actualízala
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IsPaused = isPaused;
        }
    }

    void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IsPaused = false;
        }
    }

    void OpenSettings()
    {
        // Aquí puedes mostrar otro panel con ajustes (volumen, etc.)
        Debug.Log("Abrir ajustes...");
    }

    void QuitToMenu()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene("MainMenu");
    }
}